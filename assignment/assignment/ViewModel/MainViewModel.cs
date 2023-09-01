using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment.ViewModel
{
    class MainViewModel
    {
        #region [상수]

        private static MainViewModel instance;
        private double actualWidth;
        private double actualHeight;


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

        #endregion



        #region [생성자]

        public MainViewModel()
        {
            ActualHeight = 1000;
            ActualWidth = 1500;
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
