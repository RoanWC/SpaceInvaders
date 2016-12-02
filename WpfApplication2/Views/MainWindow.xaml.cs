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
        string bulletPath = "images/donaldthumb.png";
        CustomShape bullet = new CustomShape();
        double bulletSpeed = 15;
        private List<CustomShape> enemies = new List<CustomShape>();
        int speed = 1;
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
            double  borderRight = canvas.ActualWidth, top = 0;
            CustomShape foe;
            //foreach (CustomShape foe in enemies)
            for (int i = 0; i < enemies.Count; i++)
            {
                foe = enemies[i];
                foe.PositionY = Canvas.GetTop(foe.shape);
                if (foe.PositionX + foe.shape.ActualWidth >= canvas.ActualWidth)
                {
                    speed = -1;
                    top += 1;
                    for (int j = 0; j < enemies.Count; j++)
                    {
                        Canvas.SetTop(enemies[j].shape, enemies[j].PositionY += top);
                    }
                }
               if (foe.PositionX < 0)
                {
                    speed = 1;
                    top += 1;
                    for (int j = 0; j < enemies.Count; j++)
                    {
                        Canvas.SetTop(enemies[j].shape, enemies[j].PositionY += top);
                    }

                }
                
                Canvas.SetTop(foe.shape, foe.PositionY += top);
                Canvas.SetLeft(foe.shape, foe.PositionX+=speed);
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
                    try
                    {
                        bullet.shape.Fill = new ImageBrush(new BitmapImage(new Uri(bulletPath, UriKind.Relative)));
                        bullet.shape.Width = 20;
                        bullet.shape.Height = 50;
                        Canvas.SetLeft(bullet.shape, Canvas.GetLeft(ship.shape));
                        canvas.Children.Add(bullet.shape);
                        Canvas.SetBottom(bullet.shape, 0);
                        bulletTimer.Start();
                    }
                    catch (System.ArgumentException)
                    {

                    }

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
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            bulletTimer = new DispatcherTimer();
            bulletTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            bulletTimer.Tick += moveBullet;
            var FoeYSpacing = 0.0;

            var FoeXSpacing = 0.0;
            for (int j = 0; j < 3; j++)
            {
               
                for (double i = 0; i < 8; i++)
                {
                    string relativePath = "images/hilaryclintonface.png";
                    CustomShape foe = new CustomShape(); //create the rectangle
                    foe.shape = new Rectangle();
                    foe.shape.Fill = new ImageBrush(new BitmapImage(new Uri(relativePath, UriKind.Relative)));
                    foe.shape.Width = 50;
                    foe.shape.Height = 50;
                    foe.PositionX = FoeXSpacing;
                    foe.PositionY = FoeYSpacing;
                    Canvas.SetLeft(foe.shape, FoeXSpacing);
                    FoeXSpacing += foe.shape.Width;
                    Canvas.SetTop(foe.shape, FoeYSpacing);

                    enemies.Add(foe);
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
            String shipPath = "images/ship.png";
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
            if (Canvas.GetBottom(bullet.shape) >= canvas.ActualHeight)
            {
                canvas.Children.Remove(bullet.shape);
                bottom = 0;
                bulletTimer.Stop();

            }

        }
    }
}

