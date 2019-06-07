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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BattleShips
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BigGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point pos = e.GetPosition(this);
            MessageBox.Show(string.Format("Left: {0} | Middle: {1} | Right: {2} \nPos: X:{3} Y:{4} \nCell: ({5},{6})", e.LeftButton, e.MiddleButton, e.RightButton, pos.X, pos.Y, (int)pos.X/32, (int)pos.Y/32));

        }
    }
}
