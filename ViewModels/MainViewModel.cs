using Caliburn.Micro;
using PVCase_Task.Commands;
using PVCase_Task.Models;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PVCase_Task.ViewModels {

    class MainViewModel : INotifyPropertyChanged {

        private Site _site;
        private Obstacle _obstacle;
        private Cell _cell;
        private int _canvasHeight;
        private int _canvasWidth;
        private int _yScaleFactor;
        private int _xScaleFactor;
        private string _errorMessage;
        private string _noError = "";

        public MainViewModel() {
            _site = new Site();
            _obstacle = new Obstacle();
            _cell = new Cell();
            GenerateCommandUpdate = new GenerateCommand(this);
            Shapes = new BindableCollection<Polygon>();
            Shapes.Add(_site.GetShape());
            Shapes.Add(_obstacle.GetShape());
            MinWindowHeight = "330";
            MinWindowWidth = "500";
            _yScaleFactor = 1;
            _xScaleFactor = 1;
            ErrorMessage = _noError;
        }

        public string ErrorMessage {
            get {
                return _errorMessage;
            }
            set {
                _errorMessage = value;
            }
        
        }

        public string MinWindowHeight { get; set; }

        public string MinWindowWidth { get; set; }

        public BindableCollection<Polygon> Shapes { get; set; }

        public bool CanUpdate {
            get {
                if (Width == null || Lenght == null || TiltAngle == null || RowSpacing == null || ColumnSpacing == null) {
                    return false;
                } else {
                    return !Width.Equals("0") && !Lenght.Equals("0") && !TiltAngle.Equals("-1") &&
                        !RowSpacing.Equals("0") && !ColumnSpacing.Equals("0");
                }
            }
        }
        
        public string Width {
            get {
                return _cell.Width.ToString();
            }
            set {
                if (int.TryParse(value, out int result)) {
                    try {
                        _cell.Width = result;
                        ErrorMessage = _noError;
                    }
                    catch (ArgumentException e) {
                        ErrorMessage = e.Message;
                    }
                    OnPropertyChanged("ErrorMessage");
                }
                else {
                    _cell.Width = 0;
                }
            }
        }

        public string Lenght {
            get {
                return _cell.Length.ToString();
            }
            set {
                if (int.TryParse(value, out int result)) {
                    try {
                        _cell.Length = result;
                        ErrorMessage = _noError;
                    }
                    catch (ArgumentException e) {
                        ErrorMessage = e.Message;
                    }
                    OnPropertyChanged("ErrorMessage");
                }
                else {
                    _cell.Length = 0;
                }
            }
        }

        public string TiltAngle {
            get {
                return _cell.TiltAngle.ToString();
            }
            set {
                if (int.TryParse(value, out int result)) {
                    try {
                        _cell.TiltAngle = result;
                        ErrorMessage = _noError;
                    }
                    catch (ArgumentException e) {
                        ErrorMessage = e.Message;
                    }
                    OnPropertyChanged("ErrorMessage");
                }
                else {
                    _cell.TiltAngle = -1;
                }
            }
        }

        public string RowSpacing {
            get {
                return _site.RowSpacing.ToString();
            }
            set {
                if (int.TryParse(value, out int result)) {
                    try {
                        _site.RowSpacing = result;
                        ErrorMessage = _noError;
                    }
                    catch (ArgumentException e) {
                        ErrorMessage = e.Message;
                    }
                    OnPropertyChanged("ErrorMessage");
                }
                else {
                    _site.RowSpacing = 0;
                }
            }
        }

        public string ColumnSpacing {
            get {
                return _site.ColumnSpacing.ToString();
            }
            set {
                if (int.TryParse(value, out int result)) {
                    try {
                        _site.ColumnSpacing = result;
                        ErrorMessage = _noError;
                    }
                    catch (ArgumentException e) {
                        ErrorMessage = e.Message;
                    }
                    OnPropertyChanged("ErrorMessage");
                }
                else {
                    _site.ColumnSpacing = 0;
                }
            }
        }

        public string CanvasHeight {
            get {
                return CanvasHeight;
            }
            set {
                if (!string.IsNullOrWhiteSpace(value) && !value.Equals("NaN")) {
                    _canvasHeight = Convert.ToInt32(value);
                    _yScaleFactor = _canvasHeight / Convert.ToInt32(MinWindowHeight);
                    FillToScale();
                }
            }
        }

        public string CanvasWidth {
            get {
                return CanvasWidth;
            }
            set {
                if (!string.IsNullOrWhiteSpace(value) && !value.Equals("NaN")) {
                    _canvasWidth = Convert.ToInt32(value);
                    _xScaleFactor = _canvasWidth / Convert.ToInt32(MinWindowWidth);
                    FillToScale();
                }
            }
        }

        public ICommand GenerateCommandUpdate {
            get;
            private set;
        }

        
        public void Generate() {
            Shapes.Clear();
            Shapes.Add(_site.GetShape());
            Shapes.Add(_obstacle.GetShape());

            int maxY = Convert.ToInt32(_site.GetMaxCoordinate("y"));
            int minY = Convert.ToInt32(_site.GetMinCoordinate("y"));
            int maxX = Convert.ToInt32(_site.GetMaxCoordinate("x"));
            int minX = Convert.ToInt32(_site.GetMinCoordinate("x"));


            /*
            * This is a matric of usable points, down the line I start to populate
            * it by placing cells (solar panels) and booking required area + spacing
            * in positinve x and y directions. Matrix size is layout widest part x layout highest part.
            */
            bool[,] matrix = new bool[maxX - minX, maxY - minY]; 
            for (int x = 0; x < maxX - minX; x++) {
                for (int y = 0; y < maxY - minY; y++) {
                    matrix[x, y] = true;
                }
            }
            matrix = InitialiseMatrix(matrix, maxY, minY, maxX, minX);
            for (int y = 0; y < maxY - minY; y++) {
                for (int x = 0; x < maxX - minX; x++) {
                    if (matrix[x,y] == true) {
                        bool book = true;
                        for (int xcell = 0; xcell < _cell.Length; xcell++) {
                            if (book == true) {
                                for (int ycell = 0; ycell < _cell.EffectiveWidth; ycell++) {
                                    if ((x + xcell >= maxX - minX) || (y + ycell >= maxY - minY) || 
                                        matrix[x + xcell, y + ycell] == false) {
                                        book = false;
                                        break;
                                    }
                                }
                            } else {
                                break;
                            }
                        }
                        if (book == true) {
                            int lenghtToBook = _cell.Length + _site.ColumnSpacing;
                            int widthToBook = _cell.EffectiveWidth + _site.RowSpacing;
                            for (int xcell = 0; xcell <= lenghtToBook; xcell++) {
                                for (int ycell = 0; ycell <= widthToBook; ycell++) {
                                    if ((x + xcell < maxX - minX) && (y + ycell < maxY - minY)) {
                                        matrix[x + xcell, y + ycell] = false;
                                    }
                                }
                            }

                            Shapes.Add(DrawCell(x + minX, y + minY));
                            //OnPropertyChanged("Shapes");
                        }
                    }
                }
            }

        }

        private bool[,] InitialiseMatrix(bool[,] matrix, int maxY, int minY, int maxX, int minX) {

            // Layout minimum and maximum dimentions.
            int maxYO = Convert.ToInt32(_obstacle.GetMaxCoordinate("y"));
            int minYO = Convert.ToInt32(_obstacle.GetMinCoordinate("y"));
            int maxXO = Convert.ToInt32(_obstacle.GetMaxCoordinate("x"));
            int minXO = Convert.ToInt32(_obstacle.GetMinCoordinate("x"));

            // Scaning distance in Y direction.
            int ScanNrO = maxYO - minYO;
            int ScanNrS = maxY - minY;

            // Left and Right edges with X coordinate at each Y value.
            int[] LES = new int[ScanNrS];
            int[] RES = new int[ScanNrS];
            int[] LEO = new int[ScanNrO];
            int[] REO = new int[ScanNrO];

            // Setting up edges as straing vertical lines at x start and x finish.
            for (int i = 0; i < ScanNrO; i++) {
                LEO[i] = maxXO;
                REO[i] = minXO;
            }
            for (int i = 0; i < ScanNrS; i++) {
                LES[i] = maxX;
                RES[i] = minX;
            }

            /*
            * Get right and left edges of the obstacle polygon by goig through each coordinate point.
            */
            for (int i = _obstacle.Coordinates.Count -1; i >= 0; i--) {
                var tuple = new Tuple<int[], int[]>(LEO, REO);
                if (i == 0) {
                    tuple = calculateEdges(LEO, REO, _obstacle.Coordinates.ToArray()[0], _obstacle.Coordinates.ToArray()[_obstacle.Coordinates.Count - 1], ScanNrO);
                }else {
                    tuple = calculateEdges(LEO, REO, _obstacle.Coordinates.ToArray()[i], _obstacle.Coordinates.ToArray()[i -1], ScanNrO);
                }
                LEO = tuple.Item1;
                REO = tuple.Item2;
            }

            //for (int i = _site.Coordinates.Count - 1; i >= 0; i--) {
            //    var tuple = new Tuple<int[], int[]>(LES, RES);
            //    if (i == 0) {
            //        tuple = calculateEdges(LES, RES, _site.Coordinates.ToArray()[0], _site.Coordinates.ToArray()[_site.Coordinates.Count - 1], ScanNrS);
            //    }
            //    else {
            //        tuple = calculateEdges(LES, RES, _site.Coordinates.ToArray()[i], _site.Coordinates.ToArray()[i - 1], ScanNrS);
            //    }
            //    LES = tuple.Item1;
            //    RES = tuple.Item2;
            //}

            // Using edges to determine if a coordiname in a matrix is inside the obstable.
            for (int y = 0; y < ScanNrO; y++) {
                for (int x = LEO[y]; x < REO[y]; x++) {
                    matrix[x - minX, y + minYO ] = false;
                }
            }


            //// Using edges to determine if a coordiname in a matrix is inside the obstable.
            //for (int y = 0; y < ScanNrS; y++) {
            //    for (int x = minX; x < LES[y]; x++) {
            //        matrix[x - minX, y] = false;
            //    }
            //    for (int x = RES[y]; x < maxX; x++) {
            //        matrix[x - minX, y] = false;
            //    }
            //}

            return matrix;
        }

        /*
         * Returns an item pair of left and right edge vertices which each includes coordinate array
         * This however works only for bodies where there are no valeys or double peaks. This can be
         * implemented by spliting the original layout into two polygons and fillig in matrix seperatelly.
         */
        private Tuple<int[],int[]> calculateEdges(int[] LE, int[] RE, Coordinate coor1, Coordinate coor2, int ScanNr) {
            decimal y1 = coor1.Y;
            decimal y2 = coor2.Y;
            decimal x1 = coor1.X;
            decimal x2 = coor2.X;
            decimal slope;
            decimal x;
            decimal t;

            if (y1 > y2) {
                t = x1;
                x1 = x2;
                x2 = t;

                t = y1;
                y1 = y2;
                y2 = t;
            }

            if (y2 - y1 == 0) {
                slope = (x2 - x1);
            } else {
                slope = (x2 - x1) / (y2 - y1);
            }

            x = x1;
            for (int y = 0; y < y2 - y1; y++) {
                if (x < LE[y]) {
                    LE[y] = Convert.ToInt32(x);
                }
                if (x > RE[y]) {
                    RE[y] = Convert.ToInt32(x);
                }
                x += slope;
            }

            return new Tuple<int[], int[]>(LE, RE);
        }


        /*
         * Drawns a cell (solar panel) by using cell instance dimention data and
         * and coordinates x and y where cell will expand in positive x and y
         * By its effective width and height.
         */
        private Polygon DrawCell(int x, int y) {
            Polygon cell = new Polygon();
            cell.Points.Add(new Point(x, y));
            cell.Points.Add(new Point(x, y + _cell.EffectiveWidth));
            cell.Points.Add(new Point(x + _cell.Length, y + _cell.EffectiveWidth));
            cell.Points.Add(new Point(x + _cell.Length, y));
            cell.Stroke = Brushes.Blue;
            cell.StrokeThickness = 1;
            return cell;
        }


        //Didnt finish to implement, however meant to add dynamic scaling to the layout.
        private void FillToScale() {
            BindableCollection<Polygon> updatedShapes = new BindableCollection<Polygon>();

            foreach (Polygon poly in Shapes) {
                Polygon replacement = new Polygon();
                foreach (Point p in poly.Points) {
                    replacement.Points.Add(new Point(p.X * _xScaleFactor, p.Y * _yScaleFactor));
                }
                updatedShapes.Add(replacement);
            }
            Shapes = updatedShapes;
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
