using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PVCase_Task.Models {

    class Site : DisplayableShape {

        private int _RowSpacing;
        private int _ColumnSpacing;

        public Site() {
            _Coordinates = new List<Coordinate> {
                new Coordinate(83, 136),
                new Coordinate(124, 186),
                new Coordinate(252, 155),
                new Coordinate(277, 47),
                new Coordinate(183, 82),
                new Coordinate(163, 4),
                new Coordinate(80, 25)
            };
            //RowSpacing = 0;
            //ColumnSpacing = 0;
        }

        public int RowSpacing {
            get {
                return _RowSpacing;
            }
            set {
                if (value >= 0){
                    _RowSpacing = value;
                }
                else {
                    throw new ArgumentException("Spacing cannot be negative, would not get much sun");
                }
            }
        }

        public int ColumnSpacing {
            get {
                return _ColumnSpacing;
            }
            set {
                if (value >= 0) {
                    _ColumnSpacing = value;
                }
                else {
                    throw new ArgumentException("Spacing cannot be negative, would not get much sun");
                }
            }
        }

        public override Polygon GetShape() {
            Polygon site = base.GetShape();
            site.Stroke = Brushes.Yellow;
            site.StrokeThickness = 1;
            site.HorizontalAlignment = HorizontalAlignment.Center;
            site.VerticalAlignment = VerticalAlignment.Center;
            return site;
        }
    }
}
