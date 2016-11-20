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
using System.Windows.Threading;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private double _x;
        private double _y;
        private double space = 3;

        private double deltax = 5.0;
        private double deltay = 1.0;

        private Polygon shape = new Polygon();
        private DispatcherTimer timer;

        

        // test first committ on VS
        public MainWindow()
        {
            InitializeComponent();
            StartGame();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0,0, 10);
            timer.Tick += Timer_Tick;
            timer.Start();

        }

        private void StartGame()
        {

            Enemy enemy1 = new Enemy();
            enemy1.Width = 20;
            enemy1.Height = 20;
            enemy1.Fill = Brushes.AliceBlue;
            MainCanvas.Children.Add(enemy1);


            Enemy enemy2 = new Enemy();
            enemy1.Width = 20;
            enemy1.Height = 20;
            enemy1.Fill = Brushes.AliceBlue;
            MainCanvas.Children.Add(enemy2);


            Enemy enemy3 = new Enemy();
            enemy1.Width = 20;
            enemy1.Height = 20;
            enemy1.Fill = Brushes.AliceBlue;
            MainCanvas.Children.Add(enemy3);

            enemy1.PositionX = 0;
            Canvas.SetLeft(enemy1, enemy1.PositionX);



            enemy2.PositionX = enemy1.PositionX+space;
            Canvas.SetLeft(enemy2, enemy2.PositionX);



            enemy3.PositionX = enemy3.PositionX+space;
            Canvas.SetLeft(enemy3, enemy3.PositionX);



        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //get rectangle position
            double left = Canvas.GetLeft(rectangle1);
            

            //move rectangle position
            left += deltax;

            //check if border hit
            if (left + rectangle1.ActualWidth > MainCanvas.ActualWidth)
                deltax *= -1;

            else if (left < 0)
                deltax *= -1;
           

            // set rectangle position
            Canvas.SetLeft(rectangle1, left);
           

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
