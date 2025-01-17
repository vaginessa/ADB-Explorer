﻿using ADB_Explorer.Models;
using ADB_Explorer.Services;
using ADB_Explorer.ViewModels;
using static ADB_Explorer.Services.ADBService.AdbDevice;

namespace ADB_Explorer.Helpers;

public class InProgressTestOperation : FileOperation
{
    private readonly InProgSyncProgressViewModel info;

    private InProgressTestOperation(Dispatcher dispatcher, ADBService.AdbDevice adbDevice, string filePath, AdbSyncProgressInfo adbInfo) :
        base(dispatcher, adbDevice, new FilePath(filePath))
    {
        info = new(adbInfo);
    }

    public static InProgressTestOperation CreateProgressStart(Dispatcher dispatcher, ADBService.AdbDevice adbDevice, string filePath)
    {
        return new InProgressTestOperation(dispatcher, adbDevice, filePath, new AdbSyncProgressInfo
        {
            CurrentFile = null,
            TotalPercentage = null,
            CurrentFilePercentage = null,
            CurrentFileBytesTransferred = null
        });
    }

    public static InProgressTestOperation CreateFileInProgress(Dispatcher dispatcher, ADBService.AdbDevice adbDevice, string filePath)
    {
        return new InProgressTestOperation(dispatcher, adbDevice, filePath, new AdbSyncProgressInfo
        {
            CurrentFile = null,
            TotalPercentage = 40,
            CurrentFilePercentage = null,
            CurrentFileBytesTransferred = null
        });
    }

    public static InProgressTestOperation CreateFolderInProgress(Dispatcher dispatcher, ADBService.AdbDevice adbDevice, string filePath)
    {
        return new InProgressTestOperation(dispatcher, adbDevice, filePath, new AdbSyncProgressInfo
        {
            CurrentFile = "Subfile.txt",
            TotalPercentage = 40,
            CurrentFilePercentage = 60,
            CurrentFileBytesTransferred = null
        });
    }

    public override void Start()
    {
        Status = OperationStatus.InProgress;
        StatusInfo = info;
    }

    public override void Cancel()
    {
        Status = OperationStatus.Canceled;
        StatusInfo = new CanceledOpProgressViewModel();
    }

    public void Fail(string errorMsg)
    {
        Status = OperationStatus.Failed;
        StatusInfo = new FailedOpProgressViewModel(errorMsg);
    }
}
