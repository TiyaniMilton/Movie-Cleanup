using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Movie_Cleanup.Modules
{
    public class DirScan
    {
        #region Fields
        private List<string> _files = new List<string>();
        #endregion

        #region Methods

        public string[] Browse(string directory)
        {
            DirectoryInfo dir;
            if (directory.Length == 0)
                dir = new DirectoryInfo(Directory.GetCurrentDirectory());
            else
            {

                if (directory[directory.Length - 1] == '"')
                    directory = directory.Substring(0, directory.Length - 1);

                dir = new DirectoryInfo(directory);
            }
            IterateFiles(dir);
            string[] fileArray = (string[])_files.ToArray();
            _files.Clear();
            return fileArray;
        }

        protected void IterateFiles(DirectoryInfo dir)
        {
            try
            {
                foreach (FileSystemInfo fileSystemInfo in dir.GetFileSystemInfos())
                {
                    

                    if (fileSystemInfo is FileInfo)
                    {
                        FileInfo fileInfo = (FileInfo)fileSystemInfo;
                        if (MediaFile.MediaItem.IsValidMediaItem(fileInfo.FullName))
                        //if (fileInfo.Extension.ToLower() == ".mp3" || fileInfo.Extension.ToLower() == ".wma"
                        //    || fileInfo.Extension.ToLower() == ".wav" || fileInfo.Extension.ToLower() == ".mp4"
                        //    || fileInfo.Extension.ToLower() == ".mov" || fileInfo.Extension.ToLower() == ".wmv"
                        //    || fileInfo.Extension.ToLower() == ".pmg" || fileInfo.Extension.ToLower() == ".avi" 
                        //    )
                        {
                            _files.Add(fileInfo.FullName);
                        }
                    }
                    else if (fileSystemInfo is DirectoryInfo)
                    {
                        try
                        {
                            IterateFiles((DirectoryInfo)fileSystemInfo);
                        }
                        catch { }
                    }
                }
            }
                    catch { }
        }
        #endregion
    }
}
