using System.ComponentModel;

namespace ShoutcastIntegration
{
    public class Station : INotifyPropertyChanged
    {
        private bool _isAlive;
        public string Name { get; set; }
        public string Type { get; set; }
        public int Bitrate { get; set; }
        public string Genre { get; set; }
        public string CurrentTrack { get; set; }
        public int TotalListeners { get; set; }
        public int ID { get; set; }

        public bool IsAlive
        {
            get
            {
                return _isAlive;
            }
            set
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IsAlive"));
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IsAliveString"));
                _isAlive = value;
            }
        }

        public string IsAliveString
        {
            get
            {
                if (IsAlive)
                {
                    return "Online";
                }
                return "";
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}