using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;
using System.Diagnostics;

namespace Movie_Cleanup.Modules.MediaFile
{

    public class MediaItem
    {
        public enum MediaType
        {
            Video,
            Music,
            Picture,
            Book,
            Unknown
        }

        public static MediaType MediaTypeValidator(string fileName)
        {
            MediaType mediaType = MediaType.Unknown;
            if (allowableVideoMediaTypes.Contains(System.IO.Path.GetExtension(fileName).ToLowerInvariant()))
            {
                mediaType = MediaType.Video;
            }
            else if (allowableAudioMediaTypes.Contains(System.IO.Path.GetExtension(fileName).ToLowerInvariant()))
            {
                mediaType = MediaType.Music;
            }
            else if (allowablePictureMediaTypes.Contains(System.IO.Path.GetExtension(fileName).ToLowerInvariant()))
            {
                mediaType = MediaType.Picture;
            }
            else if (allowableBookMediaTypes.Contains(System.IO.Path.GetExtension(fileName).ToLowerInvariant()))
            {
                mediaType = MediaType.Book;
            }
            else 
            {
                mediaType = MediaType.Unknown;
            }
            return mediaType;
        }

        /// <summary>
        /// check to see if dragged items are valid
        /// </summary>
        /// <returns>true if filename is valid</returns>
        public static bool IsValidMediaItem(string filename)
        {
            bool isValid = false;
            string fileExtesion = filename.Substring(filename.LastIndexOf(".")).ToLower();
            foreach (string s in MediaItem.allowableMediaTypes)
            {
                if (s.Equals(fileExtesion, StringComparison.CurrentCultureIgnoreCase))
                    isValid = true;
            }
            return isValid;
        }

        public static string[] allowableMediaTypes = 
                                { 
                                    ".mpg", ".mpeg", ".m1v", ".mp2", ".mpa", ".mov",
                                    ".mpe", ".avi",".wmv",".mp4",".flv",".mkv",".vob",
                                    ".mp3", ".wma", ".ogg", ".wav",
                                    ".png", ".jpg", ".jpeg", ".bmp",
                                    ".pdf"
                                };

        public static string[] allowableVideoMediaTypes = 
                                { 
                                    ".mpg", ".mpeg", ".m1v", ".mp2", ".mpa", ".mov",
                                    ".mpe", ".avi",".wmv",".mp4",".flv",".mkv",".vob"
                                };

        public static string[] allowableAudioMediaTypes = 
                                { 
                                    ".mp3", ".wma", ".ogg", ".wav"
                                    //, ".mpa", 
                                    //".mpe", ".avi",".wmv"
                                };
        public static string[] allowablePictureMediaTypes = 
                                { 
                                    ".png", ".jpg", ".jpeg", ".bmp"
                                    //, ".mpa", 
                                    //".mpe", ".avi",".wmv"
                                };

        public static string[] allowableBookMediaTypes = 
                                { 
                                    ".pdf"
                                    //, ".mpeg", ".m1v", ".mp2", ".mpa", 
                                    //".mpe", ".avi",".wmv"
                                };
    }
    [Serializable]
    [XmlRoot("MediaLibrary")]
    public class MediaLibrary
    {
        public MediaLibrary()
        {
            this.Music = new List<Artist>();
            this.Books = new List<Book>();
            this.Videos = new List<Video>();
            this.Playlists = new List<Playlist>();
        }
        [XmlArray("Music")]
        public List<Artist> Music { get; set; }
        [XmlArray("Playlists")]
        public List<Playlist> Playlists{ get; set; }
        [XmlArray("Videos")]
        public List<Video> Videos { get; set; }
        [XmlArray("Books")]
        public List<Book> Books { get; set; }
    }
    [Serializable]
    public class Books : List<Book> 
    {
        
    }
    [Serializable]
    public class Book
    {
        public string Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Price { get; set; }
        public string PublishDate { get; set; }
        public string Description { get; set; }
        public string Rating { get; set; }
        public string Category { get; set; }
        public string BookArt { get; set; }
    }
   
    [Serializable]
    public class Video 
    {
        private string currentFilePath;
        private string albumArtworkFilePath = AppDomain.CurrentDomain.BaseDirectory + @"\Album Artwork\";
        private string _Year = "";
        private string _Cast = "";
        private string _Category = "";
        private string _VideoArt = "";
        private string _Rating = "";
        private string _Comments = "";
        private string _Description = "";
        private string _Plot = "";

        public Video(string currentFilePath)
        {
            // TODO: Complete member initialization
            this.currentFilePath = currentFilePath;
            
            this.VideoName = System.IO.Path.GetFileNameWithoutExtension(this.currentFilePath);
            string videoid = albumArtworkFilePath + VideoName.FixWordCasing() + ".png";
            var img = GetThumbnail(currentFilePath, videoid);
            this.VideoPath = this.currentFilePath;
            this.VideoArt = videoid;
        }

        public Video()
        {
            // TODO: Complete member initialization
        }
        public string VideoId { get; set; }
        public string VideoName { get; set; }
        public string VideoPath { get; set; }
        public string Year { get { return this._Year; } set { if (value == null) { this._Year = ""; } else { this._Year = value; } } }
        public string Cast { get { return this._Cast; } set { if (value == null) { this._Cast = ""; } else { this._Cast = value; } } }
        public string Category { get { return this._Category; } set { if (value == null) { this._Category = ""; } else { this._Category = value; } } }
        public string VideoArt { get { return this._VideoArt; } set { if (value == null) { this._VideoArt = ""; } else { this._VideoArt = value; } } }
        public string Rating { get { return this._Rating; } set { if (value == null) { this._Rating = ""; } else { this._Rating = value; } } }
        public string Comments { get { return this._Comments; } set { if (value == null) { this._Comments = ""; } else { this._Comments = value; } } }
        public string Description { get { return this._Description; } set { if (value == null) { this._Description = ""; } else { this._Description = value; } } }
        public string Plot { get { return this._Plot; } set { if (value == null) { this._Plot = ""; } else { this._Plot = value; } } }
        //public string VideoName { get; set; }

        public Bitmap GetThumbnail(string video, string thumbnail)
        {
            var cmd = "ffmpeg  -itsoffset -1  -i " + '"' + video + '"' + " -vcodec mjpeg -vframes 1 -an -f rawvideo -s 320x240 " + '"' + thumbnail + '"';

            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = "/C " + cmd
            };

            var process = new Process
            {
                StartInfo = startInfo
            };

            process.Start();
            process.WaitForExit(5000);

            return LoadImage(thumbnail);
        }

        public Bitmap LoadImage(string path)
        {
            var ms = new MemoryStream(File.ReadAllBytes(path));
            return (Bitmap)System.Drawing.Image.FromStream(ms);
        }
    }

    [Serializable]
    //[XmlElement("Artist")]
    public class Artist
    {
        public Artist()
        {
            this.Albums = new List<Album>();

        }

        private string _ArtistName = "";
        private string _ArtistArt = "";
        [XmlIgnore]
        public int ArtistId { get; set; }
        public string ArtistName { get { return this._ArtistName; } set { if (value == null) { this._ArtistName = ""; } else { this._ArtistName = value; } } }
        public string ArtistArt { get { return this._ArtistArt; } set { if (value == null) { this._ArtistArt = ""; } else { this._ArtistArt = value; } } }
        [XmlArray("Albums")]
        public List<Album> Albums { get; set; }
    }
    [Serializable]
    //[XmlElement("Album")]
    public class Album 
    {
        public Album()
        {
            this.Songs = new List<Song>();
        }
        private string _AlbumName = "";
        private string _AlbumArt = "";
        private string _ArtistName = "";
        private string _Year = "";

        [XmlIgnore]
        public int AlbumId { get; set; }
        public string AlbumName { get { return this._AlbumName; } set { if (value == null) { this._AlbumName = ""; } else { this._AlbumName = value; } } }
        public string AlbumArt { get { return this._AlbumArt; } set { if (value == null) { this._AlbumArt = ""; } else { this._AlbumArt = value; } } }
        public string ArtistName { get { return this._ArtistName; } set { if (value == null) { this._ArtistName = ""; } else { this._ArtistName = value; } } }
        public string Year { get { return this._Year; } set { if (value == null) { this._Year = ""; } else { this._Year = value; } } }
        [XmlArray("Songs")]
        public List<Song> Songs { get; set; }
        [XmlIgnore]
        public int ArtistId { get; set; }
    }
    
    [Serializable]
    //[XmlElement("Song")]
    public class Song
    {
        private string _SongArt = "";//SongArt set; SongArtSongArt}
        private string _TAGID = "";//TAGID set; TAGIDTAGID}
        private string _Title = "";//Title set; TitleTitle}
        private string _Artist = "";//Artist set; ArtistArtist}
        private string _Album = "";//Album set; AlbumAlbum}
        private string _Year = "";//SongArt set; YearYear}
        private string _Comment = "";//Comment set; CommentComment}
        private string _Genre = "";//Genre set; GenreGenre}
        private string _Rating = "";//Rating set; RatingRating}
        private string _AlbumArtist = "";//AlbumArtist set; AlbumArtistAlbumArtist}
        private string _TrackNumber = "";//TrackNumber set; TrackNumberTrackNumber}
        private string _TotalTime = "";//TotalTime set; TotalTimeTotalTime}
        private string _Size = "";//Size set; SizeSize}
        private string _Kind = "";//Kind set; KindKind}
        private string _Composer = "";//Composer set; ComposerComposer}
        private string _DateModified = "";//DateModified set; DateModifiedDateModified}
        private string _DateAdded = "";//DateAdded set; DateAddedDateAdded}
        private int _SampleRate = 0;//SampleRate set; SampleRateSampleRate}
        private string _BitRate = "";//BitRate set; BitRateBitRate}
        private string _Location = "";//Location set; LocationLocation}
        private string _PlayImageButton = "";//PlayImageButton set; PlayImageButtonPlayImageButton}
        private string _AddToPlaylistImageButton = "";//AddToPlaylistImageButton set; AddToPlaylistImageButtonAddToPlaylistImageButton}
        private string _Disc = "";
        private string _Length = "";
        private string _Lyrics = "";
        private uint _BeatsPerMinute = 0;

        public string FileName { get; set; }
        public string Fullpath { get; set; }
        public string SongArt { get { return this._SongArt; } set { if (value == null) { this._SongArt = ""; } else { this._SongArt = value; } } }
        public string TAGID { get { return this._TAGID; } set { if (value == null) { this._TAGID = ""; } else { this._TAGID = value; } } }
        public string Title { get { return this._Title; } set { if (value == null) { this._Title = ""; } else { this._Title = value; } } }
        public string Artist { get { return this._Artist; } set { if (value == null) { this._Artist = ""; } else { this._Artist = value; } } }
        public string Album { get { return this._Album; } set { if (value == null) { this._Album = ""; } else { this._Album = value; } } }
        public string Year { get { return this._Year; } set { if (value == null) { this._Year = ""; } else { this._Year = value; } } }
        public string Comment { get { return this._Comment; } set { if (value == null) { this._Comment = ""; } else { this._Comment = value; } } }
        public string Genre { get { return this._Genre; } set { if (value == null) { this._Genre = ""; } else { this._Genre = value; } } }
        public string Rating { get { return this._Rating; } set { if (value == null) { this._Rating = ""; } else { this._Rating = value; } } }
        public string AlbumArtist { get { return this._AlbumArtist; } set { if (value == null) { this._AlbumArtist = ""; } else { this._AlbumArtist = value; } } }
        public string TrackNumber { get { return this._TrackNumber; } set { if (value == null) { this._TrackNumber = ""; } else { this._TrackNumber = value; } } }
        public string TotalTime { get { return this._TotalTime; } set { if (value == null) { this._TotalTime = ""; } else { this._TotalTime = value; } } }
        public string Size { get { return this._Size; } set { if (value == null) { this._Size = ""; } else { this._Size = value; } } }
        public string Kind { get { return this._Kind; } set { if (value == null) { this._Kind = ""; } else { this._Kind = value; } } }
        public string Composer { get { return this._Composer; } set { if (value == null) { this._Composer = ""; } else { this._Composer = value; } } }
        public string DateModified { get { return this._DateModified; } set { if (value == null) { this._DateModified = ""; } else { this._DateModified = value; } } }
        public string DateAdded { get { return this._DateAdded; } set { if (value == null) { this._DateAdded = ""; } else { this._DateAdded = value; } } }
        public int SampleRate { get { return this._SampleRate; } set { if (value == null) { this._SampleRate = 0; } else { this._SampleRate = value; } } }
        public string BitRate { get { return this._BitRate; } set { if (value == null) { this._BitRate = ""; } else { this._BitRate = value; } } }
        public string Location { get { return this._Location; } set { if (value == null) { this._Location = ""; } else { this._Location = value; } } }
        public string PlayImageButton { get { return this._PlayImageButton; } set { if (value == null) { this._PlayImageButton = ""; } else { this._PlayImageButton = value; } } }
        public string AddToPlaylistImageButton { get { return this._AddToPlaylistImageButton; } set { if (value == null) { this._AddToPlaylistImageButton = ""; } else { this._AddToPlaylistImageButton = value; } } }
        public string Disc { get { return this._Disc; } set { if (value == null) { this._Disc = ""; } else { this._Disc = value; } } }
        public string Length { get { return this._Length; } set { if (value == null) { this._Length = ""; } else { this._Length = value; } } }
        [XmlIgnore]
        public int ArtistId { get; set; }
        [XmlIgnore]
        public int AlbumId { get; set; }

        public string Lyrics { get { return this._Lyrics; } set { if (value == null) { this._Lyrics = ""; } else { this._Lyrics = value; } } }

        public uint BeatsPerMinute { get { return this._BeatsPerMinute; } set { if (value == null) { this._BeatsPerMinute = 0; } else { this._BeatsPerMinute = value; } } }
    }
    
}
