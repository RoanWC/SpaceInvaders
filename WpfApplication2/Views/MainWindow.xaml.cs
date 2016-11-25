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
        private List<Shape> rectangles = new List<Shape>();

        private double _x;
        private double _y;
        private double direction_x = 5.0;
        //private double direction_y = 1.0;

        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            
           // Canvas.Left = "250" Canvas.Top = "260"
            for (double i = 0; i < 400; i += 50)
            {
                //Rectangle rectangle = new Rectangle(); //create the rectangle
                Shape rectangle = new Shape();
            
                var uri = Properties.Resources.hilaryclintonface;
                rectangle.image.Source = new BitmapImage(uri);
                
                
                Canvas.SetLeft(rectangle, i += 10);
                rectangles.Add(rectangle);
               
            }

            foreach (var rect in rectangles)
            {
                MainCanvas.Children.Add(rect);
            }

            timer.Tick += Timer_Tick;
            timer.Start();
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            double left, testLeftSide, top, borderRight = MainCanvas.ActualWidth, borderBottom = 100, bottom = 900, testRightSide;
            Shape[] rectArr = rectangles.ToArray();
            foreach (Shape rect in rectArr)
            {
                Shape firstRect = rectArr[rectArr.Length - 1];
                Shape lastRect = rectArr[0];
                testLeftSide = Canvas.GetLeft(lastRect);
                testRightSide = Canvas.GetLeft(firstRect);
                left = Canvas.GetLeft(rect);
                if (testRightSide + firstRect.Width >= MainCanvas.ActualWidth)
                    direction_x = -5;
                if (testLeftSide < 0)
                {
                    direction_x = 5;
                    Canvas.SetLeft(lastRect, Canvas.GetLeft(rectArr[1]) - rectArr[1].Width - 4);
                }

                Canvas.SetLeft(rect, left += direction_x);
            }

        }
               

        private void ShipStrafeClick(object sender, KeyEventArgs e)
        {
            Shape ship = new Shape();
            BitmapUri uri = Properties.Resources.hilaryclintonface;
            //Image image = new Image();
            ship.image.Source = uri;
            Canvas.SetLeft(ship, 250);
            Canvas.SetTop(ship, 260);
            _x = Canvas.GetLeft(ship);
            _y = Canvas.GetTop(ship);

            switch (e.Key)
            {
                case Key.Right:
                    
                    if (_x + ship.Width < MainCanvas.ActualWidth)
                    {
                        _x += 12;
                        Canvas.SetLeft(ship, _x);
                        
                    }
                    break;

                case Key.Left:
                    if (_x > 0)
                    {
                        _x -= 12;
                        Canvas.SetLeft(ship, _x);
                    }
                    break;

                default:
                    
                    break;

            }
        }
    }
}
