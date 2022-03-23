using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Mp3Lib;
using System.Windows.Forms;
using System.Drawing;


namespace Movie_Cleanup.Modules
{
    public class MusicID3Tag
    {

        private byte[] _TAGID = new byte[3];      //  3
        private byte[] _Title = new byte[30];     //  30
        private byte[] _Artist = new byte[30];    //  30 
        private byte[] _Album = new byte[30];     //  30 
        private byte[] _Year = new byte[4];       //  4 
        private byte[] _Comment = new byte[30];   //  30 
        private byte[] _Genre = new byte[1];      //  1

        public string TAGID = "";       //  3
        public string Title = "";      //  30
        public string Artist = "";     //  30 
        public string Album = "";      //  30 
        public string Year = "";        //  4 
        public string Comment = "";   //  30 
        public string Genre = "";
        public string Location = "";
        public Image AlbumArt;
        public string Disc = "";
        public string Composer = "";
        public TimeSpan? Length = new TimeSpan();
        public string Song = "";
        public string Track = "";
        public string BitRate = "";
        public uint BeatsPerMinute { get; set; }
        public string Lyrics { get; set; }
         public string AlbumArtists { get; set; }

         public int AudioSampleRate = 0;

        public MusicID3Tag GetMusicTags(string fileName)
        {
            string filePath = fileName;

            using (FileStream fs = File.OpenRead(filePath))
            {
                if (fs.Length >= 128)
                {
                    MusicID3Tag tag = new MusicID3Tag();
                    fs.Seek(-128, SeekOrigin.End);
                    fs.Read(tag._TAGID, 0, tag._TAGID.Length);
                    fs.Read(tag._Title, 0, tag._Title.Length);
                    fs.Read(tag._Artist, 0, tag._Artist.Length);
                    fs.Read(tag._Album, 0, tag._Album.Length);
                    fs.Read(tag._Year, 0, tag._Year.Length);
                    fs.Read(tag._Comment, 0, tag._Comment.Length);
                    fs.Read(tag._Genre, 0, tag._Genre.Length);
                    string theTAGID = Encoding.Default.GetString(tag._TAGID);

                    if (theTAGID.Equals("TAG"))
                    {
                         Title = Encoding.Default.GetString(tag._Title);
                         Artist = Encoding.Default.GetString(tag._Artist);
                         Album = Encoding.Default.GetString(tag._Album);
                         Year = Encoding.Default.GetString(tag._Year);
                         Comment = Encoding.Default.GetString(tag._Comment);
                         Genre = Encoding.Default.GetString(tag._Genre);
                         Location = filePath;

                        //Console.WriteLine(Title);
                        //Console.WriteLine(Artist);
                        //Console.WriteLine(Album);
                        //Console.WriteLine(Year);
                        //Console.WriteLine(Comment);
                        //Console.WriteLine(Genre);
                        //Console.WriteLine();
                    }
                }
            }
            return this;
        }

        public MusicID3Tag GetTags(string filename)
        {
            MusicID3Tag tag = new MusicID3Tag();
            
            try
            {
                var mediaFile = TagLib.File.Create(filename);
                Mp3File mp3File = null;
                try
                {
                    mp3File = new Mp3File(filename);
                    Title = (mediaFile.Tag.Title == null)? "" : mediaFile.Tag.Title;
                    Artist = (mediaFile.Tag.JoinedArtists == null) ? "" : mediaFile.Tag.JoinedArtists;
                    Album = (mediaFile.Tag.Album == null) ? "" : mediaFile.Tag.Album;
                    Year = (mediaFile.Tag.Year == null) ? "" : mediaFile.Tag.Year.ToString();
                    Comment = (mediaFile.Tag.Comment == null) ? "" : mediaFile.Tag.Comment;
                    Genre = (mediaFile.Tag.Genres.Count() == 0) ? "" : mediaFile.Tag.Genres[0];
                    Disc = (mediaFile.Tag.Disc == null) ? "" : mediaFile.Tag.Disc.ToString();
                    Composer = (mediaFile.Tag.Composers.Count() == 0)? "" : mediaFile.Tag.Title;
                    Length = mediaFile.Properties.Duration;
                    AlbumArt = mp3File.TagHandler.Picture;
                    Song = (mediaFile.Tag.Title == null)? "" : mediaFile.Tag.Title;
                    Track = mediaFile.Tag.Track.ToString();
                    BitRate = mediaFile.Properties.AudioBitrate.ToString();
                    BeatsPerMinute = mediaFile.Tag.BeatsPerMinute;
                    Lyrics = mediaFile.Tag.Lyrics;
                    AlbumArtists = (mediaFile.Tag.JoinedAlbumArtists == null) ? "" : mediaFile.Tag.JoinedAlbumArtists;
                    AudioSampleRate = mediaFile.Properties.AudioSampleRate;
                    
                    //Size = mediaFile.Properties.
                   
                    Location = filename;

                    // create mp3 file wrapper; open it and read the tags
                    //mp3File = new Mp3File(filename);

                    //Title = mp3File.TagHandler.Title;
                    //Artist = mp3File.TagHandler.Artist;
                    //Album = mp3File.TagHandler.Album;
                    //Year = mp3File.TagHandler.Year;
                    //Comment = mp3File.TagHandler.Comment;
                    //Genre = mp3File.TagHandler.Genre;
                    //Disc = mp3File.TagHandler.Disc;
                    //Composer = mp3File.TagHandler.Composer;
                    //Length = mp3File.TagHandler.Length;
                    //AlbumArt = mp3File.TagHandler.Picture;
                    //Song = mp3File.TagHandler.Song;
                    //Track = mp3File.TagHandler.Track;
                    //Location = filename;
                }
                catch (Exception e)
                {
                    //MessageBox.Show(e.Message, "Error Reading Tag");
                    //return;
                }
                return this;
            }
            catch
            {
                
                return null;
            }
            
            
            

            // create dialog and give it the ID3v2 block for editing
            // this is a bit sneaky; it uses the edit dialog straight out of TagScanner.exe as if it was a dll.
            //TagEditor.ID3AdapterEdit id3Edit = new TagEditor.ID3AdapterEdit(mp3File);

            //if (id3Edit.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    try
            //    {
            //        using (new CursorKeeper(Cursors.WaitCursor))
            //        {
            //            mp3File.Update();
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        ExceptionMessageBox.Show(_form, e, "Error Writing Tag");
            //    }
            //}
        }

        public void EditExtendedTag(string filename)
        {
            Mp3File mp3File = null;

            try
            {
                // create mp3 file wrapper; open it and read the tags
                mp3File = new Mp3File(filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message, "Error Reading Tag");
                return;
            }

            //if (mp3File.TagModel != null)
            //{
            //    // create dialog and give it the ID3v2 block for editing
            //    // this is a bit sneaky; it uses the edit dialog straight out of TagScanner.exe as if it was a dll.
            //    TagEditor.ID3PowerEdit id3PowerEdit = new TagEditor.ID3PowerEdit();
            //    id3PowerEdit.TagModel = mp3File.TagModel;
            //    id3PowerEdit.ShowDialog();
            //}
        }

        public void Compact(string filename)
        {
            try
            {
                // create mp3 file wrapper; open it and read the tags
                Mp3File mp3File = new Mp3File(filename);

                //try
                //{
                //    using (new CursorKeeper(Cursors.WaitCursor))
                //    {
                //        mp3File.UpdatePacked();
                //    }
                //}
                //catch (Exception e)
                //{
                //    ExceptionMessageBox.Show(_form, e, "Error Writing Tag");
                //}
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error Reading Tag");
            }
        }



       
    }
}
