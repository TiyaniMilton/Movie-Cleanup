﻿//Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using Movie_Cleanup.Modules.Shell.Interop.Common;
using Movie_Cleanup.Modules.Core.PropertySystem;
using Movie_Cleanup.Modules.Core.Interop;
//using MS.WindowsAPICodePack.Internal;
//using Microsoft.WindowsAPICodePack.Shell.PropertySystem;

namespace Movie_Cleanup.Modules.Shell.Common
{
    internal class ShellItemArray : IShellItemArray
    {
        List<IShellItem> shellItemsList = new List<IShellItem>();

        internal ShellItemArray(IShellItem[] shellItems)
        {
            shellItemsList.AddRange(shellItems);
        }

        #region IShellItemArray Members

        public HRESULT BindToHandler(IntPtr pbc, ref Guid rbhid, ref Guid riid, out IntPtr ppvOut)
        {
            throw new NotSupportedException();
        }

        public HRESULT GetPropertyStore(int Flags, ref Guid riid, out IntPtr ppv)
        {
            throw new NotSupportedException();
        }

        public HRESULT GetPropertyDescriptionList(ref PropertyKey keyType, ref Guid riid, out IntPtr ppv)
        {
            throw new NotSupportedException();
        }

        public HRESULT GetAttributes(ShellNativeMethods.SIATTRIBFLAGS dwAttribFlags, ShellNativeMethods.SFGAO sfgaoMask, out ShellNativeMethods.SFGAO psfgaoAttribs)
        {
            throw new NotSupportedException();
        }

        public HRESULT GetCount(out uint pdwNumItems)
        {
            pdwNumItems = (uint)shellItemsList.Count;
            return HRESULT.S_OK;
        }

        public HRESULT GetItemAt(uint dwIndex, out IShellItem ppsi)
        {
            int index = (int)dwIndex;

            if (index < shellItemsList.Count)
            {
                ppsi = shellItemsList[index];
                return HRESULT.S_OK;
            }
            else
            {
                ppsi = null;
                return HRESULT.E_FAIL;
            }
        }

        public HRESULT EnumItems(out IntPtr ppenumShellItems)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
