using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace assignment.Model
{
    class DefectInfo : INotifyPropertyChanged
    {
        private double saveDefectID;

        public double defectID
        {
            get { return saveDefectID; }

            set
            {
                if (saveDefectID != value)
                {
                    saveDefectID = value;
                    OnPropertyChanged("defectID");
                }
            }
        }
        private double saveXRel;
        public double xrel
        {
            get { return saveXRel; }

            set
            {
                if (saveXRel != value)
                {
                    saveXRel = value;
                    OnPropertyChanged("xrel");
                }
            }
        }
        private double saveYRel;
        public double yrel
        {
            get { return saveYRel; }

            set
            {
                if (saveYRel != value)
                {
                    saveYRel = value;
                    OnPropertyChanged("yrel");
                }
            }
        }

        private double saveXSize;
        public double xSize
        {
            get { return saveXSize; }

            set
            {
                if (saveXSize != value)
                {
                    saveXSize = value;
                    OnPropertyChanged("xSize");
                }
            }
        }

        private double saveYSize;
        public double ySize
        {
            get { return saveYSize; }

            set
            {
                if (saveYSize != value)
                {
                    saveYSize = value;
                    OnPropertyChanged("ySize");
                }
            }
        }

        private double saveDefectArea;
        public double defectArea
        {
            get { return saveDefectArea; }

            set
            {
                if (saveDefectArea != value)
                {
                    saveDefectArea = value;
                    OnPropertyChanged("defectArea");
                }
            }
        }

        private double saveDSize;
        public double dSize
        {
            get { return saveDSize; }

            set
            {
                if (saveDSize != value)
                {
                    saveDSize = value;
                    OnPropertyChanged("dSize");
                }
            }
        }

        private double saveClassNumber;
        public double classNumber
        {
            get { return saveClassNumber; }

            set
            {
                if (saveClassNumber != value)
                {
                    saveClassNumber = value;
                    OnPropertyChanged("classNumber");
                }
            }
        }

        private double saveTest;
        public double test
        {
            get { return saveTest; }
            
            set
            {
                if (saveTest != value)
                {
                    saveTest = value;
                    OnPropertyChanged("test");
                }
            }
        }

        private double saveClusterNumber;
        public double clusterNumber
        {
            get { return saveClusterNumber; }

            set
            {
                if (saveClusterNumber != value)
                {
                    saveClusterNumber = value;
                    OnPropertyChanged("clusterNumber");
                }
            }
        }

        private double saveRoughBinNumber;
        public double roughBinNumber
        {
            get { return saveRoughBinNumber; }

            set
            {
                if (saveRoughBinNumber != value)
                {
                    saveRoughBinNumber = value;
                    OnPropertyChanged("roughBinNumber");
                }
            }
        }

        private double saveFineBinNumber;
        public double fineBinNumber
        {
            get { return saveFineBinNumber; }

            set
            {
                if (saveFineBinNumber != value)
                {
                    saveFineBinNumber = value;
                    OnPropertyChanged("fineBinNumber");
                }
            }
        }

        private double saveReviewSample;
        public double reviewSample
        {
            get { return saveReviewSample; }

            set
            {
                if (saveReviewSample != value)
                {
                    saveReviewSample = value;
                    OnPropertyChanged("reviewSample");
                }
            }
        }

        private double saveImageCount;
        public double imageCount
        {
            get { return saveImageCount; }

            set
            {
                if (saveImageCount != value)
                {
                    saveImageCount = value;
                    OnPropertyChanged("imageCount");
                }
            }
        }

        private double saveImageList;
        public double imageList
        {
            get { return saveImageList; }

            set
            {
                if (saveImageList != value)
                {
                    saveImageList = value;
                    OnPropertyChanged("imageList");
                }
            }
        }

        private Point saveDefectXY;
        public Point defectXY
        {
            get { return saveDefectXY; }

            set
            {
                if (saveDefectXY != value)
                {
                    saveDefectXY = value;
                    OnPropertyChanged("defectXY");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
