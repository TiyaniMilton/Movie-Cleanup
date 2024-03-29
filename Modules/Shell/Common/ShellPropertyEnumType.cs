//Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using Movie_Cleanup.Modules.Shell.Interop;
using Movie_Cleanup.Modules.Internal;
using Movie_Cleanup.Modules.Shell.PropertySystem;
//using MS.WindowsAPICodePack.Internal;

namespace Movie_Cleanup.Modules.Shell.Common
{
    /// <summary>
    /// Defines the enumeration values for a property type.
    /// </summary>
    public class ShellPropertyEnumType
    {
        #region Private Properties
        
        private string displayText = null;
        private PropEnumType? enumType;
        private object minValue = null, setValue = null, enumerationValue = null;

        private IPropertyEnumType NativePropertyEnumType
        {
            set;
            get;
        }

        #endregion

        #region Internal Constructor

        internal ShellPropertyEnumType(IPropertyEnumType nativePropertyEnumType)
        {
            NativePropertyEnumType = nativePropertyEnumType;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets display text from an enumeration information structure. 
        /// </summary>
        public string DisplayText
        {
            get
            {
                if (displayText == null)
                {
                    NativePropertyEnumType.GetDisplayText(out displayText);
                }
                return displayText;
            }
        }

        /// <summary>
        /// Gets an enumeration type from an enumeration information structure. 
        /// </summary>
        public PropEnumType EnumType
        {
            get
            {
                if (!enumType.HasValue)
                {
                    PropEnumType tempEnumType;
                    NativePropertyEnumType.GetEnumType(out tempEnumType);
                    enumType = tempEnumType;
                }
                return enumType.Value;
            }
        }

        /// <summary>
        /// Gets a minimum value from an enumeration information structure. 
        /// </summary>
        public object RangeMinValue
        {
            get
            {
                if (minValue == null)
                {
                    PropVariant propVar;
                    NativePropertyEnumType.GetRangeMinValue(out propVar);
                    minValue = propVar.Value;
                }
                return minValue;

            }
        }

        /// <summary>
        /// Gets a set value from an enumeration information structure. 
        /// </summary>
        public object RangeSetValue
        {
            get
            {
                if (setValue == null)
                {
                    PropVariant propVar;
                    NativePropertyEnumType.GetRangeSetValue(out propVar);
                    setValue = propVar.Value;
                }
                return setValue;

            }
        }

        /// <summary>
        /// Gets a value from an enumeration information structure. 
        /// </summary>
        public object RangeValue
        {
            get
            {
                if (enumerationValue == null)
                {
                    PropVariant propVar;
                    NativePropertyEnumType.GetValue(out propVar);
                    enumerationValue = propVar.Value;
                }
                return enumerationValue;

            }
        }

        #endregion
    }
}
