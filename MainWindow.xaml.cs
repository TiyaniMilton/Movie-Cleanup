#region references

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
using System.Media;
using Movie_Cleanup.Modules;
using System.Threading;
using System.Timers;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.IO;
using Movie_Cleanup.Controls;
using System.Xml.Serialization;
using System.Windows.Forms;
//using NDatabase;
using System.Data;
using System.Windows.Media.Animation;
//using Managed.Adb;
using Movie_Cleanup.Modules.Shell;
using System.Diagnostics;
using System.ComponentModel;
using Movie_Cleanup.Modules.MediaFile;
using Microsoft.Win32;
using Movie_Cleanup.Modules.Shell.Interop.TaskBar;
using WpfAnimatedGif;
using System.Globalization;
using System.Drawing;

#endregion references



namespace Movie_Cleanup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //System.Windows.Threading.DispatcherTimer t = new System.Windows.Threading.DispatcherTimer();
//dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
//dispatcherTimer.Interval = new TimeSpan(0,0,1);
        //dispatcherTimer.Start();

        #region public attributes

        public const Int32 WM_SYSCOMMAND = 0x112;
        public const Int32 MF_SEPARATOR = 0x800;
        public const Int32 MF_STRING = 0x0;
        public const Int32 IDM_ABOUT = 5000;

        #endregion 

        #region private attributes

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(
            IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        private static extern bool AppendMenu(
            IntPtr hMenu, Int32 wFlags, Int32 wIDNewItem,
            string lpNewItem);

        //ThreadStart tsMediaLib;
        //Thread libThread;
        private System.Windows.Forms.PictureBox albumart;
        private DispatcherTimer timer = new DispatcherTimer();
        //DispatcherTimer usbTimer = new DispatcherTimer();
       // DispatcherTimer libraryAddItemsTimer = new DispatcherTimer();
        private ObservableCollection<Movie_Cleanup.Modules.MediaFile.Song> musicLib = new ObservableCollection<Movie_Cleanup.Modules.MediaFile.Song>();
        private ArtistViewControl artistViewControl = new ArtistViewControl();
        private Playlist playList = new Playlist();
        private string albumArtworkFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\Album Artwork\";
        private string imageFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\Images";
        private string applicationPath = AppDomain.CurrentDomain.BaseDirectory + AppDomain.CurrentDomain.FriendlyName;// @"\\Movie Cleanup.exe";
        //public string libraryFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\Quin Media Library.xml";
        private ImageSource unknownArtistArt;
        private ThumbnailToolbarButton buttonPlayPause;
        private ThumbnailToolbarButton buttonNext;
        private ThumbnailToolbarButton buttonPrevious;
        private TaskbarManager tbManager = TaskbarManager.Instance;

        private List<ArtistAlbumViewControl> ArtistAlbumViewItems = new List<ArtistAlbumViewControl>();
        private List<AlbumControl> AlbumList = new List<AlbumControl>();
        private ObservableCollection<MusicLibrary> MusicLibraryItems = new ObservableCollection<MusicLibrary>();
        private List<ArtistViewControl> LibraryItems = new List<ArtistViewControl>();
        private List<VideoControl> VideoLibraryItems = new List<VideoControl>();
        
        //List<string> Genres = new List<string>();
        private string[] filePaths;
        //Movie_Cleanup.Modules.MediaFile.Songs MediaLibraryFiles = new Movie_Cleanup.Modules.MediaFile.Songs();
        //Movie_Cleanup.Modules.MediaFile.MediaLibrary MediaLibrary = new Movie_Cleanup.Modules.MediaFile.MediaLibrary();

        private RemovableDrivesManager driveDetector = null;

        //private const int WM_SYSCOMMAND = 112;//0×112;
        private HwndSource hwndSource;

        private MediaLibraryManager musicLibMan = new MediaLibraryManager();

        private SQLiteDatabase mysqlDatabase;//  = new SQLiteDatabase("QuinMediaLibrary.qbd");
        //private Managed.Adb. useMadBee = new ();
        //private Managed.Adb.AndroidDebugBridge BridgeContainer;// = new AndroidDebugBridge();

        private string DefaultMusicFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        //private System.Windows.Controls.Image imageControl = null;

        private string CurrentPlayingSongAlbumArt;// = imageFilePath + "\\Unknown Album.png";
        //private ImageAnimationController controller = null;//

        #endregion 

        public MainWindow()
        {
            
            InitializeComponent();
       
            ImageSource imageControlSource = new BitmapImage(new Uri(imageFilePath + "\\animated.gif"));
            ImageBehavior.SetAnimatedSource(imageControl,imageControlSource);
            ImageBehavior.SetRepeatBehavior(imageControl, new RepeatBehavior(TimeSpan.FromSeconds(0)));
            
            Style itemContainerStyle = new Style(typeof(System.Windows.Controls.ListViewItem));
            itemContainerStyle.Setters.Add(new Setter(System.Windows.Controls.ListViewItem.AllowDropProperty, true));
            itemContainerStyle.Setters.Add(new EventSetter(System.Windows.Controls.ListViewItem.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(s_PreviewMouseLeftButtonDown)));
            itemContainerStyle.Setters.Add(new EventSetter(System.Windows.Controls.ListViewItem.DropEvent, new System.Windows.Forms.DragEventHandler(lstvwPlayList_Drop)));
            //lstvwPlayList.ItemContainerStyle = itemContainerStyle;


            mysqlDatabase = new SQLiteDatabase("QuinMediaLibrary.qdb");
            
            //MediaLibrary.Music = new List<Modules.MediaFile.Artist>();
            //MediaLibrary.Playlists = new List<Playlist>();
            //MediaLibrary.Books = new Modules.MediaFile.Books();
            //MediaLibrary.Videos = new List<Modules.MediaFile.Video>();
            //BridgeContainer = Managed.Adb.AndroidDebugBridge.Bridge;
            //if (useMadBee())
            //{
            //BridgeContainer.manager.adb.
            //    DeviceChanged += new EventHandler<DeviceEventArgs>(beeDeviceChanged);
            //BridgeContainer.manager.adb.
                //DeviceConnected += new EventHandler<DeviceEventArgs>(beeDeviceConnected);
            //BridgeContainer.manager.adb.
            //    DeviceDisconnected += new EventHandler<DeviceEventArgs>(
            //        beeDeviceDisconnected);
            //}
            //BridgeContainer = Managed.Adb
            //BridgeContainer = (Managed.Adb.AndroidDebugBridge.Bridge).CreateBridge();
            //BridgeContainer.DeviceChanged += new EventHandler<DeviceEventArgs>(beeDeviceChanged);
            //BridgeContainer.DeviceConnected += new EventHandler<DeviceEventArgs>(beeDeviceConnected);
            //BridgeContainer.Start();
            //BridgeContainer.DeviceDisconnected += new EventHandler<DeviceEventArgs>(beeDeviceDisconnected);
           
            //spectrumAnalyzer.RegisterSoundPlayer(PlayMusic.Instance);
            
            DefaultInitializations();

            //spectrumAnalyzer.BarSpacing = 5;
            //spectrumAnalyzer.AveragePeaks = false;
            //spectrumAnalyzer.MinimumFrequency = 20;
            //spectrumAnalyzer.MaximumFrequency = 20000;
            //spectrumAnalyzer.BarCount = 32;
            //spectrumAnalyzer.PeakFallDelay = 10;
            //spectrumAnalyzer.BarHeightScaling = BarHeightScaling.Decibel;

           /// spectrumAnalyzer.

            

            List<PlaylistControl> playlistControls = new List<PlaylistControl>();
            string[] array1 = Directory.GetFiles(DefaultMusicFolder + @"\\Playlists", "*.wpl");
            PlaylistManager.LoadPlaylistOnComputer(array1, ref playlistControls);
            array1 = Directory.GetFiles(DefaultMusicFolder + @"\\Playlists", "*.m3u");
            PlaylistManager.LoadPlaylistOnComputer(array1, ref playlistControls);                        
            lstbxPlayLists.ItemsSource = playlistControls;

            LoadUsbDevice();
            LoadMediaLibrary();


                //double width = lstvwAllLibraryView.Width;
                //GridView gv = lstvwAllLibraryView.View as GridView;
                //int gridViewColumns = gv.Columns.Count;
            //for (int i = 0; i < gv.Columns.Count; i++)
            //{
            //    if (!Double.IsNaN(gv.Columns[i].Width))
            //        width -= gv.Columns[i].Width;
            //}
            
            driveDetector = new RemovableDrivesManager();
            driveDetector.DeviceArrived += new RemovableDrivesManagerEventHandler(OnDriveArrived);
            driveDetector.DeviceRemoved += new RemovableDrivesManagerEventHandler(OnDriveRemoved);
            driveDetector.QueryRemove += new RemovableDrivesManagerEventHandler(OnQueryRemove);            
            
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += timer_Tick;
                        
            _dispathcer =  Dispatcher.CurrentDispatcher;
                          
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close,
                       new ExecutedRoutedEventHandler(delegate(object sender, ExecutedRoutedEventArgs args) { this.Close(); })));
            // Insert code required on object creation below this point.
            this.SourceInitialized += new EventHandler(InitializeWindowSource);

            //lstvwLibraryView.DragOver += new DragStartedEventArgs(listView1_ItemDrag);
            //lstvwLibraryView.DragEnter += new DragEventHandler(treeView1_DragEnter);
            //lstvwPlayList.Drop += new DragEventHandler(treeView1_DragDrop);
            //lstvwLibraryView.ItemDrag += new ItemDragEventHandler(listView1_ItemDrag);
            //lstvwLibraryView.DragEnter += new DragEventHandler(treeView1_DragEnter);
            //treeView1.DragDrop += new DragEventHandler(treeView1_DragDrop);
        
            artistViewControl.OnMediaChangedEvent += new ArtistViewControl.OnMediaChangedEventHandler(albumViewControl_OnSelectedMediaChanged);
            albvwcntrlAlbumDetails.OnMediaChanged += new AlbumViewControl.OnMediaChangedEventHandler(albumViewControl_OnSelectedMediaChanged);
            albvwcntrlAlbumDetails.OnAddToPlaylist += new AlbumViewControl.OnAddToPlaylistEventHandler(albumViewControl_OnAddToPlaylist);

            Media_Drop();
        }

        //private void createVideoControl()
        //{
        //    System.Windows.Forms.Integration.WindowsFormsHost host =
        //new System.Windows.Forms.Integration.WindowsFormsHost();
        //}

        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            String[] arguments = Environment.GetCommandLineArgs();

            buttonPlayPause = new ThumbnailToolbarButton
            (Properties.Resources.Play, "Play");
            buttonPlayPause.Enabled = true;
            buttonPlayPause.Click += new EventHandler<ThumbnailButtonClickedEventArgs>(buttonPlay_Click);// new EventHandler(buttonPlay_Click);

            buttonNext = new ThumbnailToolbarButton
            (Properties.Resources.Next, "Next");
            buttonNext.Enabled = true;
            buttonNext.Click += new EventHandler<ThumbnailButtonClickedEventArgs>(buttonNext_Click);//new EventHandler(buttonNext_Click);//

            buttonPrevious = new ThumbnailToolbarButton(Properties.Resources.Previous, "Previous");
            buttonPrevious.Click += new EventHandler<ThumbnailButtonClickedEventArgs>(buttonPrevious_Click); //new EventHandler(buttonPrevious_Click);//
            //TaskbarManager.Instance.ThumbnailToolBars.AddButtons(this.Handle, buttonPrevious, buttonPlayPause, buttonNext);
            //TaskbarManager.Instance.TabbedThumbnail.SetThumbnailClip(this.Handle, new Rectangle(albumart.Location, albumart.Size));

            

            ThumbnailToolbarButton[] thumbBtn = new ThumbnailToolbarButton[3];
            thumbBtn[0] = buttonPrevious;
            thumbBtn[1] = buttonPlayPause;
            thumbBtn[2] = buttonNext;
            IntPtr windowHandle = new WindowInteropHelper(this).Handle;
            windowHandle = Process.GetCurrentProcess().MainWindowHandle;
            TaskbarManager.Instance.ThumbnailToolbars.AddButtons(windowHandle, thumbBtn);

            //AddContextMenuItem(".mp3", "MuziQBase Player", "Play with &MuziQBase Player", "\"" + System.Windows.Forms.Application.ExecutablePath + "\" \"%1\"");
            IntPtr sysMenuHandle = GetSystemMenu(windowHandle, false);
            AppendMenu(sysMenuHandle, MF_SEPARATOR, 0, string.Empty);
            AppendMenu(sysMenuHandle, MF_STRING, IDM_ABOUT, strAbout);

            //Media_Drop();

            //System.Drawing.Rectangle newRectangle = new System.Drawing.Rectangle();
            //newRectangle.Width = albumart.Size.Width - 1;
            //newRectangle.Height = albumart.Size.Height - 4;
            //newRectangle.Location = new System.Drawing.Point(albumart.Location.X + 4, albumart.Location.Y);
            //newRectangle.Clip = albumart.Size.Height - 4;
                //albumart.Location.X + 4,
        //albumart.Location.Y, albumart.Size.Width - 1, albumart.Size.Height - 4)

            //TaskbarManager.Instance.TabbedThumbnail.SetThumbnailClip(windowHandle, new System.Drawing.Rectangle(new System.Drawing.Point(), new System.Drawing.Size(100,100)));
            //TaskbarManager.Instance.TabbedThumbnail.SetThumbnailClip(this, new Rectangle());// albumart.Location, albumart.Size));

            //TaskbarManager.Instance.ThumbnailToolbars.AddButtons(this, buttonPrevious, buttonPlayPause, buttonNext);
            //TaskbarManager.Instance.TabbedThumbnail.SetThumbnailClip(windowHandle, newRectangle);
            //TaskbarManager.Instance.TabbedThumbnail.SetThumbnailClip(windowHandle, new System.Drawing.Rectangle(albumart.Location.X + 4, albumart.Location.Y, albumart.Size.Width - 1, albumart.Size.Height - 4));
        }
        private static string strAbout = "About MuziQBase Player...";
        
        public string FixWordCasing(string fullString)
        {
            string results = string.Empty;
            if (!string.IsNullOrEmpty(fullString))
            {
                TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
                results = myTI.ToTitleCase( fullString );
                results = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(fullString.ToLower());
                results = fullString.ToTitleCase(TitleCase.All);
            }
            return results.TrimEnd(' ');
        }

        private void DefaultInitializations()
        {
            //imgSave.Source = ImageConverter.ToWpfImage((System.Drawing.Image)Properties.Resources.SavePlayList);
            //imgLoad.Source = ImageConverter.ToWpfImage((System.Drawing.Image)Properties.Resources.Folder_black_128);
            //imgClear.Source = ImageConverter.ToWpfImage((System.Drawing.Image)Properties.Resources.Close);
            //img3D.Source = ImageConverter.ToWpfImage((System.Drawing.Image)Properties.Resources.HDD__21);
            //imgNormal.Source = ImageConverter.ToWpfImage(Properties.Resources.SavePlayList);


            CurrentPlayingSongAlbumArt = imageFilePath + "\\Unknown Album.png";
            tbAlbums.Visibility = System.Windows.Visibility.Visible;
            tbArtists.Visibility = System.Windows.Visibility.Visible;
            tbVideos.Visibility = System.Windows.Visibility. Collapsed;
            tbPlaylist.Visibility = System.Windows.Visibility.Collapsed;

            imgMusicCategory.Source = new BitmapImage(new Uri(imageFilePath + "\\Music.png"));
            imgVideoCategory.Source = new BitmapImage(new Uri(imageFilePath + "\\Video.png"));
            //imgCameraCategory.Source = new BitmapImage(new Uri(imageFilePath + "Camera.png"));
            imgKnowledgeCategory.Source = new BitmapImage(new Uri(imageFilePath + "\\Knowledge.png"));
            //imgSaveCategory.Source = new BitmapImage(new Uri(imageFilePath + "\\Save.png"));
            imgAppCategory.Source = new BitmapImage(new Uri(imageFilePath + "\\App space.png"));
            imgDownloadCategory.Source = new BitmapImage(new Uri(imageFilePath + "\\Downloads.png"));
            imgBookCategory.Source = new BitmapImage(new Uri(imageFilePath + "\\Book.png"));
            //imgAddPlaylistCategory.Source = new BitmapImage(new Uri(imageFilePath + "Add Playlist.png"));
            imgAddPlaylist.Source = new BitmapImage(new Uri(imageFilePath + "\\Add Playlist.png"));
            //imgExportPlaylist.Source = new BitmapImage(new Uri(imageFilePath + "\\Video Folder.png"));

            unknownArtistArt = new BitmapImage(new Uri(imageFilePath + "\\Unknown Artist.PNG"));

            MainWindow window = this;

            musicplayer = new PlayMusic(ref window);
            sldrVolume.Value = 50;

            ImageSource imgSourse = new BitmapImage(new Uri(imageFilePath + "\\Button_Shuffle.png"));
            imgShuffle.Source = imgSourse;
            lblElapsed.Content = "00:00";
            lblDuration.Content = "00:00";

            

            //AddAppInOpenWithListForExt(applicationPath, "Movie Cleanup.exe", ".mp3");
            //AddContextMenuItem(".mp3", "MuziQBase Player", "Play with &MuziQBase Player", "\"" + System.Windows.Forms.Application.ExecutablePath + "\" \"%1\"");
            //IntPtr sysMenuHandle = GetSystemMenu(Handle, false);
            //AppendMenu(sysMenuHandle, MF_SEPARATOR, 0, string.Empty);
            //AppendMenu(sysMenuHandle, MF_STRING, IDM_ABOUT, strAbout);
        }

        

        public static void AddAppInOpenWithListForExt(String appPath, String appName, String extention)
        {
            // First create your app entry in the CLASS_ROOT\Applications node
            RegistryKey key = Registry.ClassesRoot.OpenSubKey("Applications", true);
            key = key.CreateSubKey(appName + @"\shell\open\command", RegistryKeyPermissionCheck.ReadWriteSubTree);
            key.SetValue("", String.Format("\"{0}\"", appPath) + " \"%1\"", RegistryValueKind.String);

            // Now put an entry for your application inside the CLASS_ROOT\[the extention you want for]
            key = Registry.ClassesRoot.OpenSubKey(extention, true);
            key = key.CreateSubKey(String.Format("OpenWithList\\{0}", appName), RegistryKeyPermissionCheck.ReadWriteSubTree);
        }
        //Extension - Extension of the file (.zip, .txt etc.)
        //MenuName - Name for the menu item (Play, Open etc.)
        //MenuDescription - The actual text that will be shown
        //MenuCommand - Path to executable
        private bool AddContextMenuItem(string Extension, string MenuName, string MenuDescription, string MenuCommand)
        {
            bool ret = false;
            RegistryKey rkey =
              Registry.ClassesRoot.OpenSubKey(Extension);
            if (rkey != null)
            {
                string extstring = rkey.GetValue("").ToString();
                rkey.Close();
                if (extstring != null)
                {
                    if (extstring.Length > 0)
                    {
                        try
                        {
                            rkey = Registry.ClassesRoot.OpenSubKey(
                              extstring, true);
                            if (rkey != null)
                            {
                                string strkey = "shell\\" + MenuName + "\\command";
                                RegistryKey subky = rkey.CreateSubKey(strkey);
                                if (subky != null)
                                {
                                    subky.SetValue("", MenuCommand);
                                    subky.Close();
                                    subky = rkey.OpenSubKey("shell\\" +
                                      MenuName, true);
                                    if (subky != null)
                                    {
                                        subky.SetValue("", MenuDescription);
                                        subky.Close();
                                    }
                                    ret = true;
                                }
                                rkey.Close();
                            }
                        }
                        catch
                        {
                            System.Windows.MessageBox.Show("Access Denied on the Registry :(");
                        }
                    }
                }
            }
            return ret;
        }

        //public static bool ProcessCommand(string[] args)
        //{
             
        //    if (args.Length == 0 || string.Compare(args[0], "-register", true) == 0)
        //    {
                 
        //        string menuCommand = string.Format("\"{0}\" \"%L\"", System.Windows.Application.ExecutablePath);                
 
        //        //frmMain.Register(Program.FileType, Program.KeyName, Program.MenuText, menuCommand);                
                 
        //        AddContextMenuItem(FileType, KeyName, MenuText, menuCommand);
                 
        //        MessageBox.Show(string.Format("The {0} shell extension was registered.", Program.KeyName), Program.KeyName);
 
        //        return true;
        //    }          
             
        //    return false;
        //}

        public void Media_Drop()
        {
            string[] fileNames = App.OnLoadArgs;

            if (fileNames == null)
            {
                return;
            }
            List<string> files = new List<string>();
            foreach (var item in fileNames)
            {
                DirScan dirScan = new DirScan();
                string directoryPath = item;
                //string[] filePaths = dirScan.Browse(dirBrowser.DirectoryPath);

                string[] tempfileNames = dirScan.Browse(directoryPath);
                files.AddRange(tempfileNames);
            }
            if (files.Count > 0)
            {
                fileNames = files.ToArray();
            }
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
                lblProcess.Content = "Adding File: " + currentFilePath;

                MediaInfo mediaInfo = new MediaInfo(currentFilePath);

                if (mediaInfo.Tags == null)
                {
                    continue;
                }
                string unknownAlbum = imageFilePath + @"\Unknown Album.png";
                string unknownArtist = imageFilePath + @"\Unknown Artist.PNG";
                //string albumId = Guid.NewGuid().ToString();
                string albumId = FixWordCasing(mediaInfo.Tags.Artist + "_" + mediaInfo.Tags.Album);
                if (mediaInfo.Tags.AlbumArt != null)
                {
                    unknownAlbum = Movie_Cleanup.Modules.ImageConverter.SaveAlbumArt(mediaInfo.Tags.AlbumArt, albumArtworkFilePath, albumId);
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


            lstvwPlayList.ItemsSource = playList.Songs;//listOfSongs;
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

        
        /// <summary>
        /// Handles Drop Event for Media Items.
        /// </summary>
        private void Media_Drop(object sender, System.Windows.DragEventArgs e)
        {
            string[] fileNames = e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop, true) as string[];
            List<string> files = new List<string>();
            foreach (var item in fileNames)
            {
                DirScan dirScan = new DirScan();
                string directoryPath = item;
                //string[] filePaths = dirScan.Browse(dirBrowser.DirectoryPath);

                string[] tempfileNames = dirScan.Browse(directoryPath);
                files.AddRange(tempfileNames);
            }
            if (files.Count > 0)
            {
                fileNames = files.ToArray();
            }
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
                lblProcess.Content = "Adding File: " + currentFilePath;

                MediaInfo mediaInfo = new MediaInfo(currentFilePath);

                if (mediaInfo.Tags == null)
                {
                    continue;
                }
                string unknownAlbum = imageFilePath + @"\Unknown Album.png";
                string unknownArtist = imageFilePath + @"\Unknown Artist.PNG";
                //string albumId = Guid.NewGuid().ToString();
                string albumId = FixWordCasing(mediaInfo.Tags.Artist + "_" + mediaInfo.Tags.Album);
                if (mediaInfo.Tags.AlbumArt != null)
                {
                    unknownAlbum = Movie_Cleanup.Modules.ImageConverter.SaveAlbumArt(mediaInfo.Tags.AlbumArt, albumArtworkFilePath, albumId);
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

                listOfSongs.Add(newMusicLibraryItem);
                playList.Songs.Add(newMusicLibraryItem);
            }


            lstvwPlayList.ItemsSource = playList.Songs;//listOfSongs;
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
            e.Handled = true;
        }

        

        void s_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (sender is System.Windows.Controls.ListViewItem)
            {
                System.Windows.Controls.ListViewItem draggedItem = sender as System.Windows.Controls.ListViewItem;
                DragDrop.DoDragDrop(draggedItem, draggedItem.DataContext, System.Windows.DragDropEffects.Move);
                draggedItem.IsSelected = true;
            }
        }

        void lstvwPlayList_Drop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            Song droppedData = e.Data.GetData(typeof(Song)) as Song;
            Song target = ((ListBoxItem)(sender)).DataContext as Song;

            int removedIdx = lstvwPlayList.Items.IndexOf(droppedData);
            int targetIdx = lstvwPlayList.Items.IndexOf(target);

            if (removedIdx < targetIdx)
            {
                playList.Songs.Insert(targetIdx + 1, droppedData);
                playList.Songs.RemoveAt(removedIdx);
            }
            else
            {
                int remIdx = removedIdx + 1;
                if (playList.Songs.Count + 1 > remIdx)
                {
                    playList.Songs.Insert(targetIdx, droppedData);
                    playList.Songs.RemoveAt(remIdx);
                }
            }
        }

        void buttonPlay_Click(object sender, EventArgs e)
        {
            PlaySong();
        }

        public void PlaySong()
        {
            
            //string fileName = @".\Sounds\4. Mphoza - Moove (Original Mix).mp3";
//@"C:\Users\tzm0410\Music\iTunes\iTunes Media\Music\Daft Punk\Live at Coachella\05 Around The World (Harder Better Faster Stronger).mp3";
            if (musicplayer.IsPlaying)
            {
                musicplayer.Pause();
                timer.IsEnabled = false;
                timer.Stop();
                string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\Images\";
                ImageSource imgSourse = new BitmapImage(new Uri(filePath + "Play.png"));
                imgPlay.Source = imgSourse;
                SetTimerforPause();
                buttonPlayPause.Icon = Properties.Resources.Play;
                buttonPlayPause.Tooltip = "Play";
                //controller.Pause();\
                ImageBehavior.SetRepeatBehavior(imageControl, new RepeatBehavior(TimeSpan.FromSeconds(0)));
            }
            else
            {
                ImageBehavior.SetRepeatBehavior(imageControl, new RepeatBehavior(TimeSpan.FromSeconds(musicplayer.Duration)));
                //controller.Play();
                if (lstvwPlayList.Items.Count <= 0)
                {
                    playAll();
                }
                else
                {
                    playFromPlaylist(0);
                }

                buttonPlayPause.Icon = Properties.Resources.Pause;
                buttonPlayPause.Tooltip = "Pause";
                SetTimerforPlay();
                
                //musicplayer.Open(fileName);
                //musicplayer.Play(false);
                //string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\Images\";
                //ImageSource imgSourse = new BitmapImage(new Uri(filePath + "pause.png"));
                //imgPlay.Source = imgSourse;

                //ImageSource imgAlbumArtSourse = new BitmapImage(new Uri(filePath + "Unknown Album.png"));
                //imgAlbumArt.Source = imgAlbumArtSourse;
                

                //lblArtist.Content = musicplayer.MediaInfo.Tags.Artist.Replace("\0", "") + " - "+ musicplayer.MediaInfo.Tags.Title.Replace("\0", "");
                ////lblTitle.Content = musicplayer.MediaInfo.Tags.Title.Replace("\0", "");
                //lblAlbum.Content = musicplayer.MediaInfo.Tags.Album.Replace("\0", "");

                ////Ideally these things hould be done once but somehow they are coming pretty late from engine so we are doiing it here
                //seekBar.Minimum = 0;
                //seekBar.Maximum = (int)musicplayer.Duration; //Damn, double to int, i feel like killing myself
                //timer.IsEnabled = true;
                ////timer.Interval = new TimeSpan(0, 0, 1);
                ////timer.Interval = 1000;
                //timer.Start();
                //workerThreadProgress = new Thread(musicplayer.UpdatePlayProgress);
                //workerThreadProgress.Start();
            }
            //sound = new SoundPlayer(@"C:\Users\tzm0410\Music\iTunes\iTunes Media\Music\Daft Punk\Live at Coachella\05 Around The World (Harder Better Faster Stronger).mp3");
            //sound.PlayLooping();
        }

        public void PlayNextSong()
        {
            musicplayer.PlayNext();
            lstvwPlayList.SelectedIndex = musicplayer.CurrentPlayIndex;
            ImageSource imgAlbumArtSourse = new BitmapImage(new Uri(imageFilePath + @"\Unknown Album.png"));
            //imgAlbumArt.Source = imgAlbumArtSourse;
            if (musicplayer.MediaInfo.Tags.AlbumArt != null)
            {

                imgAlbumArtSourse = Movie_Cleanup.Modules.ImageConverter.ToWpfImage(musicplayer.MediaInfo.Tags.AlbumArt);
                if (imgAlbumArtSourse == null)
                {
                    imgAlbumArtSourse = new BitmapImage(new Uri(imageFilePath + @"\Unknown Album.png"));
                }
            }
            imgCurrentlyPlayingAlbumArt.Source = imgAlbumArtSourse;
            imgAlbumArt.Source = imgAlbumArtSourse;
            lblArtist.Content = musicplayer.MediaInfo.Tags.Artist.Replace("\0", "").TrimEnd(' ') + " - " + musicplayer.MediaInfo.Tags.Title.Replace("\0", "");
            lblAlbum.Content = musicplayer.MediaInfo.Tags.Album.Replace("\0", "").TrimEnd(' ');

            Movie_Cleanup.Modules.MediaFile.Song selectedPlaylistSong = lstvwPlayList.SelectedItem as Movie_Cleanup.Modules.MediaFile.Song;
            List<Modules.MediaFile.Song> listOfSongs = new List<Modules.MediaFile.Song>();

            int selectedIndex = lstvwPlayList.SelectedIndex;

            foreach (Modules.MediaFile.Song item in lstvwPlayList.Items)
            {
                if (item.Location != selectedPlaylistSong.Location)
                {
                    item.PlayImageButton = imageFilePath + @"\Music.png";
                }
                else
                {
                    item.PlayImageButton = imageFilePath + @"\playback_play.png";
                }

                listOfSongs.Add(item);
            }

            lstvwPlayList.ItemsSource = listOfSongs;
            lstvwPlayList.SelectedIndex = selectedIndex;
            currentSongIndex = musicplayer.CurrentPlayIndex;
        }
        public void PlayPreviousSong()
        {
            musicplayer.PlayPrevious();
            lstvwPlayList.SelectedIndex = musicplayer.CurrentPlayIndex;
            ImageSource imgAlbumArtSourse = new BitmapImage(new Uri(imageFilePath + @"\Unknown Album.png"));

            if (musicplayer.MediaInfo.Tags.AlbumArt != null)
            {
                imgAlbumArtSourse = Movie_Cleanup.Modules.ImageConverter.ToWpfImage(musicplayer.MediaInfo.Tags.AlbumArt);
            }
            imgCurrentlyPlayingAlbumArt.Source = imgAlbumArtSourse;
            imgAlbumArt.Source = imgAlbumArtSourse;
            lblArtist.Content = musicplayer.MediaInfo.Tags.Artist.Replace("\0", "").TrimEnd(' ') + " - " + musicplayer.MediaInfo.Tags.Title.Replace("\0", "");
            lblAlbum.Content = musicplayer.MediaInfo.Tags.Album.Replace("\0", "").TrimEnd(' ');

            Movie_Cleanup.Modules.MediaFile.Song selectedPlaylistSong = lstvwPlayList.SelectedItem as Movie_Cleanup.Modules.MediaFile.Song;
            List<Modules.MediaFile.Song> listOfSongs = new List<Modules.MediaFile.Song>();

            int selectedIndex = lstvwPlayList.SelectedIndex;

            foreach (Modules.MediaFile.Song item in lstvwPlayList.Items)
            {
                if (item.Location != selectedPlaylistSong.Location)
                {
                    item.PlayImageButton = imageFilePath + @"\Music.png";
                }
                else
                {
                    item.PlayImageButton = imageFilePath + @"\playback_play.png";
                }

                listOfSongs.Add(item);
            }

            lstvwPlayList.ItemsSource = listOfSongs;
            lstvwPlayList.SelectedIndex = selectedIndex;
            currentSongIndex = musicplayer.CurrentPlayIndex;
        }
        void buttonNext_Click(object sender, EventArgs e)
        {
            PlayNextSong();
        }

        void buttonPrevious_Click(object sender, EventArgs e)
        {
            PlayPreviousSong();
        }

        //Set progressbar Style and Enable Timer 
        private void SetTimerforPlay()
        {
            tbManager.SetProgressState(TaskbarProgressBarState.Normal);
            //progressbarTimer.Enabled = true;
        }


        private void SetTimerforPause()
        {
            tbManager.SetProgressState(TaskbarProgressBarState.Error);
            //progressbarTimer.Enabled = false;
        }


        //private void SetTaskbarthumbnail()
        //{
        //    TaskbarManager.Instance.TabbedThumbnail.SetThumbnailClip
        //(this.Handle, new Rectangle(albumart.Location.X + 4,
        //albumart.Location.Y, albumart.Size.Width - 1, albumart.Size.Height - 4));
        //}
       // public System.Drawing.Image albumart = null;
        private void SetAlbumArt(string currentlyPlayingSong)
          {
              albumart = new PictureBox();
              //albumart.Image = musicplayer.MediaInfo.Tags.AlbumArt;
              //if (playlist.SelectedItem != null)
              if (currentlyPlayingSong != null)
              {
                  TagLib.File file = TagLib.File.Create(currentlyPlayingSong);
                  if (file.Tag.Pictures.Length > 0)
                  {
                      var bin = (byte[])(file.Tag.Pictures[0].Data.Data);
                      try
                      {
                          albumart.Image = System.Drawing.Image.FromStream(new MemoryStream(bin)).GetThumbnailImage(100, 100, null, IntPtr.Zero);
                      }
                      catch
                      {
                          albumart.Image = Properties.Resources.UnknownAlbum; 
                      }
                      //albumart.Image = musicplayer.MediaInfo.Tags.AlbumArt;
                  }
                  else
                  {
                      albumart.Image = Properties.Resources.UnknownAlbum;
                  }
              }
              
          }
        private void SetTaskbarthumbnail()
        {
            IntPtr windowHandle = new WindowInteropHelper(this).Handle;
            windowHandle = Process.GetCurrentProcess().MainWindowHandle;
            TaskbarManager.Instance.TabbedThumbnail.SetThumbnailClip(windowHandle, new System.Drawing.Rectangle(albumart.Location.X + 224, albumart.Location.Y , albumart.Size.Width - 1, albumart.Size.Height - 4));
        }
        //private void SetTaskbarthumbnail()
        //  {
        //      IntPtr windowHandle = new WindowInteropHelper(this).Handle;
        //      windowHandle = Process.GetCurrentProcess().MainWindowHandle;
        //      //TaskbarManager.Instance.TabbedThumbnail.SetThumbnailClip(windowHandle, new Rectangle(albumart.Location.X + 4, albumart.Location.Y, albumart.Size.Width - 1, albumart.Size.Height - 4));
        //  }


        private void progressbarTimer_Tick(object sender, EventArgs e)
        {
            //if (timerCount <= songLength)
            //{
            //    tbManager.SetProgressValue(timerCount, songLength);
            //}
            ////lblSeconds.Text = (songLength - timerCount).ToString() + " seconds";
            //timerCount += 1;
        }

       
        List<UsbDeviceControl> UsbDrives;
        private void LoadUsbDevice()
        {
            lstbxDevices.ItemsSource = null;
            List<UsbDeviceControl> UsbDrives = new List<UsbDeviceControl>();
         
            //var drives = DriveInfo.GetDrives();

            //var removableFatDrives = drives.Where(
            //        c => c.DriveType == DriveType.Removable &&
            //        c.DriveFormat == "FAT" &&
            //        c.IsReady);

            //var andriods = from c in removableFatDrives
            //               from d in c.RootDirectory.EnumerateDirectories()
            //               where d.Name != ""//.Contains("android")
            //               select c;

            //foreach (var currentDrive in andriods)
            //{
            //    UsbDeviceControl usbDevice = new UsbDeviceControl();
            //    string driveNameValue = currentDrive.VolumeLabel;
            //    if (currentDrive.VolumeLabel == "")
            //    {
            //        driveNameValue = currentDrive.RootDirectory.Name;
            //    }
            //    usbDevice.DataContext =
            //    new UsbDrive
            //    {
            //        DriveLetter = currentDrive.RootDirectory.Name,
            //        DriveName = driveNameValue
            //    };
            //    UsbDrives.Add(usbDevice);
            //}

            //List<Device> lista = BridgeContainer.Devices.ToList();



            //foreach (Device d in lista)
            //{
            //    //AndroidDevice newDevice = new AndroidDevice();
            //    // newDevice.modelName = d.Properties["ro.build.product"].ToString();
            //    //newDevice.deviceSerialNumber = d.SerialNumber;
            //    //this.toolStripComboDevices.Items.Add(
            //    //newDevice.modelName + "(" + newDevice.deviceSerialNumber + ")");
            //    //connectedDevices.Add(newDevice);

            //    UsbDeviceControl usbDevice = new UsbDeviceControl();
            //    string driveNameValue = d.Properties["ro.build.product"].ToString();
            //    //if (currentDrive.VolumeLabel == "")
            //    //{
            //    //    driveNameValue = currentDrive.RootDirectory.Name;
            //    //}
            //    usbDevice.DataContext =
            //    new UsbDrive
            //    {
            //        DriveLetter = d.SerialNumber,
            //        DriveName = driveNameValue
            //    };
            //    UsbDrives.Add(usbDevice);
            //}

            foreach (var currentDrive in DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Removable).Select(d => d))
            {
                UsbDeviceControl usbDevice = new UsbDeviceControl();
                string driveNameValue = currentDrive.VolumeLabel;
                if (currentDrive.VolumeLabel == "")
                {
                    driveNameValue = currentDrive.RootDirectory.Name;
                }
                usbDevice.DataContext =
                new UsbDrive
                {
                    DriveLetter = currentDrive.RootDirectory.Name,
                    DriveName = driveNameValue
                };
                UsbDrives.Add(usbDevice);
            }
            
            lstbxDevices.ItemsSource = UsbDrives; 
        }

        //private Movie_Cleanup.Modules.MediaFile.Song GetMediaItem(string fileName)
        //{
        //    Movie_Cleanup.Modules.MediaFile.Song newMusicLibraryItem = new Movie_Cleanup.Modules.MediaFile.Song();
        //    MediaInfo mediaInfo = new MediaInfo(fileName);
        //    ImageSource imgAlbumArtSourse = new BitmapImage(new Uri(imageFilePath + "Unknown Album.png"));
        //    if (mediaInfo.Tags.AlbumArt != null)
        //    {
        //        imgAlbumArtSourse = ImageConverter.ToWpfBitmap(mediaInfo.Tags.AlbumArt);
        //        unknownArtistArt = imgAlbumArtSourse;
        //    }
        //    else
        //    {
        //        unknownArtistArt = new BitmapImage(new Uri(imageFilePath + "Unknown Artist.PNG"));
        //    }
        //    newMusicLibraryItem.TrackNumber = mediaInfo.Tags.Track;
        //    newMusicLibraryItem.Genre = mediaInfo.Tags.Genre;
        //    newMusicLibraryItem.Composer = mediaInfo.Tags.Composer;
        //    newMusicLibraryItem.Length = (mediaInfo.Tags.Length == null) ? mediaInfo.Tags.Length.ToString() : mediaInfo.Tags.Length.Value.ToString(@"hh\:mm\:ss");
        //    newMusicLibraryItem.Year = mediaInfo.Tags.Year;
        //    newMusicLibraryItem.Disc = mediaInfo.Tags.Disc;
        //    newMusicLibraryItem.PlayImageButton = imageFilePath + "Music.png";
        //    //newMusicLibraryItem.SongArt = imgAlbumArtSourse;
        //    newMusicLibraryItem.AddToPlaylistImageButton = imageFilePath + "Plus.png";

        //    return newMusicLibraryItem;
        //}

        private void LoadMediaLibrary()
        {
            List<Movie_Cleanup.Modules.MediaFile.Artist> Artists = new List<Movie_Cleanup.Modules.MediaFile.Artist>();
            List<Movie_Cleanup.Modules.MediaFile.Album> Albums = new List<Movie_Cleanup.Modules.MediaFile.Album>();
            List<Movie_Cleanup.Modules.MediaFile.Video> Videos = new List<Movie_Cleanup.Modules.MediaFile.Video>();
            
            DataTable dtArtist = mysqlDatabase.GetDataTable("Select  * from Artist");
            DataTable dtVideo = mysqlDatabase.GetDataTable("Select  * from Video");

            int recordRecCount = dtVideo.Rows.Count;
            //foreach (DataRow artistRow in dtArtist.Rows)
            for (int i = 0; i < recordRecCount; i++)
            {
                DataRow videoRow = dtVideo.Rows[i];
                string videoId = videoRow["VideoId"].ToString();
                Modules.MediaFile.Video newVideo = new Video()
                {
                    Cast = videoRow["Cast"].ToString(),
                    Category = videoRow["Category"].ToString(),
                    Comments = videoRow["Comments"].ToString(),
                    Description = videoRow["Description"].ToString(),
                    Plot = videoRow["Plot"].ToString(),
                    Rating = videoRow["Rating"].ToString(),
                    VideoArt = videoRow["VideoArt"].ToString(),
                    VideoId = videoId,
                    VideoName = videoRow["VideoName"].ToString(),
                    VideoPath = videoRow["VideoPath"].ToString(),
                    Year = videoRow["Year"].ToString()
                };

                Videos.Add(newVideo);
            }

            int artistRecCount = dtArtist.Rows.Count;
            //foreach (DataRow artistRow in dtArtist.Rows)
            for (int i = 0; i < artistRecCount; i++ )
            {
                DataRow artistRow = dtArtist.Rows[i];
                string artistId = artistRow["ArtistId"].ToString();
                Modules.MediaFile.Artist newArtist = MapDataTableToArtist(artistRow);

                DataTable dtAlbum = mysqlDatabase.GetDataTable(String.Format("Select  * from Album where ArtistId = {0}", artistId));
                int albumRecCount = dtAlbum.Rows.Count;
                //foreach (DataRow albumRow in dtAlbum.Rows)
                for (int j = 0; j < albumRecCount; j++)
                {
                    DataRow albumRow = dtAlbum.Rows[j];
                    string albumId = albumRow["AlbumId"].ToString();
                    Modules.MediaFile.Album newAlbum = MapDataTableToAlbum(albumRow, albumId);
                    newAlbum.ArtistName = newArtist.ArtistName;

                    DataTable dtSong = mysqlDatabase.GetDataTable(String.Format("Select  * from Song where ArtistId = {0} and AlbumId = {1}", artistId, albumId));
                    int songRecCount = dtSong.Rows.Count;
                    //foreach (DataRow songRow in dtSong.Rows)
                    for (int k = 0; k < songRecCount; k++)
                    {
                        DataRow songRow = dtSong.Rows[k];
                        Movie_Cleanup.Modules.MediaFile.Song newMusicLibraryItem = MapDataTableToSong(songRow);
                        //newAlbum.ArtistName = newMusicLibraryItem.Artist;
                        newMusicLibraryItem.Album = newAlbum.AlbumName;
                        newMusicLibraryItem.Artist = newArtist.ArtistName;
                        musicLib.Add(newMusicLibraryItem);
                        newAlbum.Songs.Add(newMusicLibraryItem);
                    }
                    newArtist.Albums.Add(newAlbum);
                    Albums.Add(newAlbum);
                }
                Artists.Add(newArtist);
            }


            //var filename = @"D:\Mix Set 5\07 Track 7.wma";//Erin Leah and N'Dinga Gaba - Rocker (Spiritchaser Remix) [2012] vk.com - [MP3JUICES.COM].mp3";
            //TagLib.File.LocalFileAbstraction tagFile = new TagLib.File.LocalFileAbstraction(filename);
            //var x = TagLib.File.Create(filename);

            //MediaLibrary = musicLibMan.Load(libraryFilePath);

            

            

            //using (var odb = OdbFactory.Open("QuinMediaLibrary.db"))
            //{


            //    foreach (Modules.MediaFile.Artist item in MediaLibrary.Music)
            //    {
            //        odb.Store(item);
            //    }
            //    var art = (from arti in odb.AsQueryable<Artist>()
            //               select arti).ToList();
            //    //odb.Store(MediaLibrary);
            //}


                     
            //Artists = Artists.GroupBy(i => i.ArtistName, (key, group) => group.First() ).ToList();
            Artists = Artists.OrderBy(o => o.ArtistName).ToList(); 
            //Artists = Artists.OrderBy(o => o.ArtistId).ToList();   
            
            //Albums = Albums.GroupBy(i => i.AlbumName, (key, group) => group.First()).ToList();
            Albums = Albums.OrderBy(o => o.AlbumName).ToList();
            List<AlbumControl> artistAlbums = new List<AlbumControl>();
            List<AlbumControl> tempArtistAlbums = (from alb in Albums
                                                   select new AlbumControl
                                                   {
                                                       DataContext = alb
                                                   }
                                  ).ToList();

            artistAlbums.AddRange(tempArtistAlbums);

            

            //Albums = Albums.OrderBy(o => o.AlbumId).ToList();
            int artistCount = Artists.Count();

            
            //foreach (var currentArtist in Artists)
            for (int k = 0; k < artistCount; k++)
            {
                Modules.MediaFile.Artist currentArtist = Artists[k];

                if (currentArtist.Albums.Count() == 0)
                {
                    continue;
                }

                //List<AlbumControl> tempArtistAlbums = (from alb in currentArtist.Albums
                //                                   select new AlbumControl
                //                                   {
                //                                       DataContext = alb
                //                                   }
                //                  ).ToList();

                //artistAlbums.AddRange(tempArtistAlbums);


                ArtistViewControl item = new ArtistViewControl();
                ArtistAlbumViewControl tempArtist = new ArtistAlbumViewControl();
                AlbumControl tempAlbum = new AlbumControl();                

                tempArtist.DataContext = currentArtist;
                int albumCount = Albums.Count();
                
                //foreach (Movie_Cleanup.Modules.MediaFile.Album alb in Albums)
                for (int i = 0; i < albumCount; i++)
                {
                    Modules.MediaFile.Album alb = Albums[i];
                    
                    
                    //int songCount = alb.Songs.Count();
                    //foreach (var art in alb.Songs)
                    //for (int j = 0; j < songCount; i++)
                    //{
                        //Modules.MediaFile.Song art = alb.Songs[j];
                        //if (art.Artist == currentArtist.ArtistName && alb.AlbumName == art.Album)
                        //if (art.ArtistId == currentArtist.ArtistId && alb.AlbumId == art.AlbumId)
                        //{
                    if (alb.ArtistId == currentArtist.ArtistId)
                    {
                        //currentArtist.Albums = new List<Modules.MediaFile.Album>();
                        currentArtist.Albums.Add(alb);
                        item.SetAlbumBinding(alb);
                        tempAlbum.DataContext = alb;
                        break;
                    }
                       // }
                   // }
                }

                item.DataContext = currentArtist;
                item.OnMediaChangedEvent += new ArtistViewControl.OnMediaChangedEventHandler(albumViewControl_OnSelectedMediaChanged);
                LibraryItems.Add(item);
                ArtistAlbumViewItems.Add(tempArtist);
                lstvwLibraryView.Children.Add(item);
            }
            //artistAlbums = artistAlbums.OrderBy(o => o.lblAlbumName).ToList();
            //LibraryItems = LibraryItems.OrderBy(o => o.lblArtistName).ToList();
            //ArtistAlbumViewItems = ArtistAlbumViewItems.OrderBy(o => o.).ToList();
            //List<AlbumControl> artistAlbums = (from alb in Albums
            //                                   select new AlbumControl
            //                                   {
            //                                       DataContext = alb
            //                                   }
            //                  ).ToList();

            int videoCount = Videos.Count();
            for (int k = 0; k < videoCount; k++)
            {
                Modules.MediaFile.Video currentVideo = Videos[k];
                VideoControl item = new VideoControl();
                item.DataContext = currentVideo;
                VideoLibraryItems.Add(item);
            }
            lstvwVideoLibraryView.ItemsSource = VideoLibraryItems;

            lstvwAllLibraryView.ItemsSource = artistAlbums;//AlbumList;
            lstvwArtistView.ItemsSource = ArtistAlbumViewItems;
            //lstvwLibraryView.ItemsSource = LibraryItems;//MusicLibraryItems;
            //lstvwLibraryView.Children.Add();// LibraryItems;

            lblProcess.Content = "Done Loading Library";
        }

        private Modules.MediaFile.Song MapDataTableToSong(DataRow songRow)
        {
            Movie_Cleanup.Modules.MediaFile.Song newMusicLibraryItem = new Modules.MediaFile.Song();
            newMusicLibraryItem.FileName = songRow["FileName"].ToString();
            newMusicLibraryItem.AlbumId = int.Parse(songRow["AlbumId"].ToString());
            newMusicLibraryItem.AlbumArtist = songRow["AlbumArtist"].ToString();
            newMusicLibraryItem.ArtistId = int.Parse(songRow["ArtistId"].ToString());
            newMusicLibraryItem.Title = songRow["Title"].ToString();
            newMusicLibraryItem.TrackNumber = songRow["TrackNumber"].ToString();
            newMusicLibraryItem.Genre = songRow["Genre"].ToString();
            newMusicLibraryItem.Composer = songRow["Composer"].ToString();
            newMusicLibraryItem.Length = songRow["Length"].ToString();
            newMusicLibraryItem.Year = songRow["Year"].ToString();
            newMusicLibraryItem.BitRate = songRow["BitRate"].ToString();
            //newMusicLibraryItem.Lyrics = songRow["Lyrics"].ToString();
            //newMusicLibraryItem.BeatsPerMinute = uint.Parse(songRow["BeatsPerMinute"].ToString());
            newMusicLibraryItem.SampleRate = int.Parse(songRow["SampleRate"].ToString());
            newMusicLibraryItem.Disc = songRow["Disc"].ToString();
            newMusicLibraryItem.PlayImageButton = songRow["PlayImageButton"].ToString();
            newMusicLibraryItem.AddToPlaylistImageButton = songRow["AddToPlaylistImageButton"].ToString();
            newMusicLibraryItem.Location = songRow["Location"].ToString();
            //musicLib.Add(newMusicLibraryItem);
            return newMusicLibraryItem;

        }

        private  Modules.MediaFile.Artist MapDataTableToArtist(DataRow artistRow)
        {

            Modules.MediaFile.Artist newArtist = new Modules.MediaFile.Artist();
            try
            {

            newArtist.ArtistArt = ImageLocationMapper(artistRow["ArtistArt"].ToString()); 
            newArtist.ArtistId = int.Parse(artistRow["ArtistId"].ToString());
            newArtist.ArtistName = artistRow["ArtistName"].ToString();
            }
            catch (Exception c)
            {

            }
            return newArtist;
        }

        private string ImageLocationMapper(string imageLocation)
        {
            try
            {
                if (imageLocation.Contains(@"\Images"))
                {
                    imageLocation = imageLocation.Substring(imageLocation.IndexOf(@"\Images"));
                    imageLocation = imageLocation.Replace(@"\Images", imageFilePath);
                }
                else if (imageLocation.Contains(@"\Album Artwork"))
                {
                    imageLocation = imageLocation.Substring(imageLocation.IndexOf(@"\Album Artwork"));
                    imageLocation = imageLocation.Replace(@"\Album Artwork", albumArtworkFilePath);
                }
            }
            catch (Exception c)
            {

            }
            return imageLocation;
        }

        private Modules.MediaFile.Album MapDataTableToAlbum(DataRow albumRow, string albumId)
        {
            Modules.MediaFile.Album newAlbum = new Modules.MediaFile.Album();
            try
            { 
                newAlbum.AlbumArt = ImageLocationMapper(albumRow["AlbumArt"].ToString()); ;
                newAlbum.AlbumId = int.Parse(albumId);
                newAlbum.AlbumName = albumRow["AlbumName"].ToString();
                newAlbum.ArtistId = int.Parse(albumRow["ArtistId"].ToString());
                newAlbum.Year = albumRow["Year"].ToString();
            }
            catch (Exception ed)
            {

            }
            return newAlbum;
        }

        private void OpenContainingFolderContextMenu_OnClick(object sender, RoutedEventArgs e)
        {
            Song song = lstvwPlayList.SelectedItem as Song;           
            if (song != null)
            {
                var directoryPath = new FileInfo(song.Location).DirectoryName;
                Process.Start(directoryPath);
            }
        }

        // Called by DriveDetector when removable device in inserted 
        private void OnDriveArrived(object sender, RemovableDrivesManagerEventArgs e)
        {
            // Report the event in the listbox.
            // e.Drive is the drive letter for the device which just arrived, e.g. "E:\\"
            string s = "Drive arrived " + e.Drive;
            //listBox1.Items.Add(s);

            //UsbDeviceControl usbDevice = new UsbDeviceControl();

            //UsbDrive usbDrive = new UsbDrive
            //{
            //    DriveLetter = e.Drive,
            //    DriveName = e.DriveName
            //};
            //usbDevice.DataContext = usbDrive;
            //lstbxDevices.Items.Add(usbDevice);

            LoadUsbDevice();

            //UsbDeviceManager usbMan = new UsbDeviceManager();
            //List<USBDeviceInfo> usbDevices = usbMan.GetUSBDevices();
            //if (lstbxDevices.Items.Count <= 0)
            //{
            //    foreach (var item in usbDevices)
            //    {
            //        if (item.Description == "USB Mass Storage Device")
            //        {
            //            UsbDeviceControl usbDevice = new UsbDeviceControl();
            //            usbDevice.DataContext = item;
            //            lstbxDevices.Items.Add(usbDevice);
            //            //lstbxDevices.ItemsSource = usbDevices;
            //            break;
            //        }
            //    }
            //}

            // If you want to be notified when drive is being removed (and be able to cancel it), 
            // set HookQueryRemove to true 
            //if (checkBoxAskMe.Checked)
            //    e.HookQueryRemove = true;
        }

        // Called by DriveDetector after removable device has been unpluged 
        private void OnDriveRemoved(object sender, RemovableDrivesManagerEventArgs e)
        {
            // TODO: do clean up here, etc. Letter of the removed drive is in e.Drive;

            // Just add report to the listbox
            //string s = "Drive removed " + e.Drive;
            //listBox1.Items.Add(s);
            //int devicesCount = lstbxDevices.Items.Count;
            //for (int i = 0; i < devicesCount; i++)
            //{
            //    UsbDrive currentItem = ((UsbDeviceControl)lstbxDevices.Items[i]).DataContext as UsbDrive;

            //    if (e.Drive == currentItem.DriveLetter)
            //    {
            //        lstbxDevices.Items.RemoveAt(i);
            //        break;
            //    }
            //}
            LoadUsbDevice();
        }

        // Called by DriveDetector when removable drive is about to be removed
        private void OnQueryRemove(object sender, RemovableDrivesManagerEventArgs e)
        {
            // Should we allow the drive to be unplugged?
            //if (checkBoxAskMe.Checked)
            //{
            //    if (MessageBox.Show("Allow remove?", "Query remove",
            //        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //        e.Cancel = false;       // Allow removal
            //    else
            //        e.Cancel = true;        // Cancel the removal of the device  
            //}
        }
        

        public void albumViewControl_OnSelectedMediaChanged(object sender)
        {
            musicplayer.CurrentPlayList = "allmymusic";
            PlaySelectedFromLibrary(sender);
        }

        public void albumViewControl_OnAddToPlaylist(object sender)
        {
            Song song = sender as Song;
            playList.Songs.Add(song);
            lstvwPlayList.Items.Add(song);

        }

        public void DragWindow(object sender, MouseButtonEventArgs args)
        {
            DragMove();
        }
        public void ButtonClicked(object sender, RoutedEventArgs args)
        {
            MainWindow win2 = new MainWindow();
            win2.ShowDialog();
        }

        private void InitializeWindowSource(object sender, EventArgs e)
        {
            hwndSource = PresentationSource.FromVisual((Visual)sender) as HwndSource;
        }

        public enum ResizeDirection
        {
            Left = 1,
            Right = 2,
            Top = 3,
            TopLeft = 4,
            TopRight = 5,
            Bottom = 6,
            BottomLeft = 7,
            BottomRight = 8,
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private void ResizeWindow(ResizeDirection direction)
        {
            SendMessage(hwndSource.Handle, WM_SYSCOMMAND, (IntPtr)(61440 + direction), IntPtr.Zero);
        }
        private SoundPlayer sound = new SoundPlayer();
        PlayMusic musicplayer  = null;//new PlayMusic();
        
       
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            PlaySong();
        }

        public void playSelectedFromPlaylist()
        {
            //PlaySelectedFromLibrary
            System.Windows.Controls.ListView senderObject = lstvwPlayList;
            musicplayer.CurrentPlayIndex = senderObject.SelectedIndex;// lstvwLibraryView.SelectedIndex;
            Movie_Cleanup.Modules.MediaFile.Song selectedSong = senderObject.SelectedItem as Movie_Cleanup.Modules.MediaFile.Song; ;//lstvwLibraryView.SelectedItem as MusicLibrary;
            musicplayer.CurrentPlayList = null;
            musicplayer.Open(selectedSong.Location);
            musicplayer.Play(false);
            PlayingFromLibrary = true;

            //playList = new Playlist();
            //playList.Songs = new ObservableCollection<MusicLibrary>();
            //foreach (MusicLibrary item in senderObject.Items)
            //{
            //    playList.Songs.Add(item);
            //}

            //lstvwPlayList.ItemsSource = playList.Songs;


            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\Images\";
            ImageSource imgSourse = new BitmapImage(new Uri(filePath + "pause.png"));
            imgPlay.Source = imgSourse;

            ImageSource imgAlbumArtSourse = new BitmapImage(new Uri(filePath + "Unknown Album.png"));
            //imgAlbumArt.Source = imgAlbumArtSourse;
            if (musicplayer.MediaInfo.Tags.AlbumArt != null)
            {
                imgAlbumArtSourse = Movie_Cleanup.Modules.ImageConverter.ToWpfBitmap(musicplayer.MediaInfo.Tags.AlbumArt);
            }


            imgCurrentlyPlayingAlbumArt.Source = imgAlbumArtSourse;
            imgAlbumArt.Source = imgAlbumArtSourse;
            lblArtist.Content = musicplayer.MediaInfo.Tags.Artist.Replace("\0", "").TrimEnd(' ') + " - " + musicplayer.MediaInfo.Tags.Title.Replace("\0", "");
            lblAlbum.Content = musicplayer.MediaInfo.Tags.Album.Replace("\0", "").TrimEnd(' ');

            //Ideally these things hould be done once but somehow they are coming pretty late from engine so we are doiing it here
            seekBar.Minimum = 0;
            seekBar.Maximum = (int)musicplayer.Duration; //Damn, double to int, i feel like killing myself
            timer.IsEnabled = true;

            timer.Start();
        }

        private void playFromPlaylist(int selectedSong)
        { 
            try
            {//SystemColors.ActiveBorderColor
                if (seekBar.Value > 0 && musicplayer.IsPaused)
                {
                    double positionValue = seekBar.Value;
                    musicplayer.SetPlayPosition(positionValue);
                    timer.IsEnabled = true;
                    timer.Start();
                }
                else if (playList.Songs.Count > 0)
                {

                    //if (selectesSong != null)
                    //{
                    //    musicplayer.PlaySelected(selectesSong);
                    //}
                    //else
                    //{

                    //MusicLibrary currentSong = lstvwPlayList.SelectedItem as MusicLibrary;
                    //musicplayer.Open(currentSong.Location);
                    //musicplayer.Play(false);

                    //currentSong.PlayImageButton = imageFilePath + "Playing.png";

                    //lstbxPlayLists.SelectedItem = currentSong;

                    musicplayer.PlayAllMusic(playList.Songs, selectedSong);
                    //}
                    string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\Images\";
                    ImageSource imgSourse = new BitmapImage(new Uri(filePath + "pause.png"));
                    imgPlay.Source = imgSourse;

                    ImageSource imgAlbumArtSourse = new BitmapImage(new Uri(filePath + "Unknown Album.png"));
                    //imgAlbumArt.Source = imgAlbumArtSourse;
                    if (musicplayer.MediaInfo.Tags.AlbumArt != null)
                    {
                        imgAlbumArtSourse = Movie_Cleanup.Modules.ImageConverter.ToWpfBitmap(musicplayer.MediaInfo.Tags.AlbumArt);
                    }

                    imgCurrentlyPlayingAlbumArt.Source = imgAlbumArtSourse;
                    imgAlbumArt.Source = imgAlbumArtSourse;
                    lblArtist.Content = musicplayer.MediaInfo.Tags.Artist.Replace("\0", "").TrimEnd(' ') + " - " + musicplayer.MediaInfo.Tags.Title.Replace("\0", "");
                    lblAlbum.Content = musicplayer.MediaInfo.Tags.Album.Replace("\0", "").TrimEnd(' ');

                    //Ideally these things hould be done once but somehow they are coming pretty late from engine so we are doiing it here
                    seekBar.Minimum = 0;
                    seekBar.Maximum = (int)musicplayer.Duration; //Damn, double to int, i feel like killing myself
                    timer.IsEnabled = true;

                    timer.Start();

                    Movie_Cleanup.Modules.MediaFile.Song selectedPlaylistSong = lstvwPlayList.SelectedItem as Movie_Cleanup.Modules.MediaFile.Song;
                    //selectedPlaylistSong.PlayImageButton = imageFilePath + @"\playback_play.png";
                    //((Movie_Cleanup.Modules.MediaFile.Song)lstvwPlayList.SelectedItem).PlayImageButton = imageFilePath + @"\playback_play.png";



                    //foreach (Movie_Cleanup.Modules.MediaFile.Song item in lstvwPlayList.Items)
                    //{
                    //    if (item.Location != selectedPlaylistSong.Location)
                    //    {
                    //        item.PlayImageButton = imageFilePath + @"\Music.png";
                    //    }
                    //}
                    List<Modules.MediaFile.Song> listOfSongs = new List<Modules.MediaFile.Song>();

                    int selectedIndex = lstvwPlayList.SelectedIndex;

                    foreach (Modules.MediaFile.Song item in lstvwPlayList.Items)
                    {
                        if (item.Location != selectedPlaylistSong.Location)
                        {
                            item.PlayImageButton = imageFilePath + @"\Music.png";
                        }
                        else
                        {
                            item.PlayImageButton = imageFilePath + @"\playback_play.png";
                        }

                        listOfSongs.Add(item);
                    }

                    lstvwPlayList.ItemsSource = listOfSongs;
                    lstvwPlayList.SelectedIndex = selectedIndex;
                    currentSongIndex = musicplayer.CurrentPlayIndex;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void playAll()
        {
            if (musicLib.Count > 0)
            {
                musicplayer.PlayAllMusic(musicLib,0);
                string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\Images\";
                ImageSource imgSourse = new BitmapImage(new Uri(filePath + "pause.png"));
                imgPlay.Source = imgSourse;

                ImageSource imgAlbumArtSourse = new BitmapImage(new Uri(filePath + "Unknown Album.png"));
                //imgAlbumArt.Source = imgAlbumArtSourse;
                if (musicplayer.MediaInfo.Tags.AlbumArt != null)
                {
                    imgAlbumArtSourse = Movie_Cleanup.Modules.ImageConverter.ToWpfBitmap(musicplayer.MediaInfo.Tags.AlbumArt);
                }

                imgCurrentlyPlayingAlbumArt.Source = imgAlbumArtSourse;
                imgAlbumArt.Source = imgAlbumArtSourse;
                lblArtist.Content = musicplayer.MediaInfo.Tags.Artist.Replace("\0", "").TrimEnd(' ') + " - " + musicplayer.MediaInfo.Tags.Title.Replace("\0", "");
                lblAlbum.Content = musicplayer.MediaInfo.Tags.Album.Replace("\0", "").TrimEnd(' ');

                //Ideally these things hould be done once but somehow they are coming pretty late from engine so we are doiing it here
                seekBar.Minimum = 0;
                seekBar.Maximum = (int)musicplayer.Duration; //Damn, double to int, i feel like killing myself
                timer.IsEnabled = true;
                
                timer.Start();
            }
            else
            {
                System.Windows.MessageBox.Show("Add music to you library or playlist");
                //var xxxx = musicplayer.PreviousPlayList();
            }
        }        
        //private void usbTimer_Tick(object sender, EventArgs e)
        //{
        //    UsbDeviceManager usbMan = new UsbDeviceManager();
        //    List<USBDeviceInfo> usbDevices = usbMan.GetUSBDevices();
        //    if (lstbxDevices.Items.Count <= 0)
        //    {
        //        foreach (var item in usbDevices)
        //        {
        //            if (item.Description == "USB Mass Storage Device")
        //            {
        //                UsbDeviceControl usbDevice = new UsbDeviceControl();
        //                usbDevice.DataContext = item;
        //                lstbxDevices.Items.Add(usbDevice);
        //                //lstbxDevices.ItemsSource = usbDevices;
        //                break;
        //            }
        //        }
        //    }

        //    Thread.Sleep(15000);
            
        //        //lstbxDevices.ItemsSource = usbDevices;
            
            
        //}

        private void libraryAddItemsTimer_Tick(object sender, EventArgs e)
        {
            //AddMediaToLibrary();
            

            //lstbxDevices.ItemsSource = usbDevices;


        }

        int currentSongIndex = 0;
        private void timer_Tick(object sender, EventArgs e)
        {
//            if (MediaPlayer.State == MediaState.Playing)
//{
//    MediaPlayer.GetVisualizationData(visData);
//}
//...
//    spritebatch.Begin();
//    for (int y = 0; y < arraySize; y++)
//    {
//        // Draw frequency spectrum display.
//        spritebatch.Draw(
//            line,
//            new Vector2((float)((-1.0 + visData.Frequencies[y]) * width + 1), y),
//            Color.White);

//        // Draw waveform from samples.
//        spritebatch.Draw(
//            line,
//            new Vector2((float)(visData.Samples[y] * width / 4 + width * 3 / 4), y),
//            sampleLine,
//            Color.White);
//    }
//    spriteBatch.End();



            //spectrumAnalyzer.soundPlayer_PropertyChanged(0);

            ////Ideally these things hould be done once but somehow they are coming pretty late from engine so we are doiing it here
            //seekBar.Minimum = 0;
            //seekBar.Maximum = (int)musicplayer.Duration; //Damn, double to int, i feel like killing myself
            ImageBehavior.SetRepeatBehavior(imageControl, new RepeatBehavior(TimeSpan.FromSeconds(musicplayer.Duration)));

            lblDuration.Content = musicplayer.DurationString;
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\Images\";
            ImageSource imgSourse = new BitmapImage(new Uri(filePath + "pause.png"));
            imgPlay.Source = imgSourse;
            //spectrumAnalyzer.BassEngine = musicplayer;
            //spectrumAnalyzer.soundPlayer_PropertyChanged(musicplayer.StreamHandle);
            SetAlbumArt(musicplayer.CurrentlyPlayingSong);
            
            if (musicplayer.CurruntPosition <= musicplayer.Duration)
            {
                tbManager.SetProgressValue((int)musicplayer.CurruntPosition, (int)musicplayer.Duration);
            }
            
            //if (lstvwLibraryView.Items.Count > 0)
            if(lstvwLibraryView.Children.Count > 0)
            {
                //lstvwLibraryView.Items[0]
                //lstvwLibraryView.Items[0]Selected = true;
                //lstvwLibraryView.;
                //lstvwLibraryView.SelectedIndex = musicplayer.CurrentPlayIndex;
                
            }
            if (currentSongIndex != musicplayer.CurrentPlayIndex)
            {
                Modules.MediaFile.Song selectedPlaylistSong = lstvwPlayList.SelectedItem as Modules.MediaFile.Song;
                //selectedPlaylistSong.PlayImageButton = imageFilePath + @"\playback_play.png";

                List<Modules.MediaFile.Song> listOfSongs = new List<Modules.MediaFile.Song>();
                lstvwPlayList.SelectedIndex = musicplayer.CurrentPlayIndex;
                int selectedIndex = lstvwPlayList.SelectedIndex;

                foreach (Modules.MediaFile.Song item in lstvwPlayList.Items)
                {
                    if (item.Location != selectedPlaylistSong.Location)
                    {
                        item.PlayImageButton = imageFilePath + @"\Music.png";
                    }
                    else
                    {
                        item.PlayImageButton = imageFilePath + @"\playback_play.png";
                        SetAlbumArt(item.Location);
                    }

                    listOfSongs.Add(item);
                }

                lstvwPlayList.ItemsSource = listOfSongs;
                lstvwPlayList.SelectedIndex = selectedIndex;
                currentSongIndex = musicplayer.CurrentPlayIndex;
            }

            
            SetTaskbarthumbnail();

            
            //CommandManager.InvalidateRequerySuggested();
            //seekBar.Value = (int)musicplayer.TrackPosition; //Damn, double to int, i feel like killing myself
            //this.Dispatcher.Invoke(DispatcherPriority.Normal, new TimerWorkItem());

            _dispathcer.Invoke(new Action(() => { TimerWorkItem();  }));
            
        }
        private Dispatcher _dispathcer;
        private void TimerWorkItem()
        {
            seekBar.Minimum = 0;
            seekBar.Maximum = (int)musicplayer.Duration;
            seekBar.Value = (int)musicplayer.CurruntPosition;
            lblElapsed.Content = musicplayer.CurruntPositionString;
            
        }

        private void seekBar_Scroll(object sender, EventArgs e)
        {
            musicplayer.CurruntPosition = seekBar.Value;
        }
                  
        
        private void sldrVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double volumeValue = sldrVolume.Value;            
            musicplayer.SetVolume(int.Parse(volumeValue.ToString("00")));
            lblVolume.Content = volumeValue.ToString("00") + "%";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            musicplayer.Repeat();
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\Images\";

            if (musicplayer.RepeatMode == PlayMusic.RepeatModes.All)
            {
                ImageSource imgSourse = new BitmapImage(new Uri(filePath+ "arrow-repeat.png"));
                imgRepeat.Source = imgSourse;
            }
            else if (musicplayer.RepeatMode == PlayMusic.RepeatModes.None)
            {
                ImageSource imgSourse = new BitmapImage(new Uri(filePath + "arrow_right.png"));
                imgRepeat.Source = imgSourse;
            }
            else if (musicplayer.RepeatMode == PlayMusic.RepeatModes.Single)
            {
                ImageSource imgSourse = new BitmapImage(new Uri(filePath + "arrow-repeat-once.png"));
                imgRepeat.Source = imgSourse;
            }
        }

        private void seekBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            
        }
        private void Slider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            //musicplayer.TrackPosition = seekBar.Value;
            //this.dragStarted = false;
            //seekBar.Minimum = 0;
            //seekBar.Maximum = (int)musicplayer.Duration;
            double positionValue = seekBar.Value;
            musicplayer.SetPlayPosition(positionValue);
            timer.IsEnabled = true;
            timer.Start();
        }
        private void Slider_DragStarted(object sender, DragStartedEventArgs e)
        {
            //this.dragStarted = true;
            timer.IsEnabled = false;
            timer.Stop();
            musicplayer.Pause();// Pause();
            //double positionValue = seekBar.Value;
            //musicplayer.SetPlayPosition(positionValue);
            
            
        }

        private void btnMute_Click(object sender, RoutedEventArgs e)
        {
            musicplayer.Mute();
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\Images\";

            if (musicplayer.IsMute == true)
            {
                ImageSource imgSourse = new BitmapImage(new Uri(filePath + "Mute.png"));
                imgMute.Source = imgSourse;
            }
            else if (musicplayer.IsMute == false)
            {
                ImageSource imgSourse = new BitmapImage(new Uri(filePath + "Volume-Mute.png"));
                imgMute.Source = imgSourse;
            }
            
        }

        

       

        private void imgRepeat_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            musicplayer.Repeat();
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\Images\";

            if (musicplayer.RepeatMode == PlayMusic.RepeatModes.All)
            {
                ImageSource imgSourse = new BitmapImage(new Uri(filePath + "arrow-repeat.png"));
                imgRepeat.Source = imgSourse;
            }
            else if (musicplayer.RepeatMode == PlayMusic.RepeatModes.None)
            {
                ImageSource imgSourse = new BitmapImage(new Uri(filePath + "arrow_right.png"));
                imgRepeat.Source = imgSourse;
            }
            else if (musicplayer.RepeatMode == PlayMusic.RepeatModes.Single)
            {
                ImageSource imgSourse = new BitmapImage(new Uri(filePath + "arrow-repeat-once.png"));
                imgRepeat.Source = imgSourse;
            }
        }

        private void imgMute_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            musicplayer.Mute();
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\Images\";

            if (musicplayer.IsMute == true)
            {
                ImageSource imgSourse = new BitmapImage(new Uri(filePath + "Mute.png"));
                imgMute.Source = imgSourse;
            }
            else if (musicplayer.IsMute == false)
            {
                ImageSource imgSourse = new BitmapImage(new Uri(filePath + "Volume-Mute.png"));
                imgMute.Source = imgSourse;
            }
        }

        

        private void mnuAddFolderToLibrary_Click(object sender, RoutedEventArgs e)
        {
            //libraryAddItemsTimer.IsEnabled = true;
            //libraryAddItemsTimer.Start();
            ImageSource imgSourse = new BitmapImage(new Uri(imageFilePath + @"\Arrow-Right.png"));
            DirBrowser dirBrowser = new DirBrowser();
            if (dirBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DirScan dirScan = new DirScan();
                //string[] filePaths = dirScan.Browse(dirBrowser.DirectoryPath);
                filePaths = dirScan.Browse(dirBrowser.DirectoryPath);
                //libThread.Start();
                AddMediaToLibrary(false);
            }

        }

        public void AddMediaToLibrary(bool initialate)
        {
            //ImageSource imgSourse = new BitmapImage(new Uri(imageFilePath + "Arrow-Right.png"));
            //DirBrowser dirBrowser = new DirBrowser();
            //if (dirBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    DirScan dirScan = new DirScan();
            //    string[] filePaths = dirScan.Browse(dirBrowser.DirectoryPath);
                //int i = 0;

            List<Movie_Cleanup.Modules.MediaFile.Artist> Artists = new List<Movie_Cleanup.Modules.MediaFile.Artist>();
            List<Movie_Cleanup.Modules.MediaFile.Album> Albums = new List<Movie_Cleanup.Modules.MediaFile.Album>();
            List<Movie_Cleanup.Modules.MediaFile.Video> Videos = new List<Movie_Cleanup.Modules.MediaFile.Video>();

                string currentAblum = string.Empty;
                int pathCount = filePaths.Count();
                for (int i = 0; i < pathCount; i++)
                {
                    string currentFilePath = filePaths[i];

                    if (MediaItem.MediaTypeValidator(currentFilePath) == MediaItem.MediaType.Video)
                    {

                    //}

                    //if (System.IO.Path.GetExtension(currentFilePath).ToLowerInvariant() == ".mp4")
                    //{
                        Video videoFile = new Video(currentFilePath);
                        int x = mysqlDatabase.InsertVideo_Sql(videoFile);
                        Videos.Add(videoFile);
                        continue;
                    }
                    //foreach (string currentFilePath in filePaths)
                    //{
                        //if (!initialate)
                        //{
                        //    MediaLibraryFiles.Add(new MediaFile
                        //    {
                        //        Fullpath = currentFilePath,
                        //        FileName = System.IO.Path.GetFileName(currentFilePath)
                        //    });
                        //}


                        //MusicID3Tag tempTagger = new MusicID3Tag().GetTags(currentFilePath);
                        Movie_Cleanup.Modules.MediaFile.Song newMusicLibraryItem = new Movie_Cleanup.Modules.MediaFile.Song();
                        lblProcess.Content = "Adding File: " + currentFilePath;

                        MediaInfo mediaInfo = new MediaInfo(currentFilePath);

                        if (mediaInfo.Tags == null)
                        {
                            continue;
                        }
                        //ImageSource imgAlbumArtSourse = new BitmapImage(new Uri(imageFilePath + "Unknown Album.png"));
                        string unknownAlbum = imageFilePath + @"\Unknown Album.png";
                        string unknownArtist = imageFilePath + @"\Unknown Artist.PNG";
                        string albumId = FixWordCasing(mediaInfo.Tags.Artist + "_" + mediaInfo.Tags.Album);//Guid.NewGuid().ToString();

                        if (mediaInfo.Tags.AlbumArt != null && !File.Exists(albumArtworkFilePath + albumId))
                        {
                            //imgAlbumArtSourse = ImageConverter.ToWpfBitmap(mediaInfo.Tags.AlbumArt);
                            //unknownArtistArt = imgAlbumArtSourse;
                            unknownAlbum = Movie_Cleanup.Modules.ImageConverter.SaveAlbumArt(mediaInfo.Tags.AlbumArt, albumArtworkFilePath, albumId);
                            newMusicLibraryItem.SongArt = unknownAlbum;
                            unknownArtist = unknownAlbum;
                        }
                        else
                        {
                            unknownArtistArt = new BitmapImage(new Uri(imageFilePath + @"\Unknown Artist.PNG"));
                            newMusicLibraryItem.SongArt = unknownAlbum;
                        }

                        //Bitmap bti = Bitmap.FromFile("");Image
                        //System.Drawing.Image tempImage = System.Drawing.Image.FromFile("a.png");

                        //System.Drawing.Image tempImage = Bitmap.FromFile(""); // new System.Drawing.Image();// = new System.Drawing.Image();
                        //tempImage.Source = unknownArtistArt;
                        newMusicLibraryItem.FileName = System.IO.Path.GetFileName(currentFilePath);
                        newMusicLibraryItem.Album = FixWordCasing(string.IsNullOrEmpty(mediaInfo.Tags.Album) ? "Unknown Album" : mediaInfo.Tags.Album);
                        newMusicLibraryItem.AlbumArtist = FixWordCasing(string.IsNullOrEmpty(mediaInfo.Tags.AlbumArtists) ? "Unknown Album Artists" : mediaInfo.Tags.AlbumArtists);
                        newMusicLibraryItem.Artist = FixWordCasing(string.IsNullOrEmpty(mediaInfo.Tags.Artist) ? "Unknown Artist" : mediaInfo.Tags.Artist);
                        newMusicLibraryItem.Title = FixWordCasing(string.IsNullOrEmpty(mediaInfo.Tags.Title) ? "Unknown Title" : mediaInfo.Tags.Title);
                        newMusicLibraryItem.TrackNumber = mediaInfo.Tags.Track;
                        newMusicLibraryItem.Genre = FixWordCasing(mediaInfo.Tags.Genre);
                        newMusicLibraryItem.Composer = FixWordCasing(mediaInfo.Tags.Composer);
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
                        newMusicLibraryItem.Location = currentFilePath; // mediaInfo.Tags.Location;
                        newMusicLibraryItem.Fullpath = new FileInfo(currentFilePath).DirectoryName; 
                        //newMusicLibraryItem.AddMediaItem(tempTagger);
                        musicLib.Add(newMusicLibraryItem);
                        //MusicLibraryItems.Add(newMusicLibraryItem);


                        //add artist to list of artists
                        //List<Movie_Cleanup.Modules.MediaFile.Artist> xArtist = (from artist in Artists.AsEnumerable()
                        //                                                        where artist.ArtistName == newMusicLibraryItem.Artist
                        //                                                        select artist).ToList();

                        Modules.MediaFile.Artist currentArtist = (from artist in Artists.AsEnumerable()
                                                                                where artist.ArtistName == newMusicLibraryItem.Artist
                                                                                select artist).FirstOrDefault();//.ToArray();

                        //Movie_Cleanup.Modules.MediaFile.Artist currentArtist = new Movie_Cleanup.Modules.MediaFile.Artist();

                        //if (xArtist.Count() > 0)
                        if (currentArtist != null)
                        {
                            newMusicLibraryItem.ArtistId = currentArtist.ArtistId;
                            //currentArtist = xArtist[0];
                            //newMusicLibraryItem.ArtistId = currentArtist.ArtistId;
                        }
                        else
                        {
                            currentArtist = new Movie_Cleanup.Modules.MediaFile.Artist();
                            currentArtist.ArtistArt = unknownArtist;//unknownArtistArt; //imageFilePath + "Unknown Artist.PNG";
                            currentArtist.ArtistName = newMusicLibraryItem.Artist;
                            Artists.Add(currentArtist);
                            int artistId = mysqlDatabase.InsertArtist_Sql(currentArtist);
                            currentArtist.ArtistId = artistId;
                            newMusicLibraryItem.ArtistId = currentArtist.ArtistId;
                        }

                        //List<Movie_Cleanup.Modules.MediaFile.Album> xAlbum = (from album in Albums.AsEnumerable()
                        //                                                      where album.AlbumName == newMusicLibraryItem.Album
                        //                                                      select album).ToList();

                        Movie_Cleanup.Modules.MediaFile.Album currentAlbum = (from album in Albums.AsEnumerable()
                                                                              where album.AlbumName == newMusicLibraryItem.Album
                                                                        select album).FirstOrDefault();

                        //currentAlbum = Albums.Select(e => e.AlbumName == newMusicLibraryItem.Album).FirstOrDefault();

                        //List<string> xGenre = (from genre in Genres.AsEnumerable()
                        //                      where genre == newMusicLibraryItem.Genre
                        //                      select genre).ToList();

                        //if (xGenre.Count <= 0)
                        //{
                        //    Genres.Add(newMusicLibraryItem.Genre);
                        //}

                        //add Albums to List of albums
                        //Movie_Cleanup.Modules.MediaFile.Album currentAlbum = new Movie_Cleanup.Modules.MediaFile.Album();
                        //if (xAlbum.Count > 0)
                        if (currentAlbum != null)
                        {                            
                            newMusicLibraryItem.AlbumId = currentAlbum.AlbumId;
                            int x = mysqlDatabase.InsertSong_Sql(newMusicLibraryItem);
                            currentAlbum.Songs.Add(newMusicLibraryItem);
                            //currentAlbum = xAlbum[0];
                            //newMusicLibraryItem.AlbumId = currentAlbum.AlbumId;
                            //int x = mysqlDatabase.InsertSong_Sql(newMusicLibraryItem);
                            //currentAlbum.Songs.Add(newMusicLibraryItem);
                        }
                        else
                        {
                            currentAlbum = new Movie_Cleanup.Modules.MediaFile.Album()
                            {
                                AlbumName = newMusicLibraryItem.Album,
                                //AlbumId = albumId,
                                //AlbumArt = newMusicLibraryItem.AlbumArtist,
                                Year = newMusicLibraryItem.Year,
                                AlbumArt = unknownAlbum
                                //Tracks = new List<Movie_Cleanup.Modules.MediaFile.Song>()
                            };
                            //currentAlbum.Songs.Add(newMusicLibraryItem);
                            //currentAlbum.Tracks.Add(newMusicLibraryItem);
                            Albums.Add(currentAlbum);
                            currentAlbum.ArtistId = newMusicLibraryItem.ArtistId;
                            currentAlbum.AlbumId = mysqlDatabase.InsertAlbum_Sql(currentAlbum);

                            newMusicLibraryItem.AlbumId = currentAlbum.AlbumId;
                            int x = mysqlDatabase.InsertSong_Sql(newMusicLibraryItem);
                            currentAlbum.Songs.Add(newMusicLibraryItem);

                        }



                        //var mediaArtist = new Movie_Cleanup.Modules.MediaFile.Artist();
                        //var mediaAlbum = new Movie_Cleanup.Modules.MediaFile.Album();
                        //mediaArtist.Add()


                        //if (!initialate)
                        //{
                        //    MediaLibraryFiles.Add(new Movie_Cleanup.Modules.MediaFile.Artist
                        //    {
                        //        Fullpath = currentFilePath,
                        //        FileName = System.IO.Path.GetFileName(currentFilePath)
                        //    });
                        //}

                    }
                //List<Movie_Cleanup.Modules.MediaFile.Artist> completeArtistAlbum = new List<Movie_Cleanup.Modules.MediaFile.Artist>();
                //Genres.Sort();
                //Albums = Albums.OrderBy(o=>o.AlbumName).ToList();
                //Artists = Artists.OrderBy(o => o.ArtistName).ToList();

                
                //Artists = Artists.GroupBy(i => i.ArtistId, (key, group) => group.First()).ToList();
                Artists = Artists.OrderBy(o => o.ArtistName).ToList();
                
                //Albums = Albums.GroupBy(i => i.AlbumId, (key, group) => group.First()).ToList();
                Albums = Albums.OrderBy(o => o.AlbumName).ToList();
                Videos = Videos.OrderBy(o => o.VideoName).ToList();

                
                //foreach (var currentArtist in Artists)
                //{
                    
                //    ArtistViewControl item = new ArtistViewControl();
                //    ArtistAlbumViewControl tempArtist = new ArtistAlbumViewControl();
                //    AlbumControl tempAlbum = new AlbumControl();

                //    tempArtist.DataContext = currentArtist;

                //    foreach (Movie_Cleanup.Modules.MediaFile.Album alb in Albums)
                //    {
                //        foreach (var art in alb.Songs)//alb.Tracks)
                //        {
                //            if (art.Artist == currentArtist.ArtistName && alb.AlbumName == art.Album)
                //            {
                //                currentArtist.Albums.Add(alb);
                //                //currentArtist.Albums.Add(alb);
                //                item.SetAlbumBinding(alb);
                //                tempAlbum.DataContext = alb;
                //                break;
                //            }
                //        }
                //    }

                //    item.DataContext = currentArtist;
                //    item.OnMediaChangedEvent += new ArtistViewControl.OnMediaChangedEventHandler(albumViewControl_OnSelectedMediaChanged);
                //    LibraryItems.Add(item);
                //    ArtistAlbumViewItems.Add(tempArtist);
                //    //completeArtistAlbum.Add(currentArtist);
                //    //MediaLibrary.Music.Add(currentArtist);
                //}
                int artistCount = Artists.Count();
                //foreach (var currentArtist in Artists)
                for (int k = 0; k < artistCount; k++)
                {
                    Modules.MediaFile.Artist currentArtist = Artists[k];
                    ArtistViewControl item = new ArtistViewControl();
                    ArtistAlbumViewControl tempArtist = new ArtistAlbumViewControl();
                    AlbumControl tempAlbum = new AlbumControl();

                    tempArtist.DataContext = currentArtist;
                    int albumCount = Albums.Count();

                    //foreach (Movie_Cleanup.Modules.MediaFile.Album alb in Albums)
                    for (int i = 0; i < albumCount; i++)
                    {
                        Modules.MediaFile.Album alb = Albums[i];
                        //int songCount = alb.Songs.Count();
                        //foreach (var art in alb.Songs)
                        //for (int j = 0; j < songCount; i++)
                        //{
                        //Modules.MediaFile.Song art = alb.Songs[j];
                        //if (art.Artist == currentArtist.ArtistName && alb.AlbumName == art.Album)
                        //if (art.ArtistId == currentArtist.ArtistId && alb.AlbumId == art.AlbumId)
                        //{
                        if (alb.ArtistId == currentArtist.ArtistId)
                        {
                            //currentArtist.Albums = new List<Modules.MediaFile.Album>();
                            currentArtist.Albums.Add(alb);
                            item.SetAlbumBinding(alb);
                            tempAlbum.DataContext = alb;
                            break;
                        }
                        // }
                        // }
                    }

                    item.DataContext = currentArtist;
                    item.OnMediaChangedEvent += new ArtistViewControl.OnMediaChangedEventHandler(albumViewControl_OnSelectedMediaChanged);
                    LibraryItems.Add(item);
                    ArtistAlbumViewItems.Add(tempArtist);
                    lstvwLibraryView.Children.Add(item);
                }
                
                List<AlbumControl> artistAlbums = (from alb in Albums
                                                   select new AlbumControl
                                                   {
                                                       DataContext = alb
                                                   }
                                  ).ToList();


                int videoCount = Videos.Count();
                for (int k = 0; k < videoCount; k++)
                {
                    Modules.MediaFile.Video currentVideo = Videos[k];
                    VideoControl item = new VideoControl();
                    item.DataContext = currentVideo;
                    VideoLibraryItems.Add(item);
                }
                lstvwVideoLibraryView.ItemsSource = VideoLibraryItems;
                //artistAlbums = artistAlbums.OrderBy(o => o.lblAlbumName).ToList();
                //LibraryItems = LibraryItems.OrderBy(o => o.lblArtistName).ToList();

                lstvwAllLibraryView.ItemsSource = artistAlbums;//AlbumList;
                lstvwArtistView.ItemsSource = ArtistAlbumViewItems;
                //lstvwLibraryView.ItemsSource = LibraryItems;//MusicLibraryItems;
                //lstvwLibraryViewFiltered.ItemsSource = LibraryItems;

                
                
            
                lblProcess.Content = "Done Adding Songs to Library";

                //MediaLibraryFiles.Save(libraryFilePath);

                //using (StreamWriter myWriter = new StreamWriter(libraryFilePath, false))
                //{
                //    XmlSerializer mySerializer = new XmlSerializer(typeof(Movie_Cleanup.Modules.MediaFile.MediaLibrary));
                //    mySerializer.Serialize(myWriter, MediaLibrary);
                //    myWriter.Close();
                //}

                //XmlSerializer serializer = new XmlSerializer(typeof(List<Artist>));
                //TextWriter tw = new StreamWriter(libraryFilePath);
                //serializer.Serialize(tw, completeArtistAlbum);
                //tw.Close();
            //}
            //libThread.Abort();
        }

        private void lstvwLibraryView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var x = e.Source as MusicLibrary;
            

            //PopupTest.Placement = System.Windows.Controls.Primitives.PlacementMode.Mouse;
            //PopupTest.StaysOpen = false;
            //PopupTest.Height = 250;
            //PopupTest.Width = 500;
            //PopupTest.IsOpen = true;
        }
        private void listLocal_DragOver(object sender, System.Windows.DragEventArgs e)
        {
            //if (!e.Data.GetDataPresent(typeof(ListViewItem))) return;
            //Point p = lstvwLibraryView.PointToClient(MousePosition);
            //ListViewItem targetItem = lstvwLibraryView.GetItemAt(p.X, p.Y);
            //if (targetItem != null)               //if dropping on a target item
            //{
            //    targetItem.Selected = true;
            //    if (targetItem.SubItems.Count > 1) e.Effect = DragDropEffects.None;//if IsFile
            //    else e.Effect = DragDropEffects.Copy;
            //    return;
            //}
            //foreach (ListViewItem item in lstvwLibraryView.Items) item.Selected = false; //if dragging into current address
            //e.Effect = DragDropEffects.Copy;
        }

        private void imgPlayPrevious_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PlayPreviousSong();
            //if (lstvwPlayList.Items.Count < musicplayer.CurrentPlayIndex)
            //{
            //    musicplayer.CurrentPlayIndex--;
            //}
            //else
            //{
            //    musicplayer.CurrentPlayIndex = lstvwPlayList.Items.Count;
            //}

            
            //musicplayer.PlayPrevious();
            //lstvwPlayList.SelectedIndex = musicplayer.CurrentPlayIndex;
            //ImageSource imgAlbumArtSourse = new BitmapImage(new Uri(imageFilePath + @"\Unknown Album.png"));
            
            //if (musicplayer.MediaInfo.Tags.AlbumArt != null)
            //{
            //    imgAlbumArtSourse = ImageConverter.ToWpfImage(musicplayer.MediaInfo.Tags.AlbumArt);
            //}
            //imgCurrentlyPlayingAlbumArt.Source = imgAlbumArtSourse;
            //imgAlbumArt.Source = imgAlbumArtSourse;
            //lblArtist.Content = musicplayer.MediaInfo.Tags.Artist.Replace("\0", "").TrimEnd(' ') + " - " + musicplayer.MediaInfo.Tags.Title.Replace("\0", "");
            //lblAlbum.Content = musicplayer.MediaInfo.Tags.Album.Replace("\0", "").TrimEnd(' ');

            //Movie_Cleanup.Modules.MediaFile.Song selectedPlaylistSong = lstvwPlayList.SelectedItem as Movie_Cleanup.Modules.MediaFile.Song;
            //List<Modules.MediaFile.Song> listOfSongs = new List<Modules.MediaFile.Song>();

            //int selectedIndex = lstvwPlayList.SelectedIndex;

            //foreach (Modules.MediaFile.Song item in lstvwPlayList.Items)
            //{
            //    if (item.Location != selectedPlaylistSong.Location)
            //    {
            //        item.PlayImageButton = imageFilePath + @"\Music.png";
            //    }
            //    else
            //    {
            //        item.PlayImageButton = imageFilePath + @"\playback_play.png";
            //    }

            //    listOfSongs.Add(item);
            //}

            //lstvwPlayList.ItemsSource = listOfSongs;
            //lstvwPlayList.SelectedIndex = selectedIndex;
            //currentSongIndex = musicplayer.CurrentPlayIndex;
            //PlaySelectedFromLibrary(lstvwLibraryView);
            //playFromPlaylist();
            //string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\Images\";
            //musicplayer.PlayPrevious();
            //ImageSource imgAlbumArtSourse = new BitmapImage(new Uri(filePath + "Unknown Album.png"));
            ////imgAlbumArt.Source = imgAlbumArtSourse;
            //if (musicplayer.MediaInfo.Tags.AlbumArt != null)
            //{
            //    imgAlbumArtSourse = ImageConverter.ToWpfImage(musicplayer.MediaInfo.Tags.AlbumArt);
            //}
            //imgCurrentlyPlayingAlbumArt.Source = imgAlbumArtSourse;
            //imgAlbumArt.Source = imgAlbumArtSourse;
            //lblArtist.Content = musicplayer.MediaInfo.Tags.Artist.Replace("\0", "").TrimEnd(' ') + " - " + musicplayer.MediaInfo.Tags.Title.Replace("\0", "");
            //lblAlbum.Content = musicplayer.MediaInfo.Tags.Album.Replace("\0", "").TrimEnd(' ');
            
        }

        private void imgPlayNext_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            PlayNextSong();
            //if (lstvwPlayList.Items.Count > musicplayer.CurrentPlayIndex)
            //{
            //    musicplayer.CurrentPlayIndex++;
            //}
            //else
            //{
            //    musicplayer.CurrentPlayIndex = 0;
            //}
            
            //musicplayer.PlayNext();
            //lstvwPlayList.SelectedIndex = musicplayer.CurrentPlayIndex;
            //ImageSource imgAlbumArtSourse = new BitmapImage(new Uri(imageFilePath + @"\Unknown Album.png"));
            ////imgAlbumArt.Source = imgAlbumArtSourse;
            //if (musicplayer.MediaInfo.Tags.AlbumArt != null)
            //{

            //    imgAlbumArtSourse = ImageConverter.ToWpfImage(musicplayer.MediaInfo.Tags.AlbumArt);
            //    if (imgAlbumArtSourse == null)
            //    {
            //        imgAlbumArtSourse = new BitmapImage(new Uri(imageFilePath + @"\Unknown Album.png"));
            //    }
            //}
            //imgCurrentlyPlayingAlbumArt.Source = imgAlbumArtSourse;
            //imgAlbumArt.Source = imgAlbumArtSourse;
            //lblArtist.Content = musicplayer.MediaInfo.Tags.Artist.Replace("\0", "").TrimEnd(' ') + " - " + musicplayer.MediaInfo.Tags.Title.Replace("\0", "");
            //lblAlbum.Content = musicplayer.MediaInfo.Tags.Album.Replace("\0", "").TrimEnd(' ');

            //Movie_Cleanup.Modules.MediaFile.Song selectedPlaylistSong = lstvwPlayList.SelectedItem as Movie_Cleanup.Modules.MediaFile.Song;
            //List<Modules.MediaFile.Song> listOfSongs = new List<Modules.MediaFile.Song>();

            //int selectedIndex = lstvwPlayList.SelectedIndex;

            //foreach (Modules.MediaFile.Song item in lstvwPlayList.Items)
            //{
            //    if (item.Location != selectedPlaylistSong.Location)
            //    {
            //        item.PlayImageButton = imageFilePath + @"\Music.png";
            //    }
            //    else
            //    {
            //        item.PlayImageButton = imageFilePath + @"\playback_play.png";
            //    }

            //    listOfSongs.Add(item);
            //}

            //lstvwPlayList.ItemsSource = listOfSongs;
            //lstvwPlayList.SelectedIndex = selectedIndex;
            //currentSongIndex = musicplayer.CurrentPlayIndex;

            //if (PlayingFromLibrary == true)
            //{
            //    if (lstvwPlayList.Items.Count > musicplayer.CurrentPlayIndex)
            //    {
            //        musicplayer.CurrentPlayIndex++;
            //    }
            //    else
            //    {
            //        musicplayer.CurrentPlayIndex = 0;
            //    }
                
            //    lstvwPlayList.SelectedIndex = musicplayer.CurrentPlayIndex;
            //    //PlaySelectedFromLibrary(lstvwLibraryView);
            //    playFromPlaylist();
            //}
            //else
            //{
            //    string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\Images\";
            //    musicplayer.PlayNext();
            //    ImageSource imgAlbumArtSourse = new BitmapImage(new Uri(filePath + "Unknown Album.png"));
            //    //imgAlbumArt.Source = imgAlbumArtSourse;
            //    if (musicplayer.MediaInfo.Tags.AlbumArt != null)
            //    {
            //        imgAlbumArtSourse = ImageConverter.ToWpfImage(musicplayer.MediaInfo.Tags.AlbumArt);
            //    }
            //    imgCurrentlyPlayingAlbumArt.Source = imgAlbumArtSourse;
            //    imgAlbumArt.Source = imgAlbumArtSourse;
            //    lblArtist.Content = musicplayer.MediaInfo.Tags.Artist.Replace("\0", "").TrimEnd(' ') + " - " + musicplayer.MediaInfo.Tags.Title.Replace("\0", "");
            //    lblAlbum.Content = musicplayer.MediaInfo.Tags.Album.Replace("\0", "").TrimEnd(' ');
            //}
        }

        private void lstvwLibraryView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PlaySelectedFromLibrary(sender);
        }
        private void lstvwPlayListViewer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Playlist selectedPlayList = (lstbxPlayLists.SelectedItem as PlaylistControl).DataContext as Playlist;
            musicplayer.CurrentPlayList = selectedPlayList.PlaylistFileName;
            PlaySelectedFromLibrary(sender);
        }
        public void PlaySelectedFromLibrary(object sender)
        {
            try
            {

            
            System.Windows.Controls.ListView senderObject = sender as System.Windows.Controls.ListView;
            musicplayer.CurrentPlayIndex = senderObject.SelectedIndex;// lstvwLibraryView.SelectedIndex;
            Modules.MediaFile.Song selectedSong = senderObject.SelectedItem as Modules.MediaFile.Song; ;//lstvwLibraryView.SelectedItem as MusicLibrary;
            //musicplayer.CurrentPlayList = null;
            //musicplayer.Open(selectedSong.Location);
            //musicplayer.Play(false);

            
            PlayingFromLibrary = true;
            lstvwPlayList.SelectedIndex = senderObject.SelectedIndex;

            

            playList = new Playlist();
            playList.Songs = new ObservableCollection<Movie_Cleanup.Modules.MediaFile.Song>();
            foreach (Movie_Cleanup.Modules.MediaFile.Song item in senderObject.Items)
            {
                playList.Songs.Add(item);
            }

            lstvwPlayList.ItemsSource = playList.Songs;
            playFromPlaylist(senderObject.SelectedIndex);


            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\Images\";
            ImageSource imgSourse = new BitmapImage(new Uri(filePath + "pause.png"));
            imgPlay.Source = imgSourse;

            ImageSource imgAlbumArtSourse = new BitmapImage(new Uri(filePath + "Unknown Album.png"));
            //imgAlbumArt.Source = imgAlbumArtSourse;
            if (musicplayer.MediaInfo.Tags.AlbumArt != null)
            {
                imgAlbumArtSourse = Movie_Cleanup.Modules.ImageConverter.ToWpfBitmap(musicplayer.MediaInfo.Tags.AlbumArt);
            }


            imgCurrentlyPlayingAlbumArt.Source = imgAlbumArtSourse;
            imgAlbumArt.Source = imgAlbumArtSourse;
            lblArtist.Content = musicplayer.MediaInfo.Tags.Artist.Replace("\0", "").TrimEnd(' ') + " - " + musicplayer.MediaInfo.Tags.Title.Replace("\0", "");
            lblAlbum.Content = musicplayer.MediaInfo.Tags.Album.Replace("\0", "").TrimEnd(' ');

            //Ideally these things hould be done once but somehow they are coming pretty late from engine so we are doiing it here
            seekBar.Minimum = 0;
            seekBar.Maximum = (int)musicplayer.Duration; //Damn, double to int, i feel like killing myself
            timer.IsEnabled = true;

            timer.Start();
            }
            catch (Exception exp)
            {

               // System.Windows.MessageBox.Show("Unable to Play Media file: File corrupt or something");
            }
        }

        //private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        //{
        //    listView1.DoDragDrop(listView1.SelectedItems, DragDropEffects.Move);
        //}
        private void listView1_ItemDrag(object sender, DragStartedEventArgs e)
        {
            //lstvwLibraryView.DoDragDrop(lstvwLibraryView.SelectedItems, DragDropEffects.Copy);
        }

        private void treeView1_DragEnter(object sender, System.Windows.DragEventArgs e)
        {
            //if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)))
            //    e.Effect = DragDropEffects.Copy;
        }

        private void treeView1_DragDrop(object sender, System.Windows.DragEventArgs e)
        {
            //if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection).ToString(), false))
            //{
            //    Point loc = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
            //    TreeNode destNode = ((TreeView)sender).GetNodeAt(loc);
            //    TreeNode tnNew;

            //    ListView.SelectedListViewItemCollection lstViewColl =
            //        (ListView.SelectedListViewItemCollection)e.Data.GetData(typeof(ListView.SelectedListViewItemCollection));
            //    foreach (ListViewItem lvItem in lstViewColl)
            //    {
            //        tnNew = new TreeNode(lvItem.Text);
            //        tnNew.Tag = lvItem;

            //        destNode.Nodes.Insert(destNode.Index + 1, tnNew);
            //        destNode.Expand();
            //        // Remove this line if you want to only copy items
            //        // from ListView and not move them
            //        //lvItem.Remove();
            //    }
            //}
        }







        public bool PlayingFromLibrary { get; set; }

        private void lstvwPlayList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Modules.MediaFile.Song selectedPlaylistSong = lstvwPlayList.SelectedItem as Modules.MediaFile.Song;
            //selectedPlaylistSong.PlayImageButton = imageFilePath + @"\playback_play.png";

            ObservableCollection<Modules.MediaFile.Song> listOfSongs = new ObservableCollection<Modules.MediaFile.Song>();

            int selectedIndex = lstvwPlayList.SelectedIndex;

            foreach (Modules.MediaFile.Song item in lstvwPlayList.Items)
            {
                if (item.Location != selectedPlaylistSong.Location)
                {
                    item.PlayImageButton = imageFilePath + @"\Music.png";
                }
                else
                {
                    item.PlayImageButton = imageFilePath + @"\playback_play.png";
                }

                listOfSongs.Add(item);
            }
            playList.Songs = listOfSongs;
            lstvwPlayList.ItemsSource = listOfSongs;
            lstvwPlayList.SelectedIndex = selectedIndex;
            currentSongIndex = musicplayer.CurrentPlayIndex;
            //((Modules.MediaFile.Song)lstvwPlayList.SelectedItem).PlayImageButton = imageFilePath + @"\playback_play.png";

            //foreach (Modules.MediaFile.Song item in lstvwPlayList.Items)
            //{
            //    if (item.Location != selectedPlaylistSong.Location)
            //    {
            //        item.PlayImageButton = imageFilePath + @"\Music.png";
            //    }
            //}
            playFromPlaylist(lstvwPlayList.SelectedIndex);
        }

        private void lstvwArtistView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ArtistAlbumViewControl selectedArtistItem = lstvwArtistView.SelectedItem as ArtistAlbumViewControl;
            if (selectedArtistItem != null)
            {
                lstvwLibraryView.Children.Clear();
                

                Modules.MediaFile.Artist selectedArtist = selectedArtistItem.DataContext as Modules.MediaFile.Artist;
                //string artistId = selectedArtist.ArtistId.ToString();
                //List<Movie_Cleanup.Modules.MediaFile.Artist> Artists = new List<Movie_Cleanup.Modules.MediaFile.Artist>();
                //List<Movie_Cleanup.Modules.MediaFile.Album> Albums = new List<Movie_Cleanup.Modules.MediaFile.Album>();

                //DataTable dtArtist = mysqlDatabase.GetDataTable(String.Format("Select * from Artist where ArtistId = {0}", artistId));
                //Modules.MediaFile.Artist newArtist = new Modules.MediaFile.Artist();
                //foreach (DataRow artistRow in dtArtist.Rows)
                //{
                //    newArtist = MapDataTableToArtist(artistRow); 

                //    DataTable dtAlbum = mysqlDatabase.GetDataTable(String.Format("Select * from Album where ArtistId = {0}", artistId));
                //    foreach (DataRow albumRow in dtAlbum.Rows)
                //    {
                //        string albumId = albumRow["AlbumId"].ToString();
                //        Modules.MediaFile.Album newAlbum = MapDataTableToAlbum(albumRow, albumId);
                //        newAlbum.ArtistName = newArtist.ArtistName;

                //        DataTable dtSong = mysqlDatabase.GetDataTable(String.Format("Select * from Song where ArtistId = {0} and AlbumId = {1}", artistId, albumId));

                //        foreach (DataRow songRow in dtSong.Rows)
                //        {
                //            Movie_Cleanup.Modules.MediaFile.Song newMusicLibraryItem = MapDataTableToSong(songRow);
                //            newMusicLibraryItem.Album = newAlbum.AlbumName;
                //            newMusicLibraryItem.Artist = newArtist.ArtistName;
                //            newAlbum.Songs.Add(newMusicLibraryItem);
                //        }
                //        newArtist.Albums.Add(newAlbum);
                //        Albums.Add(newAlbum);
                //    }
                //}



                // MusicLibrary selectedArtist = selectedArtistControl.DataContext as MusicLibrary;
                List<ArtistViewControl> selectedLibraryItems = new List<ArtistViewControl>();
                //foreach (var currentArtist in Artists)
                //{
                //if (currentArtist.ArtistName == selectedArtist.ArtistName)
                //if (currentArtist.ArtistId == selectedArtist.ArtistId)
                //{
                ArtistViewControl item = new ArtistViewControl();

                List<Modules.MediaFile.Album> Albums = selectedArtist.Albums;

                Albums = Albums.GroupBy(i => i.AlbumId, (key, group) => group.First()).ToList();
                Albums = Albums.OrderBy(o => o.AlbumId).ToList();
                int albumCount = Albums.Count();

                for (int i = 0; i < albumCount; i++)
                //foreach (Modules.MediaFile.Album alb in selectedArtist.Albums)
                {
                    Modules.MediaFile.Album alb = Albums[i];
                    //foreach (var art in alb.Songs)
                    //{
                    //if (art.Artist == currentArtist.ArtistName && alb.AlbumName == art.Album)
                    //if (alb.AlbumId == art.AlbumId)
                    //{
                    //currentArtist.Albums = new List<Modules.MediaFile.Album>();
                    //currentArtist.Albums.Add(alb);
                    //newArtist.Albums.Add(alb);
                    item.SetAlbumBinding(alb);
                    item.DataContext = selectedArtist;//newArtist;
                    item.OnMediaChangedEvent += new ArtistViewControl.OnMediaChangedEventHandler(albumViewControl_OnSelectedMediaChanged);
                    selectedLibraryItems.Add(item);
                    try
                    {
                        lstvwLibraryView.Children.Add(item);
                    }
                    catch { }
                    //break;
                    //}
                    //}
                }
                //item.DataContext = selectedArtist;//newArtist;
                //item.OnMediaChangedEvent += new ArtistViewControl.OnMediaChangedEventHandler(albumViewControl_OnSelectedMediaChanged);
                //selectedLibraryItems.Add(item);
                //}
                //}
                //lstvwLibraryView.ItemsSource = selectedLibraryItems;//MusicLibraryItems;

            }
        }
        

        private void lstvwAllLibraryView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           

            AlbumControl selectedAlbumItem = lstvwAllLibraryView.SelectedItem as AlbumControl;
            if (selectedAlbumItem != null)
            {
               Modules.MediaFile.Album selectedAlbum = selectedAlbumItem.DataContext as Modules.MediaFile.Album;

                ////lstvwLibraryViewFiltered.Items.Add(selectedAlbum);
                ////lstvwAlbumTracks.ItemsSource = selectedAlbum.Tracks;
                ////lstvwLibraryViewFiltered.ItemsSource = selectedAlbum
                
                albvwcntrlAlbumDetails.DataContext = selectedAlbum;

                ////Storyboard sb = new Storyboard();

                ////DoubleAnimation slide = new DoubleAnimation();
                ////slide.To = 220.0;
                ////slide.From = 0;
                ////slide.Duration = new Duration(TimeSpan.FromMilliseconds(40.0));

                ////// Set the target of the animation
                ////Storyboard.SetTarget(slide, rowAlbumTracks);
                ////Storyboard.SetTargetProperty(slide, new PropertyPath("RenderTransform.(TranslateTransform.X)"));

                ////// Kick the animation off
                ////sb.Children.Add(slide);
                ////sb.Begin();

                //// rowAlbumTracks.BeginStoryboard((Storyboard)this.FindResource("SlideDownAnimation"));
                ////rowAlbumTracks.BeginStoryboard((Storyboard)this.FindResource("ShowChangeMusicPanel"));
                ////ShowChangeMusicPanel.Begin();
                GridLengthAnimation gla = new GridLengthAnimation();
                gla.From = new GridLength(0, GridUnitType.Star);
                gla.To = new GridLength(230, GridUnitType.Star);
                gla.Duration = new TimeSpan(0, 0,0,0,600);
                rowAlbumTracks.BeginAnimation(RowDefinition.HeightProperty, gla);
            }
            //rowAlbumTracks.Height = new GridLength(220);
            //double currentHeight = rowAlbumTracks.Height.Value;
            //while (rowAlbumTracks.Height.Value < rowAlbumTracks.MaxHeight)
            //{
            //    currentHeight += 20;
            //    rowAlbumTracks.Height = new GridLength(currentHeight);
            //    System.Threading.Thread.Sleep(100);
            //}
            //PopupTest.Placement = System.Windows.Controls.Primitives.PlacementMode.Mouse;
            ////PopupTest.Parent = lstvwAllLibraryView;
            //PopupTest.StaysOpen = false;
            //PopupTest.Height = 250;
            //PopupTest.Width = 600;
            //PopupTest.IsOpen = true;
        }

        private void btnClosePopUp_Click(object sender, RoutedEventArgs e)
        {
            //PopupTest.IsOpen = false;
            //Storyboard sb = new Storyboard();

            //DoubleAnimation slide = new DoubleAnimation();
            //slide.To = 0;
            //slide.From = 220.0;
            //slide.Duration = new Duration(TimeSpan.FromMilliseconds(40.0));

            //// Set the target of the animation
            //Storyboard.SetTarget(slide, rowAlbumTracks);
            //Storyboard.SetTargetProperty(slide, new PropertyPath("RenderTransform.(TranslateTransform.X)"));

            //// Kick the animation off
            //sb.Children.Add(slide);
            //sb.Begin();
            //rowAlbumTracks.Height = new GridLength(0);

            //this.BeginStoryboard((Storyboard)this.FindResource("SlideUpAnimation"));
            //this.BeginStoryboard((Storyboard)this.FindResource("ShowChangeMusicPanel"));
            
            //ShowChangeMusicPanel.Begin();
            //if (ChangePasswordPanel.Height == 0)
            //{
            //    ShowChangeMusicPanel.Begin();
            //}
            //else
            //{
            //    HideChangeMusicPanel.Begin();
            //}
    
            //double currentHeight = rowAlbumTracks.Height.Value;
            //while (rowAlbumTracks.Height.Value > rowAlbumTracks.MinHeight)
            //{
            //    currentHeight -= 20;
            //    rowAlbumTracks.Height = new GridLength(currentHeight);
            //    System.Threading.Thread.Sleep(100);
            //}

            GridLengthAnimation gla = new GridLengthAnimation();
            gla.From = new GridLength(230, GridUnitType.Star);
            gla.To = new GridLength(0, GridUnitType.Star);
            gla.Duration = new TimeSpan(0, 0, 0,0,600);
            rowAlbumTracks.BeginAnimation(RowDefinition.HeightProperty, gla);
        }

        private void lstvwAlbumTracks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //MusicLibrary selectedSong = lstvwAlbumTracks.SelectedItem as MusicLibrary;
            PlaySelectedFromLibrary(sender);
        }

        private void lstvwPlayList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Modules.MediaFile.Song selectedPlaylistSong = lstvwPlayList.SelectedItem as Movie_Cleanup.Modules.MediaFile.Song;
            //selectedPlaylistSong.PlayImageButton = imageFilePath + @"\playback_play.png";

            //((Modules.MediaFile.Song)lstvwPlayList.SelectedItem).PlayImageButton = imageFilePath + @"\playback_play.png";

            //foreach (Modules.MediaFile.Song item in lstvwPlayList.Items)
            //{
            //    if (item.Location != selectedPlaylistSong.Location)
            //    {
            //        item.PlayImageButton = imageFilePath + @"\Music.png";
            //    }
            //}
        }

        private void txtContents_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            
            //List<ArtistViewControl> selectedLibraryItems = new List<ArtistViewControl>();
            //foreach (var currentArtist in Artists)
            //{
            //    if (currentArtist.ArtistName.ToLower() == txtContents.Text.ToLower())
            //    {
            //        ArtistViewControl item = new ArtistViewControl();

            //        foreach (Modules.MediaFile.Album alb in Albums)
            //        {
            //            foreach (var art in alb.Songs)
            //            {
            //                if (art.Artist == currentArtist.ArtistName && alb.AlbumName == art.Album)
            //                {
            //                    currentArtist.Albums = new List<Modules.MediaFile.Album>();
            //                    //currentArtist.Albums.Add(alb);
            //                    currentArtist.Albums.Add(alb);
            //                    item.SetAlbumBinding(alb);
            //                    break;
            //                }
            //            }
            //        }
            //        item.DataContext = currentArtist;
            //        item.OnMediaChangedEvent += new ArtistViewControl.OnMediaChangedEventHandler(albumViewControl_OnSelectedMediaChanged);
            //        selectedLibraryItems.Add(item);
            //    }
            //}

            //if (selectedLibraryItems.Count > 0)
            //{
            //    lstvwLibraryViewFiltered.ItemsSource = selectedLibraryItems;
            //}
            //else
            //{
            //    lstvwLibraryViewFiltered.ItemsSource = LibraryItems;
            //}
        }

        //private void lstGenre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    string selectedGenre = lstGenre.SelectedItem as string;
        //    List<ArtistViewControl> selectedLibraryItems = new List<ArtistViewControl>();
        //    foreach (var currentArtist in Artists)
        //    {
                
        //            ArtistViewControl item = new ArtistViewControl();

        //            foreach (Album alb in Albums)
        //            {
        //                foreach (var art in alb.Tracks)
        //                {
        //                    if (selectedGenre.ToLower() == art.Genre.ToLower()
        //                        && art.Artist == currentArtist.ArtistName && alb.AlbumName == art.Album)
        //                    {
        //                        //if (art.Artist == currentArtist.ArtistName && alb.AlbumName == art.Album)
        //                        {
        //                            currentArtist.Albums = new List<Album>();
        //                            currentArtist.Albums.Add(alb);
        //                            item.SetAlbumBinding(alb);
        //                            break;
        //                        }
        //                    }
        //                }
        //            }
        //            item.DataContext = currentArtist;
        //            item.OnMediaChangedEvent += new ArtistViewControl.OnMediaChangedEventHandler(albumViewControl_OnSelectedMediaChanged);
        //            selectedLibraryItems.Add(item);
                
        //    }

        //    //if (selectedLibraryItems.Count > 0)
        //    //{
        //    //    lstvwLibraryViewFiltered.ItemsSource = selectedLibraryItems;
        //    //}
        //    //else
        //    //{
        //    //    lstvwLibraryViewFiltered.ItemsSource = LibraryItems;
        //    //}
        //}

        //private void lstArtist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    //ArtistAlbumViewControl selectedArtistItem = lstArtist.SelectedItem as ArtistAlbumViewControl;
        //    Artist selectedArtist = lstArtist.SelectedItem as Artist;
        //    // MusicLibrary selectedArtist = selectedArtistControl.DataContext as MusicLibrary;
        //    List<ArtistViewControl> selectedLibraryItems = new List<ArtistViewControl>();
        //    foreach (var currentArtist in Artists)
        //    {
        //        if (currentArtist.ArtistName == selectedArtist.ArtistName)
        //        {
        //            ArtistViewControl item = new ArtistViewControl();

        //            foreach (Album alb in Albums)
        //            {
        //                foreach (var art in alb.Tracks)
        //                {
        //                    if (art.Artist == currentArtist.ArtistName && alb.AlbumName == art.Album)
        //                    {
        //                        currentArtist.Albums = new List<Album>();
        //                        currentArtist.Albums.Add(alb);
        //                        item.SetAlbumBinding(alb);
        //                        break;
        //                    }
        //                }
        //            }
        //            item.DataContext = currentArtist;
        //            item.OnMediaChangedEvent += new ArtistViewControl.OnMediaChangedEventHandler(albumViewControl_OnSelectedMediaChanged);
        //            selectedLibraryItems.Add(item);
        //        }
        //    }
        //    //if (selectedLibraryItems.Count > 0)
        //    //{
        //    //    lstvwLibraryViewFiltered.ItemsSource = selectedLibraryItems;
        //    //}
        //    //else
        //    //{
        //    //    lstvwLibraryViewFiltered.ItemsSource = LibraryItems;
        //    //}
        //}

        public string dev_con_synclock { get; set; }

        private void lstbxtmVideo_Selected(object sender, RoutedEventArgs e)
        {
            tbAlbums.Visibility = System.Windows.Visibility.Collapsed;
            tbArtists.Visibility = System.Windows.Visibility.Collapsed;
            tbVideos.Visibility = System.Windows.Visibility.Visible;

            tbVideos.IsSelected = true;
        }
        private void lstbxtmMusic_Selected(object sender, RoutedEventArgs e)
        {
            tbAlbums.Visibility = System.Windows.Visibility.Visible;
            tbArtists.Visibility = System.Windows.Visibility.Visible;
            tbVideos.Visibility = System.Windows.Visibility.Collapsed;

            tbAlbums.IsSelected = true;
        }

        private void mnuCleanLibrary_Click(object sender, RoutedEventArgs e)
        {
            bool isDeleted = false;
            isDeleted = mysqlDatabase.ClearTable("Song");
            if (isDeleted)
            {
                isDeleted = mysqlDatabase.ClearTable("Album");
                if (isDeleted)
                {
                    isDeleted = mysqlDatabase.ClearTable("Artist");
                    if (isDeleted)
                    {
                        lstvwAllLibraryView.ItemsSource = null;
                        
                        lstvwArtistView.ItemsSource = null;
                        //lstvwLibraryView.ItemsSource = null;
                        lstvwLibraryView.Children.Clear();

                        System.Windows.MessageBox.Show("Done Cleaning Library, Please build a your new library");
                    }
                    else
                    {
                        System.Windows.MessageBox.Show(":( unable to perform clean up of your media Library, something is wrong");
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show(":( unable to perform clean up of your media Library, something is wrong");
                }
            }
            else
            {
                System.Windows.MessageBox.Show(":( unable to perform clean up of your media Library, something is wrong");
            }
        }

        private void lblSave_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void lblLoad_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void lblClear_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void lbl3D_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void lblNormal_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void btnMaximise_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Maximized;
        }

        private void btnMinimise_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void lstbxPlayLists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            lstvwPlayListViewer.ItemsSource = null;
            Playlist selectedPlayList = (lstbxPlayLists.SelectedItem as PlaylistControl).DataContext as Playlist;
            if (selectedPlayList.PlaylistFileName.Substring(selectedPlayList.PlaylistFileName.LastIndexOf(".")) == ".m3u")
            {
                var plylst = PlaylistManager.LoadPlaylist(selectedPlayList.PlaylistFileName);
                lstvwPlayListViewer.ItemsSource = plylst.Songs;
                tbPlaylist.Header = string.Format("Playlist ({0})", plylst.PlaylistName);
            }
            else
            {
                selectedPlayList.LoadPlaylist();
                lstvwPlayListViewer.ItemsSource = selectedPlayList.Songs;
                tbPlaylist.Header = string.Format("Playlist ({0})", selectedPlayList.PlaylistName);
            }

            
            tbPlaylist.Visibility = System.Windows.Visibility.Visible;
            tbPlaylist.Focus();
            //selectedPlayList = PlaylistManager.LoadPlaylist(selectedPlayList.PlaylistFileName);
        }

        private void lstvwVideoLibraryView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Video selectedVideo = (lstvwVideoLibraryView.SelectedItem as VideoControl).DataContext as Video;

            //musicplayer.PlayVideo(selectedVideo);

            ////musicplayer.Pause();

            ////VideoMediaControl.Source = new Uri(selectedVideo.VideoPath);
            ////VideoMediaControl.Play();

            //videoGrid.Height = new GridLength(220);

            //VideoDrawing drawing = new VideoDrawing();
            //drawing.Rect = new Rect(0, 0, 300, 200);
            //drawing.Player = musicplayer.VideoDisplay;
            ////player.Play();
            //musicplayer.Pause();
            //DrawingBrush brush = new DrawingBrush(drawing);
            //this.Background = brush;
        }

        private void lstvwVideoLibraryView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Video selectedVideo = (lstvwVideoLibraryView.SelectedItem as VideoControl).DataContext as Video;

            musicplayer.PlayVideo(selectedVideo);

            videoGrid.Height = new GridLength(220);
            lblArtist.Content = musicplayer.MediaInfo.Tags.Artist.Replace("\0", "").TrimEnd(' ') + " - " + musicplayer.MediaInfo.Tags.Title.Replace("\0", "");
            lblAlbum.Content = musicplayer.MediaInfo.Tags.Album.Replace("\0", "").TrimEnd(' ');

            //Ideally these things hould be done once but somehow they are coming pretty late from engine so we are doiing it here
            seekBar.Minimum = 0;
            seekBar.Maximum = (int)musicplayer.Duration; //Damn, double to int, i feel like killing myself
            timer.IsEnabled = true;

            timer.Start();
        }

        //private void lstvwVideoLibraryView_MouseDoubleClick(object sender, SelectionChangedEventArgs e)
        //{
        //    Video selectedVideo = (lstvwVideoLibraryView.SelectedItem as VideoControl).DataContext as Video;

        //    musicplayer.PlayVideo(selectedVideo);           

        //    videoGrid.Height = new GridLength(220);

        //}


      
    }
}
