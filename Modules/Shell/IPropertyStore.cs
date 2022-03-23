//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Security;
//using System.Runtime.InteropServices;
//using Movie_Cleanup.Modules.Core.PropertySystem;
//using Movie_Cleanup.Modules.Internal;

//namespace Movie_Cleanup.Modules.Shell
//{
//    //[SuppressUnmanagedCodeSecurity, Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), SecurityCritical(SecurityCriticalScope.Everything)]
//    //internal interface IPropertyStore
//    //{
//    //    uint GetCount();
//    //    PKEY GetAt(uint iProp);
//    //    [SecurityCritical]
//    //    void GetValue([In] ref PKEY pkey, [In] [Out] PROPVARIANT pv);
//    //    [SecurityCritical]
//    //    void SetValue([In] ref PKEY pkey, PROPVARIANT pv);
//    //    void Commit();
//    //}

//    /// <summary>
//        /// is defined in propsys.h
//        /// </summary>
//        [Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99"),
//            InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
//        interface IPropertyStore
//        {
//            int GetCount(out int propCount);
//            int GetAt(int property, out PropertyKey key);
//            int GetValue(ref PropertyKey key, out PropVariant value);
//            int SetValue(ref PropertyKey key, ref PropVariant value);
//            int Commit();
//        }
//}
