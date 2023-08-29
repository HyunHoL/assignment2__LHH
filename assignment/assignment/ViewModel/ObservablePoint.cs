using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment.ViewModel
{
    class ObservablePoint : INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;
        private double x;

        public double X
        {
            get { return x; }

            set
            {
                if (x != value)
                {
                    x = value;
                    OnPropertyChanged(nameof(X));
                }
            }
        }

        double y;
        public double Y
        {
            get { return y; }

            set
            {
                if (y != value)
                {
                    y = value;
                    OnPropertyChanged(nameof(Y));
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
