using assignment.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace assignment.ViewModel
{
    class DefectImageVM : INotifyPropertyChanged
    {
        #region [상수]

        private GetAllInfo share;
        private BitmapSource loadImage;

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

        public BitmapSource LoadImage
        {
            get { return loadImage; }

            set
            {
                if (loadImage != value)
                {
                    loadImage = value;
                    OnPropertyChanged("LoadImage");
                }
            }
        }


        #endregion



        #region [생성자]

        public DefectImageVM()
        {

            share = GetAllInfo.Instance;
            share.PropertyChanged += GetAllInfo_PropertyChanged;
        }

        #endregion



        #region [public Method]

        /**
        * @brief 이미지 번호에 맞게 이미지를 출력해주는 함수  
        * @note Patch-notes
        * 2022-09-04|이현호|
        */

        public void AddImage()
        {
            if (share.TifValue.imageNum != 0) 
                LoadImage = share.TifValue.imageFile.Frames[share.TifValue.imageNum - 1];

            else
            {
                LoadImage = share.TifValue.imageFile.Frames[share.TifValue.imageNum];
            }
        }

        #endregion



        #region [private Method]

        /**
        * @brief GetAllInfo 클래스에서 TifValue의 값이 변경되었을 때, 이벤트를 받아오는 함수  
        * @note Patch-notes
        * 2022-09-04|이현호|
        */

        private void GetAllInfo_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TifValue")
            {
                AddImage();
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
