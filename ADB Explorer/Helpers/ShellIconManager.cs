﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ADB_Explorer.Helpers
{
    public static class ShellIconManager
    {
        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_LARGEICON = 0x0;
        private const uint SHGFI_SMALLICON = 0x1;
        private const uint SHGFI_USEFILEATTRIBUTES = 0x10;
        private const uint SHGFI_LINKOVERLAY = 0x8000;
        private const uint FILE_ATTRIBUTE_NORMAL = 0x80;

        [StructLayout(LayoutKind.Sequential)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        [DllImport("Shell32.dll")]
        private static extern IntPtr SHGetFileInfo(
            string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

        [DllImport("User32.dll")]
        private static extern int DestroyIcon(IntPtr hIcon);

        public enum IconSize : uint
        {
            Large = SHGFI_LARGEICON,
            Small = SHGFI_SMALLICON
        }
        private static Icon GetIcon(string filePath, uint flags)
        {
            SHFILEINFO shinfo = new SHFILEINFO();
            IntPtr hImgSmall = SHGetFileInfo(
                filePath,
                FILE_ATTRIBUTE_NORMAL,
                ref shinfo,
                (uint)Marshal.SizeOf(shinfo),
                SHGFI_ICON | flags);

            Icon icon = (Icon)Icon.FromHandle(shinfo.hIcon).Clone();
            DestroyIcon(shinfo.hIcon);
            return icon;
        }

        public static Icon GetExtensionIcon(string extension, IconSize iconSize, bool isLink)
        {
            return GetIcon(extension, SHGFI_USEFILEATTRIBUTES | (uint)iconSize | (isLink ? SHGFI_LINKOVERLAY : 0));
        }

        public static Icon GetFileIcon(string filePath, IconSize iconSize, bool isLink)
        {
            return GetIcon(filePath, (uint)iconSize | (isLink ? SHGFI_LINKOVERLAY : 0));
        }
    }
}
