using System;
using System.ComponentModel;

namespace ProjektSI
{
    [Serializable]
    public class Pole : INotifyPropertyChanged
    {


        #region Zmienne
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        
        private Block typSciany;

        public Block TypSciany
        {
            get
            {
                return typSciany;
            }
            set
            {
                typSciany = value;
                OnPropertyChanged("TypSciany");
            }
        }

        #endregion

        #region Kontruktor
        public Pole()
        {

        }
        public Pole(Block bl)
        {
            TypSciany = bl;
        }
        public Pole(Block bl, int XPosition, int YPosition)
        {
            TypSciany = bl;
            this.XPosition = XPosition;
            this.YPosition = YPosition;
        }
        #endregion

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string property)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }




    }
}
