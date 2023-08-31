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
        private double actualWidth;
        private double actualHeight;
        private double rectangleWidth;
        private double rectangleHeight;
        private GetAllInfo share;
        private ObservableCollection<Point> coordinates;
        private ObservableCollection<Point> defectCoordinates;

        #endregion

        #region [속성]

        public double ActualWidth
        {
            get { return actualWidth; }

            set
            {
                actualWidth = value;
                OnPropertyChanged("ActualWidth");
                UpdateSampleTestPlan();
                UpdateDefectXY();

            }
        }

        public double ActualHeight
        {
            get { return actualHeight; }

            set
            {
                actualHeight = value;
                OnPropertyChanged("ActualHeight");
                UpdateSampleTestPlan();
                UpdateDefectXY();
            }
        }

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
            share.PropertyChanged += GetAllInfo_PropertyChanged;
        }

        #endregion

        #region [public Method]

        public void UpdateSampleTestPlan ()
        {
            for (int i = 0; i < GetAllInfo.Instance.Wafer.sampleTestPlan.Count; i++)
            {
                Point saveValue = new Point();
                saveValue.X = (GetAllInfo.Instance.Wafer.sampleTestPlan[i].X + 9) * 30;
                saveValue.Y = (Math.Abs(GetAllInfo.Instance.Wafer.sampleTestPlan[i].Y - 24) * 10);
                Coordinates.Add(saveValue);
            }
        }

        public void UpdateDefectXY()
        {
            for (int i = 0; i < GetAllInfo.Instance.DefectList.Count; i++)
            {
                Point saveValue = new Point();
                saveValue.X = (GetAllInfo.Instance.DefectXY[i].X + 9) * 30;
                saveValue.Y = (Math.Abs(GetAllInfo.Instance.DefectXY[i].Y - 24) * 10);
                DefectCoordinates.Add(saveValue);
            }
        }

        #endregion

        #region [private Method]
        public event PropertyChangedEventHandler PropertyChanged;
        
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

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
