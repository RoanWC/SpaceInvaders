using System;
using System.Collections;
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
        private List<Rectangle> _rectangles = new List<Rectangle>();

        private double _x;
        private double _y;

        private double delta_x = 5.0;
        private double delta_y = 1.0;

        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0,0, 30);
            
            for (double i = 0; i < 200; i += 30)
            {
                Rectangle rectangle = new Rectangle(); //create the rectangle
                rectangle.Fill = Brushes.Cyan;
                rectangle.Width = 30;
                rectangle.Height = 30;

                Canvas.SetLeft(rectangle, ++i);
                _rectangles.Add(rectangle);
            }

            foreach (var rect in _rectangles)
            {
                MainCanvas.Children.Add(rect);
            }

            Canvas.SetLeft(Ship, 250);
            Canvas.SetTop(Ship, 260);
            timer.Tick += Timer_Tick;
            timer.Start();
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
    

            foreach (var shape in _rectangles)
            {


                var left = Canvas.GetLeft(shape);

                //move rectangle position
                left += delta_x;

                //check if border hit
                if (left + shape.ActualWidth+1 > MainCanvas.ActualWidth)
                    delta_x = -delta_x;

                else if (left < 0)
                    delta_x = -delta_x;


                // set rectangle position
                Canvas.SetLeft(shape, left);

            }


        }

        private void ShipStrafeClick(object sender, KeyEventArgs e)
        {
            _x = Canvas.GetLeft(Ship);
            _y = Canvas.GetTop(Ship);

            switch (e.Key)
            {
                case Key.Right:
                    
                    if (_x + Ship.ActualWidth < MainCanvas.ActualWidth)
                    {
                        _x += 12;
                        Canvas.SetLeft(Ship, _x);
                        
                    }
                    break;

                case Key.Left:
                    if (_x > 0)
                    {
                        _x -= 12;
                        Canvas.SetLeft(Ship, _x);
                    }
                    break;

                default:
                    
                    break;

            }
        }
    }
}
