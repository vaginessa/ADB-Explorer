﻿using ADB_Explorer.Models;

namespace ADB_Explorer.Services;

public class FilePullOperation : FileSyncOperation
{
    public FilePullOperation(Dispatcher dispatcher, ADBService.AdbDevice adbDevice, FilePath sourcePath, FilePath targetPath)
        : base(dispatcher, OperationType.Pull, adbDevice.PullFile, adbDevice, sourcePath, targetPath) { }
}
