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
        public Player player { get; set; }

        private Random rand = new Random();

        public MainWindow()
        {
            Options.LoadOptions();
            player = new Player();
            InitializeComponent();
            UpdateStats();
            shipBoard = new ShipBoard(canvasBoard.gridSize, Options.allowedShips.ToArray());
            player.shipBoard = shipBoard;
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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(shipBoard.IsAllShipsPlaced())
            {
                Connection connection = new Connection();
                connection.ThisIsMe(player);

                this.IsEnabled = false;

                new GameWindow(connection, player, this);
            }
        }

        public void UpdateStats()
        {
            game_label.Content = string.Format("Games: {0,6}", player.games);
            wins_label.Content = string.Format("Wins: {0,9}", player.wins);
            loss_label.Content = string.Format("Loss: {0,10}", player.loss);
            shots_label.Content = string.Format("Shots: {0,8}", player.shots);
            hits_label.Content = string.Format("Hits: {0,11}", player.hits);
            miss_label.Content = string.Format("Miss: {0,10}", player.miss);
        }
    }
}
