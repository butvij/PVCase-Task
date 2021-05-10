using System;
using System.ComponentModel;

namespace PVCase_Task.Models {

    class Cell {

        private int _Width;
        private int _Length;
        private int _TiltAngle;
        private int _EffectiveWidth;

        public Cell(int Width, int Length, int TiltAngle) {
            this.Width = Width;
            this.Length = Length;
            this.TiltAngle = TiltAngle;
            _EffectiveWidth = Convert.ToInt32(Width * Math.Cos(TiltAngle));
        }

        public Cell() {

        }

        public int Width {

            get {
                return _Width;
            }
            set {
                if (value >= 0) {
                    _Width = value;
                    _EffectiveWidth = Convert.ToInt32(Width * Math.Cos((Math.PI / 180) * TiltAngle));
                } else {
                    throw new ArgumentException("Width cannot be negative");
                }
            }
        }

        public int Length {

            get {
                return _Length;
            }
            set {
                if (value >= 0) {
                    _Length = value;
                } else {
                    throw new ArgumentException("Length cannot be negative");
                }
            }
        }

        public int TiltAngle {

            get {
                return _TiltAngle;
            }
            set {
                if (value >= 0 && value <= 60) {
                    _TiltAngle = value;
                    _EffectiveWidth = Convert.ToInt32(Width * Math.Cos((Math.PI / 180) * TiltAngle));
                } else if (value == -1) {
                    _TiltAngle = value;
                } else {
                    throw new ArgumentException("Please use tilt angle between 0 and 60 degrees");
                }
            }
        }

        public int EffectiveWidth {

            get {
                return _EffectiveWidth;
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
