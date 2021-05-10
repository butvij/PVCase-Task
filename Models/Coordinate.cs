using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PVCase_Task.Models {

    class Coordinate {

        public Coordinate(int X, int Y) {
            this.X = X;
            this.Y = Y;
        }

        private int _X;
        private int _Y;

        public int X {
            get {
                return _X;
            }
            set {
                if (value >= 0) {
                    _X = value;
                } else {
                    throw new ArgumentException("X coordinate cannot be negative");
                }
            }
        }

        public int Y {
            get {
                return _Y;
            }
            set {
                if (value >= 0) {
                    _Y = value;
                }
                else {
                    throw new ArgumentException("Y coordinate cannot be negative");
                }
            }
        }

        public Point ToPoint() {
            return new Point(X, Y);
        }

        //public override bool Equals(object obj) {
        //    if ((obj == null) || !GetType().Equals(obj.GetType())) {
        //        return false;
        //    }
        //    else {
        //        Coordinate coor = (Coordinate)obj;
        //        return (X == coor.X) && (Y == coor.Y);
        //    }
        //}

        //public override int GetHashCode() {
        //    return (X << 2) ^ Y;
        //}
    }
}
