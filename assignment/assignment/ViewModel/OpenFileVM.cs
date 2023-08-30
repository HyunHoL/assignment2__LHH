using assignment.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace assignment.ViewModel
{
    class OpenFileVM
    {
        #region [상수]

        public ShareInfo shareInfo;
        public event EventHandler OpenedFile;
        private int value;
        #endregion

        #region [인터페이스]

        public ICommand OpenFileCommand { get; }

        #endregion

        #region [속성]

        public int Value
        {
            get { return value; }

            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    OnValueChanged(EventArgs.Empty);
                }
            }
        }

        #endregion

        #region [생성자]

        public OpenFileVM()
        {
            shareInfo = ShareInfo.Instance;
            OpenFileCommand = new RelayCommand(LoadFile);
        }

        #endregion



        #region [public Method]



        #endregion



        #region [private Method]

        private void LoadFile (object parameter)
        {
            shareInfo.GetFileData();
            shareInfo.GetDefectList();
            shareInfo.ReadWaferInfo();
        }

        protected virtual void OnValueChanged(EventArgs e)
        {
            OpenedFile?.Invoke(this, e);
        }
        #endregion
    }
}
