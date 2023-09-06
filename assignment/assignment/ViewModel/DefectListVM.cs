using assignment.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace assignment.ViewModel
{
    class DefectListVM : INotifyPropertyChanged
    {
        #region [상수]
        private static DefectListVM instance;
        private string displayLotID, displayWaferID, displayDeviceID, displayFileTimestamp, displayNullText, defectListNum, dieNum, defectCount;
        private DefectInfo selectedDefectList;
        private GetAllInfo share;
        private ObservableCollection<DefectInfo> defectValue;
        private PointViewModel pointVM;
        private Point saveCoordinate;

        #endregion


        #region [속성]

        public ICommand IncreaseDefectNum { get; }
        public ICommand DecreaseDefectNum { get; }
        public static DefectListVM Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DefectListVM();
                }
                return instance;
            }
        }

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
                    Instance.SaveCoordinate = selectedDefectList.defectXY;
                    share.TifValue = saveNum;
                    DefectListNum = share.TifValue.imageNum + " / " + share.DefectList.Count;
                    OnPropertyChanged("SelectedDefectList");
                }
            }
        }

        public Point SaveCoordinate
        {
            get { return saveCoordinate; }

            set
            {
                if (saveCoordinate != value)
                {
                    saveCoordinate = value;
                    OnPropertyChanged("SaveCoordinate");
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

        public string DieNum
        {
            get { return dieNum; }

            set
            {
                if (dieNum != value)
                {
                    dieNum = value;
                    OnPropertyChanged("DieNum");
                }
            }
        }

        public string DefectCount
        {
            get { return defectCount; }

            set
            {
                if (defectCount != value)
                {
                    defectCount = value;
                    OnPropertyChanged("DefectCount");
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

        #endregion        

        #region [생성자]

        public DefectListVM()
        {
            DefectValue = new ObservableCollection<DefectInfo>();
            share = GetAllInfo.Instance;
            share.PropertyChanged += GetAllInfo_PropertyChanged;
            IncreaseDefectNum = new RelayCommand(UpDefectNumber);
            DecreaseDefectNum = new RelayCommand(DownDefectNumber);
            PointVM = PointViewModel.Instance;
            PointVM.PropertyChanged += PointViewModel_PropertyChanged;
        }

        #endregion


        #region [public Method]

        /**
        * @brief DefectList View에 값을 출력하게 해주는 함수  
        * @note Patch-notes
        * 2022-08-31|이현호|
        */

        public void AddValue()
        {
            DefectListNum = share.TifValue.imageNum + " / " + share.DefectList.Count;
            
            if (share.Wafer.defectCount[share.Wafer.defectCountIndex] == 0)
            {
                DefectCount = 0 + " / " + 0;
            }

            else
            {
                share.Wafer.displayValue += 1;
                DefectCount = share.Wafer.displayValue + " / " + share.Wafer.defectCount[share.Wafer.defectCountIndex];
            }

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

        public void UpDefectNumber(object paramerter)
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

        public void DownDefectNumber(object parameter)
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

        public void UpDefectCount (object parameter)
        {
            int saveValue = (int)parameter;

            if (share.Wafer.defectCount[share.Wafer.defectCountIndex] == 0)
            {
                share.Wafer.displayValue = 0;
                return;
            }

            share.Wafer.displayValue += saveValue;

            TifFileInfo saveNum = new TifFileInfo();

            if (share.TifValue.imageNum == 0 || share.TifValue.imageNum == 1)
            {
                share.TifValue.imageNum = share.DefectList.Count + 1;
            }

            saveNum.imageNum = share.TifValue.imageNum + saveValue;
            saveNum.imageFile = share.TifValue.imageFile;
            saveNum.filePath = share.TifValue.filePath;
            share.TifValue = saveNum;

            DefectCount = share.Wafer.displayValue + " / " + share.Wafer.defectCount[share.Wafer.defectCountIndex];
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

        private void PointViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsClicked")
            {
                DefectListNum = share.TifValue.imageNum + " / " + share.DefectList.Count;
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
