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
using System.IO;
using Microsoft.Win32;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
namespace SpaceInvaders
{
    public partial class MainWindow : Window
    {
        Random rand = new Random();
        private DispatcherTimer strafeTimer;
        private DispatcherTimer bulletTimer;
        string bulletPath = "Resources/donaldthumb.png";
        private List<CustomShape> enemies = new List<CustomShape>();
        private List<CustomShape> shipbullets = new List<CustomShape>();
        private List<CustomShape> enemybullets = new List<CustomShape>();
        CustomShape barrier1 = new CustomShape();
        CustomShape barrier2 = new CustomShape();
        CustomShape barrier3 = new CustomShape();
        CustomShape ship = new CustomShape();
        double bulletSpeed = 8;
        double speed = 1;
        bool leftPressed;
        bool rightPressed;
        bool isPaused = false;
        int killCount = 0;
        int difficulty = 1;
        int rows = 3;
        int cols = 8;
        double top = 0.0;
        SoundPlayer player = new System.Media.SoundPlayer("Resources/shotSound.wav");
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
            strafeTimer = new DispatcherTimer();
            bulletTimer = new DispatcherTimer();
            strafeTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            strafeTimer.Tick += move;
            bulletTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            bulletTimer.Tick += moveBullet;
            string barrierPath = "Resources/barrier.png";
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
            ship.Name = "Ship";
            ship.shape.Width = 50;
            ship.shape.Height = 50;
            String shipPath = "Resources/ship.png";
            String backGroundPath = "Resources/background.gif";
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
            {
                rows++;
                speed += 0.5;
            }
            else if (difficulty > 3)
                cols++;
            var FoeYSpacing = 0.0;
            var FoeXSpacing = 1.0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    string relativePath = "Resources/hilaryclintonface.png";
                    CustomShape foe = new CustomShape(); //create the rectangle
                    foe.shape = new Rectangle();
                    foe.shape.Fill = new ImageBrush(new BitmapImage(new Uri(relativePath, UriKind.Relative)));
                    foe.Name = "Enemy row" + i + " col " + j;
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
                foreach (var item in shipbullets)
                {
                    canvas.Children.Remove(item.shape);
                }
                bulletTimer.Stop();
                shipbullets.Clear();
                createLevel(++difficulty);
            }
            Boolean diretionChanged = false;
            for (int i = 0; i < enemies.Count; i++)
            {
                double canvaswidth = Math.Round(canvas.ActualWidth);
                if (enemies[i].PositionX + enemies[i].shape.Width >= canvaswidth)
                {
                    speed = -speed;
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
                    speed = -speed;
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
            int enemyCount = enemies.Count;
            for (int i = 0; i < enemyCount; i++)
            {
                for (int j = 0; j < shipbullets.Count; j++)
                {
                    try
                    {
                        if ((shipbullets[j].PositionY <= enemies[i].PositionY + enemies[i].shape.Height && shipbullets[j].PositionY >= enemies[i].PositionY)
                        &&
                        (shipbullets[j].PositionX + shipbullets[j].shape.Width > enemies[i].PositionX &&
                         shipbullets[j].PositionX <= enemies[i].PositionX + enemies[i].shape.Width))
                        {
                            canvas.Children.Remove(enemies[i].shape);
                            canvas.Children.Remove(shipbullets[j].shape);
                            enemies.Remove(enemies[i]);
                            shipbullets.Remove(shipbullets[j]);
                            enemyCount = enemies.Count;
                            updateKillCount();
                        }
                        else if (shipbullets[j].PositionY < 0)
                        {
                            canvas.Children.Remove(shipbullets[j].shape);
                            shipbullets.Remove(shipbullets[j]);
                        }
                    }
                    catch (ArgumentOutOfRangeException ioe)
                    {
                        continue;
                    }
                }
            }
            for (int z = 0; z < shipbullets.Count; z++)
            {
                shipbullets[z].PositionY -= bulletSpeed;
                Canvas.SetTop(shipbullets[z].shape, shipbullets[z].PositionY);
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
                case Key.S:
                    saveFile();
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
                case Key.Space:
                    CustomShape bullet = new CustomShape();
                    bullet.shape = new Rectangle();
                    //player.Play();
                    bullet.shape.Fill = new ImageBrush(new BitmapImage(new Uri(bulletPath, UriKind.Relative)));
                    bullet.Name = "Bullet";
                    bullet.shape.Width = 10;
                    bullet.shape.Height = 20;
                    Canvas.SetTop(bullet.shape, canvas.ActualHeight - ship.shape.ActualHeight);
                    Canvas.SetLeft(bullet.shape, Canvas.GetLeft(ship.shape) + (ship.shape.ActualWidth / 2.0));
                    shipbullets.Add(bullet);
                    bullet.PositionY = Canvas.GetTop(bullet.shape);
                    bullet.PositionX = Canvas.GetLeft(bullet.shape);
                    canvas.Children.Add(bullet.shape);
                    bulletTimer.Start();
                    break;
            }
        }
        private void saveFile()
        {
            strafeTimer.Stop();
            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            List<String> state = new List<String>();
            OpenFileDialog fileChooser = new OpenFileDialog();
            fileChooser.ShowDialog();
            String fileName = fileChooser.FileName;
            state.Add("count:" + enemies.Count);
            for (int i = 0; i < enemies.Count; i++)
            {
                state.Add(i + ":" + enemies[i].PositionX + ":" + enemies[i].PositionY + ":" + enemies[i].shape.Height + ":" + enemies[i].shape.Width + ":" + enemies[i].Health);
            }
            state.Add("--End Enemies");
            CustomShape a = enemies[1];
            using (StreamWriter outputFile = new StreamWriter(fileName))
            {
                foreach (string line in state)
                    outputFile.WriteLine(line);
            }
            strafeTimer.Start();
        }
        //Window.DialogResult result;
        //String fileName;
        //OpenFileDialog fileChooser = new OpenFileDialog()
        //result = fileChooser.ShowDialog();
        //fileName = fileChooser.FileName;
        //FileStream file = new FileStream;
        //StreamWriter a = new StreamWriter();
    }
}
