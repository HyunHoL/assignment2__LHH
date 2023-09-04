using assignment.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace assignment.ViewModel
{

    class WaferMapVM : INotifyPropertyChanged
    {

        #region [상수]

        private double rectangleWidth;
        private double rectangleHeight;
        private GetAllInfo share;
        private MainViewModel mainVM;
        private ObservableCollection<Point> coordinates;
        private ObservableCollection<Point> defectCoordinates;

        #endregion

        #region [속성]


        public double RectangleWidth
        {
            get { return rectangleWidth; }

            set
            {
                if (rectangleWidth != value)
                {
                    rectangleWidth = value;
                    OnPropertyChanged("RectangleWidth");
                }
            }
        }

        public double RectangleHeight
        {
            get { return rectangleHeight; }

            set
            {
                if (rectangleHeight != value)
                {
                    rectangleHeight = value;
                    OnPropertyChanged("RectangleHeight");
                }
            }
        }

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

        public ObservableCollection<Point> Coordinates
        {
            get { return coordinates; }
            set
            {
                if (coordinates != value)
                {
                    coordinates = value;
                    OnPropertyChanged("Coordinates");
                }
            }
        }

        public ObservableCollection<Point> DefectCoordinates
        {
            get { return defectCoordinates; }

            set
            {
                if (defectCoordinates != value)
                {
                    defectCoordinates = value;
                    OnPropertyChanged("DefectCoordinates");
                }
            }
        }

        #endregion

        #region [생성자]

        public WaferMapVM()
        {
            Coordinates = new ObservableCollection<Point>();
            DefectCoordinates = new ObservableCollection<Point>();
            Share = GetAllInfo.Instance;
            MainVM = MainViewModel.Instance;
            share.PropertyChanged += GetAllInfo_PropertyChanged;
            mainVM.PropertyChanged += MainViewModel_PropertyChanged;
        }

        #endregion

        #region [public Method]

        /**
        * @brief Die의 좌표에 사각형을 그려 WaferMap을 그리게 해주는 함수
        * 날짜|작성자|설명
        * 2023-08-30|이현호|
        * 2023-09-01|이현호|Rectangle의 Size를 동적으로 변경 가능하게 하는 기능 추가
        */

        public void UpdateSampleTestPlan ()
        {
            Coordinates.Clear();
            Point MaxValue = GetMaxValue();
            Point MinValue = GetMinValue();

            RectangleWidth = MainViewModel.Instance.ActualWidth / (2 * (MaxValue.X - MinValue.X + 1)) - 0.5;
            RectangleHeight = MainViewModel.Instance.ActualHeight / (2 * (MaxValue.Y - MinValue.Y + 1)) - 0.4;

            for (int i = 0; i < GetAllInfo.Instance.Wafer.sampleTestPlan.Count; i++)
            {
                Point saveValue = new Point();
                saveValue.X = (GetAllInfo.Instance.Wafer.sampleTestPlan[i].X - MinValue.X) * RectangleWidth + 1;
                saveValue.Y = (Math.Abs(GetAllInfo.Instance.Wafer.sampleTestPlan[i].Y - MaxValue.Y) * RectangleHeight) + 1;
                Coordinates.Add(saveValue);
            }
        }

        /**
        * @brief Defect이 있는 Die의 좌표에 사각형을 그려 WaferMap을 그리게 해주는 함수
        * 날짜|작성자|설명
        * 2023-08-30|이현호|
        * 2023-09-01|이현호|Rectangle의 Size를 동적으로 변경 가능하게 하는 기능 추가
        */

        public void UpdateDefectXY()
        {
            DefectCoordinates.Clear();
            Point MaxValue = GetMaxValue();
            Point MinValue = GetMinValue();

            RectangleWidth = MainViewModel.Instance.ActualWidth / (2 * (MaxValue.X - MinValue.X + 1)) - 0.5;
            RectangleHeight = MainViewModel.Instance.ActualHeight / (2 * (MaxValue.Y - MinValue.Y + 1)) - 0.4;

            for (int i = 0; i < GetAllInfo.Instance.DefectList.Count; i++)
            {
                Point saveValue = new Point();
                saveValue.X = (GetAllInfo.Instance.DefectXY[i].X - MinValue.X) * RectangleWidth + 1;
                saveValue.Y = (Math.Abs(GetAllInfo.Instance.DefectXY[i].Y - MaxValue.Y) * RectangleHeight) + 1;
                DefectCoordinates.Add(saveValue);
            }
        }

        #endregion

        #region [private Method]

        /**
        * @brief 좌표의 X, Y 최대값을 뽑아주는 함수
        * 날짜|작성자|설명
        * 2023-09-01|이현호|
        */

        private Point GetMaxValue()
        {
            Point maxXY = new Point();
            double maxX = 0;
            double maxY = 0;
            maxX = GetAllInfo.Instance.Wafer.sampleTestPlan.Max(point => point.X);
            maxY = GetAllInfo.Instance.Wafer.sampleTestPlan.Max(point => point.Y);
            maxXY.X = maxX;
            maxXY.Y = maxY;
            return maxXY;
        }

        /**
        * @brief 좌표의 X, Y 최소값을 뽑아주는 함수
        * 날짜|작성자|설명
        * 2023-09-01|이현호|
        */

        private Point GetMinValue()
        {
            Point minXY = new Point();
            double minX = 0;
            double minY = 0;
            minX = GetAllInfo.Instance.Wafer.sampleTestPlan.Min(point => point.X);
            minY = GetAllInfo.Instance.Wafer.sampleTestPlan.Min(point => point.Y);
            minXY.X = minX;
            minXY.Y = minY;
            return minXY;
        }

        private void GetAllInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Wafer")
            {
                UpdateSampleTestPlan();
            }

            else if (e.PropertyName == "DefectXY")
            {
                UpdateDefectXY();
            }
        }

        private void MainViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((e.PropertyName == "ActualWidth" || e.PropertyName == "ActualHeight") && GetAllInfo.Instance.Wafer.sampleTestPlan != null && GetAllInfo.Instance.DefectXY != null)
            {
                UpdateSampleTestPlan();
                UpdateDefectXY();
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
