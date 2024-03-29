﻿//Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using Movie_Cleanup.Modules.Shell.Interop.Common;

namespace Movie_Cleanup.Modules.Shell.Common
{
    /// <summary>
    /// A folder in the Shell Namespace
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "This will complicate the class hierarchy and naming convention used in the Shell area")]
    public class ShellFileSystemFolder : ShellFolder
    {
        #region Internal Constructor

        internal ShellFileSystemFolder()
        {

        }

        internal ShellFileSystemFolder(IShellItem2 shellItem)
        {
            nativeShellItem = shellItem;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Constructs a new ShellFileSystemFolder object given a folder path
        /// </summary>
        /// <param name="path">The folder path</param>
        /// <remarks>ShellFileSystemFolder created from the given folder path.</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object)", Justification = "We are not currently handling globalization or localization")]
        static public ShellFileSystemFolder FromFolderPath(string path)
        {
            // Get the absolute path
            string absPath = ShellHelper.GetAbsolutePath(path);
            
            // Make sure this is valid
            if(!Directory.Exists(absPath))
                throw new DirectoryNotFoundException(string.Format("The given path does not exist ({0})", path));

            ShellFileSystemFolder folder = new ShellFileSystemFolder();
            folder.ParsingName = absPath;
            folder.Path = absPath;
            return folder;
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// Path for this Folder e.g. c:\Windows
        /// </summary>
        private string internalPath = null;

        #endregion

        #region Public Properties

        /// <summary>
        /// The path for this Folder
        /// </summary>
        virtual public string Path
        {
            get
            {
                if (internalPath == null && NativeShellItem != null)
                {
                    internalPath = ShellHelper.GetParsingName(NativeShellItem);
                }
                return internalPath;
            }
            protected set
            {
                this.internalPath = value;
            }
        }

        #endregion

    }
}