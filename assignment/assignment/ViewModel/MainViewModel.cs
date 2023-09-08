using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace assignment.ViewModel
{
    class MainViewModel
    {
        #region [상수]

        private static MainViewModel instance;
        private double actualWidth;
        private double actualHeight;
        private Point newStartPoint, newEndPoint;

        #endregion

        #region [인스턴스]
        public static MainViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MainViewModel();
                }

                return instance;
            }
        }

        #endregion

        #region [속성]

        public double ActualWidth
        {
            get { return actualWidth; }

            set
            {
                double widthDifference = Math.Abs(value - actualWidth);

                if (widthDifference >= 50)
                {
                    actualWidth = value;
                    OnPropertyChanged("ActualWidth");
                }
            }
        }

        public double ActualHeight
        {
            get { return actualHeight; }

            set
            {
                double heightDifference = Math.Abs(value - actualHeight);

                if (heightDifference >= 50)
                {
                    actualHeight = value;
                    OnPropertyChanged("ActualHeight");
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
        #endregion



        #region [생성자]

        public MainViewModel()
        {
            ActualHeight = 1000;
            ActualWidth = 1500;
            NewStartPoint = new Point { X = 780, Y = 400 };
            NewEndPoint = new Point { X = 900, Y = 400 };
        }

        #endregion



        #region [public Method]



        #endregion



        #region [private Method]

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
