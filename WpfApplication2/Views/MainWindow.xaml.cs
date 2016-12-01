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
using WpfApplication2;


namespace SpaceInvaders
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rand = new Random();
        private DispatcherTimer timer;
        private DispatcherTimer bulletTimer;
        string bulletPath = "images/donaldthumb.jpg";
        CustomShape bullet = new CustomShape();
        double bulletSpeed = 15;
        private List<CustomShape> enemies = new List<CustomShape>();
        int speed = 2;
        double top = 0, bottom = 0, bullY, bullX, bullW, bullH, foeY, foeX, foeW, foeH;
        CustomShape barrier1 = new CustomShape();
        CustomShape barrier2 = new CustomShape();
        CustomShape barrier3 = new CustomShape();
        CustomShape ship = new CustomShape();
        public MainWindow()
        {
            Loaded += delegate
                {
                    InitializeComponent();
                };


        }

        public void move(object sender, EventArgs e)
        {
            double left, testLeftSide, borderRight = canvas.ActualWidth, testRightSide;
            foreach (CustomShape foe in enemies)
            {
                CustomShape lastFoe = enemies[enemies.Count - 1];
                CustomShape firstFoe = enemies[0];
                testLeftSide = Canvas.GetLeft(firstFoe.shape);
                testRightSide = Canvas.GetLeft(lastFoe.shape);
                left = Canvas.GetLeft(foe.shape);
                foe.PositionY = Canvas.GetTop(foe.shape);
                if (testRightSide + lastFoe.shape.ActualWidth >= canvas.ActualWidth)
                {
                    speed = -3;
                    foe.PositionY++;
                }
                if (testLeftSide < 0)
                {
                    speed = 3;
                    Canvas.SetLeft(firstFoe.shape, Canvas.GetLeft(enemies[1].shape) - enemies[1].shape.ActualWidth - 8);
                    foe.PositionY+=2;
                }
                
                Canvas.SetTop(foe.shape, foe.PositionY);
                Canvas.SetLeft(foe.shape, left += speed);
            }

        }

        private void ShipStrafeClick(object sender, KeyEventArgs e)
        {

            double x = Canvas.GetLeft(ship.shape), i = canvas.ActualWidth;

            switch (e.Key)
            {
                case Key.Right:

                    if (x + ship.shape.ActualWidth < canvas.ActualWidth)
                    {
                        x += 12;
                        Canvas.SetLeft(ship.shape, x);

                    }
                    break;

                case Key.Left:
                    if (x > 0)
                    {
                        x -= 12;
                        Canvas.SetLeft(ship.shape, x);
                    }
                    break;

                case Key.Space:
                    bullet.shape.Fill = new ImageBrush(new BitmapImage(new Uri(bulletPath, UriKind.Relative)));
                    bullet.shape.Width = 20;
                    bullet.shape.Height = 50;
                    Canvas.SetLeft(bullet.shape, Canvas.GetLeft(ship.shape));
                    canvas.Children.Add(bullet.shape);
                    Canvas.SetBottom(bullet.shape, 0);
                    bulletTimer.Start();

                    break;
                default:

                    break;

            }
        }

        private void start_button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();

            window.Close();
            start_button.Visibility = Visibility.Hidden;
            bullet.shape = new Rectangle();
            Canvas.SetBottom(bullet.shape, 0);
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            bulletTimer = new DispatcherTimer();
            bulletTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            bulletTimer.Tick += moveBullet;
            var FoeYSpacing = 0.0;

            var FoeXSpacing = 0.0;
            for (int j = 0; j < 3; j++)
            {
               
                for (double i = 0; i < 8; i++)
                {
                    string relativePath = "images/hilaryclintonface.jpg";
                    CustomShape foe = new CustomShape(); //create the rectangle
                    foe.shape = new Rectangle();
                    foe.shape.Fill = new ImageBrush(new BitmapImage(new Uri(relativePath, UriKind.Relative)));
                    foe.shape.Width = 50;
                    foe.shape.Height = 50;
                    FoeXSpacing += foe.shape.Width + foe.PositionX;
                    Canvas.SetLeft(foe.shape, FoeXSpacing += 10);
                    Canvas.SetTop(foe.shape, FoeYSpacing);

                    enemies.Add(foe);
                    /*
                    if (i == 7 || i == 15|| i == 23)
                    {
                        FoeXSpacing = 0;
                    
                    }

                    if (i > 8)
                    {

                        Canvas.SetTop(foe.shape, FoeYSpacing);

                    }
                    else
                    {
                       
                    }
              */

                }
                FoeXSpacing = 0.0;
                FoeYSpacing += enemies[0].Height+50;
            }

            foreach (CustomShape foe in enemies)
            {
                canvas.Children.Add(foe.shape);

            }
            string barrierPath = "images/barrier.png";
            barrier1.shape = new Rectangle();
            barrier2.shape = new Rectangle();
            barrier3.shape = new Rectangle();
            barrier1.shape.Width = 100;
            barrier1.shape.Height = 50;
            barrier1.shape.Fill = Brushes.Cyan;

            barrier2.shape.Width = 100;
            barrier2.shape.Height = 50;
            barrier2.shape.Fill = Brushes.Cyan;

            barrier3.shape.Width = 100;
            barrier3.shape.Height = 50;
            barrier3.shape.Fill = Brushes.Cyan;

            barrier1.shape.Fill = new ImageBrush(new BitmapImage(new Uri(barrierPath, UriKind.Relative)));
            barrier2.shape.Fill = new ImageBrush(new BitmapImage(new Uri(barrierPath, UriKind.Relative)));
            barrier3.shape.Fill = new ImageBrush(new BitmapImage(new Uri(barrierPath, UriKind.Relative)));


            Canvas.SetLeft(barrier1.shape, 10);
            Canvas.SetBottom(barrier1.shape, 50);

            Canvas.SetLeft(barrier2.shape, 200);
            Canvas.SetBottom(barrier2.shape, 50);

            Canvas.SetLeft(barrier3.shape, 400);
            Canvas.SetBottom(barrier3.shape, 50);
            canvas.Children.Add(barrier1.shape);
            canvas.Children.Add(barrier2.shape);
            canvas.Children.Add(barrier3.shape);
            ship.shape = new Rectangle();
            ship.shape.Width = 50;
            ship.shape.Height = 50;
            String shipPath = "images/ship.jpg";
            String backGroundPath = "images/background.gif";
            ship.shape.Fill = new ImageBrush(new BitmapImage(new Uri(shipPath, UriKind.Relative)));
            Canvas.SetLeft(ship.shape, 200);
            Canvas.SetBottom(ship.shape, 10);
            canvas.Children.Add(ship.shape);
            canvas.Background = new ImageBrush(new BitmapImage(new Uri(backGroundPath, UriKind.Relative)));
            timer.Tick += move;
            timer.Start();
        }

        public void moveBullet(object sender, EventArgs e)
        {
            bullX = Canvas.GetLeft(bullet.shape); // left
            bullY = Canvas.GetTop(bullet.shape); // top
            bullW = bullet.shape.ActualWidth;
            bullH = bullet.shape.ActualHeight;

            Canvas.SetBottom(bullet.shape, bottom += bulletSpeed);
            foreach (CustomShape foe in enemies)
            {
                foeX = Canvas.GetLeft(foe.shape); // left
                foeY = Canvas.GetTop(foe.shape); // top
                foeW = foe.shape.ActualWidth;
                foeH = foe.shape.ActualHeight;
                if (bullX + bullW > foeX && bullY + bullH > foeY
                    && bullY < foeY + foeH && bullX < foeX + foeW)
                {
                    canvas.Children.Remove(foe.shape);
                    canvas.Children.Remove(bullet.shape);
                    bottom = 0;
                    bulletTimer.Stop();
                }
            }
            if (Canvas.GetBottom(bullet.shape) >= canvas.ActualHeight)
            {
                canvas.Children.Remove(bullet.shape);
                bottom = 0;
                bulletTimer.Stop();

            }

        }
    }
}

