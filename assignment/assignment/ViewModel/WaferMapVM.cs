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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace assignment.ViewModel
{
    public class PointViewModel : INotifyPropertyChanged
    {

        private static PointViewModel instance = null;
        private bool isClicked;

        public static PointViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PointViewModel();
                }
                return instance;
            }
        }

        public Point selectedXY { get; set; }

        public bool IsClicked
        {
            get { return isClicked; }
            set
            {
                if (isClicked != value)
                {
                    isClicked = value;
                    OnPropertyChanged("IsClicked");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    class WaferMapVM : INotifyPropertyChanged
    {

        #region [상수]

        private double rectangleWidth;
        private double rectangleHeight;
        private GetAllInfo share;
        private MainViewModel mainVM;
        private PointViewModel pointVM;
        private DefectListVM defectVM;
        private ObservableCollection<Point> coordinates;
        private ObservableCollection<PointViewModel> defectCoordinates;

        #endregion

        public ICommand MouseEnterCommand { get; }

        public ICommand ToggleSelectionCommand { get; private set; }


        private void ToggleSelection(object parameter)
        {
            var target = parameter as PointViewModel;
            int index = -1;
            for (int i = 0; i < DefectCoordinates.Count; i++)
            {
                if (DefectCoordinates[i].selectedXY == target.selectedXY)
                {
                    index = i;
                }
            }

            GetAllInfo.Instance.TifValue.imageNum = (int)GetAllInfo.Instance.DefectList[index].defectID;

            if (PointVM.IsClicked != DefectCoordinates[index].IsClicked)
            {
                PointVM.IsClicked = !PointVM.IsClicked;
            }

            PointVM.IsClicked = !DefectCoordinates[index].IsClicked;

            for (int j = 0; j < DefectCoordinates.Count; j++)
            {
                if (DefectCoordinates[j].IsClicked == true)
                    DefectCoordinates[j].IsClicked = false;
            }

            DefectCoordinates[index].IsClicked = !DefectCoordinates[index].IsClicked;
        }

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

        public ObservableCollection<PointViewModel> DefectCoordinates
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
        
        public PointViewModel PointVM
        {
            get { return pointVM; }

            set
            {
                if (pointVM != value)
                {
                    pointVM = value;
                    OnPropertyChanged("PointVM");
                }
            }
        }

        public DefectListVM DefectVM
        {
            get { return defectVM; }

            set
            {
                if (defectVM != value)
                {
                    defectVM = value;
                    OnPropertyChanged("DefectVM");
                }
            }
        }

        #endregion

        #region [생성자]

        public WaferMapVM()
        {
            Coordinates = new ObservableCollection<Point>();
            DefectCoordinates = new ObservableCollection<PointViewModel>();
            Share = GetAllInfo.Instance;
            MainVM = MainViewModel.Instance;
            PointVM = PointViewModel.Instance;
            DefectVM = DefectListVM.Instance;
            share.PropertyChanged += GetAllInfo_PropertyChanged;
            mainVM.PropertyChanged += MainViewModel_PropertyChanged;
            defectVM.PropertyChanged += DefectListVM_PropertyChanged;
            ToggleSelectionCommand = new RelayCommand<object>(ToggleSelection);
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
            Point MaxValue = new Point();
            Point MinValue = new Point();

            if (GetAllInfo.Instance.Wafer.sampleTestPlan.Count != 0)
            {
                MaxValue = GetMaxValue();
                MinValue = GetMinValue();
            }

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
            Point MaxValue = new Point();
            Point MinValue = new Point();

            if (GetAllInfo.Instance.Wafer.sampleTestPlan.Count != 0)
            {
                MaxValue = GetMaxValue();
                MinValue = GetMinValue();
            }

            RectangleWidth = MainViewModel.Instance.ActualWidth / (2 * (MaxValue.X - MinValue.X + 1)) - 0.5;
            RectangleHeight = MainViewModel.Instance.ActualHeight / (2 * (MaxValue.Y - MinValue.Y + 1)) - 0.4;

            for (int i = 0; i < GetAllInfo.Instance.DefectList.Count; i++)
            {
                PointViewModel saveValue = new PointViewModel();
                saveValue.selectedXY = new Point{ X = (GetAllInfo.Instance.DefectXY[i].X - MinValue.X) * RectangleWidth + 1, Y = Math.Abs(GetAllInfo.Instance.DefectXY[i].Y - MaxValue.Y) * RectangleHeight +1 };
                saveValue.IsClicked = false;
                //saveValue.selectedXY.Y = (Math.Abs(GetAllInfo.Instance.DefectXY[i].Y - MaxValue.Y) * RectangleHeight) + 1;
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

        /**
        * @brief GetAllInfo 클래스에서 이벤트를 받아오는 함수
        * 날짜|작성자|설명
        * 2023-09-01|이현호|
        */

        private void GetAllInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Wafer")
            {
                UpdateSampleTestPlan();
            }

            else if (e.PropertyName == "DefectList")
            {
                UpdateDefectXY();
            }
        }

        /**
        * @brief MainViewModel 클래스에서 이벤트를 받아오는 함수
        * 날짜|작성자|설명
        * 2023-09-01|이현호|
        */

        private void MainViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((e.PropertyName == "ActualWidth" || e.PropertyName == "ActualHeight") && GetAllInfo.Instance.Wafer.sampleTestPlan != null && GetAllInfo.Instance.DefectXY != null)
            {
                UpdateSampleTestPlan();
                UpdateDefectXY();
            }
        }

        /**
        * @brief DefectListVM 클래스에서 버튼, DefectList가 클릭되었을 때, WaferMap에 파란 테두리를 그려주는 함수
        * 날짜|작성자|설명
        * 2023-09-07|이현호|
        * 2023-09-12|이현호|target의 인덱스를 찾는 부분을 GetIndex 메서드로 따로 빼줌
        */

        private void DefectListVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SaveCoordinate")
            {
                Point target = new Point();
                Point MaxValue = GetMaxValue();
                Point MinValue = GetMinValue();
                target = DefectListVM.Instance.SaveCoordinate;
                target.X = (target.X - MinValue.X) * RectangleWidth + 1;
                target.Y = Math.Abs((target.Y - MaxValue.Y) * RectangleHeight) + 1;

                int index = -1;

                index = GetIndex(target, index);

                if (index == -1)
                {
                    return;
                }

                GetAllInfo.Instance.TifValue.imageNum = (int)GetAllInfo.Instance.DefectList[index].defectID;

                if (PointVM.IsClicked != DefectCoordinates[index].IsClicked)
                {
                    PointVM.IsClicked = !PointVM.IsClicked;
                }

                PointVM.IsClicked = !DefectCoordinates[index].IsClicked;

                for (int j = 0; j < DefectCoordinates.Count; j++)
                {
                    if (DefectCoordinates[j].IsClicked == true)
                        DefectCoordinates[j].IsClicked = false;
                }

                DefectCoordinates[index].IsClicked = !DefectCoordinates[index].IsClicked;
            }
        }

        /**
        * @brief 좌표에 해당하는 인덱스 값을 반환해주는 함수
        * @param target -> 찾고자하는 좌표 값, index 반환하는 인덱스 값을 저장하는 곳
        * @return (int) : 찾고자하는 좌표의 인덱스
        * 날짜|작성자|설명
        * 2023-09-12|이현호|
        */

        private int GetIndex(Point target, int index)
        {
            for (int i = 0; i < DefectCoordinates.Count; i++)
            {

                if (DefectCoordinates[i].selectedXY == target)
                {
                    if (i + 3 >= DefectCoordinates.Count)
                    {
                        index = i;
                        break;
                    }

                    else if (DefectCoordinates[i + 3].selectedXY == target)
                    {
                        index = i + 3;
                        break;
                    }

                    else if (DefectCoordinates[i + 2].selectedXY == target)
                    {
                        index = i + 2;
                        break;
                    }

                    else if (DefectCoordinates[i + 1].selectedXY == target)
                    {
                        index = i + 1;
                        break;
                    }

                    else
                    {
                        index = i;
                        break;
                    }
                }
            }

            return index;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
