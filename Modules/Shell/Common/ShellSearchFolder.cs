﻿//Copyright (c) Microsoft Corporation.  All rights reserved.

using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Security;
using Movie_Cleanup.Modules.Shell.Interop.Common;
using Movie_Cleanup.Modules.Core.PropertySystem;
using Movie_Cleanup.Modules.Shell.Interop;
using Movie_Cleanup.Modules.Core.Interop;
//using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
//using MS.WindowsAPICodePack.Internal;

namespace Movie_Cleanup.Modules.Shell.Common
{
    /// <summary>
    /// Create and modify search folders.
    /// </summary>
    public class ShellSearchFolder : ShellSearchCollection
    {
        /// <summary>
        /// Create a simple search folder. Once the appropriate parameters are set, 
        /// the search folder can be enumerated to get the search results.
        /// </summary>
        /// <param name="searchCondition">Specific condition on which to perform the search (property and expected value)</param>
        /// <param name="searchScopePath">List of folders/paths to perform the search on. These locations need to be indexed by the system.</param>
        public ShellSearchFolder(SearchCondition searchCondition, params ShellContainer[] searchScopePath)
        {
            CoreHelpers.ThrowIfNotWin7();

            NativeSearchFolderItemFactory = (ISearchFolderItemFactory)new SearchFolderItemFactoryCoClass();

            this.SearchCondition = searchCondition;

            List<string> paths = new List<string>();
            foreach (ShellContainer cont in searchScopePath)
                paths.Add(cont.ParsingName);

            this.SearchScopePaths = paths.ToArray();
        }

        /// <summary>
        /// Create a simple search folder. Once the appropiate parameters are set, 
        /// the search folder can be enumerated to get the search results.
        /// </summary>
        /// <param name="searchCondition">Specific condition on which to perform the search (property and expected value)</param>
        /// <param name="searchScopePath">List of folders/paths to perform the search on. These locations need to be indexed by the system.</param>
        public ShellSearchFolder(SearchCondition searchCondition, params string[] searchScopePath)
        {
            CoreHelpers.ThrowIfNotWin7();

            NativeSearchFolderItemFactory = (ISearchFolderItemFactory)new SearchFolderItemFactoryCoClass();

            this.SearchScopePaths = searchScopePath;
            this.SearchCondition = searchCondition;
        }

        internal ISearchFolderItemFactory NativeSearchFolderItemFactory
        {
            get;
            set;
        }

        private SearchCondition searchCondition;
        /// <summary>
        /// Gets the <see cref="Microsoft.WindowsAPICodePack.Shell.SearchCondition"/> of the search. 
        /// When this property is not set, the resulting search will have no filters applied.
        /// </summary>
        public SearchCondition SearchCondition
        {
            get { return searchCondition; }
            private set
            {
                searchCondition = value;

                NativeSearchFolderItemFactory.SetCondition(searchCondition.NativeSearchCondition);
            }
        }

        private string[] searchScopePaths;
        /// <summary>
        /// Gets the search scope, as specified using an array of locations to search. 
        /// The search will include this location and all its subcontainers. The default is FOLDERID_Profile
        /// </summary>
        public string[] SearchScopePaths
        {
            get
            {
                return searchScopePaths;
            }
            private set
            {
                searchScopePaths = value;
                List<IShellItem> shellItems = new List<IShellItem>();

                Guid shellItemGuid = new Guid(ShellIIDGuid.IShellItem);
                Guid shellItemArrayGuid = new Guid(ShellIIDGuid.IShellItemArray);

                // Create IShellItem for all the scopes we were given
                foreach (string path in searchScopePaths)
                {
                    IShellItem scopeShellItem;

                    int hr = ShellNativeMethods.SHCreateItemFromParsingName(path, IntPtr.Zero, ref shellItemGuid, out scopeShellItem);

                    if (CoreErrorHelper.Succeeded(hr))
                        shellItems.Add(scopeShellItem);
                }

                // Create a new IShellItemArray
                IShellItemArray scopeShellItemArray = new ShellItemArray(shellItems.ToArray());

                // Set the scope on the native ISearchFolderItemFactory
                HRESULT hResult = NativeSearchFolderItemFactory.SetScope(scopeShellItemArray);

                if (!CoreErrorHelper.Succeeded((int)hResult))
                    Marshal.ThrowExceptionForHR((int)hResult);
            }
        }

        internal override IShellItem NativeShellItem
        {
            get
            {
                IShellItem shellItem;
                Guid guid = new Guid(ShellIIDGuid.IShellItem);

                if (NativeSearchFolderItemFactory != null)
                {
                    int hr = NativeSearchFolderItemFactory.GetShellItem(ref guid, out shellItem);

                    if (!CoreErrorHelper.Succeeded(hr))
                        Marshal.ThrowExceptionForHR(hr);

                    return shellItem;
                }
                else
                    return null;
            }
        }


        /// <summary>
        /// Creates a list of stack keys, as specified. If this method is not called, 
        /// by default the folder will not be stacked.
        /// </summary>
        /// <param name="canonicalNames">Array of canonical names for properties on which the folder is stacked.</param>
        /// <exception cref="System.ArgumentException">If one of the given canonical names is invalid.</exception>
        public void SetStacks(params string[] canonicalNames)
        {
            List<PropertyKey> propertyKeyList = new List<PropertyKey>();

            foreach (string prop in canonicalNames)
            {
                // Get the PropertyKey using the canonicalName passed in
                PropertyKey propKey;
                int result = PropertySystemNativeMethods.PSGetPropertyKeyFromName(prop, out propKey);

                if (!CoreErrorHelper.Succeeded(result))
                    throw new ArgumentException("The given CanonicalName is not valid.", "canonicalNames", Marshal.GetExceptionForHR(result));

                propertyKeyList.Add(propKey);
            }

            if (propertyKeyList.Count > 0)
                SetStacks(propertyKeyList.ToArray());
        }

        /// <summary>
        /// Creates a list of stack keys, as specified. If this method is not called, 
        /// by default the folder will not be stacked.
        /// </summary>
        /// <param name="propertyKeys">Array of property keys on which the folder is stacked.</param>
        public void SetStacks(params PropertyKey[] propertyKeys)
        {
            if (propertyKeys.Length > 0)
                NativeSearchFolderItemFactory.SetStacks((uint)propertyKeys.Length, propertyKeys);
        }

        /// <summary>
        /// Sets the search folder display name.
        /// </summary>
        public string DisplayName
        {
            set
            {
                HRESULT hr = NativeSearchFolderItemFactory.SetDisplayName(value);

                if (!CoreErrorHelper.Succeeded((int)hr))
                    Marshal.ThrowExceptionForHR((int)hr);
            }
        }

        /// <summary>
        /// Sets the search folder icon size.
        /// The default settings are based on the FolderTypeID which is set by the 
        /// SearchFolder::SetFolderTypeID method.
        /// </summary>
        public int IconSize
        {
            set
            {
                HRESULT hr = NativeSearchFolderItemFactory.SetIconSize(value);

                if (!CoreErrorHelper.Succeeded((int)hr))
                    Marshal.ThrowExceptionForHR((int)hr);
            }
        }

        /// <summary>
        /// Sets a search folder type ID, as specified. 
        /// </summary>
        public Guid FolderTypeID
        {
            set
            {
                HRESULT hr = NativeSearchFolderItemFactory.SetFolderTypeID(value);

                if (!CoreErrorHelper.Succeeded((int)hr))
                    Marshal.ThrowExceptionForHR((int)hr);
            }
        }

        /// <summary>
        /// Sets folder logical view mode. The default settings are based on the FolderTypeID which is set 
        /// by the SearchFolder::SetFolderTypeID method.        
        /// </summary>
        /// <param name="mode">The logical view mode to set.</param>
        public void SetFolderLogicalViewMode(FolderLogicalViewMode mode)
        {
            HRESULT hr = NativeSearchFolderItemFactory.SetFolderLogicalViewMode(mode);

            if (!CoreErrorHelper.Succeeded((int)hr))
                Marshal.ThrowExceptionForHR((int)hr);
        }

        /// <summary>
        /// Creates a new column list whose columns are all visible, 
        /// given an array of PropertyKey structures. The default is based on FolderTypeID.
        /// </summary>
        public PropertyKey[] VisibleColumns
        {
            set
            {
                HRESULT hr = NativeSearchFolderItemFactory.SetVisibleColumns((uint)value.Length, value);

                if (!CoreErrorHelper.Succeeded((int)hr))
                    Marshal.ThrowExceptionForHR((int)hr);
            }
        }

        /// <summary>
        /// Creates a list of sort column directions, as specified.
        /// </summary>
        public SortColumn[] SortColumns
        {
            set
            {
                HRESULT hr = NativeSearchFolderItemFactory.SetSortColumns((uint)value.Length, value);

                if (!CoreErrorHelper.Succeeded((int)hr))
                    Marshal.ThrowExceptionForHR((int)hr);
            }
        }

        /// <summary>
        /// Sets a group column, as specified. If no group column is specified, no grouping occurs. 
        /// </summary>
        public PropertyKey GroupColumn
        {
            set
            {
                HRESULT hr = NativeSearchFolderItemFactory.SetGroupColumn(ref value);

                if (!CoreErrorHelper.Succeeded((int)hr))
                    Marshal.ThrowExceptionForHR((int)hr);
            }
        }


        /// <summary>
        /// Indicates whether this feature is supported on the current platform.
        /// </summary>
        public static bool IsPlatformSupported
        {
            get
            {
                // We need Windows Vista onwards ...
                return CoreHelpers.RunningOnWin7;
            }
        }
    }
}
