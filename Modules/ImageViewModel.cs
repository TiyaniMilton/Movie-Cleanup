//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
////using GalaSoft.MvvmLight;

//namespace Movie_Cleanup.Modules
//{
//    /// <summary>
//    /// This class contains properties that a View can data bind to.
//    /// <para>
//    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
//    /// </para>
//    /// <para>
//    /// You can also use Blend to data bind with the tool's support.
//    /// </para>
//    /// <para>
//    /// See http://www.galasoft.ch/mvvm/getstarted
//    /// </para>
//    /// </summary>
//    public class ImageViewModel : ViewModelBase
//    {
//        /// <summary>
//        /// Initializes a new instance of the ImageViewModel class.
//        /// </summary>
//        public ImageViewModel()
//        {

//        }

//        #region Image Name

//        private string _imageName;
//        public string ImageName
//        {
//            get { return _imageName; }
//            set
//            {
//                if (value == _imageName)
//                    return;

//                _imageName = value;
//                RaisePropertyChanged("ImageName");
//            }
//        }

//        #endregion

//        #region Image Width

//        private int _imageWidth;
//        public int ImageWidth
//        {
//            get { return _imageWidth; }
//            set
//            {
//                if (value == _imageWidth)
//                    return;

//                _imageWidth = value;
//                RaisePropertyChanged("ImageWidth");
//            }
//        }

//        #endregion

//        #region Image Height

//        private int _imageHeight;
//        public int ImageHeight
//        {
//            get { return _imageHeight; }
//            set
//            {
//                if (value == _imageHeight)
//                    return;

//                _imageHeight = value;
//                RaisePropertyChanged("ImageHeight");
//            }
//        }

//        private void RaisePropertyChanged(string p)
//        {
//            throw new NotImplementedException();
//        }

//        #endregion

//        #region File Type

//        private string _imageFileType;
//        public string FileType
//        {
//            get { return _imageFileType; }
//            set
//            {
//                if (value == _imageFileType)
//                    return;

//                _imageFileType = value;
//                RaisePropertyChanged("FileType");
//            }
//        }

//        #endregion

//        #region Image Depth

//        private int _imageDepth;
//        public int ImageDepth
//        {
//            get { return _imageDepth; }
//            set
//            {
//                if (value == _imageDepth)
//                    return;

//                _imageDepth = value;
//                RaisePropertyChanged("ImageDepth");
//            }
//        }

//        #endregion

//        #region Date Created

//        private DateTime _dateCreated;
//        public DateTime DateCreated
//        {
//            get { return _dateCreated; }
//            set
//            {
//                if (value == _dateCreated)
//                    return;

//                _dateCreated = value;
//                RaisePropertyChanged("DateCreated");
//            }
//        }

//        #endregion
//    }
//}
