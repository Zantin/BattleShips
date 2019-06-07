using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BattleShips
{
    public class BigGrid : Canvas
    {
        private const int size = 32; //Cell Size // do something less hardcoded

        public BigGrid()
        {
        }

        protected override void OnRender(DrawingContext dc)
        {
            Pen pen = new Pen(Brushes.Black, 0.1);

            dc.DrawRectangle(new SolidColorBrush(Color.FromRgb(0,0,0)), pen, new Rect(-1, -1, 322, 322));
            dc.DrawRectangle(new SolidColorBrush(Color.FromRgb(255, 255, 255)), pen, new Rect(0, 0, 320, 320));

            // vertical lines
            double pos = 0;
            int count = 0;
            do
            {
                dc.DrawLine(pen, new Point(pos, 0), new Point(pos, DesiredSize.Height));
                pos += size;
                count++;
            }
            while (pos < DesiredSize.Width);

            string title = count.ToString();

            // horizontal lines
            pos = 0;
            count = 0;
            do
            {
                dc.DrawLine(pen, new Point(0, pos), new Point(DesiredSize.Width, pos));
                pos += size;
                count++;
            }
            while (pos < DesiredSize.Height);

            // display the grid size (debug mode only!)
            title += "x" + count;
            dc.DrawText(new FormattedText(title, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 20, Brushes.White), new Point(0, 0));

            
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return availableSize;
        }
    }
}
