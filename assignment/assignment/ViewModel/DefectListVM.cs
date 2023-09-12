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
        private int saveIndex;

        #endregion


        #region [속성]

        public ICommand IncreaseDefectNum { get; }
        public ICommand DecreaseDefectNum { get; }
        public ICommand IncreaseDieNum { get; }
        public ICommand DecreaseDieNum { get; }
        public ICommand IncreaseDefectCount { get; }
        public ICommand DecreaseDefectCount { get; }
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
        * 2023-09-04|이현호|
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
                    Point target = share.DefectXY[share.TifValue.imageNum - 1];

                    for (int i = 0; i < share.Wafer.sampleTestPlan.Count; i++)
                    {
                        if (share.Wafer.sampleTestPlan[i] == target)
                        {
                            share.Wafer.dieNumIndex = i;
                            break;
                        }
                    }

                    if (share.Wafer.defectCount[share.Wafer.dieNumIndex] == 0)
                    {
                        share.Wafer.displayValue = 0;
                    }

                    else if (share.Wafer.dieNumIndex == saveIndex)
                    {
                        share.Wafer.displayValue += 1;
                    }

                    else
                    {
                        share.Wafer.displayValue = 1;
                    }

                    DieNum = share.Wafer.dieNumIndex + " / " + share.Wafer.sampleTestPlan.Count;
                    DefectCount = share.Wafer.displayValue + " / " + share.Wafer.defectCount[share.Wafer.dieNumIndex];
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
            IncreaseDefectNum = new RelayCommand<object>(UpDefectNumber);
            DecreaseDefectNum = new RelayCommand<object>(DownDefectNumber);
            PointVM = PointViewModel.Instance;
            PointVM.PropertyChanged += PointViewModel_PropertyChanged;
            IncreaseDieNum = new RelayCommand<object>(UpDieNum);
            DecreaseDieNum = new RelayCommand<object>(DownDieNum);
            IncreaseDefectCount = new RelayCommand<object>(UpDefectCount);
            DecreaseDefectCount = new RelayCommand<object>(DownDefectCount);
        }

        #endregion


        #region [public Method]

        /**
        * @brief DefectList View에 값을 출력하게 해주는 함수  
        * @note Patch-notes
        * 2023-08-31|이현호|
        */

        public void AddValue()
        {
            DefectListNum = share.TifValue.imageNum + " / " + share.DefectList.Count;

            Point target = share.DefectXY[share.TifValue.imageNum - 1];

            for (int i = 0; i < share.Wafer.sampleTestPlan.Count; i++)
            {
                if (share.Wafer.sampleTestPlan[i] == target)
                {
                    share.Wafer.dieNumIndex = i;
                    break;
                }
            }

            if (share.Wafer.defectCount[share.Wafer.dieNumIndex] == 0)
            {
                share.Wafer.displayValue = 0;
            }

            else
            {
                share.Wafer.displayValue = 1;
            }

            Instance.SaveCoordinate = share.Wafer.sampleTestPlan[share.Wafer.dieNumIndex];
            DieNum = share.Wafer.dieNumIndex + " / " + share.Wafer.sampleTestPlan.Count;
            DefectCount = share.Wafer.displayValue + " / " + share.Wafer.defectCount[share.Wafer.dieNumIndex];
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
        * 2023-09-04|이현호|
        */

        public void UpDefectNumber(object parameter)
        {
            int saveValue = int.Parse((string)parameter);
            TifFileInfo saveNum = new TifFileInfo();

            if (share.TifValue.imageNum == share.DefectList.Count)
            {
                return;
            }

            if (share.TifValue.imageNum == 0)
            {
                share.TifValue.imageNum += 1;
            }

            saveNum.imageNum = share.TifValue.imageNum + saveValue;
            saveNum.imageFile = share.TifValue.imageFile;
            saveNum.filePath = share.TifValue.filePath;
            share.TifValue = saveNum;

            Point target = share.DefectXY[share.TifValue.imageNum - 1];

            for (int i = 0; i < share.Wafer.sampleTestPlan.Count; i++)
            {
                if (share.Wafer.sampleTestPlan[i] == target)
                {
                    share.Wafer.dieNumIndex = i;
                    break;
                }
            }

            if (share.Wafer.defectCount[share.Wafer.dieNumIndex] == 0)
            {
                share.Wafer.displayValue = 0;
            }

            else if (share.Wafer.dieNumIndex == saveIndex)
            {
                share.Wafer.displayValue += 1;
            }

            else
            {
                share.Wafer.displayValue = 1;
            }

            saveIndex = share.Wafer.dieNumIndex;
            Instance.SaveCoordinate = share.Wafer.sampleTestPlan[share.Wafer.dieNumIndex];
            DieNum = share.Wafer.dieNumIndex + " / " + share.Wafer.sampleTestPlan.Count;
            DefectCount = share.Wafer.displayValue + " / " + share.Wafer.defectCount[share.Wafer.dieNumIndex];
            DefectListNum = share.TifValue.imageNum + " / " + share.DefectList.Count;
        }

        /**
        * @brief 버튼을 클릭하였을 때, 한 Die에 여러 Defect이 존재할 경우 다음 Defect으로 넘겨주는 함수  
        * @note Patch-notes
        * 2023-09-07|이현호|
        */

        public void UpDefectCount(object parameter)
        {
            if (share.Wafer.displayValue == share.Wafer.defectCount[share.Wafer.dieNumIndex])
            {
                return;
            }

            if (share.Wafer.defectCount[share.Wafer.dieNumIndex] == 0)
            {
                share.Wafer.displayValue = 0;
            }

            share.Wafer.displayValue += int.Parse((string)parameter);
            TifFileInfo saveNum = new TifFileInfo();
            saveNum.imageNum = share.TifValue.imageNum + int.Parse((string)parameter);
            saveNum.imageFile = share.TifValue.imageFile;
            saveNum.filePath = share.TifValue.filePath;
            share.TifValue = saveNum;

            Instance.SaveCoordinate = share.Wafer.sampleTestPlan[share.Wafer.dieNumIndex];
            DieNum = share.Wafer.dieNumIndex + " / " + share.Wafer.sampleTestPlan.Count;
            DefectCount = share.Wafer.displayValue + " / " + share.Wafer.defectCount[share.Wafer.dieNumIndex];
            DefectListNum = share.TifValue.imageNum + " / " + share.DefectList.Count;
        }

        /**
        * @brief 버튼을 클릭하였을 때, 다음 Die로 넘겨주는 함수  
        * @note Patch-notes
        * 2023-09-07|이현호|
        */

        public void UpDieNum (object parameter)
        {
            if (share.Wafer.dieNumIndex == share.Wafer.sampleTestPlan.Count)
            {
                return;
            }

            int saveValue = int.Parse((string)parameter);
            share.Wafer.dieNumIndex += saveValue;
            TifFileInfo saveNum = new TifFileInfo();

            if (share.Wafer.defectCount[share.Wafer.dieNumIndex] == 0)
            {
                share.Wafer.displayValue = 0;
            }

            else
            {
                Point target = share.Wafer.sampleTestPlan[share.Wafer.dieNumIndex];
                share.Wafer.displayValue = 1;

                for (int i = 0; i < share.DefectXY.Count; i++)
                {
                    if (target == share.DefectXY[i])
                    {
                        saveNum.imageNum = i;
                        saveNum.imageFile = share.TifValue.imageFile;
                        saveNum.filePath = share.TifValue.filePath;
                        share.TifValue = saveNum;

                        break;
                    }
                }
            }

            Instance.SaveCoordinate = share.Wafer.sampleTestPlan[share.Wafer.dieNumIndex];
            DieNum = share.Wafer.dieNumIndex + " / " + share.Wafer.sampleTestPlan.Count;
            DefectCount = share.Wafer.displayValue + " / " + share.Wafer.defectCount[share.Wafer.dieNumIndex];
            DefectListNum = share.TifValue.imageNum + " / " + share.DefectList.Count;
        }

        /**
        * @brief 버튼을 클릭하였을 때, 이전 Die로 넘겨주는 함수  
        * @note Patch-notes
        * 2023-09-07|이현호|
        */

        public void DownDieNum (object parameter)
        {
            if (share.Wafer.dieNumIndex == 1)
            {
                return;
            }

            int saveValue = int.Parse((string)parameter);
            share.Wafer.dieNumIndex += saveValue;
            TifFileInfo saveNum = new TifFileInfo();

            if (share.Wafer.defectCount[share.Wafer.dieNumIndex] == 0)
            {
                share.Wafer.displayValue = 0;
            }

            else
            {
                Point target = share.Wafer.sampleTestPlan[share.Wafer.dieNumIndex];
                share.Wafer.displayValue = 1;

                for (int i = 0; i < share.DefectXY.Count; i++)
                {
                    if (target == share.DefectXY[i])
                    {
                        saveNum.imageNum = i;
                        saveNum.imageFile = share.TifValue.imageFile;
                        saveNum.filePath = share.TifValue.filePath;
                        share.TifValue = saveNum;
                        break;
                    }
                }
            }

            Instance.SaveCoordinate = share.Wafer.sampleTestPlan[share.Wafer.dieNumIndex];
            DieNum = share.Wafer.dieNumIndex + " / " + share.Wafer.sampleTestPlan.Count;
            DefectCount = share.Wafer.displayValue + " / " + share.Wafer.defectCount[share.Wafer.dieNumIndex];
            DefectListNum = share.TifValue.imageNum + " / " + share.DefectList.Count;
        }

        /**
        * @brief 버튼을 클릭하였을 때, 한 Die에 여러 Defect이 존재할 경우 이전 Defect으로 넘겨주는 함수  
        * @note Patch-notes
        * 2023-09-07|이현호|
        */
        public void DownDefectCount(object parameter)
        {
            if (share.Wafer.displayValue == 1 || share.Wafer.displayValue == 0)
            {
                return;
            }

            if (share.Wafer.defectCount[share.Wafer.dieNumIndex] == 0)
            {
                share.Wafer.displayValue = 0;
            }

            share.Wafer.displayValue += int.Parse((string)parameter);
            TifFileInfo saveNum = new TifFileInfo();
            saveNum.imageNum = share.TifValue.imageNum + int.Parse((string)parameter);
            saveNum.imageFile = share.TifValue.imageFile;
            saveNum.filePath = share.TifValue.filePath;
            share.TifValue = saveNum;

            Instance.SaveCoordinate = share.Wafer.sampleTestPlan[share.Wafer.dieNumIndex];
            DieNum = share.Wafer.dieNumIndex + " / " + share.Wafer.sampleTestPlan.Count;
            DefectCount = share.Wafer.displayValue + " / " + share.Wafer.defectCount[share.Wafer.dieNumIndex];
            DefectListNum = share.TifValue.imageNum + " / " + share.DefectList.Count;
        }

        /**
        * @brief 버튼을 클릭하였을 때, 이전 Defect으로 넘겨주는 함수  
        * @note Patch-notes
        * 2023-09-04|이현호|
        */

        public void DownDefectNumber(object parameter)
        {
            int saveValue = int.Parse((string)parameter);
            TifFileInfo saveNum = new TifFileInfo();

            if (share.TifValue.imageNum == 1)
            {
                return;
            }

            saveNum.imageNum = share.TifValue.imageNum + saveValue;
            saveNum.imageFile = share.TifValue.imageFile;
            saveNum.filePath = share.TifValue.filePath;
            share.TifValue = saveNum;

            Point target = share.DefectXY[share.TifValue.imageNum - 1];

            for (int i = 0; i < share.Wafer.sampleTestPlan.Count; i++)
            {
                if (share.Wafer.sampleTestPlan[i] == target)
                {
                    share.Wafer.dieNumIndex = i;
                    break;
                }
            }

            if (share.Wafer.defectCount[share.Wafer.dieNumIndex] == 0)
            {
                share.Wafer.displayValue = 0;
            }

            else
            {
                share.Wafer.displayValue = 1;
            }

            Instance.SaveCoordinate = share.Wafer.sampleTestPlan[share.Wafer.dieNumIndex];
            DieNum = share.Wafer.dieNumIndex + " / " + share.Wafer.sampleTestPlan.Count;
            DefectCount = share.Wafer.displayValue + " / " + share.Wafer.defectCount[share.Wafer.dieNumIndex];
            DefectListNum = share.TifValue.imageNum + " / " + share.DefectList.Count;
        }

        #endregion

        #region [private Method]

        /**
        * @brief GetAllInfo 클래스에서 이벤트를 받아오는 함수
        * @note Patch-notes
        * 2023-09-04|이현호|
        */

        private void GetAllInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DefectList")
            {
                AddValue();
            }
        }

        /**
        * @brief PointViewModel 클래스에서 IsClicked 이벤트가 발생하여 WaferMap이 변경되었을 때, DefectListView에도 적용해주는 함수
        * @note Patch-notes
        * 2023-09-06|이현호|
        */

        private void PointViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsClicked")
            {
                DefectListNum = share.TifValue.imageNum + " / " + share.DefectList.Count;
                Point target = share.DefectXY[share.TifValue.imageNum - 1];

                for (int i = 0; i < share.Wafer.sampleTestPlan.Count; i++)
                {
                    if (share.Wafer.sampleTestPlan[i] == target)
                    {
                        share.Wafer.dieNumIndex = i;
                        break;
                    }
                }

                if (share.Wafer.defectCount[share.Wafer.dieNumIndex] == 0)
                {
                    share.Wafer.displayValue = 0;
                }

                else
                {
                    share.Wafer.displayValue = 1;
                }

                Instance.SaveCoordinate = share.Wafer.sampleTestPlan[share.Wafer.dieNumIndex];
                DieNum = share.Wafer.dieNumIndex + " / " + share.Wafer.sampleTestPlan.Count;
                DefectCount = share.Wafer.displayValue + " / " + share.Wafer.defectCount[share.Wafer.dieNumIndex];
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
