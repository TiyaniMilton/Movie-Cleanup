﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace Movie_Cleanup.Modules
{
    class PanelMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double)
            {
                double width = (double)value;
                Thickness panelMarginThickness = new Thickness(0, 0, width * -1, 0);
                return panelMarginThickness;
            }

            throw new NotImplementedException();

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
