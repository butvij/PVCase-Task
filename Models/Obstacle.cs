using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PVCase_Task.Models {

    class Obstacle : DisplayableShape {

        public Obstacle() {
            _Coordinates = new List<Coordinate>() {
                new Coordinate(173, 123),
                new Coordinate(182, 143),
                new Coordinate(145, 147),
                new Coordinate(134, 121)
            };
        }

        public override Polygon GetShape() {
            Polygon obstacle = base.GetShape();
            obstacle.Stroke = Brushes.Red;
            obstacle.StrokeThickness = 1;
            obstacle.HorizontalAlignment = HorizontalAlignment.Center;
            obstacle.VerticalAlignment = VerticalAlignment.Center;
            return obstacle;
        }

    }
}
