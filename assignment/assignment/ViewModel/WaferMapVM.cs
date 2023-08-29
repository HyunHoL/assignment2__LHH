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

        private ShareInfo shareInfo;
        private WaferInfo wafer;
        private ObservableCollection<ObservablePoint> coordinates;

        public ObservableCollection<ObservablePoint> Coordinates
        {
            get { return coordinates; }
            set
            {
                if (coordinates != value)
                {
                    coordinates = value;
                    OnPropertyChanged(nameof(Coordinates));
                }
            }
        }

        public ShareInfo Share
        {
            get;
            set;
        }

        public WaferMapVM()
        {
            Coordinates = new ObservableCollection<ObservablePoint>();
            shareInfo = ShareInfo.Instance;
            //UpdateSampleTestPlan();
        }

        public void UpdateSampleTestPlan()
        {
            if (shareInfo.IsFileOpened == true)
            {
                foreach (Point samplePoint in ShareInfo.Instance.Wafer.sampleTestPlan)
                {
                    ObservablePoint saveValue = new ObservablePoint();
                    saveValue.X = samplePoint.X;
                    saveValue.Y = samplePoint.Y;
                    Coordinates.Add(saveValue);
                }
            }
            //coordinates.Clear();
            //Coordinates.Clear();

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
