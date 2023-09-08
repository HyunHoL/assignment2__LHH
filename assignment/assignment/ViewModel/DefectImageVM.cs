using assignment.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        private Point startPoint;
        private Point endPoint;
        private string distance;
        private Point newStartPoint, newEndPoint;
        private bool drag;

        #endregion

        public ICommand MouseLeftButtonDownCommand { get; }
        public ICommand MouseLeftButtonUpCommand { get; }

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

        public Point StartPoint
        {
            get { return startPoint; }

            set
            {
                startPoint = value;
                OnPropertyChanged("StartPoint");
            }
        }

        public Point EndPoint
        {
            get { return endPoint; }

            set
            {
                endPoint = value;
                OnPropertyChanged("EndPoint");
            }
        }

        public string Distance
        {
            get { return distance; }

            set
            {
                if (distance != value)
                {
                    distance = value;
                    OnPropertyChanged("Distance");
                }
            }
        }

        public bool Drag
        {
            get { return drag; }

            set
            {
                if (drag != value)
                {
                    drag = value;
                    OnPropertyChanged("Drag");
                }
            }
        }

        public Point NewStartPoint
        {
            get { return newStartPoint; }

            set
            {
                if (newStartPoint != value)
                {
                    newStartPoint = value;
                    OnPropertyChanged("NewStartPoint");
                }
            }
        }

        public Point NewEndPoint
        {
            get { return newEndPoint; }

            set
            {
                if (newEndPoint != value)
                {
                    newEndPoint = value;
                    OnPropertyChanged("NewEndPoint");
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
            MouseLeftButtonDownCommand = new RelayCommand<object>(MouseLeftButtonDown);
            MouseLeftButtonUpCommand = new RelayCommand<object>(MouseLeftButtonUp);
            
        }

        #endregion

        #region [public Method]

        public void MouseLeftButtonDown(object parameter)
        {
            startPoint = Mouse.GetPosition(null);
            Drag = true;
        }

        public void MouseLeftButtonUp(object parameter)
        {
            endPoint = Mouse.GetPosition(null);
            NewStartPoint = new Point { X = startPoint.X - MainViewModel.Instance.ActualWidth / 2, Y = startPoint.Y - MainViewModel.Instance.ActualHeight / 2 };
            NewEndPoint = new Point { X = endPoint.X - MainViewModel.Instance.ActualWidth / 2, Y = endPoint.Y - MainViewModel.Instance.ActualHeight / 2 };
            Drag = false;
            double saveResult = 50 * Math.Sqrt(Math.Pow(startPoint.X - endPoint.X, 2) + Math.Pow(startPoint.Y - endPoint.Y, 2)) / 230;
            saveResult = Math.Floor(saveResult / ZoomLevel * 1000) / 1000.0;
            Distance = saveResult.ToString() + "μm";
        }

        /**
        * @brief 이미지 번호에 맞게 이미지를 출력해주는 함수  
        * @note Patch-notes
        * 2022-09-04|이현호|
        */

        public void AddImage()
        {
            if (share.TifValue.imageNum != 0 && share.TifValue.imageFile != null)
            {
                LoadImage = share.TifValue.imageFile.Frames[share.TifValue.imageNum - 1];
            }

            else if (share.TifValue.imageFile != null)
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
