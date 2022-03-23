using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Movie_Cleanup.Modules;

namespace Movie_Cleanup.Controls
{
    /// <summary>
    /// Interaction logic for UsbDeviceControl.xaml
    /// </summary>
    public partial class UsbDeviceControl : UserControl
    {
        public UsbDeviceControl()
        {
            InitializeComponent();
            string imageFilePath = AppDomain.CurrentDomain.BaseDirectory + @"Images\";
            ImageSource imgDeviceImageSource = new BitmapImage(new Uri(imageFilePath + "iPhone.png"));
            imgDeviceImage.Source = imgDeviceImageSource;
            //lblDriveName.Content = ((RemovableDrivesManagerEventArgs)DataContext).DriveName;
            //lblDriveName.Content =;
        }
    }
}
