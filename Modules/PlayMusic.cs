using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
//using Un4seen.Bass;
//using WPFSoundVisualizationLib;
using WMPLib;

namespace Movie_Cleanup.Modules
{


    public class PlayMusic //: ISpectrumPlayer
    {
        //private WMPLib.WindowsMediaPlayer wplayer0;
        private AxWMPLib.AxWindowsMediaPlayer wplayer = null;
        public MediaInfo MediaInfo;
        //public System.Timers.Timer playProgressTimer = new System.Timers.Timer(200);
        //private Timer playProgressTimer = new Timer(new TimerCallback(playProgressTimer));
        public RepeatModes RepeatMode = RepeatModes.All;
        public string DurationString = "";//, CurruntPositionString = "";
        public double Duration = 0;
        private double currentTrackPosition = 0;
        private bool paused = false;
        public string CurrentPlayList = null; 

        private int activeStreamHandle;
       private bool canPlay;
       private bool canPause;
       private bool canStop;

        private readonly int fftDataSize = (int)FFTDataSize.FFT2048;
      //private readonly int maxFFT = (int)(BASSData.BASS_DATA_AVAILABLE | BASSData.BASS_DATA_FFT2048);
      private int sampleFrequency = 44100;

      private MainWindow App { get; set; }

      public PlayMusic(ref MainWindow app)
      {
          App = app;
          this.InitMediaPlayer();
          wplayer.SendToBack();
          //wplayer = new WMPLib.WindowsMediaPlayer();
          //wplayer = app.axWmp as WMPLib.WindowsMediaPlayer;
          //wplayer.uiMode = "none";
          //wplayer.stretchToFit = true;

          //wplayer.settings.autoStart = false;
          //wplayer.currentPlaylist = null;
          wplayer.settings.volume = 50;
          //wplayer0.PlayStateChange += new WMPLib._WMPOCXEvents_PlayStateChangeEventHandler(Player_PlayStateChange);
          wplayer.settings.setMode("loop", true);
          //wplayer0.MediaError += new WMPLib._WMPOCXEvents_MediaErrorEventHandler(Player_MediaError);

          this.CurrentPlayList = "unknownPlaylist";
          //playProgressTimer.Enabled = false;
          //playProgressTimer.Elapsed += new System.Timers.ElapsedEventHandler(playProgressTimer_Tick);
          //playProgressTimer.AutoReset = true;
      }

      void InitMediaPlayer()
      {
          wplayer = App.formsHost.Child as AxWMPLib.AxWindowsMediaPlayer;
          wplayer.uiMode = "none";
          //wplayer.settings.setMode("loop", false);
          wplayer.stretchToFit = true;
          wplayer.enableContextMenu = false;
          wplayer.ErrorEvent += new EventHandler(player_ErrorEvent);
          wplayer.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(player_PlayStateChange);
          wplayer.ClickEvent += new AxWMPLib._WMPOCXEvents_ClickEventHandler(player_ClickEvent);
          //wplayer.DoubleClickEvent += new AxWMPLib._WMPOCXEvents_DoubleClickEventHandler(wplayer_DoubleClickEvent);
      }

      void player_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
      {
          if (e.newState == (int)WMPLib.WMPPlayState.wmppsMediaEnded)
          {
              SwitchToNormalScreen();
              Stop();
          }
          
          else if (e.newState == (int)WMPLib.WMPPlayState.wmppsStopped)
          {
              //Stop();
              this.IsPlaying = false;
              //playProgressTimer.Enabled = false;
              this.Close();
          }
          else if (e.newState == (int)WMPLib.WMPPlayState.wmppsPaused)
          {
              //Stop();
              //playProgressTimer.Enabled = false;
              double time = wplayer.Ctlcontrols.currentPosition; //return always 0 for you, because you pause first and after get the value
              wplayer.Ctlcontrols.pause();
              wplayer.Ctlcontrols.currentPosition = time;
              //this.controls.play();
              this.IsPlaying = false;
              this.IsPaused = true;
              paused = true;

              //this.Close();
          }
          else if (e.newState == (int)WMPLib.WMPPlayState.wmppsPlaying)
          {
              //SwitchToFullScreen();
              //PreviousPlayList();
              this.IsPaused = false;
              paused = false;

              this.CurrentlyPlayingSong = wplayer.currentMedia.sourceURL;

              ObservableCollection<string> items = new ObservableCollection<string>();
              //for (int i = 0; i < wplayer.currentPlaylist.count; i++)
              //{
              var media = wplayer.currentMedia;
              for (int j = 0; j < media.attributeCount; j++)
              {
                  string name = media.getAttributeName(j);
                  var dd = media.getItemInfo(name);
                  var d = media.isReadOnlyItem(name).ToString();
              }
              items.Add(media.name);
              //}
              string path = wplayer.currentMedia.sourceURL;
              if (ActiveStreamHandle != 0)
              {
                  CanPlay = true;
                  //return true;
              }
              else
              {
                  ActiveStreamHandle = 0;
                  CanPlay = false;
              }

              Play();

              this.TrackPosition = wplayer.Ctlcontrols.currentItem.attributeCount;
              Duration = wplayer.currentMedia.duration;
              DurationString = wplayer.currentMedia.durationString;
              this.IsPlaying = true;
             
              for (int i = 0; i < wplayer.currentPlaylist.count; i++)
              {
                  if (wplayer.currentMedia.sourceURL == wplayer.currentPlaylist.Item[i].sourceURL)
                  {//wplayer.currentMedia.get_isIdentical(pl.Item[i])){
                      CurrentPlayIndex = i;
                      break;
                  }
              }

              MediaInfo = new Modules.MediaInfo(wplayer.currentMedia.sourceURL);
              //playProgressTimer.Enabled = true;
          }
      }

      private void SwitchToFullScreen()
      {
          App.WindowStyle = WindowStyle.None;
          //this.WindowState = System.Windows.WindowState.Normal;
          App.WindowState = System.Windows.WindowState.Maximized;
      }

      private void SwitchToNormalScreen()
      {          
          App.WindowStyle = WindowStyle.SingleBorderWindow;
          App.WindowState = System.Windows.WindowState.Normal;
      }

      void player_ErrorEvent(object sender, EventArgs e)
      {
          //deal with error here
          //messageLabel.Content = "An error occured while trying to play the video.";
      }

    private enum UIMode
    {
        Normal,
        FullScreen
    }

    private UIMode PlayerScreenMode = UIMode.Normal;
    private void wplayer_DoubleClickEvent(object sender, AxWMPLib._WMPOCXEvents_ClickEvent e)
    {
        AxWMPLib.AxWindowsMediaPlayer player = sender as AxWMPLib.AxWindowsMediaPlayer;
        if (UIMode.FullScreen == PlayerScreenMode)
        {
            //SwitchToNormalScreen();
            player.fullScreen = false;
            //player.SendToBack;
            PlayerScreenMode = UIMode.Normal;
        }
        else
        {
            //SwitchToFullScreen();
            player.fullScreen = true;
            PlayerScreenMode = UIMode.FullScreen;
        }
    }

      void player_ClickEvent(object sender, AxWMPLib._WMPOCXEvents_ClickEvent e)
      {
          AxWMPLib.AxWindowsMediaPlayer player = sender as AxWMPLib.AxWindowsMediaPlayer;
          if (UIMode.FullScreen == PlayerScreenMode)
          {
              //SwitchToNormalScreen();
              player.fullScreen = false;
              PlayerScreenMode = UIMode.Normal;
          }
          else
          {
              //SwitchToFullScreen();
              player.fullScreen = true;
              PlayerScreenMode = UIMode.FullScreen;
          }
      }

      private void Player_PlayStateChange(int NewState)//(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)//
      {
          if ((WMPLib.WMPPlayState)NewState == WMPLib.WMPPlayState.wmppsMediaEnded)
          {
              Stop();
              if (RepeatMode == RepeatModes.All)
              {

              }
              else if (RepeatMode == RepeatModes.All)
              {

              }
              else if (RepeatMode == RepeatModes.All)
              {

              }
          }
          else if ((WMPLib.WMPPlayState)NewState == WMPLib.WMPPlayState.wmppsStopped)
          {
              //Stop();
              this.IsPlaying = false;
              //playProgressTimer.Enabled = false;
              this.Close();
          }
          else if ((WMPLib.WMPPlayState)NewState == WMPLib.WMPPlayState.wmppsPaused)
          {
              //Stop();
              //playProgressTimer.Enabled = false;
              //double time = wplayer0.controls.currentPosition; //return always 0 for you, because you pause first and after get the value
              //wplayer0.controls.pause();
              //wplayer0.controls.currentPosition = time;
              //this.controls.play();
              this.IsPlaying = false;
              this.IsPaused = true;
              paused = true;

              //this.Close();
          }
          else if ((WMPLib.WMPPlayState)NewState == WMPLib.WMPPlayState.wmppsPlaying)
          {
              //PreviousPlayList();
              this.IsPaused = false;
              paused = false;

              this.CurrentlyPlayingSong = wplayer.currentMedia.sourceURL;

              ObservableCollection<string> items = new ObservableCollection<string>();
              //for (int i = 0; i < wplayer.currentPlaylist.count; i++)
              //{
              var media = wplayer.currentMedia;
              for (int j = 0; j < media.attributeCount; j++)
              {
                  string name = media.getAttributeName(j);
                  var dd = media.getItemInfo(name);
                  var d = media.isReadOnlyItem(name).ToString();
              }
              items.Add(media.name);
              //}
              string path = wplayer.currentMedia.sourceURL;
              //ActiveStreamHandle = Bass.BASS_StreamCreateFile(path, 0, 0, BASSFlag.BASS_DEFAULT);
              //ActiveStreamHandle = Bass.BASS_StreamCreateFile(path, 0, 0, BASSFlag.BASS_STREAM_PRESCAN);    
              //ActiveStreamHandle = Bass.BASS_StreamCreateFile(path, 0, 0, BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_PRESCAN);            
              if (ActiveStreamHandle != 0)
              {
                  CanPlay = true;
                  //return true;
              }
              else
              {
                  ActiveStreamHandle = 0;
                  CanPlay = false;
              }

              Play();

              //this.TrackPosition = wplayer0.controls.currentItem.attributeCount;
              //Duration = wplayer0.currentMedia.duration;
              //DurationString = wplayer0.currentMedia.durationString;
              //CurruntPositionString = wplayer.controls.currentPositionString;
              //TimeSpan ts = TimeSpan.Parse(DurationString);
              //Milliseconds = ts.TotalMilliseconds;
              //MediaInfo = new MediaInfo(FileName);
              this.IsPlaying = true;
              if (ActiveStreamHandle != 0)
              {
                  //Bass.BASS_StreamFree(ActiveStreamHandle);
              }

              //for (int i = 0; i < wplayer0.currentPlaylist.count; i++)
              //{
              //    if (wplayer.currentMedia.sourceURL == wplayer.currentPlaylist.Item[i].sourceURL)
              //    {//wplayer.currentMedia.get_isIdentical(pl.Item[i])){
              //        CurrentPlayIndex = i;
              //        break;
              //    }
              //}

              MediaInfo = new Modules.MediaInfo(wplayer.currentMedia.sourceURL);
              //playProgressTimer.Enabled = true;
          }
      }

      private void Player_MediaError(object pMediaObject)
      {
          MessageBox.Show("Cannot play media file.");
          this.Close();
      }

      private static PlayMusic instance;
      private WMPLib.IWMPPlaylist pList;

      public WMPLib.IWMPPlaylist PlayList
      {
          set
          {
              pList = value;
          }          
          get
          {
              if (pList == null)
              {
                  initPlayList();
              }
              return pList;
          }
      }


      private void initPlayList()
      {
          IWMPPlaylistArray pListArray;

          // get the list by name first
          pListArray = wplayer.playlistCollection.getByName(this.CurrentPlayList);

          // if the name exists, use the existing playlist
          // otherwise, create a new one and set pList to it.
          if (pListArray != null && pListArray.count > 0)
              pList = pListArray.Item(0);
          else
              pList = wplayer.playlistCollection.newPlaylist(this.CurrentPlayList);
      }

      public void ControlPlaylist(string mediaUrl)
      {
          IWMPMedia newMedia;
          newMedia = wplayer.newMedia(mediaUrl);

          if (newMedia != null)
          {
              // access the playlist by the property name
              PlayList.appendItem(newMedia);
          }
      }

      //public static PlayMusic Instance
      //{
      //    get
      //    {
      //        if (instance == null)
      //            instance = new PlayMusic();
      //        return instance;
      //    }
      //}

      public void Stop()
      {
          if (ActiveStreamHandle != 0)
          {
              //Bass.BASS_ChannelStop(ActiveStreamHandle);
              //Bass.BASS_ChannelSetPosition(ActiveStreamHandle, ChannelPosition);
          }
          IsPlaying = false;
          CanStop = false;
          CanPlay = true;
          CanPause = false;

          wplayer.Ctlcontrols.stop();
          //wplayer.close();
          IsPlaying = false;
      }
  
      //public void Pause()
      //{
      //    if (IsPlaying && CanPause)
      //    {
      //        Bass.BASS_ChannelPause(ActiveStreamHandle);
      //       IsPlaying = false;
      //        CanPlay = true;
      //        CanPause = false;
      //    }
      //}
  
      public void Play()
      {
          if (CanPlay)
          {
              //PlayCurrentStream();
              IsPlaying = true;
              CanPause = true;
              CanPlay = false;
              CanStop = true;
          }
      }

      //private void PlayCurrentStream()
      //{
      //    // Play Stream
      //    if (ActiveStreamHandle != 0)
      //    {
      //        //Bass.BASS_ChannelPlay(ActiveStreamHandle, false);
      //    }
      //}

     // public int GetFFTFrequencyIndex(int frequency)
     // {
     //     //return Utils.FFTFrequency2Index(frequency, fftDataSize, sampleFrequency);
     //     return 0;
     // }
 
     //public bool GetFFTData(float[] fftDataBuffer)
     //{
     //    //return (Bass.BASS_ChannelGetData(ActiveStreamHandle, fftDataBuffer, maxFFT)) > 0;
     //    return true;
     //}


        public void CreateNewPlayList(string playlistName, List<MusicLibrary> Songs)
        {
            this.wplayer.settings.autoStart = false;
            // Create a new playlist
            //WMPLib.IWMPPlaylist oldplaylist;
            WMPLib.IWMPPlaylist newplaylist;
            //oldplaylist = Me.Player.newPlaylist("Original Sorted Playlist", "file:///c:\Users\Matt\Music\Playlists\One True Playlist.wpl")
            newplaylist = this.wplayer.newPlaylist("Smart Shuffled Playlist", "");

            // The value i will keep track of the number of songs left to copy,
            // which in turn helps us keep track of the range for valid random numbers.
            // For songsRemaining As Integer = numberOfSongs - 1 To 0 Step -1
            for (int i = 0; i < Songs.Count; i++)
            {
                WMPLib.IWMPMedia mediaItem = wplayer.newMedia(Songs[i].Location);
                newplaylist.appendItem(mediaItem);
            }
            // Pick a random song from whatever remains in the old list:
            //Dim SongToCopy As Integer = Microsoft.VisualBasic.Rnd() * songsRemaining
            //Dim mediaItem = oldplaylist.Item(SongToCopy)

            // Append it to the new list
            //newplaylist.appendItem(mediaItem);

            //' Remove it from the old list, which will have its count decrease
            //oldplaylist.removeItem(mediaItem)
            //Next
            this.wplayer.currentPlaylist = newplaylist;
        }

        public enum RepeatModes
        {
            None,
            Single,
            All
        };

        //public object GetVisualizationData()
        //{
        //    //WMPLib.IWMPSyncDevice.GetVisualizationData(visData);
        //}

        public double CurruntPosition
        {
            get
            {
                return wplayer.Ctlcontrols.currentPosition;
            }
            set //Set current position, perhaps from seekbar of UI
            {
                wplayer.Ctlcontrols.currentPosition = value;
            }
        }
        public string CurruntPositionString
        {
            get
            {
                return wplayer.Ctlcontrols.currentPositionString;
            }
            set //Set current position, perhaps from seekbar of UI
            {
                //wplayer.controls.currentPositionString = value;
            }
        }

        public ObservableCollection<string> PreviousPlayList()
        {
            ObservableCollection<string> items = new ObservableCollection<string>();
            for (int i = 0; i < wplayer.currentPlaylist.count; i++)
            {
                var media = wplayer.currentPlaylist.get_Item(i);
                for (int j = 0; j < media.attributeCount; j++)
                {                    
                    string name = media.getAttributeName(j);
                    var dd = media.getItemInfo(name);
                    var d = media.isReadOnlyItem(name).ToString();
                }
                items.Add(media.name);
            }
            return items;
        }

       

        public int ActiveStreamHandle
        {
            get { return activeStreamHandle; }
            protected set
            {
                int oldValue = activeStreamHandle;
                activeStreamHandle = value;
                if (oldValue != activeStreamHandle)
                    NotifyPropertyChanged("ActiveStreamHandle");
            }
        }  
      public bool CanPlay
      {
          get { return canPlay; }
          protected set
          {
              bool oldValue = canPlay;
             canPlay = value;
             if (oldValue != canPlay)
                 NotifyPropertyChanged("CanPlay");
         }
     } 
     public bool CanPause
     {
         get { return canPause; }
         protected set
         {
             bool oldValue = canPause;
             canPause = value;
             //if (oldValue != canPause)
                 //NotifyPropertyChanged("CanPause");
         }
     }
     public bool CanStop
     {
         get { return canStop; }
         protected set
         {
             bool oldValue = canStop;
             canStop = value;
             //if (oldValue != canStop)
              //   NotifyPropertyChanged("CanStop");
         }
     }  

        //[System.Runtime.InteropServices.DllImport("winmm.DLL", EntryPoint = "PlaySound", SetLastError = true, CharSet = CharSet.Unicode, ThrowOnUnmappableChar = true)]
        //private static extern bool PlaySound(string szSound, System.IntPtr hMod, PlaySoundFlags flags);

        //[System.Flags]
        //public enum PlaySoundFlags : int
        //{
        //    SND_SYNC = 0x0000,
        //    SND_ASYNC = 0x0001,
        //    SND_NODEFAULT = 0x0002,
        //    SND_LOOP = 0x0008,
        //    SND_NOSTOP = 0x0010,
        //    SND_NOWAIT = 0x00002000,
        //    SND_FILENAME = 0x00020000,
        //    SND_RESOURCE = 0x00040004
        //}
        
        private string cmd;
        private bool isOpen;
        //public bool IsPlaying;
        private string FileName;
        private bool isPlaying;
  
      #region INotifyPropertyChanged
      public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
     #endregion
 
     public bool IsPlaying
     {
         get { return isPlaying; }
         protected set
         {
             bool oldValue = isPlaying;
             isPlaying = value;
             if (oldValue != isPlaying)
                 NotifyPropertyChanged("IsPlaying");
             //positionTimer.IsEnabled = value;
         }
     }
     public bool IsPaused
     {
         get { return paused; }
         protected set
         {
             bool oldValue = paused;
             paused = value;
             if (oldValue != paused)
                 NotifyPropertyChanged("IsPaused");
             //positionTimer.IsEnabled = value;
         }
     }

        //[DllImport("winmm.dll")]
        //private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);
        public void Close()
        {
            cmd = "close MediaFile";
            //mciSendString(cmd, null, 0, IntPtr.Zero);
            isOpen = false;
        }
        public void Open(string sFileName)
        {
            FileName = sFileName;
            cmd = "open " + sFileName + " type mpegvideo alias MediaFile";
            //mciSendString(cmd, null, 0, IntPtr.Zero);
            isOpen = true;
            //MediaInfo.FileName = sFileName;
        }
        public void Play(bool loop)
        {
            if (isOpen)
            {
                //cmd = "play MediaFile";
                //mciSendString(cmd, null, 0, IntPtr.Zero);
                //if (loop)
                //    cmd = " REPEAT";
                //mciSendString(cmd, null, 0, IntPtr.Zero);

                //WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer();

                //wplayer.controls.stop();
                //wplayer.close();
                if (!paused && CurrentPlayList == null)//wplayer.currentPlaylist == null)
                {
                    wplayer.URL = FileName;
                }
                //if (wplayer.currentPlaylist == null)
                //else
                //{
                //    wplayer.controls.currentPosition = currentTrackPosition;
                //}
                wplayer.Ctlcontrols.play();
                IsPlaying = true;
                paused = false;
                
                MediaInfo = new Modules.MediaInfo(FileName);               
                //WMPLib.IWMPPlaylist playList = wplayer.mediaCollection.getAll();
                
            }
        }
        public void Pause()
        {
            if (IsPlaying && CanPause)
            {
                //Bass.BASS_ChannelPause(ActiveStreamHandle);
                IsPlaying = false;
                CanPlay = true;
                CanPause = false;
                IsPaused = true;
            }
            //if (isOpen)
            {
                //cmd = "play MediaFile";
                //mciSendString(cmd, null, 0, IntPtr.Zero);
                //if (loop)
                //    cmd = " REPEAT";
                //mciSendString(cmd, null, 0, IntPtr.Zero);
                paused = true;
                IsPaused = true;
                wplayer.Ctlcontrols.pause();
                
                //TrackPosition = wplayer.controls.currentPosition;
                //wplayer.controls.stop();
                //wplayer.close();
                IsPlaying = false;
                
            }
        }
        public void SetVolume(int newVolumeValue)
        {            
            wplayer.settings.volume = newVolumeValue; //(wplayer.settings.volume + 10);            
        }

        public void SetPlayPosition(double newPositionValue)
        {
            //double time = newPositionValue;//wplayer.controls.currentPosition; //return always 0 for you, because you pause first and after get the value
            //wplayer.controls.pause();

            wplayer.Ctlcontrols.currentPosition = newPositionValue;
            wplayer.Ctlcontrols.play();
            //wplayer.controls.currentPosition = newPositionValue; //(wplayer.settings.volume + 10);            
        }

        public void VolumeUp()
        {
            if (wplayer.settings.volume < 90)
            {
                wplayer.settings.volume = (wplayer.settings.volume + 10);
            }
        }

        public void VolumeDown()
        {
            if (wplayer.settings.volume > 1)
            {
                wplayer.settings.volume = (wplayer.settings.volume - (wplayer.settings.volume / 2));
            }
        }

        private double trackPosition = 0;
        public double TrackPosition
        {
            get
            {
                this.trackPosition = UpdatePlayProgress();
                return this.trackPosition;
            }
            set 
            {
                this.trackPosition = value;
                //wplayer.controls.currentPosition = this.trackPosition;
            }
        }

        private double UpdatePlayProgress()
        {
            //while (this.IsPlaying && this.isOpen)
            //{
            if (wplayer.currentMedia != null)
            {
                return wplayer.Ctlcontrols.currentPosition;
            }
            else
            {
                wplayer.Ctlcontrols.currentPosition = 0;
                return 0;
            }
            //}
        }
         
        internal void Repeat()
        {
            if (RepeatMode == RepeatModes.All)
            {
                RepeatMode = RepeatModes.None;
                wplayer.settings.setMode("loop", false);
            }
            else if (RepeatMode == RepeatModes.None)
            {
                RepeatMode = RepeatModes.Single;
                wplayer.settings.setMode("loop", false);
            }
            else if(RepeatMode == RepeatModes.Single)
            {
                RepeatMode = RepeatModes.All;
                wplayer.settings.setMode("loop", true);
            }
             //wplayer.
        }

        public bool IsMute = false;

        public  void Mute()
        {
            if (this.IsMute == true)
            {
                wplayer.settings.mute = false;
                this.IsMute = false;
            }
            else
            {
                wplayer.settings.mute = true;
                this.IsMute = true;
            }
        }
        //WMPLib.IWMPPlaylist pl;

        //WMPLib.IWMPPlaylistArray plItems;
        public void PlayAllMusic(ObservableCollection<Movie_Cleanup.Modules.MediaFile.Song> myLibrary, int selectesSong)
        {
            //string myPlaylist = "allmymusic";

            //WMPLib.IWMPPlaylist pl;

            WMPLib.IWMPPlaylistArray plItems;

            // get the list by name first
            //plItems = wplayer.playlistCollection.getByName(myPlaylist);

            //if (this.CurrentPlayList == "allmymusic")
            //{
            //    plItems = wplayer.playlistCollection.getByName(myPlaylist);

            //    // if the name exists, use the existing playlist
            //    // otherwise, create a new one and set pList to it.
            //    if (plItems != null && plItems.count > 0)// && myPlaylist == "allmymusic")
            //    {
            //        this.PlayList = plItems.Item(0);
            //        wplayer.playlistCollection.remove(this.PlayList);
            //        //pl = plItems.Item(0);
            //        //wplayer.playlistCollection.remove(pl);
            //        //pl = wplayer.playlistCollection.newPlaylist(myPlaylist);
            //        //pl.clear();
            //        this.PlayList.clear();
            //    }
            //    else
            //    {
            //        try
            //        {
            //            this.PlayList = wplayer.playlistCollection.newPlaylist(myPlaylist);//"playlist");
            //            //pl = wplayer.playlistCollection.newPlaylist(myPlaylist);//"playlist");
            //        }
            //        catch
            //        {
            //            //pl = plItems.Item(0);
            //            //wplayer.playlistCollection.remove(pl);
            //            //pl.clear();
            //            this.PlayList = plItems.Item(0);
            //            wplayer.playlistCollection.remove(this.PlayList);
            //            this.PlayList.clear();
            //            //pl = wplayer.playlistCollection.newPlaylist(myPlaylist);
            //        }//"Untitled playlist"); };

            //    }
                

            //    foreach (var item in myLibrary)
            //    {

            //        string musicFile01 = item.Location;
            //        if (System.IO.File.Exists(musicFile01))
            //        {
            //            WMPLib.IWMPMedia m1 = wplayer.newMedia(musicFile01);
            //            //pl.appendItem(m1);                    
            //            this.PlayList.appendItem(m1);
            //        }
            //    }
            //}
            //else
            {
                plItems = null;
                pList = null;
                //CurrentPlayList = myPlaylist;

                foreach (var item in myLibrary)
                {
                    string musicFile01 = item.Location;
                    if (System.IO.File.Exists(musicFile01))
                    {
                        ControlPlaylist(musicFile01);
                    }
                }

            }

            #region Old working code
            
            //// if the name exists, use the existing playlist
            //// otherwise, create a new one and set pList to it.
            //if (plItems != null && plItems.count > 0)// && myPlaylist == "allmymusic")
            //{
            //    this.PlayList = plItems.Item(0);
            //    wplayer.playlistCollection.remove(this.PlayList);
            //    //pl = plItems.Item(0);
            //    //wplayer.playlistCollection.remove(pl);
            //    //pl = wplayer.playlistCollection.newPlaylist(myPlaylist);
            //    //pl.clear();
            //    this.PlayList.clear();
            //}
            //else
            //{
            //    try
            //    {
            //        this.PlayList = wplayer.playlistCollection.newPlaylist(myPlaylist);//"playlist");
            //        //pl = wplayer.playlistCollection.newPlaylist(myPlaylist);//"playlist");
            //    }
            //    catch
            //    {
            //        //pl = plItems.Item(0);
            //        //wplayer.playlistCollection.remove(pl);
            //        //pl.clear();
            //        this.PlayList = plItems.Item(0);
            //        wplayer.playlistCollection.remove(this.PlayList);
            //        this.PlayList.clear();
            //        //pl = wplayer.playlistCollection.newPlaylist(myPlaylist);
            //    }//"Untitled playlist"); };

            //}
            ////plItems = wplayer.playlistCollection.getByName(myPlaylist);

            ////if (plItems.count == 0)
            ////{

            ////    pl = wplayer.playlistCollection.newPlaylist(myPlaylist);
            ////}
            ////else
            ////{
            ////    pl = wplayer.playlistCollection.newPlaylist(myPlaylist);
            ////    //pl = plItems.Item(0);
            ////}

            //foreach (var item in myLibrary)
            //{

            //    string musicFile01 = item.Location;
            //    if (System.IO.File.Exists(musicFile01))
            //    {
            //        WMPLib.IWMPMedia m1 = wplayer.newMedia(musicFile01);
            //        //pl.appendItem(m1);                    
            //        this.PlayList.appendItem(m1);
            //    }
            //}
            #endregion
            //if (pl.count == 0)
            if (this.PlayList.count == 0)
            {
                throw new Exception("Unable to Play, Media File could not be found");
            }
            if (selectesSong != 0)
            {
                wplayer.currentPlaylist = this.PlayList;
                //wplayer.currentPlaylist = pl;
                PlaySelected(selectesSong);
            }
            else if (myLibrary[0] != null)
            {
                wplayer.currentPlaylist = this.PlayList;
                //wplayer.currentPlaylist = pl;
                //this.Open(myLibrary[CurrentPlayIndex].Location);
                //this.Play(false);
                wplayer.Ctlcontrols.play();
                MediaInfo = new Modules.MediaInfo(wplayer.currentMedia.sourceURL);
                CurrentPlayIndex = 0;
            }
            
        }
        public int CurrentPlayIndex = 0;
        internal void PlayNext()
        {
            //CurrentPlayIndex++;
            //string nextFileUrl = wplayer.currentPlaylist.Item[currentPlayIndex].sourceURL;
            //this.Open(nextFileUrl);
            //this.Play(false);

            wplayer.Ctlcontrols.next();

            for (int i = 0; i < wplayer.currentPlaylist.count; i++)
            {
                if (wplayer.currentMedia.sourceURL == wplayer.currentPlaylist.Item[i].sourceURL)
                {//wplayer.currentMedia.get_isIdentical(pl.Item[i])){
                    CurrentPlayIndex = i;
                    break;
                }
            }
                   
            MediaInfo = new Modules.MediaInfo(wplayer.currentMedia.sourceURL);   
        }

        internal void PlayPrevious()
        {
            //if (currentPlayIndex > 0)
            //{
            //    currentPlayIndex--;
            //}
            //else
            //{
            //    currentPlayIndex = 0;
            //}
            //string nextFileUrl = wplayer.currentPlaylist.Item[currentPlayIndex].sourceURL;
            //this.Open(nextFileUrl);
            //this.Play(false);
            wplayer.Ctlcontrols.previous();

            for (int i = 0; i < wplayer.currentPlaylist.count; i++)
            {
                if (wplayer.currentMedia.sourceURL == wplayer.currentPlaylist.Item[i].sourceURL)
                //wplayer.currentMedia.get_isIdentical(pl.Item[i])){
                {
                    CurrentPlayIndex = i;
                }
            }

            MediaInfo = new Modules.MediaInfo(wplayer.currentMedia.sourceURL);
            //CurrentPlayIndex--;
        }

        private void PlaySelected(int selectesSong)
        {
           // WMPLib.IWMPMedia selectedMedia = wplayer.newMedia(selectesSong.Location);

            WMPLib.IWMPMedia media = wplayer.currentPlaylist.get_Item(selectesSong);
            CurrentPlayIndex = selectesSong;
            // Play the media item.
            wplayer.Ctlcontrols.playItem(media);
            //wplayer.controls.playItem(selectedMedia);
            //for (int i = 0; i < wplayer.currentPlaylist.count - 1; i++)
            //{
            //    if (selectedMedia == wplayer.currentPlaylist.Item[i])
            //    {
            //        CurrentPlayIndex = i;
            //        //wplayer.currentMedia = selectedMedia;
            //        wplayer.controls.playItem(selectedMedia);
            //        //wplayer.controls.
            //        //wplayer.controls.play();
            //        break;
            //    }
            //}
            //wplayer.controls.playItem(selectedMedia);
            MediaInfo = new Modules.MediaInfo(wplayer.currentMedia.sourceURL); 
        }
        public int StreamHandle
        {
            get
            {
                return int.Parse(MediaInfo.Tags.BitRate);
            }
        }


        public string CurrentlyPlayingSong { get; set; }

        public void Play(string fileName)
        {
                
                wplayer.URL = fileName;
               // WMPLib.WMPVideoCtrl videoControl = null;
                //videoControl = 


                wplayer.Ctlcontrols.play();
                IsPlaying = true;
                paused = false;
                //wplayer.fullScreen = true;
                MediaInfo = new Modules.MediaInfo(fileName);
                //WMPLib.IWMPPlaylist playList = wplayer.mediaCollection.getAll();

                
        }

        internal void PlayVideo(MediaFile.Video selectedVideo)
        {
            wplayer.URL = selectedVideo.VideoPath;
            // WMPLib.WMPVideoCtrl videoControl = null;
            //videoControl = 


            wplayer.Ctlcontrols.play();
            IsPlaying = true;
            paused = false;
            //wplayer.fullScreen = true;
            //MediaInfo = new Modules.MediaInfo(fileName);
            //WMPLib.IWMPPlaylist playList = wplayer.mediaCollection.getAll();
        }

        public System.Windows.Media.MediaPlayer VideoDisplay { get; set; }
    }

    
}
