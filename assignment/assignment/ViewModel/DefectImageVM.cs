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
        private bool startDrag;
        private Point dragPoint;
        private double firstWidth;
        #endregion

        #region [인터페이스]

        public ICommand MouseLeftButtonDownCommand { get; }
        public ICommand MouseLeftButtonUpCommand { get; }
        public ICommand MouseDraggingCommand { get; }

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
                if (startPoint != value)
                {
                    startPoint = value;
                    OnPropertyChanged("StartPoint");
                }
            }
        }

        public Point EndPoint
        {
            get { return endPoint; }

            set
            {
                if (endPoint != value)
                {
                    endPoint = value;
                    OnPropertyChanged("EndPoint");
                }
            }
        }

        public Point DragPoint
        {
            get { return dragPoint; }

            set
            {
                if (dragPoint != value)
                {
                    dragPoint = value;
                    OnPropertyChanged("DragPoint");
                }
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

        public bool StartDrag
        {
            get { return startDrag; }

            set
            {
                if (startDrag != value)
                {
                    startDrag = value;
                    OnPropertyChanged("StartDrag");
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
            MouseDraggingCommand = new RelayCommand<object>(MouseLeftDragging);
            firstWidth = MainViewModel.Instance.ActualWidth / 2;
        }

        #endregion

        #region [public Method]

        /**
        * @brief 마우스 왼쪽을 클릭했을 때, 해당 위치의 좌표를 받아오는 함수
        * @param 마우스 클릭 시, 해당 위치의 좌표
        * @note Patch-notes
        * 날짜|작성자|설명
        * 2022-09-08|이현호|
        * 2022-09-11|이현호|DragPoint의 초기값을 StartPoint로 지정, Line이 이상하게 그려져 X값 조정
        */

        public void MouseLeftButtonDown(object parameter)
        {
            StartPoint = new Point { X = Mouse.GetPosition(null).X - WindowWidth + 5, Y = Mouse.GetPosition(null).Y};
            DragPoint = StartPoint;
            StartDrag = true;
        }

        /**
        * @brief 마우스 왼쪽을 클릭을 해제했을 때, 해당 위치의 좌표를 받아오고 StartPoint와 EndPoint의 거리를 출력해주는 함수
        * @param 마우스 클릭 해제 시, 해당 위치의 좌표
        * @note Patch-notes
        * 날짜|작성자|설명
        * 2022-09-08|이현호|
        * 2022-09-11|이현호|Line이 이상하게 그려져 X값 조정
        */

        public void MouseLeftButtonUp(object parameter)
        {
            EndPoint = new Point { X = Mouse.GetPosition(null).X - WindowWidth + 5, Y = Mouse.GetPosition(null).Y};
            StartDrag = false;
            double saveResult = 50 * Math.Sqrt(Math.Pow(StartPoint.X - EndPoint.X, 2) + Math.Pow(StartPoint.Y - EndPoint.Y, 2)) / 230 * (firstWidth / WindowWidth);
            saveResult = Math.Floor(saveResult / ZoomLevel * 1000) / 1000.0;
            Distance = saveResult.ToString() + "μm";
        }

        /**
        * @brief 마우스 왼쪽을 클릭했을 때, 이동하는 마우스의 좌표를 받아오는 함수
        * @param 마우스 Drag시, 해당 위치의 좌표
        * @note Patch-notes
        * 날짜|작성자|설명
        * 2022-09-11|이현호|
        * */

        public void MouseLeftDragging(object parameter)
        {
            if (StartDrag == true)
            {
                DragPoint = new Point { X = Mouse.GetPosition(null).X - WindowWidth + 5, Y = Mouse.GetPosition(null).Y };
            }
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

        /**
        * @brief PointViewModel 클래스에서 IsClicked의 값이 변경되었을 때, 이벤트를 받아오는 함수  
        * @note Patch-notes
        * 2022-09-07|이현호|
        */

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

        /**
        * @brief MainViewModel 클래스에서 Window Size 값이 변경되었을 때, 이벤트를 받아오는 함수  
        * @note Patch-notes
        * 2022-09-07|이현호|
        */

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
