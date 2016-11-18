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

 
            
            shape.Points.Add(new Point(50, 0));
            shape.Points.Add(new Point(0, 100));
            shape.Points.Add(new Point(100, 100));
            shape.Fill = Brushes.LightBlue;
            shape.Stroke = Brushes.Cyan;
           // timer.Tick += timer1_Tick;



        }



      

      


        private void keyboardpressedClick(object sender, KeyEventArgs e)
        {

            _x = Canvas.GetLeft(Polygon1);
            _y = Canvas.GetTop(Polygon1);
            
            switch(e.Key)
            {
                case Key.Right:
                Canvas.SetLeft(shape, _x -= 10);
                    break;

                case Key.Left:
                    Canvas.SetLeft(shape, _x -= 10);
                    break;

                case Key.Space:
                    Canvas.SetTop(shape, _y += 10);
                    break;

                default:
                    MessageBox.Show("Nothing");
                    break;



            }
            
        }
    }
}
