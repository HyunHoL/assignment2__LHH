using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace assignment.Model
{
    class ShareInfo
    {
        #region [상수]

        DefectInfo defectInfo;
        WaferInfo waferInfo;
        FileInfo fileInfo;

        private WaferInfo wafer;
        private List<DefectInfo> defectList;
        private FileInfo fileValue;

        #endregion

        #region [인스턴스]

        public WaferInfo Wafer
        {
            get { return wafer; }

            set
            {
                if (wafer != value)
                {
                    wafer = value;
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
                }
            }
        }

        #endregion

        #region [속성]



        #endregion



        #region [생성자]

        public ShareInfo()
        {
            defectInfo = new DefectInfo();
            waferInfo = new WaferInfo();
            fileInfo = new FileInfo();
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

            FileValue.filePath = selectedFilePath;

            string fileExtension = Path.GetExtension(FileValue.filePath);

            if (fileExtension.Equals(".001", StringComparison.OrdinalIgnoreCase))
            {
                FileValue.fileData = File.ReadAllText(FileValue.filePath);
            }
        }

        public void ReadWaferInfo()
        {
            int waferIDIndex = FileValue.fileData.IndexOf("WaferID ") + "WaferID".Length + 1;
            int endWaferIDIndex = FileValue.fileData.IndexOf(';', waferIDIndex);
            Wafer.waferID = FileValue.fileData.Substring(waferIDIndex, endWaferIDIndex - waferIDIndex);

            int timestampIndex = FileValue.fileData.IndexOf("FileTimestamp ") + "FileTimestamp".Length + 1;
            int endTimestampIndex = FileValue.fileData.IndexOf(';', timestampIndex);
            Wafer.fileTimestamp = FileValue.fileData.Substring(timestampIndex, endTimestampIndex - timestampIndex);

            int lotIDIndex = FileValue.fileData.IndexOf("LotID ") + "LotID".Length + 1;
            int endLotIDIndex = FileValue.fileData.IndexOf(';', lotIDIndex);
            Wafer.lotID = FileValue.fileData.Substring(lotIDIndex, endLotIDIndex - lotIDIndex);

            int deviceIDIndex = FileValue.fileData.IndexOf("DeviceID ") + "DeviceID".Length + 1;
            int endDeviceIDIndex = fileValue.fileData.IndexOf(';', deviceIDIndex);
            Wafer.deviceID = FileValue.fileData.Substring(deviceIDIndex, endDeviceIDIndex - deviceIDIndex);

            int testPlanIndex = fileValue.fileData.IndexOf("SampleTestPlan") + "SampleTestPlan".Length + 4;
            int endIndex = fileValue.fileData.IndexOf(';', testPlanIndex);

            string substringFile = fileValue.fileData.Substring(testPlanIndex, endIndex - testPlanIndex);
            string[] lines = substringFile.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (values.Length == 2 && int.TryParse(values[0], out int value1) && int.TryParse(values[1], out int value2))
                {
                    Point saveValue = new Point();
                    saveValue.X = value1;
                    saveValue.Y = value2;
                    Wafer.sampleTestPlan.Add(saveValue);
                }
            }

            for (int i = 0; i < Wafer.sampleTestPlan.Count; i++)
            {
                Point saveValue = new Point();
                saveValue.X = Wafer.sampleTestPlan[i].X + 9;
                saveValue.Y = Math.Abs(Wafer.sampleTestPlan[i].Y - 24);
                Wafer.sampleTestPlan.Add(saveValue);
            }
        }

        #endregion


        #region [private Method]



        #endregion
    }
}
