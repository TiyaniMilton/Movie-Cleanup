﻿//Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Text;
using Movie_Cleanup.Modules.Shell.Common;
using Movie_Cleanup.Modules.Shell.Interop.Common;
//using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
//using MS.WindowsAPICodePack.Internal;

namespace Movie_Cleanup.Modules.Shell.Common
{
    /// <summary>
    /// Represents a link to existing FileSystem or Virtual item.
    /// </summary>
    public class ShellLink : ShellObject
    {
        /// <summary>
        /// Path for this file e.g. c:\Windows\file.txt,
        /// </summary>
        private string internalPath = null;

        #region Internal Constructors

        internal ShellLink(IShellItem2 shellItem)
        {
            nativeShellItem = shellItem;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The path for this link
        /// </summary>
        virtual public string Path
        {
            get
            {
                if (internalPath == null && NativeShellItem != null)
                {
                    internalPath = base.ParsingName;
                }
                return internalPath;
            }
            protected set
            {
                this.internalPath = value;
            }
        }

        private string internalTargetLocation;
        /// <summary>
        /// Gets the location to which this link points to.
        /// </summary>
        public string TargetLocation
        {
            get
            {
                if (string.IsNullOrEmpty(internalTargetLocation) && NativeShellItem2 != null)
                    internalTargetLocation = this.Properties.System.Link.TargetParsingPath.Value;

                return internalTargetLocation;
            }
            set
            {
                if (value == null)
                    return;

                internalTargetLocation = value;

                if (NativeShellItem2 != null)
                    this.Properties.System.Link.TargetParsingPath.Value = internalTargetLocation;
            }
        }

        /// <summary>
        /// Gets the ShellObject to which this link points to.
        /// </summary>
        public ShellObject TargetShellObject
        {
            get
            {
                return ShellObjectFactory.Create(TargetLocation);
            }
        }

        /// <summary>
        /// Gets or sets the link's title
        /// </summary>
        public string Title
        {
            get
            {
                if (NativeShellItem2 != null)
                    return this.Properties.System.Title.Value;
                else
                    return null;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                if (NativeShellItem2 != null)
                    this.Properties.System.Title.Value = value;
            }
        }

        #endregion
    }
}
