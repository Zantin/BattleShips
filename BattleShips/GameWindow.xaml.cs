using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public GameWindow(Connection connection, Player player)
        {
            InitializeComponent();
            this.connection = connection;
            this.player = player;
            connection.Subscribe(this);
            connection.ThisIsMe(player);
        }

        public void Receive(List<object> result)
        {
            if(result[0].GetType() == typeof(ServerToClient))
            {

            }
        }

        private void AttackBoard_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                Point pos = e.GetPosition(attackCanvas);
                if ((pos.X >= 0 && pos.X < attackCanvas.gridWidth) && (pos.Y >= 0 && pos.Y < attackCanvas.gridHeight))
                {
                    Vector2i attack = new Vector2i((int)pos.X / attackCanvas.gridWidth, (int)pos.Y / attackCanvas.gridHeight);
                }
            }
        }
    }
}
