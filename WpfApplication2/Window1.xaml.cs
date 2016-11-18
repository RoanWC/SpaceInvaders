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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private double _x;
        private double _y;

        private Polygon shape = new Polygon();
        private System.Windows.Threading.DispatcherTimer timer;




        // test first committ on VS
        public MainWindow()
        {
            InitializeComponent();



        }



        private void ShipStrafeClick(object sender, KeyEventArgs e)
        {
            _x = Canvas.GetLeft(Polygon1);
            _y = Canvas.GetTop(Polygon1);

            switch (e.Key)
            {
                case Key.Right:
                    _x += 20;
                    Canvas.SetLeft(Polygon1, _x);
                    break;

                case Key.Left:
                    _x -= 20;
                    Canvas.SetLeft(Polygon1, _x);
                    break;

                default:
                    MessageBox.Show("Nothing");
                    break;

            }
        }
    }
}
