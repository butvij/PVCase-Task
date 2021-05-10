using System.Collections.Generic;
using System.Linq;
using System.Windows.Shapes;

namespace PVCase_Task.Models {

    class DisplayableShape {

        protected List<Coordinate> _Coordinates;

        public int GetMaxCoordinate(string coordinate) {
            int result = 0;

            if (coordinate.ToLower().Equals("x")) {
                foreach (Coordinate c in _Coordinates) {
                    if (c.X > result) {
                        result = c.X;
                    }
                }
            } else if (coordinate.ToLower().Equals("y")) {
                foreach (Coordinate c in _Coordinates) {
                    if (c.Y > result) {
                        result = c.Y;
                    }
                }
            }
            return result;
        }

        public int GetMinCoordinate(string coordinate) {
            int result = int.MaxValue;

            if (coordinate.ToLower().Equals("x")) {
                foreach (Coordinate c in _Coordinates) {
                    if (c.X < result) {
                        result = c.X;
                    }
                }
            }
            else if (coordinate.ToLower().Equals("y")) {
                foreach (Coordinate c in _Coordinates) {
                    if (c.Y < result) {
                        result = c.Y;
                    }
                }
            }
            return result;
        }

        public List<Coordinate> Coordinates {
            get {
                return _Coordinates;
            }
        }

    public virtual Polygon GetShape() {
            if (_Coordinates == null || !_Coordinates.Any()) {
                return null;
            }
            Polygon shape = new Polygon();
            foreach (Coordinate coor in _Coordinates) {
                shape.Points.Add(coor.ToPoint());
            }
            return shape;
        }
    }
}
