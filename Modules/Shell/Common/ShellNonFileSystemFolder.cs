// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Movie_Cleanup.Modules.Shell.Common;
using Movie_Cleanup.Modules.Shell.Interop.Common;

namespace Movie_Cleanup.Modules.Shell.Common
{
    /// <summary>
    /// Represents a Non FileSystem folder (e.g. My Computer, Control Panel)
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "This will complicate the class hierarchy and naming convention used in the Shell area")]
    public class ShellNonFileSystemFolder : ShellFolder
    {
        #region Internal Constructors

        internal ShellNonFileSystemFolder()
        {

        }

        internal ShellNonFileSystemFolder(IShellItem2 shellItem)
        {
            nativeShellItem = shellItem;
        }

        #endregion

    }
}
