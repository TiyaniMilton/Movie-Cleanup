//Copyright (c) Microsoft Corporation.  All rights reserved.

using Movie_Cleanup.Modules.Shell.Interop.Common;
namespace Movie_Cleanup.Modules.Shell.Common
{
    /// <summary>
    /// Represents a non filesystem item (e.g. virtual items inside Control Panel)
    /// </summary>
    public class ShellNonFileSystemItem : ShellObjectNode
    {
        internal ShellNonFileSystemItem(IShellItem2 shellItem)
        {
            nativeShellItem = shellItem;
        }
    }
}
