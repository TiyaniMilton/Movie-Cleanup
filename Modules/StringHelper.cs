﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Movie_Cleanup.Modules
{
    public static class StringHelper
    {
        public static string FixWordCasing(this string fullString)
        {
            string results = string.Empty;
            if (!string.IsNullOrEmpty(fullString))
            {
                TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
                results = myTI.ToTitleCase(fullString);
                results = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(fullString.ToLower());
                results = fullString.ToTitleCase(TitleCase.All);
            }
            return results.TrimEnd(' ');
        }
        private static CultureInfo ci = new CultureInfo("en-US");
        //Convert all first latter
        public static string ToTitleCase(this string str)
        {
            str = str.ToLower();
            var strArray = str.Split(' ');
            if (strArray.Length > 1)
            {
                strArray[0] = ci.TextInfo.ToTitleCase(strArray[0]);
                return string.Join(" ", strArray);
            }
            return ci.TextInfo.ToTitleCase(str);
        }
        public static string ToTitleCase(this string str, TitleCase tcase)
        {
            str = str.ToLower();
            switch (tcase)
            {
                case TitleCase.First:
                    var strArray = str.Split(' ');
                    if (strArray.Length > 1)
                    {
                        strArray[0] = ci.TextInfo.ToTitleCase(strArray[0]);
                        return string.Join(" ", strArray);
                    }
                    break;
                case TitleCase.All:
                    return ci.TextInfo.ToTitleCase(str);
                default:
                    break;
            }
            return ci.TextInfo.ToTitleCase(str);
        }
    }
    
    public enum TitleCase
    {
        First,
        All
    }
}
