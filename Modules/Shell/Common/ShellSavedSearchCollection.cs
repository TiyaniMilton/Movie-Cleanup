//Copyright (c) Microsoft Corporation.  All rights reserved.

//using MS.WindowsAPICodePack.Internal;
using Movie_Cleanup.Modules.Shell.Interop.Common;
using Movie_Cleanup.Modules.Core.Interop;
namespace Movie_Cleanup.Modules.Shell.Common
{
    /// <summary>
    /// Represents a saved search
    /// </summary>
    public class ShellSavedSearchCollection : ShellSearchCollection
    {
        internal ShellSavedSearchCollection(IShellItem2 shellItem)
            :base(shellItem)
        {
            CoreHelpers.ThrowIfNotWin7();
        }

        /// <summary>
        /// Indicates whether this feature is supported on the current platform.
        /// </summary>
        public static bool IsPlatformSupported
        {
            get
            {
                // We need Windows Vista onwards ...
                return CoreHelpers.RunningOnWin7;
            }
        }
    }
}
