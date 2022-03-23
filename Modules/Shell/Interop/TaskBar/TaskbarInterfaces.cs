//Copyright (c) Microsoft Corporation.  All rights reserved.

namespace Movie_Cleanup.Modules.Shell.Interop.TaskBar
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
    public interface IJumpListTask
    {
    }
}
