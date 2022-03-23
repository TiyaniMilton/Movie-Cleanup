using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections;
using System.Xml;
using System.IO;

namespace Movie_Cleanup.Modules
{
    [Serializable]
    public class Playlist
    {
        public string PlaylistName { get; set; }
        private string playlistFileName = string.Empty;
        private string DefaultMusicFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        public string PlaylistFileName
        {
            get { return this.playlistFileName; }
            set
            {
                //string[] array1 = Directory.GetFiles(DefaultMusicFolder + @"\\Playlists", "*.wpl");
                //readPlaylist playlistReader = new readPlaylist();
                //playlistReader.playListPath = DefaultMusicFolder + @"\Playlists\" + value;
                this.playlistFileName = DefaultMusicFolder + @"\Playlists\" + value;
                //List<string> playlistFiles = playlistReader.PlayList;
                //this.Songs = PlaylistManager.LoadPlaylist(playlistFiles);
            }
        }
        public string PlaylistArt { get; set; }
        public ObservableCollection<Movie_Cleanup.Modules.MediaFile.Song> Songs { get; set; }
        public int SelectedIndex { get; set; }

        public void LoadPlaylist()
        {
            readPlaylist playlistReader = new readPlaylist();
            playlistReader.playListPath = this.playlistFileName; 
            List<string> playlistFiles = playlistReader.PlayList;
            this.Songs = new ObservableCollection<MediaFile.Song>();
            this.Songs = PlaylistManager.LoadPlaylist(playlistFiles);
        }

        public Playlist()
        {
            this.Songs = new ObservableCollection<MediaFile.Song>();

            
        }
    }

    class readPlaylist
    {
        //private ArrayList name = new ArrayList();
        private List<string> name = new List<string>();
        //public ArrayList fileListName;
        private string m_xmlFile;

        /// <SUMMARY>
        /// The Windows Media Playlist Path xxx.wpl file
        /// </SUMMARY>
        public string playListPath
        {
            get
            {
                return m_xmlFile;
            }
            set
            {
                m_xmlFile = value;
                Makeplaylist();
            }
        }

        /// <SUMMARY>
        /// Return an Arraylist of file found in Windows Media Playlist file
        /// </SUMMARY>
        public List<string> PlayList
        {
            get
            {
                return name;
            }
        }

        /// <SUMMARY>
        /// Fills up an Arraylist with titles found in the
        /// Windows Media Playlist file.
        /// Using XmlTextReader
        /// </SUMMARY>
        private void Makeplaylist()
        {
            XmlTextReader readList = new XmlTextReader(m_xmlFile);
            while (readList.Read())
            {
                if (readList.NodeType == XmlNodeType.Element)
                {
                    if (readList.LocalName.Equals("media"))
                    {
                        name.Add(readList.GetAttribute(0).ToString());
                    }
                }
            }
        }

        private void loadLista()
        {
            //Lista.Rows.Clear();


            //if (fileDlg.FileName.Substring(fileDlg.FileName.LastIndexOf(".")) == ".m3u")
            //{
                string fileName = null;
                FileStream fStream = null;
                StreamReader sReader = null;

                try
                {
                    fStream = new FileStream(m_xmlFile, FileMode.Open, FileAccess.Read);
                    sReader = new StreamReader(fStream);

                    while ((fileName = sReader.ReadLine()) != null)
                    {
                        if (fileName.Length > 0 && fileName.Substring(0, 1) != "#" && fileName.Substring(0, 1) != "\n") //Checks whether the first character of the line is not # or Enter
                        {
                            string[] row1 = { "false", fileName.Substring(fileName.LastIndexOf("\\") + 1), fileName.Substring(0, fileName.LastIndexOf("\\")) }; //Stores the song details in string array so that it can be added to the Grid
                            //Lista.Rows.Add(row1);
                            //name.Add(row1);
                        }
                    }
                    fStream.Close();
                    sReader.Close();
                }
                catch (Exception ex)
                {
                    //errLog = ex.Message;

                    if (fStream != null)
                        fStream.Close();

                    if (sReader != null)
                        sReader.Close();
                }

            //}
            //return;
        }

    }
}
