// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;

namespace Movie_Cleanup.Modules.Shell.Common
{
    /// <summary>
    /// Represents the base class for all types of folders (filesystem and non filesystem)
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "This will complicate the class hierarchy and naming convention used in the Shell area")]
    public abstract class ShellFolder : ShellContainer
    {
        
    }
}
