using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Movie_Cleanup.Modules
{
    public class MediaInfo
    {
        public string TrackNumber { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Duration { get; set; }
        public string FileName { get; set; }
        public string Genre { get; set; }
        public string Type { get; set; }
        public string Plays { get; set; }
        public string Rating { get; set; }
        public MusicID3Tag Tags = new MusicID3Tag();

        public string TrackName { get; set; }

        public MediaInfo(string sFileName)
        {
            Tags = Tags.GetTags(sFileName);
        }
    }
}
