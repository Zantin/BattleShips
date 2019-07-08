using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BattleShipsLibrary;
using Orientation = BattleShipsLibrary.Orientation;

namespace BattleShips
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ShipBoard shipBoard;

        private Random rand = new Random();

        public MainWindow()
        {
            InitializeComponent();
            shipBoard = new ShipBoard(canvasBoard.gridSize, new Vector2i(2, 1), new Vector2i(3, 2), new Vector2i(4, 1), new Vector2i(5, 1));
            canvasBoard.SetShipBoard(shipBoard);
        }

        private Vector2i firstPoint;
        private Vector2i secondPoint;

        private void BigGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            /*
            if(e.MiddleButton == MouseButtonState.Pressed)
            {
                Point p = e.GetPosition(canvasBoard);
                attackBoard.UpdateBoard(new Vector2i((int)p.X / canvasBoard.cellWidth, (int)p.Y / canvasBoard.cellHeight), shipBoard.Attack(new Vector2i((int)p.X / canvasBoard.cellWidth, (int)p.Y / canvasBoard.cellHeight)));
                canvasBoard.InvalidateVisual();
            }
            */

            if (e.RightButton == MouseButtonState.Pressed)
            {
                if (firstPoint != null)
                {
                    firstPoint = null;
                    secondPoint = null;
                    canvasBoard.firstPoint = null;
                    canvasBoard.InvalidateVisual();
                }

                else
                {
                    Point p = e.GetPosition(canvasBoard);
                    Ship temp = shipBoard.ContainsShip((int)p.X / canvasBoard.cellWidth, (int)p.Y / canvasBoard.cellHeight);
                    if (temp != null)
                    {
                        shipBoard.RemoveShip(temp);
                        canvasBoard.InvalidateVisual();
                    }
                }

            }
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point pos = e.GetPosition(canvasBoard);
                //MessageBox.Show(string.Format("Left: {0} | Middle: {1} | Right: {2} \nPos: X:{3} Y:{4} \nCell: ({5},{6})", e.LeftButton, e.MiddleButton, e.RightButton, pos.X, pos.Y, (int)pos.X/32, (int)pos.Y/32));
                if ((pos.X >= 0 && pos.X < canvasBoard.gridWidth) && (pos.Y >= 0 && pos.Y < canvasBoard.gridHeight))
                {
                    if (firstPoint == null)
                    {
                        firstPoint = new Vector2i((int)pos.X / canvasBoard.cellWidth, (int)pos.Y / canvasBoard.cellHeight);
                        canvasBoard.firstPoint = firstPoint;
                        canvasBoard.InvalidateVisual();
                    }
                    if (firstPoint != null)
                    {
                        secondPoint = new Vector2i((int)pos.X / canvasBoard.cellWidth, (int)pos.Y / canvasBoard.cellHeight);
                        if (!(firstPoint.y == secondPoint.y || firstPoint.x == secondPoint.x))
                            secondPoint = null;

                        else if (firstPoint.y == secondPoint.y && firstPoint.x == secondPoint.x)
                            secondPoint = null;
                    }

                    if (firstPoint != null && secondPoint != null)
                    {
                        Orientation orientation;
                        if (firstPoint.x == secondPoint.x)
                            orientation = Orientation.Vertical;
                        else
                            orientation = Orientation.Horizontal;

                        Vector2i startPoint = orientation == Orientation.Horizontal ? (firstPoint.x < secondPoint.x ? firstPoint : secondPoint) : (firstPoint.y < secondPoint.y ? firstPoint : secondPoint);

                        int size = orientation == Orientation.Horizontal ? (startPoint.Equals(firstPoint) ? secondPoint.x - startPoint.x : firstPoint.x - startPoint.x) : (startPoint.Equals(firstPoint) ? secondPoint.y - startPoint.y : firstPoint.y - startPoint.y);

                        shipBoard.AddShip(new Ship(orientation, startPoint, size + 1));
                        canvasBoard.InvalidateVisual();
                        firstPoint = null;
                        secondPoint = null;
                        startPoint = null;
                        canvasBoard.firstPoint = null;
                    }
                }
            }


            //shipBoard.AddShip(new Ship(Orientation.Horizontal, 1,1,3));
            //canvasBoard.SetShips(shipBoard);
            //shipBoard.AddShip(new Ship(Orientation.Vertical,3,4,3));
            //shipBoard.AddShip(new Ship(Orientation.Horizontal, 5,5,4));
            //shipBoard.AddShip(new Ship(Orientation.Vertical, 4, 4, 3));



        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Player player = new Player();
            player.shipBoard = this.shipBoard;
            player.username = "Testing";
            Connection connection = new Connection();
            connection.ThisIsMe(player);

            this.IsEnabled = false;

            new GameWindow(connection, player);
        }
    }
}
