using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media.Imaging;
using Movie_Cleanup.Modules.MediaFile;
using System.Collections.ObjectModel;
using Movie_Cleanup.Controls;
using WMPLib;

namespace Movie_Cleanup.Modules
{
    public class PlaylistManager
    {
        public static void LoadPlaylistOnComputer(string[] array1, ref List<PlaylistControl> playlistControls)
        {
            for (int i = 0; i < array1.Length; i++)
            {
                array1[i] = System.IO.Path.GetFileName(array1[i]);

                PlaylistControl item = new PlaylistControl()
                {
                    DataContext = new Playlist
                    {
                        PlaylistArt = imageFilePath + "\\Playlist.png",
                        PlaylistFileName = array1[i],
                        SelectedIndex = i,
                        PlaylistName = System.IO.Path.GetFileNameWithoutExtension(array1[i])
                    }
                };

                playlistControls.Add(item);
            }
        }


        public static ObservableCollection<Song> LoadPlaylist(List<string> FileNames)
        {
            return buildPlaylist(FileNames).Songs;
        }
        public static Playlist LoadPlaylist(string FileName)
        {
            //Lista.Rows.Clear();
            //string[] fileNamesList;
            List<string> files = new List<string>();
            if (FileName.Substring(FileName.LastIndexOf(".")) == ".m3u")
            {
                string fileName = null;
                FileStream fStream = null;
                StreamReader sReader = null;
                try
                {
                    fStream = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                    sReader = new StreamReader(fStream);
                    while ((fileName = sReader.ReadLine()) != null)
                    {
                        if (fileName.Length > 0 && fileName.Substring(0, 1) != "#" && fileName.Substring(0, 1) != "\n") //Checks whether the first character of the line is not # or Enter 
                        {
                            string[] row1 = { "false", fileName.Substring(fileName.LastIndexOf("\\") + 1), fileName.Substring(0, fileName.LastIndexOf("\\")) }; //Stores the song details in string array so that it can be added to the Grid 
                            //Lista.Rows.Add(row1);
                            files.Add(fileName);
                        }
                    }
                    fStream.Close();
                    sReader.Close();
                }
                catch (Exception ex)
                {
                    errLog = ex.Message;
                    if (fStream != null)
                        fStream.Close();
                    if (sReader != null)
                        sReader.Close();
                }
            }
            return buildPlaylist(files);
        }

        private static string errLog { get; set; }

        private static Playlist buildPlaylist(List<string> fileNames)
        {
            //string[] fileNames = App.OnLoadArgs;

            if (fileNames.Count() <= 0)
            {
                return new Playlist();
            }
            playList = new Playlist();
            //List<string> files = new List<string>();
            //foreach (var item in fileNames)
            //{
            //    DirScan dirScan = new DirScan();
            //    string directoryPath = item;
            //    //string[] filePaths = dirScan.Browse(dirBrowser.DirectoryPath);

            //    string[] tempfileNames = dirScan.Browse(directoryPath);
            //    files.AddRange(tempfileNames);
            //}
            //if (files.Count > 0)
            //{
            //    fileNames = files.ToArray();
            //}
            //if (fileNames.Length == 1)
            //{                
            //    DirScan dirScan = new DirScan();
            //    string directoryPath = e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop, true) as string;
            //    //string[] filePaths = dirScan.Browse(dirBrowser.DirectoryPath);

            //    fileNames = dirScan.Browse(directoryPath);
            //}

            string currentAblum = string.Empty;
            int pathCount = fileNames.Count();
            List<Song> listOfSongs = new List<Song>();

            for (int i = 0; i < pathCount; i++)
            {
                string currentFilePath = fileNames[i];

                Movie_Cleanup.Modules.MediaFile.Song newMusicLibraryItem = new Movie_Cleanup.Modules.MediaFile.Song();
                //lblProcess.Content = "Adding File: " + currentFilePath;

                MediaInfo mediaInfo = new MediaInfo(currentFilePath);

                if (mediaInfo.Tags == null)
                {
                    continue;
                }
                string unknownAlbum = imageFilePath + @"\Unknown Album.png";
                string unknownArtist = imageFilePath + @"\Unknown Artist.PNG";
                //string albumId = Guid.NewGuid().ToString();
                string albumId = (mediaInfo.Tags.Artist + "_" + mediaInfo.Tags.Album).FixWordCasing();
                if (mediaInfo.Tags.AlbumArt != null)
                {
                    unknownAlbum = ImageConverter.SaveAlbumArt(mediaInfo.Tags.AlbumArt, albumArtworkFilePath, albumId);
                    newMusicLibraryItem.SongArt = unknownAlbum;
                    unknownArtist = unknownAlbum;
                }
                else
                {
                    unknownArtistArt = new BitmapImage(new Uri(imageFilePath + @"\Unknown Artist.PNG"));
                    newMusicLibraryItem.SongArt = unknownAlbum;
                }
                newMusicLibraryItem.FileName = System.IO.Path.GetFileName(currentFilePath);
                newMusicLibraryItem.Album = mediaInfo.Tags.Album;
                newMusicLibraryItem.AlbumArtist = mediaInfo.Tags.AlbumArtists;
                newMusicLibraryItem.Artist = mediaInfo.Tags.Artist;
                newMusicLibraryItem.Title = mediaInfo.Tags.Title;
                newMusicLibraryItem.TrackNumber = mediaInfo.Tags.Track;
                newMusicLibraryItem.Genre = mediaInfo.Tags.Genre;
                newMusicLibraryItem.Composer = mediaInfo.Tags.Composer;
                newMusicLibraryItem.Length = (mediaInfo.Tags.Length == null) ? mediaInfo.Tags.Length.ToString() : mediaInfo.Tags.Length.Value.ToString(@"hh\:mm\:ss");
                newMusicLibraryItem.Year = mediaInfo.Tags.Year;
                newMusicLibraryItem.BitRate = mediaInfo.Tags.BitRate;
                newMusicLibraryItem.Lyrics = mediaInfo.Tags.Lyrics;
                newMusicLibraryItem.BeatsPerMinute = mediaInfo.Tags.BeatsPerMinute;
                newMusicLibraryItem.SampleRate = mediaInfo.Tags.AudioSampleRate;
                newMusicLibraryItem.Disc = mediaInfo.Tags.Disc;
                newMusicLibraryItem.PlayImageButton = imageFilePath + @"\Music.png";
                //newMusicLibraryItem.AlbumArtist = ImageConverter.SaveAlbumArt(tempImage, albumArtworkFilePath, "");//imgAlbumArtSourse;
                newMusicLibraryItem.AddToPlaylistImageButton = imageFilePath + @"\Plus.png";
                newMusicLibraryItem.Location = currentFilePath;

                //listOfSongs.Add(newMusicLibraryItem);
                playList.Songs.Add(newMusicLibraryItem);
            }

            return playList;

            //lstvwPlayList.ItemsSource = playList.Songs;//listOfSongs;
            //keep a dictionary of added files
            //foreach (string f in fileNames)
            //{
            //    if (IsValidMediaItem(f))
            //        mediaItems.Add(f.Substring(f.LastIndexOf(@"\") + 1), new MediaItem(@f, 0));
            //}

            ////now add to the list
            //foreach (MediaItem mi in mediaItems.Values)
            //    lstMediaItems.Items.Add(mi);

            // Mark the event as handled, so the control's native Drop handler is not called.

        }

       


        public static BitmapImage unknownArtistArt { get; set; }

        private static string albumArtworkFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\Album Artwork\";
        private static string imageFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\Images";

        public static Playlist playList = new Playlist();

        //private WMPLib.IWMPPlaylist pList;

		

        //public WMPLib.IWMPPlaylist PlayList
        //{
        //    get
        //    {
        //        if(pList == null)
        //        {
        //            initPlayList();
        //        }
        //        return pList;
        //    }
        //}


        //private void initPlayList()
        //{
        //    IWMPPlaylistArray pListArray;

        //    // get the list by name first
        //    pListArray = player.playlistCollection.getByName("playlist");

        //    // if the name exists, use the existing playlist
        //    // otherwise, create a new one and set pList to it.
        //    if (pListArray != null && pListArray.count > 0)
        //        pList = pListArray.Item(0);
        //    else
        //        pList = player.playlistCollection.newPlaylist("playlist");
        //}

        //public void ControlPlaylist(string mediaUrl, PlayMusic player)
        //{
        //    IWMPMedia newMedia;
        //    newMedia = player.newMedia(mediaUrl);

        //    if (newMedia != null)
        //    {
        //        // access the playlist by the property name
        //        PlayList.appendItem(newMedia);
        //    }
        //}
    }


}
