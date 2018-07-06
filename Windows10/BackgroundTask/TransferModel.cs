/*
 * 扩展了 DownloadOperation 和 UploadOperation，用于 MVVM 绑定数据
 */

using System;
using System.ComponentModel;
using Windows.Networking.BackgroundTransfer;

namespace Windows10.BackgroundTask
{
    public class TransferModel : INotifyPropertyChanged
    {
        public DownloadOperation DownloadOperation { get; set; }
        public UploadOperation UploadOperation { get; set; }

        public string Source { get; set; }
        public string Destination { get; set; }

        private string _progress;
        public string Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                RaisePropertyChanged("Progress");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
