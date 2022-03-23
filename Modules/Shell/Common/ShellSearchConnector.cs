//Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using Movie_Cleanup.Modules.Shell.Interop.Common;
using Movie_Cleanup.Modules.Core.Interop;
//using MS.WindowsAPICodePack.Internal;

namespace Movie_Cleanup.Modules.Shell.Common
{
    /// <summary>
    /// A Serch Connector folder in the Shell Namespace
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "This will complicate the class hierarchy and naming convention used in the Shell area")]
    public sealed class ShellSearchConnector : ShellSearchCollection
    {

        #region Private Constructor

        internal ShellSearchConnector()
        {
            CoreHelpers.ThrowIfNotWin7();
        }

        internal ShellSearchConnector(IShellItem2 shellItem)
        {
            CoreHelpers.ThrowIfNotWin7();

            nativeShellItem = shellItem;
        }

        #endregion

        /// <summary>
        /// Indicates whether this feature is supported on the current platform.
        /// </summary>
        public static bool IsPlatformSupported
        {
            get
            {
                // We need Windows 7 onwards ...
                return CoreHelpers.RunningOnWin7;
            }
        }
    }
}
