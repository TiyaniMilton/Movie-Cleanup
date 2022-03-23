//Copyright (c) Microsoft Corporation.  All rights reserved.

//using MS.WindowsAPICodePack.Internal;
using Movie_Cleanup.Modules.Shell.Interop.Common;
namespace Movie_Cleanup.Modules.Shell.Common
{
    /// <summary>
    /// Represents the base class for all search-related classes.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "This will complicate the class hierarchy and naming convention used in the Shell area")]
    public class ShellSearchCollection : ShellContainer
    {
        internal ShellSearchCollection()
        {
        }

        internal ShellSearchCollection(IShellItem2 shellItem):base(shellItem)
        {
        }
    }
}
