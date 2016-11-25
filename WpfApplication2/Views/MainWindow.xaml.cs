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
using System.Windows.Threading;

namespace SpaceInvaders
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rand = new Random();
        private DispatcherTimer timer;
        private List<Rectangle> rectangles = new List<Rectangle>();
        int speed = 2;
        double top = 0;
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            for (double i = 0; i < 400; i += 50)
            {
                string relativePath = "images/hilaryclintonface.jpg";
                Rectangle rectangle = new Rectangle(); //create the rectangle
                rectangle.Fill = new ImageBrush(new BitmapImage(new Uri(relativePath, UriKind.Relative)));
                rectangle.Width = 50;
                rectangle.Height = 50;

                Canvas.SetLeft(rectangle, i += 10);
                rectangles.Add(rectangle);
                //
            }

            foreach (var rect in rectangles)
            {
                canvas.Children.Add(rect);
            }

            Polygon shape1 = new Polygon();
            Polygon shape2 = new Polygon();
            Polygon shape3 = new Polygon();

            //Canvas.SetBottom(shape, );


            timer.Tick += move;
            timer.Start();
        }

        public void move(object sender, EventArgs e)
        {
            double left, testLeftSide, borderRight = canvas.ActualWidth, borderBottom = 100, bottom = 900, testRightSide;
            Rectangle[] rectArr = rectangles.ToArray();
            foreach (Rectangle rect in rectArr)
            {
                Rectangle firstRect = rectArr[rectArr.Length - 1];
                Rectangle lastRect = rectArr[0];
                testLeftSide = Canvas.GetLeft(lastRect);
                testRightSide = Canvas.GetLeft(firstRect);
                left = Canvas.GetLeft(rect);
                //top = Canvas.GetTop(firstRect);
                if (testRightSide + firstRect.ActualWidth >= canvas.ActualWidth)
                {
                    speed = -3;
                    top++;
                }
                if (testLeftSide <= 0)
                {
                    speed = 3;
                    Canvas.SetLeft(lastRect, Canvas.GetLeft(rectArr[1]) - rectArr[1].ActualWidth - 8);
                    top += 2;
                }
                Canvas.SetTop(rect, top);
                Canvas.SetLeft(rect, left += speed);
            }

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
