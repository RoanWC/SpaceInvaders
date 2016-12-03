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
using System.Media;


namespace SpaceInvaders
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rand = new Random();
        private DispatcherTimer strafeTimer;
        private DispatcherTimer bulletTimer;
        string bulletPath = "images/donaldthumb.png";
        private List<CustomShape> enemies = new List<CustomShape>();
        CustomShape bullet = new CustomShape();
        CustomShape barrier1 = new CustomShape();
        CustomShape barrier2 = new CustomShape();
        CustomShape barrier3 = new CustomShape();
        CustomShape ship = new CustomShape();
        double bulletSpeed = 8;
        double speed = 1;
        bool leftPressed;
        bool rightPressed;
        bool isLockSpaceBar = true;
        bool isPaused = false;
        int killCount = 0;
        int difficulty = 1;
        int rows = 3;
        int cols = 8;
        double top = 0.0;
        SoundPlayer player = new System.Media.SoundPlayer("Sounds/shotSound.wav");
        public MainWindow()
        {

            Loaded += delegate
                {
                    InitializeComponent();

                };
        }

        private void NewGameClick(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            Credits.Foreground = null;
            window.Close();
            start_button.Visibility = Visibility.Hidden;


            bullet.shape = new Rectangle();
            Canvas.SetBottom(bullet.shape, 0);
            strafeTimer = new DispatcherTimer();
            bulletTimer = new DispatcherTimer();
            strafeTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            strafeTimer.Tick += move;
            bulletTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            bulletTimer.Tick += moveBullet;
            
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
            createLevel(difficulty);
          
        }
        public void createLevel(int difficulty)
        {

           
            if (difficulty > 1)
                rows++;
            else if (difficulty > 3)
                cols++;

            var FoeYSpacing = 0.0;
            var FoeXSpacing = 1.0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    string relativePath = "images/hilaryclintonface.png";
                    CustomShape foe = new CustomShape(); //create the rectangle
                    foe.shape = new Rectangle();
                    foe.shape.Fill = new ImageBrush(new BitmapImage(new Uri(relativePath, UriKind.Relative)));
                    foe.shape.Width = 50.0;
                    foe.shape.Height = 50.0;
                    foe.PositionX = FoeXSpacing;
                    foe.PositionY = FoeYSpacing;
                    foe.Health = 3;
                    Canvas.SetLeft(foe.shape, FoeXSpacing);
                    FoeXSpacing += foe.shape.Width;
                    Canvas.SetTop(foe.shape, FoeYSpacing);
                    enemies.Add(foe);
                    
                }
                FoeXSpacing = 0.0;
                FoeYSpacing += enemies[i].shape.Height;

            }
            foreach (CustomShape foe in enemies)
            {
                canvas.Children.Add(foe.shape);
            }
            
            strafeTimer.Start();
        }
    

        public void move(object sender, EventArgs e)
        {

            if (enemies.Count == 0)
            {
                strafeTimer.Stop();
                top = 0.0;
                createLevel(++difficulty);
                
            }
            Boolean diretionChanged = false;
            for (int i = 0; i < enemies.Count; i++)
            {
                
                double canvaswidth = Math.Round(canvas.ActualWidth);
                if (enemies[i].PositionX + enemies[i].shape.Width >= canvaswidth)
                {
                    speed = -1.0;
                    top = 2;
                    diretionChanged = true;
                    for (int j = 0; j < enemies.Count; j++)
                    {
                        
                        enemies[j].PositionY += top;
                        Canvas.SetTop(enemies[j].shape, enemies[j].PositionY);
                        
                    }



                }
                else if (enemies[i].PositionX < 0)
                {
                    speed = 1.0;
                    top = 2.0;
                    diretionChanged = true;
                    for (int j = 0; j < enemies.Count; j++)
                    {
                        
                        enemies[j].PositionY += top;
                        Canvas.SetTop(enemies[j].shape, enemies[j].PositionY);
                        
                    }
                    
                    
                }

                if (diretionChanged)
                {

                    for (int k = 0; k < enemies.Count; k++)
                    {
                        enemies[k].PositionX += speed;
                        Canvas.SetLeft(enemies[k].shape, enemies[k].PositionX);
                    }

                    diretionChanged = false;
                    i = enemies.Count;
                    
                }
                else
                {
                    enemies[i].PositionX += speed;
                    Canvas.SetLeft(enemies[i].shape, enemies[i].PositionX);
                }
                
                
            }

           
            double x = Canvas.GetLeft(ship.shape);
            if (leftPressed)
            {
                if (x > 0)
                {
                    x -= 3;
                    Canvas.SetLeft(ship.shape, x);
                }
            }
            if (rightPressed)
            {
                if (x + ship.shape.ActualWidth < canvas.ActualWidth)
                {
                    x += 3;
                    Canvas.SetLeft(ship.shape, x);

                }

            }

        }
        public void updateKillCount()
        {
            killCount++;
            kills.Text = Convert.ToString(killCount); 
        }

        public void moveBullet(object sender, EventArgs e)
        {
            Canvas.SetTop(bullet.shape, bullet.PositionY -= bulletSpeed);
            if (bullet.PositionY < 0)
            {
                canvas.Children.Remove(bullet.shape);
                isLockSpaceBar = true;
                bulletTimer.Stop();
            }
            CustomShape foe;
            for (int i = 0; i < enemies.Count; i++)
            {
                foe = enemies[i];
                if ((bullet.PositionY < enemies[i].PositionY + enemies[i].shape.Height && bullet.PositionY > enemies[i].PositionY) 
                    &&
                    (bullet.PositionX + bullet.shape.Width > enemies[i].PositionX + 8 &&
                    bullet.PositionX < enemies[i].PositionX + enemies[i].shape.Width - 8 ))
                {
                    enemies.Remove(foe);
                    canvas.Children.Remove(foe.shape);
                    canvas.Children.Remove(bullet.shape);
                    bulletTimer.Stop();
                    updateKillCount();
                    isLockSpaceBar = true;

                }
            }
            
        }

        private void kDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    leftPressed = true;
                    break;
                case Key.Right:
                    rightPressed = true;
                    break;
                case Key.Space:
                    try
                    {
                        if (isLockSpaceBar)
                        {

                            player.Play();
                            bullet.shape.Fill = new ImageBrush(new BitmapImage(new Uri(bulletPath, UriKind.Relative)));
                            bullet.shape.Width = 10;
                            bullet.shape.Height = 20;
                            Canvas.SetTop(bullet.shape, canvas.ActualHeight - ship.shape.ActualHeight-10);
                            Canvas.SetLeft(bullet.shape, Canvas.GetLeft(ship.shape) + (ship.shape.ActualWidth / 2.0)-5);
                            canvas.Children.Add(bullet.shape);
                            bullet.PositionY = Canvas.GetTop(bullet.shape);
                            bullet.PositionX = Canvas.GetLeft(bullet.shape);
                            bulletTimer.Start();
                            isLockSpaceBar = false;
                            
                        }
                    }
                    catch (System.ArgumentException)
                    {
                       
                     }
                    break;
                case Key.P:
                    if (isPaused)
                    {
                        strafeTimer.Start();
                        isPaused = !isPaused;
                        break;
                    }
                    strafeTimer.Stop();
                    isPaused = !isPaused;
                        
                    break;

            }
        }

        private void kUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    leftPressed = false;
                    break;
                case Key.Right:
                    rightPressed = false;
                    break;
               
                   
            }

        }

      
    }
}

