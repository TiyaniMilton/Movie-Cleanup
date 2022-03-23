using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Movie_Cleanup.Modules.Shell.Interop.Common;

namespace Movie_Cleanup.Modules.Shell
{
    /// <summary>
    /// Interface for jump list items
    /// </summary>
    public interface IJumpListItem
    {
        /// <summary>
        /// Gets or sets this item's path
        /// </summary>
        string Path { get; set; }
    }

    /// <summary>
    /// Interface for jump list tasks
    /// </summary>
    public abstract class JumpListTask
    {
        internal abstract IShellLinkW NativeShellLink { get; }
    }
}
