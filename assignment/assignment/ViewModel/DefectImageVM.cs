using assignment.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace assignment.ViewModel
{
    class DefectImageVM : INotifyPropertyChanged
    {
        #region [상수]

        private GetAllInfo share;
        private BitmapSource loadImage;
        private PointViewModel pointVM;
        private MainViewModel mainVM;

        #endregion



        #region [속성]

        public GetAllInfo Share
        {
            get { return share; }

            set
            {
                if (share != value)
                {
                    if (share != null)
                    {
                        share.PropertyChanged -= GetAllInfo_PropertyChanged;
                    }

                    share = value;

                    if (share != null)
                    {
                        share.PropertyChanged += GetAllInfo_PropertyChanged;
                    }

                    OnPropertyChanged("Share");
                }
            }
        }

        public BitmapSource LoadImage
        {
            get { return loadImage; }

            set
            {
                if (loadImage != value)
                {
                    loadImage = value;
                    OnPropertyChanged("LoadImage");

                }
            }
        }


        #endregion

        public PointViewModel PointVM
        {
            get { return pointVM; }

            set
            {
                if (pointVM != value)
                {
                    if (pointVM != null)
                    {
                        pointVM.PropertyChanged -= PointViewModel_PropertyChanged;
                    }

                    pointVM = value;

                    if (pointVM != null)
                    {
                        pointVM.PropertyChanged += PointViewModel_PropertyChanged;
                    }

                    OnPropertyChanged("PointVM");
                }
            }
        }

        private double zoomLevel = 1.0;

        public double ZoomLevel
        {
            get { return zoomLevel; }
            set
            {
                if (zoomLevel != value)
                {
                    double scaleDifference = Math.Abs(value - zoomLevel);

                    zoomLevel = value;
                    OnPropertyChanged(nameof(ZoomLevel));
                }
            }
        }

        private double windowWidth;

        public double WindowWidth
        {
            get { return windowWidth; }

            set
            {
                if (windowWidth != value)
                {
                    windowWidth = value;
                    OnPropertyChanged("WindowWidth");
                }
            }
        }

        private double windowHeight;

        public double WindowHeight
        {
            get { return windowHeight; }

            set
            {
                if (windowHeight != value)
                {
                    windowHeight = value;
                    OnPropertyChanged("WindowHeight");
                }
            }
        }

        public MainViewModel MainVM
        {
            get { return mainVM; }

            set
            {
                if (mainVM != value)
                {
                    if (mainVM != null)
                    {
                        mainVM.PropertyChanged -= MainViewModel_PropertyChanged;
                    }

                    mainVM = value;

                    if (mainVM != null)
                    {
                        mainVM.PropertyChanged += MainViewModel_PropertyChanged;
                    }

                    OnPropertyChanged("MainVM");
                }
            }
        }

        #region [생성자]

        public DefectImageVM()
        {
            share = GetAllInfo.Instance;
            share.PropertyChanged += GetAllInfo_PropertyChanged;
            WindowWidth = MainViewModel.Instance.ActualWidth / 2;
            WindowHeight = MainViewModel.Instance.ActualHeight / 2;
            PointVM = PointViewModel.Instance;
            PointVM.PropertyChanged += PointViewModel_PropertyChanged;
            MainVM = MainViewModel.Instance;
            mainVM.PropertyChanged += MainViewModel_PropertyChanged;
        }

        #endregion



        #region [public Method]

        /**
        * @brief 이미지 번호에 맞게 이미지를 출력해주는 함수  
        * @note Patch-notes
        * 2022-09-04|이현호|
        */

        public void AddImage()
        {
            if (share.TifValue.imageNum != 0)
            {
                LoadImage = share.TifValue.imageFile.Frames[share.TifValue.imageNum - 1];
            }

            else
            {
                LoadImage = share.TifValue.imageFile.Frames[share.TifValue.imageNum];
            }
            WindowWidth = MainViewModel.Instance.ActualWidth / 2;
            WindowHeight = MainViewModel.Instance.ActualHeight / 2;
        }

        #endregion



        #region [private Method]

        private void PointViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsClicked")
            {
                AddImage();
            }
        }

        /**
        * @brief GetAllInfo 클래스에서 TifValue의 값이 변경되었을 때, 이벤트를 받아오는 함수  
        * @note Patch-notes
        * 2022-09-04|이현호|
        */

        private void GetAllInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TifValue")
            {
                AddImage();
            }
        }

        private void MainViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ActualWidth" || e.PropertyName == "ActualHeight")
            {
                AddImage();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
