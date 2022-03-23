using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;
using System.Windows;

namespace Movie_Cleanup.Modules
{
    internal class SQLiteDatabase
    {
        String dbConnection;
        String _dbName;
        /// <summary>     
        ///     Default Constructor for SQLiteDatabase Class. 013    
        /// </summary> 014    
        public SQLiteDatabase(string dbName)
        {
            dbConnection = string.Format("Data Source={0}", dbName);
            _dbName = dbName;
        }
        /// <summary>     
        ///     Single Param Constructor for specifying the DB file.     
        /// </summary> 
        /// <param name="inputFile">The File containing the DB</param> 
        //public SQLiteDatabase(String inputFile)
        //{
        //    dbConnection = String.Format("Data Source={0}", inputFile);
        //}
        /// <summary> 
        ///  Single Param Constructor for specifying advanced connection options.   
        /// </summary>     
        /// <param name="connectionOpts">A dictionary containing all desired options and their values</param> 
        /// public SQLiteDatabase(Dictionary<String, String> connectionOpts) 033    { 034        String str = ""; 035        foreach (KeyValuePair<String, String> row in connectionOpts) 036        { 037            str += String.Format("{0}={1}; ", row.Key, row.Value); 038        } 039        str = str.Trim().Substring(0, str.Length - 1); 040        dbConnection = str; 041    } 042 043    /// <summary> 044    ///     Allows the programmer to run a query against the Database. 045    
        /// </summary> 
        /// <param name="sql">The SQL to run</param>     
        /// <returns>A DataTable containing the result set.</returns>     
        public DataTable GetDataTable(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                SQLiteConnection cnn = new SQLiteConnection(dbConnection);
                cnn.Open();
                SQLiteCommand mycommand = new SQLiteCommand(cnn);
                mycommand.CommandText = sql;
                SQLiteDataReader reader = mycommand.ExecuteReader();
                dt.Load(reader);
                reader.Close();
                cnn.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return dt;
        }
        /// <summary>    
        ///     Allows the programmer to interact with the database for purposes other than a query.     
        /// </summary> 072    /// <param name="sql">The SQL to be run.</param>     
        /// <returns>An Integer containing the number of rows updated.</returns>     
        public int ExecuteNonQuery(string sql)
        {
            SQLiteConnection cnn = new SQLiteConnection(dbConnection);
            cnn.Open();
            SQLiteCommand mycommand = new SQLiteCommand(cnn);
            mycommand.CommandText = sql;
            int rowsUpdated = mycommand.ExecuteNonQuery();
            cnn.Close();
            return rowsUpdated;
        }
        /// <summary> 
        /// Allows the programmer to retrieve single items from the DB. 
        /// </summary>     
        /// <param name="sql">The query to run.</param> 
        /// <returns>A string.</returns> 
        public string ExecuteScalar(string sql)
        {
            SQLiteConnection cnn = new SQLiteConnection(dbConnection);
            cnn.Open();
            SQLiteCommand mycommand = new SQLiteCommand(cnn);
            mycommand.CommandText = sql;
            object value = mycommand.ExecuteScalar();
            cnn.Close(); if (value != null)
            {
                return value.ToString();
            }
            return "";
        }
        /// <summary> 
        /// Allows the programmer to easily update rows in the DB.     
        /// </summary>     
        /// <param name="tableName">The table to update.</param> 
        /// <param name="data">A dictionary containing Column names and their new values. </param> 
        /// <param name="where">The where clause for the update statement.</param> 
        /// <returns>A boolean true or false to signify success or failure.</returns> 
        public bool Update(String tableName, Dictionary<String, String> data, String where)
        {
            String vals = "";
            Boolean returnCode = true;
            if (data.Count >= 1)
            {
                foreach (KeyValuePair<String, String> val in data)
                {
                    vals += String.Format(" {0} = '{1}',", val.Key.ToString(), val.Value.ToString());
                }
                vals = vals.Substring(0, vals.Length - 1);
            }
            try
            {
                this.ExecuteNonQuery(String.Format(" {0} set {1} where {2};", tableName, vals, where));
            }
            catch
            {
                returnCode = false;
            }
            return returnCode;
        }

        /// <summary> 
        ///  Allows the programmer to easily delete rows from the DB. 
        /// </summary> 138    /// <param name="tableName">The table from which to de
        /// <returns>A boolean true or false to signify success or failure.</returns> 
        public bool Delete(String tableName, String where)
        {
            Boolean returnCode = true;
            try
            {
                this.ExecuteNonQuery(String.Format("from {0} where {1};", tableName, where));
            }

            catch (Exception fail)
            {
                MessageBox.Show(fail.Message);
                returnCode = false;
            }
            return returnCode;
        }
        /// <summary> 
        /// Allows the programmer to easily insert into the DB 
        /// </summary> 
        /// <param name="tableName">The table into which we insert the data.</param> 
        /// <param name="data">A dictionary containing the column names and data for the insert.</param> 
        /// <returns>A boolean true or false to signify success or failure.</returns> 
        public bool Insert(String tableName, Dictionary<String, Object> data)
        {
            String columns = "";
            String values = "";
            Boolean returnCode = true;
            foreach (KeyValuePair<String, Object> val in data)
            {
                columns += String.Format(" {0},", val.Key.ToString());
                
                if ((val.Value).GetType() != typeof(string))
                {
                    values += String.Format(" {0},", val.Value);
                }
                else
                {
                    values += String.Format(" '{0}',", val.Value.ToString().Replace("'", "''"));
                }
            }
            columns = columns.Substring(0, columns.Length - 1);
            values = values.Substring(0, values.Length - 1);
            try
            {
                this.ExecuteNonQuery(String.Format("Insert into {0}({1}) values({2});", tableName, columns, values));
            }
            catch (Exception fail)
            {
                MessageBox.Show(fail.Message);
                returnCode = false;
            }
            return returnCode;
        }

        public int InsertArtist_Sql(Modules.MediaFile.Artist Artist)
        {
            Dictionary<String, Object> data = new Dictionary<string,object>();
            int artistId = 0;
                      

            data.Add("ArtistName", Artist.ArtistName);
            data.Add("ArtistArt", Artist.ArtistArt);

            if (Insert("Artist", data))
            {
                DataTable dt = GetDataTable("select ArtistId from Artist order by ArtistId desc");
                if (dt.Rows.Count > 0)
                {
                    artistId = int.Parse(dt.Rows[0][0].ToString());
                }
            }
            return artistId;
        }

        public int InsertAlbum_Sql(Modules.MediaFile.Album Album)
        {
            Dictionary<String, object> data = new Dictionary<string, object>();
            int albumId = 0;

            data.Add("AlbumName", Album.AlbumName);
            data.Add("AlbumArt", Album.AlbumArt);
            data.Add("ArtistId", Album.ArtistId);
            data.Add("Year", Album.Year);

            if (Insert("Album", data))
            {
                DataTable dt = GetDataTable("select AlbumId from Album order by AlbumId desc");
                if (dt.Rows.Count > 0)
                {
                    albumId = int.Parse(dt.Rows[0][0].ToString());
                }
            }
            return albumId;
        }

        internal int InsertVideo_Sql(MediaFile.Video video)
        {
            Dictionary<String, object> data = new Dictionary<string, object>();
            int vidoeId = 0;

            data.Add("VideoName", video.VideoName);
            data.Add("VideoPath", video.VideoPath);
            data.Add("VideoArt", video.VideoArt);
            data.Add("Year", video.Year);
            data.Add("Rating", video.Rating);
            data.Add("Plot", video.Plot);
            data.Add("Cast", video.Cast);
            data.Add("Category", video.Category);
            data.Add("Comments", video.Comments);
            data.Add("Description", video.Description);
            

            if (Insert("Video", data))
            {
                DataTable dt = GetDataTable("select VideoId from Video order by VideoId desc");
                if (dt.Rows.Count > 0)
                {
                    vidoeId = int.Parse(dt.Rows[0][0].ToString());
                }
            }
            return vidoeId;
        }

        public int InsertSong_Sql(Modules.MediaFile.Song Song)
        {
            Dictionary<String, object> data = new Dictionary<string, object>();
            int songId = 0;

            data.Add("Title", Song.Title);
            data.Add("FileName", Song.FileName);
            data.Add("ArtistId", Song.ArtistId);
            data.Add("Year", Song.Year);
            //data.Add("Fullpath", Song.Fullpath);
            data.Add("SongArt", Song.SongArt);
            data.Add("AlbumId", Song.AlbumId);
            data.Add("Comment", Song.Comment);
            data.Add("Genre", Song.Genre);
            data.Add("Rating", Song.Rating);
            data.Add("AlbumArtist", Song.AlbumArtist);
            data.Add("TrackNumber", Song.TrackNumber);
            data.Add("TotalTime", Song.TotalTime);
            data.Add("Composer", Song.Composer);
            data.Add("Location", Song.Location);
            data.Add("PlayImageButton", Song.PlayImageButton);
            data.Add("AddToPlaylistImageButton", Song.AddToPlaylistImageButton);
            data.Add("Disc", Song.Disc);
            data.Add("Length", Song.Length);
            data.Add("TAGID", Song.TAGID);
            data.Add("Size", Song.Size);
            data.Add("Kind", Song.Kind);
            data.Add("BitRate", Song.BitRate);
            data.Add("SampleRate", Song.SampleRate);
            //data.Add("DateModified", Song.DateModified);
           // data.Add("DateAdded", Song.DateAdded);

            if (Insert("Song", data))
            {
                DataTable dt = GetDataTable("select SongId from Song order by SongId desc");
                if (dt.Rows.Count > 0)
                {
                    songId = int.Parse(dt.Rows[0][0].ToString());
                }
            }
            return songId;
        }

        /// <summary>
        ///     Allows the programmer to easily delete all data from the DB.
        /// </summary>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool ClearDB()
        {
            DataTable tables;
            try
            {
                tables = this.GetDataTable("select NAME from SQLITE_MASTER where type='table' order by NAME;");
                foreach (DataRow table in tables.Rows)
                {
                    this.ClearTable(table["NAME"].ToString());
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Allows the user to easily clear all data from a specific table.
        /// </summary>
        /// <param name="table">The name of the table to clear.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>

        public bool ClearTable(String table)
        {
            try
            {
                this.ExecuteNonQuery(String.Format("delete from {0};", table));
                return true;
            }
            catch
            {
                return false;
            }
        }



        
    }
}