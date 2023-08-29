using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace assignment.Model
{
    class ShareInfo : INotifyPropertyChanged
    {
        #region [상수]

        private static ShareInfo instance;
        public DefectInfo saveValue;
        private WaferInfo wafer;
        private FileInfo fileValue;
        private List<DefectInfo> defectList;
        private bool isFileOpened = false;
        #endregion

        #region [인스턴스]

        public static ShareInfo Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ShareInfo();
                }

                return instance;
            }
        }

        #endregion

        #region [속성]


        public WaferInfo Wafer { get; set; }
        public FileInfo FileValue { get; set; }
        public List<DefectInfo> DefectList { get; set; }

        public bool IsFileOpened
        {
            get { return isFileOpened; }

            set
            {
                if (isFileOpened != value )
                {
                    isFileOpened = value;
                    OnPropertyChanged("IsFileOpened");
                }
            }
        }

        public void ChangedValue ()
        {
            IsFileOpened = !IsFileOpened;
        }
        #endregion



        #region [생성자]

        public ShareInfo()
        {
            instance = null;
            FileValue = new FileInfo();
            DefectList = new List<DefectInfo>();
            Wafer = new WaferInfo();
            saveValue = new DefectInfo();
        }

        #endregion


        #region [public Method]

        public void GetFileData()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"C:\Users\hhlee\OneDrive - 에이티아이\바탕 화면\Klarf 과제";
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
        }

        public void ReadWaferInfo()
        {
            int waferIDIndex = Instance.FileValue.fileData.IndexOf("WaferID ") + "WaferID".Length + 1;
            int endWaferIDIndex = Instance.FileValue.fileData.IndexOf(';', waferIDIndex);
            Instance.Wafer.waferID = FileValue.fileData.Substring(waferIDIndex, endWaferIDIndex - waferIDIndex);

            int timestampIndex = Instance.FileValue.fileData.IndexOf("FileTimestamp ") + "FileTimestamp".Length + 1;
            int endTimestampIndex = Instance.FileValue.fileData.IndexOf(';', timestampIndex);
            Instance.Wafer.fileTimestamp = Instance.FileValue.fileData.Substring(timestampIndex, endTimestampIndex - timestampIndex);

            int lotIDIndex = Instance.FileValue.fileData.IndexOf("LotID ") + "LotID".Length + 1;
            int endLotIDIndex = Instance.FileValue.fileData.IndexOf(';', lotIDIndex);
            Instance.Wafer.lotID = Instance.FileValue.fileData.Substring(lotIDIndex, endLotIDIndex - lotIDIndex);

            int deviceIDIndex = Instance.FileValue.fileData.IndexOf("DeviceID ") + "DeviceID".Length + 1;
            int endDeviceIDIndex = Instance.FileValue.fileData.IndexOf(';', deviceIDIndex);
            Instance.Wafer.deviceID = Instance.FileValue.fileData.Substring(deviceIDIndex, endDeviceIDIndex - deviceIDIndex);

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
                    saveValue.X = value1 + 9;
                    saveValue.Y = Math.Abs(value2 - 24);
                    Instance.Wafer.sampleTestPlan.Add(saveValue);
                }
            }
        }

        public void GetDefectList()
        {
            int defectIndex = Instance.FileValue.fileData.IndexOf("DefectList") + "DefectList".Length;
            int endIndex = Instance.FileValue.fileData.IndexOf(';', defectIndex);
            string substringFile = Instance.FileValue.fileData.Substring(defectIndex, endIndex - defectIndex);
            string[] lines = substringFile.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            string[] values = new string[17];

            for (int i = 0; i < lines.Length / 2; i++)
            {

                string[] defectValue;
                defectValue = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                for (int k = 0; k < defectValue.Length; k++)
                {
                    values[k] = defectValue[k];
                }

                AddInfo(values);

            }
        }
        #endregion


        #region [private Method]

        private void AddInfo(string[] values)
        {
            if (values.Length < 17)
            {
                return;
            }

            saveValue.defectID = double.Parse(values[0]);
            saveValue.xrel = double.Parse(values[1]);
            saveValue.yrel = double.Parse(values[2]);
            saveValue.defectXY.X = double.Parse(values[3]) + 9;
            saveValue.defectXY.Y = Math.Abs(double.Parse(values[4]) - 24);
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

            Instance.DefectList.Add(saveValue);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
