﻿//Copyright (c) Microsoft Corporation.  All rights reserved.

using System.IO;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices;
using Movie_Cleanup.Modules.Shell.Interop.Common;
using Movie_Cleanup.Modules.Core.Interop;
//using Microsoft.WindowsAPICodePack.Shell;
//using MS.WindowsAPICodePack.Internal;

namespace Movie_Cleanup.Modules.Shell.Common
{
    class ShellFolderItems : IEnumerator<ShellObject>
    {
        #region Private Fields

        private IEnumIDList nativeEnumIdList = null;
        private ShellObject currentItem = null;
        ShellContainer nativeShellFolder;

        #endregion

        #region Internal Constructor

        internal ShellFolderItems(ShellContainer nativeShellFolder)
        {
            this.nativeShellFolder = nativeShellFolder;

            HRESULT hr = nativeShellFolder.NativeShellFolder.EnumObjects(
                IntPtr.Zero,
                ShellNativeMethods.SHCONT.SHCONTF_FOLDERS | ShellNativeMethods.SHCONT.SHCONTF_NONFOLDERS,
                out nativeEnumIdList);

            if (!CoreErrorHelper.Succeeded((int)hr))
                Marshal.ThrowExceptionForHR((int)hr);
        }

        #endregion

        #region IEnumerator<ShellObject> Members

        public ShellObject Current
        {
            get
            {
                return currentItem;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (nativeEnumIdList != null)
            {
                Marshal.ReleaseComObject(nativeEnumIdList);
                nativeEnumIdList = null;
            }
        }

        #endregion

        #region IEnumerator Members

        object IEnumerator.Current
        {
            get
            {
                return currentItem;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            if (nativeEnumIdList == null)
                return false;

            IntPtr item;
            uint numItemsReturned;
            uint itemsRequested = 1;
            HRESULT hr = nativeEnumIdList.Next(itemsRequested, out item, out numItemsReturned);

            if (numItemsReturned < itemsRequested || hr != HRESULT.S_OK)
                return false;

            currentItem = ShellObjectFactory.Create(item, nativeShellFolder);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            if(nativeEnumIdList != null)
                nativeEnumIdList.Reset();
        }


        #endregion
    }
}
