using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace assignment.Model
{
    class GetAllInfo : INotifyPropertyChanged
    {
        #region [상수]

        private static GetAllInfo instance;
        public WaferInfo wafer;
        public FileInfo fileValue;
        public List<DefectInfo> defectList;
        public List<Point> defectXY;
        public TifFileInfo tifValue;

        #endregion

        #region [인스턴스]

        public static GetAllInfo Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GetAllInfo();
                }

                return instance;
            }
        }

        #endregion

        #region [속성]

        public WaferInfo Wafer
        {
            get { return wafer; }

            set
            {
                if (wafer != value)
                {
                    wafer = value;
                    OnPropertyChanged("Wafer");
                }
            }
        }

        public FileInfo FileValue 
        {
            get { return fileValue; }

            set
            {
                if (fileValue != value)
                {
                    fileValue = value;
                    OnPropertyChanged("FileValue");
                }
            }
        }

        public List<DefectInfo> DefectList
        {
            get { return defectList; }

            set
            {
                if (defectList != value)
                {
                    defectList = value;
                    OnPropertyChanged("DefectList");
                }
            }
        }

        public List<Point> DefectXY
        {
            get { return defectXY; }

            set
            {
                if (defectXY != value)
                {
                    defectXY = value;
                    OnPropertyChanged("DefectXY");
                }
            }
        }

        public TifFileInfo TifValue
        {
            get { return tifValue; }

            set
            {
                if (tifValue != value)
                {
                    tifValue = value;
                    OnPropertyChanged("TifValue");
                }
            }
        }

        #endregion

        #region [생성자]

        public GetAllInfo()
        {
            DefectXY = new List<Point>();
            instance = null;
            FileValue = new FileInfo();
            DefectList = new List<DefectInfo>();
            Wafer = new WaferInfo();
            TifValue = new TifFileInfo();
            TifValue.imageNum = 1;
        }

        #endregion


        #region [public Method]

        public void LoadFolder()
        {
            
        }

        /**
        * @brief 파일을 열고 001 파일의 경로와 001 파일에 들어 있는 정보를 저장하는 함수  
        * @note Patch-notes
        * 2022-08-28|이현호|
        */

        public void GetFileData()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"C:\Users\hhlee\OneDrive - 에이티아이\바탕 화면\Klarf 과제";
            Instance.FileValue.folderPath = openFileDialog.InitialDirectory;
            string selectedFilePath = "";

            if (openFileDialog.ShowDialog() == true)
            {
                selectedFilePath = openFileDialog.FileName;
            }

            Instance.FileValue.filePath = selectedFilePath;

            string fileExtension = Path.GetExtension(Instance.FileValue.filePath);

            if (fileExtension.Equals(".001", StringComparison.OrdinalIgnoreCase))
            {
                Instance.FileValue.fileData = File.ReadAllText(FileValue.filePath);
            }

            ReadWaferInfo();
            GetDefectList();
            GetTifData();

        }

        /**
        * @brief 001 파일 정보에서 필요한 Wafer 정보를 추출해내는 함수  
        * @note Patch-notes
        * 2022-08-28|이현호|
        */

        public void ReadWaferInfo()
        {
            WaferInfo newWafer = new WaferInfo();

            int waferIDIndex = Instance.FileValue.fileData.IndexOf("WaferID ") + "WaferID".Length + 1;
            int endWaferIDIndex = Instance.FileValue.fileData.IndexOf(';', waferIDIndex);
            newWafer.waferID = FileValue.fileData.Substring(waferIDIndex, endWaferIDIndex - waferIDIndex);

            int timestampIndex = Instance.FileValue.fileData.IndexOf("FileTimestamp ") + "FileTimestamp".Length + 1;
            int endTimestampIndex = Instance.FileValue.fileData.IndexOf(';', timestampIndex);
            newWafer.fileTimestamp = Instance.FileValue.fileData.Substring(timestampIndex, endTimestampIndex - timestampIndex);

            int lotIDIndex = Instance.FileValue.fileData.IndexOf("LotID ") + "LotID".Length + 1;
            int endLotIDIndex = Instance.FileValue.fileData.IndexOf(';', lotIDIndex);
            newWafer.lotID = Instance.FileValue.fileData.Substring(lotIDIndex, endLotIDIndex - lotIDIndex);

            int deviceIDIndex = Instance.FileValue.fileData.IndexOf("DeviceID ") + "DeviceID".Length + 1;
            int endDeviceIDIndex = Instance.FileValue.fileData.IndexOf(';', deviceIDIndex);
            newWafer.deviceID = Instance.FileValue.fileData.Substring(deviceIDIndex, endDeviceIDIndex - deviceIDIndex);

            int testPlanIndex = Instance.FileValue.fileData.IndexOf("SampleTestPlan") + "SampleTestPlan".Length + 4;
            int endIndex = Instance.FileValue.fileData.IndexOf(';', testPlanIndex);

            string substringFile = Instance.FileValue.fileData.Substring(testPlanIndex, endIndex - testPlanIndex);
            string[] lines = substringFile.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (values.Length == 2 && int.TryParse(values[0], out int value1) && int.TryParse(values[1], out int value2))
                {
                    Point saveValue = new Point();
                    saveValue.X = value1;
                    saveValue.Y = value2;
                    newWafer.sampleTestPlan.Add(saveValue);
                }
            }
            Instance.Wafer = newWafer;
        }

        /**
        * @brief 001 파일 정보에서 필요한 Defect 정보를 추출해내는 함수  
        * @note Patch-notes
        * 2022-08-30|이현호|
        */

        public void GetDefectList()
        {
            List<DefectInfo> newDefectList = new List<DefectInfo>();
            List<Point> newDefectXY = new List<Point>();

            int defectIndex = Instance.FileValue.fileData.IndexOf("DefectList") + "DefectList".Length;
            int endIndex = Instance.FileValue.fileData.IndexOf(';', defectIndex);
            string substringFile = Instance.FileValue.fileData.Substring(defectIndex, endIndex - defectIndex);
            string[] lines = substringFile.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            string[] values = new string[17];

            for (int i = 0; i < lines.Length; i = i + 2)
            {
                DefectInfo saveValue = new DefectInfo();
                string[] defectValue;
                defectValue = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                for (int k = 0; k < defectValue.Length; k++)
                {
                    values[k] = defectValue[k];
                }

                AddInfo(values, saveValue);
                newDefectList.Add(saveValue);
                newDefectXY.Add(new Point { X = saveValue.defectXY.X, Y = saveValue.defectXY.Y });
            }
            Instance.DefectList = newDefectList;
            Instance.DefectXY = newDefectXY;
        }

        /**
        * @brief TIF 파일에서 이미지 정보를 추출해내는 함수  
        * @note Patch-notes
        * 2022-09-04|이현호|
        */

        public void GetTifData()
        {
            TifFileInfo saveValue = new TifFileInfo();

            if (Instance.FileValue.folderPath != null)
            {
                saveValue.filePath = Directory.GetFiles(Instance.FileValue.folderPath, "*.tif");
            }

            saveValue.imageFile = new TiffBitmapDecoder(new Uri(saveValue.filePath[0], UriKind.Absolute), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

            TifValue = saveValue;
        }

        #endregion


        #region [private Method]

        /**
        * @brief Defect정보 중 Defect List에 값을 채워주는 함수  
        * @note Patch-notes
        * 2022-08-30|이현호|
        */

        private void AddInfo(string[] values, DefectInfo saveValue)
        {
            if (values.Length < 17)
            {
                return;
            }

            saveValue.defectID = double.Parse(values[0]);
            saveValue.xrel = double.Parse(values[1]);
            saveValue.yrel = double.Parse(values[2]);
            saveValue.defectXY = new Point { X = double.Parse(values[3]), Y = double.Parse(values[4]) } ;
            saveValue.xSize = double.Parse(values[5]);
            saveValue.ySize = double.Parse(values[6]);
            saveValue.defectArea = double.Parse(values[7]);
            saveValue.dSize = double.Parse(values[8]);
            saveValue.classNumber = double.Parse(values[9]);
            saveValue.test = double.Parse(values[10]);
            saveValue.clusterNumber = double.Parse(values[11]);
            saveValue.roughBinNumber = double.Parse(values[12]);
            saveValue.fineBinNumber = double.Parse(values[13]);
            saveValue.reviewSample = double.Parse(values[14]);
            saveValue.imageCount = double.Parse(values[15]);
            saveValue.imageList = double.Parse(values[16]);

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
