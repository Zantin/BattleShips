using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using BattleShipsLibrary;
using System.Globalization;
using System.Diagnostics;
using System.Windows.Shapes;

namespace BattleShips
{
    public class MyCanvas : Canvas
    {
        public static DependencyProperty gridSizeProperty;
        public static DependencyProperty cellWidthProperty;
        public static DependencyProperty cellHeightProperty;

        public static DependencyProperty drawShipsProperty;
        public static DependencyProperty drawAttacksProperty;
        public static DependencyProperty drawBlanksProperty;
        public static DependencyProperty drawMissesProperty;

        public static DependencyProperty isSetupWindowProperty;

        private Stopwatch stopwatch = new Stopwatch();

        private long[] timings = new long[6];

        private ShipBoard shipBoard;
        public Vector2i firstPoint;

        private AttackBoard attackBoard;

        static MyCanvas()
        {
            gridSizeProperty = DependencyProperty.Register("gridSize", typeof(int), typeof(MyCanvas));
            cellWidthProperty = DependencyProperty.Register("cellWidth", typeof(int), typeof(MyCanvas));
            cellHeightProperty = DependencyProperty.Register("cellHeight", typeof(int), typeof(MyCanvas));

            drawShipsProperty = DependencyProperty.Register("drawShips", typeof(bool), typeof(MyCanvas));
            drawAttacksProperty = DependencyProperty.Register("drawAttacks", typeof(bool), typeof(MyCanvas));
            drawBlanksProperty = DependencyProperty.Register("drawBlanks", typeof(bool), typeof(MyCanvas));
            drawMissesProperty = DependencyProperty.Register("drawMisses", typeof(bool), typeof(MyCanvas));

            isSetupWindowProperty = DependencyProperty.Register("isSetupWindow", typeof(bool), typeof(MyCanvas));
        }

        public int gridSize
        {
            get { return (int)base.GetValue(gridSizeProperty); }
            set { base.SetValue(gridSizeProperty, value); }
        }

        public int cellWidth
        {
            get { return (int)base.GetValue(cellWidthProperty); }
            set { base.SetValue(cellWidthProperty, value); }
        }

        public int cellHeight
        {
            get { return (int)base.GetValue(cellHeightProperty); }
            set { base.SetValue(cellHeightProperty, value); }
        }

        public bool drawShips
        {
            get { return (bool)base.GetValue(drawShipsProperty); }
            set { base.SetValue(drawShipsProperty, value); }
        }

        public bool drawAttacks
        {
            get { return (bool)base.GetValue(drawAttacksProperty); }
            set { base.SetValue(drawAttacksProperty, value); }
        }

        public bool drawBlanks
        {
            get { return (bool)base.GetValue(drawBlanksProperty); }
            set { base.SetValue(drawBlanksProperty, value); }
        }

        public bool drawMisses
        {
            get { return (bool)base.GetValue(drawMissesProperty); }
            set { base.SetValue(drawMissesProperty, value); }
        }

        public bool isSetupWindow
        {
            get { return (bool)base.GetValue(isSetupWindowProperty); }
            set { base.SetValue(isSetupWindowProperty, value); }
        }

        public int gridWidth
        {
            get { return cellWidth * gridSize; }
        }

        public int gridHeight
        {
            get { return cellHeight * gridSize; }
        }

        private const string txt = "Play";


        SolidColorBrush waterBrush = new SolidColorBrush(Color.FromRgb(64, 64, 255));
        SolidColorBrush shipBrush = new SolidColorBrush(Color.FromRgb(64, 255, 64));
        SolidColorBrush hitBrush = new SolidColorBrush(Color.FromRgb(255, 64, 64));
        SolidColorBrush missBrush = new SolidColorBrush(Color.FromRgb(192, 192, 64));
        SolidColorBrush blankBrush = new SolidColorBrush(Color.FromRgb(224, 224, 224));

        Pen shipPen = new Pen(Brushes.Black, 0.0);
        Pen gridPen = new Pen(Brushes.Black, 0.2);

        protected override void OnRender(DrawingContext dc)
        {
            DrawGrid(dc);

            if (drawShips)
            {
                DrawShips(dc);
            }

            if (drawAttacks)
            {
                DrawAttacks(dc);
            }

            if (isSetupWindow)
            {
                DrawText(dc);
                if (firstPoint != null)
                    dc.DrawEllipse(shipBrush, gridPen, new Point((firstPoint.x + 0.5) * cellWidth, (firstPoint.y + 0.5) * cellHeight), cellWidth / 4, cellHeight / 4);
            }

            #region DebugDrawTime
            /*
            timings[5]++;

            stopwatch.Start();
            DrawGrid(dc);
            stopwatch.Stop();
            timings[0] += stopwatch.ElapsedTicks;
            Debug.WriteLine(string.Format("{0} : {1,10}ticks", "DrawGrid", stopwatch.ElapsedTicks));
            Debug.WriteLine(string.Format("{0} : {1,10}ticks", "Average", timings[0] / timings[5]));
            stopwatch.Reset();

            if(drawShips)
            {
                stopwatch.Start();
                DrawShips(dc);
                stopwatch.Stop();
                timings[1] += stopwatch.ElapsedTicks;
                Debug.WriteLine(string.Format("{0} : {1,10}ticks", "DrawShips", stopwatch.ElapsedTicks));
                Debug.WriteLine(string.Format("{0} : {1,10}ticks", "Average", timings[1] / timings[5]));
                stopwatch.Reset();
            }

            if(drawAttacks)
            {
                stopwatch.Start();
                DrawAttacks(dc);
                stopwatch.Stop();
                timings[2] += stopwatch.ElapsedTicks;
                Debug.WriteLine(string.Format("{0} : {1,10}ticks", "DrawAttacks", stopwatch.ElapsedTicks));
                Debug.WriteLine(string.Format("{0} : {1,10}ticks", "Average", timings[2] / timings[5]));
                stopwatch.Reset();
            }

            if(isSetupWindow)
            {
                stopwatch.Start();
                DrawText(dc);
                stopwatch.Stop();
                timings[3] += stopwatch.ElapsedTicks;
                Debug.WriteLine(string.Format("{0}: {1,10}ticks", "DrawText", stopwatch.ElapsedTicks));
                Debug.WriteLine(string.Format("{0} : {1,10}ticks", "Average", timings[3] / timings[5]));
                stopwatch.Reset();

                if (firstPoint != null)
                    dc.DrawEllipse(shipBrush, gridPen, new Point((firstPoint.x + 0.5) * cellWidth, (firstPoint.y + 0.5) * cellHeight), cellWidth / 4, cellHeight / 4);

            }

            //dc.DrawText(new FormattedText(title, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 20, Brushes.White), new Point(0, 0));

            //new Point((part.position.x + 0.5) * cellWidth, (part.position.y + 0.5) * cellHeight), cellWidth / 4, cellHeight / 4);
            stopwatch.Reset();
            */
            #endregion
        }

        private void DrawGrid(DrawingContext dc)
        {
            /*
            for (int x = 0; x < gridSize; x++)
            {
                for(int y = 0; y < gridSize; y++)
                {
                    dc.DrawRectangle(waterBrush, gridPen, new Rect(x * cellWidth, y * cellHeight, cellWidth, cellHeight));
                }
            }
            */
            
            dc.DrawRectangle(waterBrush, gridPen, new Rect(0, 0, cellWidth*gridSize, cellHeight*gridSize));
            for (int x = 0; x < gridSize; x++)
            {
                dc.DrawLine(gridPen, new Point(x * cellWidth, 0), new Point(x * cellWidth, cellHeight*gridSize));
            }
            for (int y = 0; y < gridSize; y++)
            {
                dc.DrawLine(gridPen, new Point(0, y * cellHeight), new Point(cellWidth*gridSize, y * cellHeight));
            }
            

        }

        private void DrawAttacks(DrawingContext dc)
        {
            if (attackBoard != null)
            {
                for(int x = 0; x < attackBoard.size; x++)
                {
                    for(int y = 0; y < attackBoard.size; y++)
                    {
                        if (attackBoard.attacked[x, y])
                        {
                            if (attackBoard.hitOrMiss[x, y])
                                DrawPolygon(dc, new Cross(x * cellWidth, y * cellHeight, cellWidth, cellHeight).polygon, hitBrush);
                            else if(drawMisses)
                                DrawPolygon(dc, new Cross(x * cellWidth, y * cellHeight, cellWidth, cellHeight).polygon, missBrush);
                            //Draw Cross = Hit
                        }
                        else if(drawBlanks)
                            dc.DrawRectangle(blankBrush, gridPen, new Rect(x * cellWidth, y * cellHeight, cellWidth, cellHeight));
                    }
                }
            }
        }

        private void DrawShips(DrawingContext dc)
        {
            if (shipBoard != null)
            {
                foreach(Ship ship in shipBoard.ships)
                {
                    DrawShip(dc, ship);
                }
            }  
        }

        private void DrawShip(DrawingContext dc, Ship ship)
        {
            foreach (ShipPart part in ship.shipParts)
            {
                dc.DrawEllipse(shipBrush, shipPen, new Point((part.position.x + 0.5) * cellWidth, (part.position.y + 0.5) * cellHeight), cellWidth / 4, cellHeight / 4);
            }
            if (ship.orientation == BattleShipsLibrary.Orientation.Horizontal)
                dc.DrawRectangle(shipBrush, shipPen, new Rect(ship.shipParts[0].position.x * cellWidth + cellWidth / 2, ship.shipParts[0].position.y * cellHeight + cellHeight / 4, (ship.size - 1) * cellWidth, cellHeight / 2));
            else
                dc.DrawRectangle(shipBrush, shipPen, new Rect(ship.shipParts[0].position.x * cellWidth + cellWidth / 4, ship.shipParts[0].position.y * cellHeight + cellHeight / 2, cellWidth / 2, (ship.size - 1) * cellHeight));
        }

        private const string txt1 = "Ship Size | Ships Left";
        private const string txt2 = "    2     |      ";
        private const string txt3 = "    3     |      ";
        private const string txt4 = "    4     |      ";
        private const string txt5 = "    5     |      ";

        private void DrawText(DrawingContext dc)
        {
            dc.DrawText(new FormattedText(txt1, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 16, Brushes.Black), new Point(345.5, 10.5));
            dc.DrawText(new FormattedText(txt2 + (shipBoard.GetAmountOfShipAllowedOfSize(2) - shipBoard.GetAmountOfShipOfSize(2)) , CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 14, Brushes.Black), new Point(375, 30.5));
            dc.DrawText(new FormattedText(txt3 + (shipBoard.GetAmountOfShipAllowedOfSize(3) - shipBoard.GetAmountOfShipOfSize(3)), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 14, Brushes.Black), new Point(375, 50.5));
            dc.DrawText(new FormattedText(txt4 + (shipBoard.GetAmountOfShipAllowedOfSize(4) - shipBoard.GetAmountOfShipOfSize(4)), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 14, Brushes.Black), new Point(375, 70.5));
            dc.DrawText(new FormattedText(txt5 + (shipBoard.GetAmountOfShipAllowedOfSize(5) - shipBoard.GetAmountOfShipOfSize(5)), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 14, Brushes.Black), new Point(375, 90.5));

        }

        public void SetShipBoard(ShipBoard shipBoard)
        {
            this.shipBoard = shipBoard;
            this.InvalidateVisual();
        }

        public void SetAttackBoard(AttackBoard attackBoard)
        {
            this.attackBoard = attackBoard;
            this.InvalidateVisual();
        }

        public void DrawPolygon(DrawingContext dc, Polygon polygon, SolidColorBrush color)
        {
            StreamGeometry streamGeometry = new StreamGeometry();
            using (StreamGeometryContext geometryContext = streamGeometry.Open())
            {
                geometryContext.BeginFigure(polygon.Points[0], true, true);
                geometryContext.PolyLineTo(polygon.Points.Skip(1).ToArray(), true, false);
            }

            dc.DrawGeometry(color, gridPen, streamGeometry);
        }

        
        private class Cross
        {
            public Polygon polygon = new Polygon();

            private PointCollection points = new PointCollection
            {
                new Point(0.1875, 0.03125),
                new Point(0.46875, 0.3125),
                new Point(0.53125, 0.3125),
                new Point(0.8125, 0.03125),
                new Point(0.96875, 0.1875),
                new Point(0.6875, 0.46875),
                new Point(0.6875, 0.53125),
                new Point(0.96875, 0.8125),
                new Point(0.8125, 0.96875),
                new Point(0.53125, 0.6875),
                new Point(0.46875, 0.6875),
                new Point(0.1875, 0.96875),
                new Point(0.03125, 0.8125),
                new Point(0.3125, 0.53125),
                new Point(0.3125, 0.46875),
                new Point(0.03125, 0.1875)
            };

            public Cross(double x, double y, double width, double height)
            {
                for(int i = 0; i < points.Count; i++)
                {
                    points[i] = new Point((points[i].X * width) + x, (points[i].Y * height) + y);
                }

                polygon.Points = points;
            }

        }
        
    }
}
