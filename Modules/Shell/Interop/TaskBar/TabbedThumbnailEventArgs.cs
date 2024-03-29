// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Windows;

namespace Movie_Cleanup.Modules.Shell.Interop.TaskBar
{
    /// <summary>
    /// Event args for various Tabbed Thumbnail related events
    /// </summary>
    public class TabbedThumbnailEventArgs : EventArgs
    {
        /// <summary>
        /// Creates a Event Args for a specific tabbed thumbnail event.
        /// </summary>
        /// <param name="windowHandle">Window handle for the control/window related to the event</param>
        /// <param name="preview">TabbedThumbnail related to this event</param>
        public TabbedThumbnailEventArgs(IntPtr windowHandle, Modules.Shell.Interop.TaskBar.TabbedThumbnail preview)
        {
            TabbedThumbnail = preview;
            WindowHandle = windowHandle;
            WindowsControl = null;
        }

        /// <summary>
        /// Creates a Event Args for a specific tabbed thumbnail event.
        /// </summary>
        /// <param name="windowsControl">WPF Control (UIElement) related to the event</param>
        /// <param name="preview">TabbedThumbnail related to this event</param>
        public TabbedThumbnailEventArgs(UIElement windowsControl, Modules.Shell.Interop.TaskBar.TabbedThumbnail preview)
        {
            TabbedThumbnail = preview; 
            WindowHandle = IntPtr.Zero;
            WindowsControl = windowsControl;
        }

        /// <summary>
        /// Gets the Window handle for the specific control/window that is related to this event.
        /// </summary>
        /// <remarks>For WPF Controls (UIElement) the WindowHandle will be IntPtr.Zero. 
        /// Check the WindowsControl property to get the specific control associated with this event.</remarks>
        public IntPtr WindowHandle
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the WPF Control (UIElement) that is related to this event. This property may be null
        /// for non-WPF applications.
        /// </summary>
        public UIElement WindowsControl
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the TabbedThumbnail associated with this event.
        /// </summary>
        public TabbedThumbnail TabbedThumbnail
        {
            get;
            private set;
        }
    }
}
