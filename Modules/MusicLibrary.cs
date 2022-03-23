using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Movie_Cleanup.Modules
{
    [XmlInclude(typeof(System.Windows.Media.ImageSource))]
    [Serializable]
    public class Artist
    {
        public string ArtistName { get; set; }
        public List<Album> Albums { get; set; }
        
        [XmlIgnore]
        public System.Windows.Media.ImageSource ArtistArt { get; set; }
    }
    [XmlInclude(typeof(System.Windows.Media.ImageSource))]
    [Serializable]
    public class Album
    {
        public string AlbumName { get; set; }
        public List<MusicLibrary> Tracks { get; set; }

        [XmlIgnore]
        public System.Windows.Media.ImageSource AlbumArt { get; set; }

        public string Year { get; set; }
    }
    [XmlInclude(typeof(System.Windows.Media.ImageSource))]
    [Serializable]
    public class MusicLibrary
    {
        public string TAGID { get; set; }       //  3
        public string Title { get; set; }      //  30
        public string Artist { get; set; }     //  30 
        public string Album { get; set; }      //  30 
        public string Year { get; set; }        //  4 
        public string Comment { get; set; }   //  30 
        public string Genre { get; set; }
        public string AlbumArtist { get; set; }
        public string TrackNumber { get; set; }
        public string TotalTime { get; set; }
        public string Size { get; set; }
        public string Kind { get; set; }
        public string Composer { get; set; }
        public string DateModified { get; set; }
        public string DateAdded { get; set; }
        public string SampleRate { get; set; }
        public string BitRate { get; set; }
        public string Location { get; set; }
        public string PlayImageButton { get; set; }

        internal void AddMediaItem(MusicID3Tag tempTagger)
        {
            this.Album = tempTagger.Album.Replace("\0", ""); 
            this.Artist = tempTagger.Artist.Replace("\0", ""); 
            this.Title = tempTagger.Title.Replace("\0", ""); 
            this.Year = tempTagger.Year.Replace("\0", ""); ;
            this.Comment = tempTagger.Comment.Replace("\0", ""); 
            this.Genre = tempTagger.Genre.Replace("\0", "");
            this.Location = tempTagger.Location.Replace("\0", ""); 
            //this.Album = tempTagger.Album;
            //this.Album = tempTagger.Album;
            //this.Album = tempTagger.Album;
            //this.Album = tempTagger.Album;
            //this.Album = tempTagger.Album;

        }
        [XmlIgnore]
        public System.Windows.Media.ImageSource AlbumArt { get; set; }

        public string AddToPlaylistImageButton { get; set; }

        public string Disc { get; set; }

        public string Length { get; set; }
    }
}
