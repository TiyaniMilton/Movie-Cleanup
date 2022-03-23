using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Movie_Cleanup.Modules
{
    static class ImageConverter
    {
        public static BitmapImage ToWpfImage(this System.Drawing.Image img)
        {
            MemoryStream ms = new MemoryStream();  // no using here! BitmapImage will dispose the stream after loading
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

            BitmapImage ix = new BitmapImage();
            try
            {
                ix.BeginInit();
                ix.CacheOption = BitmapCacheOption.OnLoad;
                ix.StreamSource = ms;
                ix.EndInit();
            }
            catch { ix = null; }
            return ix;

            //BitmapImage image = new BitmapImage();
            //image.BeginInit();
            //image.UriSource = new Uri("pack://application:,,,/NameOfAssembly;component/Path/To/Image.png");
            //image.EndInit();

            //return image;
        }
        public static BitmapImage ToWpfImage(this System.Drawing.Bitmap img)
        {
            MemoryStream ms = new MemoryStream();  // no using here! BitmapImage will dispose the stream after loading
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

            BitmapImage ix = new BitmapImage();
            try
            {
                ix.BeginInit();
                ix.CacheOption = BitmapCacheOption.OnLoad;
                ix.StreamSource = ms;
                ix.EndInit();
            }
            catch { ix = null; }
            return ix;

            //BitmapImage image = new BitmapImage();
            //image.BeginInit();
            //image.UriSource = new Uri("pack://application:,,,/NameOfAssembly;component/Path/To/Image.png");
            //image.EndInit();

            //return image;
        }

        public static BitmapSource ToWpfBitmap(this System.Drawing.Image bitmap)//(this Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Bmp);

                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                // Force the bitmap to load right now so we can dispose the stream.
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }

        public static string SaveAlbumArt(System.Drawing.Image bitmap,string path,string filename)
        {
            try
            {
                if (bitmap != null)
                {
                    bitmap.Save(path + @"" + filename + ".png", ImageFormat.Png);
                    return path + filename + ".png";
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("There was a problem saving the file." +
                //    "Check the file permissions.");
            }
            return path + @"Unknown Album.png";


            //System.Drawing.Bitmap bmpOut = new System.Drawing.Bitmap(NewWidth, NewHeight);
            //System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmpOut);
            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //g.FillRectangle(System.Drawing.Brushes.White, 0, 0, NewWidth, NewHeight);
            //g.DrawImage(new System.Drawing.Bitmap(fupProduct.PostedFile.InputStream), 0, 0, NewWidth, NewHeight);
            //MemoryStream stream = new MemoryStream();
            //switch (fupProduct.FileName.Substring(fupProduct.FileName.IndexOf('.') + 1).ToLower())
            //{
            //    case "jpg":
            //        bmpOut.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            //        break;
            //    case "jpeg":
            //        bmpOut.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            //        break;
            //    case "tiff":
            //        bmpOut.Save(stream, System.Drawing.Imaging.ImageFormat.Tiff);
            //        break;
            //    case "png":
            //        bmpOut.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            //        break;
            //    case "gif":
            //        bmpOut.Save(stream, System.Drawing.Imaging.ImageFormat.Gif);
            //        break;
            //}
            //String saveImagePath = Server.MapPath("../") + "Images/Thumbnail/" + fupProduct.FileName.Substring(fupProduct.FileName.IndexOf('.'));
            //bmpOut.Save(saveImagePath);
        }


    }
}
