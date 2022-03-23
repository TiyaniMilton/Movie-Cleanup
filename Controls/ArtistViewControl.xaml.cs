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

namespace Movie_Cleanup.Controls
{
    /// <summary>
    /// Interaction logic for ArtistViewControl.xaml
    /// </summary>
    public partial class ArtistViewControl : UserControl
    {
        //public AlbumViewControl albumViewControl = new AlbumViewControl();

        //public delegate void SelectedMediaChangedEventHandler(object senderObject);
        //public event SelectedMediaChangedEventHandler OnMediaChanged;

        public delegate void OnMediaChangedEventHandler(object senderObject);
        //public event OnMediaChangedEventHandler OnMediaChanged;
        public event OnMediaChangedEventHandler OnMediaChangedEvent;

        //public MainWindow m;// = new MainWindow();

        public ArtistViewControl()
        {
            InitializeComponent();
            //m = new MainWindow();
            //albumViewControl.OnMediaChanged += new AlbumViewControl.OnMediaChangedEventHandler(albumViewControl_OnSelectedMediaChanged);
        }

        public void albumViewControl_OnSelectedMediaChanged(object sender)
        {
            OnMediaChangedEventHandler handler = OnMediaChangedEvent;
            if (handler != null)
            {
                OnMediaChangedEvent(sender);
            }
            else
            {
                //m.PlaySelectedFromLibrary();
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }


        private void lstvwAlbumTracks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            albumViewControl_OnSelectedMediaChanged(sender);
        }

        internal void SetMaterControl(ref MainWindow mainWindow)
        {
           // m = mainWindow;
        }

        
        internal void SetAlbumBinding(Modules.MediaFile.Album alb)
        {
            AlbumViewControl albumControl = new AlbumViewControl();
            albumControl.DataContext = alb;
            albumControl.OnMediaChanged += new AlbumViewControl.OnMediaChangedEventHandler(albumControl_OnMediaChanged);
            lstAlbum.Items.Add(albumControl);
        }
        public void albumControl_OnMediaChanged(object sender)
        {
            OnMediaChangedEventHandler handler = OnMediaChangedEvent;
            if (handler != null)
            {
                OnMediaChangedEvent(sender);
            }
        }
    }
}
