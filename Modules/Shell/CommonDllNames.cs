using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Movie_Cleanup.Modules.Shell
{
    /// <summary>
    /// Class to hold string references to common interop DLLs.
    /// </summary>
    internal static class CommonDllNames
    {
        /// <summary>
        /// User32.dll
        /// </summary>
        public const string User32 = "user32.dll";
        /// <summary>
        /// Shell32.dll
        /// </summary>
        public const string Shell32 = "shell32.dll";
    }
}
