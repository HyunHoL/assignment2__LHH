using assignment.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace assignment.ViewModel
{
    class DefectListVM : INotifyPropertyChanged
    {
        #region [상수]

        private string displayLotID, displayWaferID, displayDeviceID, displayFileTimestamp, displayNullText, defectListNum;
        private DefectInfo selectedDefectList;
        private GetAllInfo share;
        private ObservableCollection<DefectInfo> defectValue;

        #endregion



        #region [속성]

        public ICommand IncreaseNum { get; }
        public ICommand DecreaseNum { get; }

        /**
        * @brief DefectList를 클릭하였을 때, 해당 DefectList에서 이미지 번호를 뽑아오는 함수  
        * @note Patch-notes
        * 2022-09-04|이현호|
        */

        public DefectInfo SelectedDefectList
        {
            get { return selectedDefectList; }

            set
            {
                if (selectedDefectList != value)
                {
                    selectedDefectList = value;
                    TifFileInfo saveNum = new TifFileInfo();
                    saveNum.imageNum = (int)selectedDefectList.defectID;
                    saveNum.imageFile = share.TifValue.imageFile;
                    saveNum.filePath = share.TifValue.filePath;

                    share.TifValue = saveNum;
                    DefectListNum = share.TifValue.imageNum + " / " + share.DefectList.Count;
                    OnPropertyChanged("SelectedDefectList");
                }
            }
        }

        public string DefectListNum
        {
            get { return defectListNum; }

            set
            {
                if (defectListNum != value)
                {
                    defectListNum = value;
                    OnPropertyChanged("DefectListNum");
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

        #endregion        

        #region [생성자]

        public DefectListVM()
        {
            DefectValue = new ObservableCollection<DefectInfo>();
            share = GetAllInfo.Instance;
            share.PropertyChanged += GetAllInfo_PropertyChanged;

            IncreaseNum = new RelayCommand(UpNumber);
            DecreaseNum = new RelayCommand(DownNumber);
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

        /**
        * @brief DefectList View에 값을 출력하게 해주는 함수  
        * @note Patch-notes
        * 2022-08-31|이현호|
        */

        public void AddValue()
        {
            DefectListNum = share.TifValue.imageNum + " / " + share.DefectList.Count;
            DisplayNullText = "";
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

        /**
        * @brief 버튼을 클릭하였을 때, 다음 Defect으로 넘겨주는 함수  
        * @note Patch-notes
        * 2022-09-04|이현호|
        */

        public void UpNumber(object paramerter)
        {
            int saveValue = int.Parse((string)paramerter);
            TifFileInfo saveNum = new TifFileInfo();

            if (share.TifValue.imageNum == 0)
            {
                share.TifValue.imageNum += 1;
            }

            else if (share.TifValue.imageNum == share.DefectList.Count)
            {
                share.TifValue.imageNum = 0;
            }

            saveNum.imageNum = share.TifValue.imageNum + saveValue;
            saveNum.imageFile = share.TifValue.imageFile;
            saveNum.filePath = share.TifValue.filePath;
            share.TifValue = saveNum;
            DefectListNum = share.TifValue.imageNum + " / " + share.DefectList.Count;
        }

        /**
        * @brief 버튼을 클릭하였을 때, 이전 Defect으로 넘겨주는 함수  
        * @note Patch-notes
        * 2022-09-04|이현호|
        */

        public void DownNumber(object parameter)
        {
            int saveValue = int.Parse((string)parameter);
            TifFileInfo saveNum = new TifFileInfo();

            if (share.TifValue.imageNum == 0 || share.TifValue.imageNum == 1)
            {
                share.TifValue.imageNum = share.DefectList.Count + 1;
            }

            saveNum.imageNum = share.TifValue.imageNum + saveValue;
            saveNum.imageFile = share.TifValue.imageFile;
            saveNum.filePath = share.TifValue.filePath;
            share.TifValue = saveNum;
            DefectListNum = share.TifValue.imageNum + " / " + share.DefectList.Count;
        }

        #endregion

        #region [private Method]

        private void GetAllInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DefectList")
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
