using System;
using System.Net;
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
using Movie_Cleanup.Modules.MediaFile;
using System.Diagnostics;
using System.IO;

namespace Movie_Cleanup.Controls
{
    /// <summary>
    /// Interaction logic for AlbumViewControl.xaml
    /// </summary>
    public partial class AlbumViewControl : UserControl
    {

        public delegate void OnMediaChangedEventHandler(object senderObject);
        public event OnMediaChangedEventHandler OnMediaChanged;

        public delegate void OnAddToPlaylistEventHandler(object senderObject);
        public event OnAddToPlaylistEventHandler OnAddToPlaylist;

        public AlbumViewControl()
        {
            InitializeComponent();
        }

        private void OnSelectedMediaChanged(object e)
        {
            //SelectedMediaChangedEventHandler handler = OnMediaChanged;
           // if (handler != null)
            {
                OnMediaChanged.Invoke(e);
            }
        }

        //private void lstvwAlbumTracks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{            
        //   OnSelectedMediaChanged(sender);
        //}

        public void albumViewControl_OnSelectedMediaChanged(object sender)
        {
            OnMediaChangedEventHandler handler = OnMediaChanged;
            if (handler != null)
            {
                OnMediaChanged(sender);
            }
            else
            {
                OnMediaChanged.Invoke(sender);
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
         private void GetTagInfoContextMenuContextMenu_OnClick(object sender, RoutedEventArgs e)
        {
            Song song = lstvwAlbumTracks.SelectedItem as Song;
            var directoryPath = song.Location;
            //nutritionlistView.Items.Remove(lstvwAlbumTracks.SelectedItem);  // remove the selected Item 
        } 
        private void OpenContainingFolderContextMenu_OnClick(object sender, RoutedEventArgs e)
        {
            Song song = lstvwAlbumTracks.SelectedItem as Song;
            if (song != null)
            {
                var directoryPath = new FileInfo(song.Location).DirectoryName;
                Process.Start(directoryPath);
            }
            //nutritionlistView.Items.Remove(lstvwAlbumTracks.SelectedItem);  // remove the selected Item 
        }

        private void imgAddToPlaylist_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Song song = this.Tag as Song;
            song = lstvwAlbumTracks.SelectedItem as Song;
            if (song != null)
            {
                OnAddToPlaylistEventHandler handler = OnAddToPlaylist;
                if (handler != null)
                {
                    OnAddToPlaylist(song);
                }
                else
                {
                    OnAddToPlaylist.Invoke(song);
                    //m.PlaySelectedFromLibrary();
                }
            }
        }

    }
}
