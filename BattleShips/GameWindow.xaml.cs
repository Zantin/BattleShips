using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BattleShipsLibrary;

namespace BattleShips
{
    /// <summary>
    /// Interaction logic for AttackWindow.xaml
    /// </summary>
    public partial class GameWindow : Window, ICallbackAble
    {
        Connection connection;
        Player player;
        AttackBoard attackBoard;
        AttackBoard enemyAttacks;
        MainWindow mainWindow;

        private bool gameOver = false;

        private List<object> result;

        public GameWindow(Connection connection, Player player, MainWindow mainWindow)
        {
            InitializeComponent();
            this.connection = connection;
            this.player = player;
            this.mainWindow = mainWindow;
            attackBoard = new AttackBoard(player.shipBoard.boardSize);
            attackCanvas.SetAttackBoard(attackBoard);
            shipCanvas.SetShipBoard(player.shipBoard);
            enemyAttacks = new AttackBoard(player.shipBoard.boardSize);
            shipCanvas.SetAttackBoard(enemyAttacks);
            
            connection.Subscribe(this);
        }

        public void Receive(List<object> result)
        {
            this.result = result;
            Dispatcher.Invoke(Handle);
        }

        private void Handle()
        {
            if (result[1].GetType() == typeof(ServerToClient))
            {
                ServerToClient command = (ServerToClient)result[1];
                if (command == ServerToClient.Hit)
                {
                    attackBoard.UpdateBoard((Vector2i)result[2], true);
                    attackCanvas.InvalidateVisual();
                }
                else if (command == ServerToClient.Miss)
                {
                    attackBoard.UpdateBoard((Vector2i)result[2], false);
                    attackCanvas.InvalidateVisual();
                }
                else if (command == ServerToClient.Win)
                {
                    player.AddWin();
                    player.AddHits(attackBoard.hits);
                    player.AddMiss(attackBoard.misses);
                    mainWindow.UpdateStats();
                    mainWindow.Activate();
                    mainWindow.IsEnabled = true;
                    gameOver = true;
                }
                else if (command == ServerToClient.Loss)
                {
                    player.AddLoss();
                    player.AddHits(attackBoard.hits);
                    player.AddMiss(attackBoard.misses);
                    mainWindow.UpdateStats();
                    mainWindow.Activate();
                    mainWindow.IsEnabled = true;
                    gameOver = true;
                }
                else if (command == ServerToClient.BattleReady)
                {
                    this.Show();
                }
                else if (command == ServerToClient.YourTurn)
                {
                    //Added if something special should happen
                }
                else if (command == ServerToClient.EnemyAttack)
                {
                    enemyAttacks.UpdateBoard((Vector2i)result[2], (bool)result[3]);
                    shipCanvas.InvalidateVisual();
                }
                else if (command == ServerToClient.EnemyUsername)
                {
                    Title = string.Format("Enemy: {0}", (string)result[2]);
                }
                else if (command == ServerToClient.EnemyShips)
                {
                    attackCanvas.SetShipBoard((ShipBoard)result[2]);
                    attackCanvas.drawBlanks = false;
                    attackCanvas.drawShips = true;
                    attackCanvas.InvalidateVisual();
                }
                
            }
        }

        private void AttackBoard_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                Point pos = e.GetPosition(attackCanvas);
                if ((pos.X >= 0 && pos.X < attackCanvas.gridWidth) && (pos.Y >= 0 && pos.Y < attackCanvas.gridHeight))
                {
                    Vector2i attack = new Vector2i((int)pos.X / attackCanvas.cellWidth, (int)pos.Y / attackCanvas.cellHeight);
                    if (!attackBoard.attacked[attack.x,attack.y])
                        connection.Attack(attack);
                }
                //MessageBox.Show(string.Format("X: {0} Y: {1}", ((int)pos.X / attackCanvas.cellWidth).ToString(), ((int)pos.Y / attackCanvas.cellHeight).ToString()));
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(!gameOver)
            {
                connection.GiveUp();
            }
            if(!connection.isClosed)
                connection.CloseConnection();
            if (!mainWindow.IsEnabled)
            {
                mainWindow.Activate();
                mainWindow.IsEnabled = true;
            }
        }
    }
}
