using assignment.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment.ViewModel
{
    class DefectListVM : INotifyPropertyChanged
    {
        #region [상수]

        private string displayLotID, displayWaferID, displayDeviceID, displayFileTimestamp, displayNullText;
        private GetAllInfo share;
        private ObservableCollection<DefectInfo> defectValue;

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

        #endregion        

        #region [생성자]

        public DefectListVM()
        {
            DefectValue = new ObservableCollection<DefectInfo>();
            share = GetAllInfo.Instance;
            share.PropertyChanged += GetAllInfo_PropertyChanged;
        }

        #endregion



        #region [public Method]

        public ObservableCollection<DefectInfo> DefectValue
        {
            get { return defectValue; }

            set
            {
                if (defectValue != value)
                {
                    defectValue = value;
                    OnPropertyChanged("DefectValue");
                }
            }
        }

        public string DisplayLotID
        {
            get { return displayLotID; }

            set
            {
                if (displayLotID != value)
                {
                    displayLotID = value;
                    OnPropertyChanged("DisplayLotID");
                }
            }
        }

        public string DisplayWaferID
        {
            get { return displayWaferID; }

            set
            {
                if (displayWaferID != value)
                {
                    displayWaferID = value;
                    OnPropertyChanged("DisplayWaferID");
                }
            }
        }

        public string DisplayDeviceID
        {
            get { return displayDeviceID; }

            set
            {
                if (displayDeviceID != value)
                {
                    displayDeviceID = value;
                    OnPropertyChanged("DisplayDeviceID");
                }
            }
        }

        public string DisplayFileTimestamp
        {
            get { return displayFileTimestamp; }

            set
            {
                if (displayFileTimestamp != value)
                {
                    displayFileTimestamp = value;
                    OnPropertyChanged("DisplayFileTimestamp");
                }
            }
        }

        public string DisplayNullText
        {
            get { return displayNullText; }

            set
            {
                if (displayNullText != value)
                {
                    displayNullText = value;
                    OnPropertyChanged("DisplayNullText");
                }
            }
        }

        public void AddValue()
        {
            DisplayWaferID = "WaferID : " + share.Wafer.waferID;
            DisplayLotID = "LotID : " + share.Wafer.lotID;
            DisplayDeviceID = "DeviceID : " + share.Wafer.deviceID;
            DisplayFileTimestamp = "FileTimestamp : " + share.Wafer.fileTimestamp;

            for (int i = 0; i < GetAllInfo.Instance.DefectList.Count; i++)
            {
                DefectInfo saveValue = new DefectInfo();
                saveValue = GetAllInfo.Instance.DefectList[i];
                DefectValue.Add(saveValue);
            }
        }

        #endregion

        #region [private Method]

        private void GetAllInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Wafer")
            {
                AddValue();
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
