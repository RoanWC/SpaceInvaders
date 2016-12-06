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
using WpfApplication2.Views;
namespace SpaceInvaders
{
    public partial class MainWindow : Window
    {
        Boolean isLoadedGame = false;
        Random randomNumber = new Random();
        private DispatcherTimer strafeTimer;
        private DispatcherTimer bulletTimer;
        private DispatcherTimer enemyBulletTimer;
        private DispatcherTimer enemyAttackTimer;
        string bulletPath = "Resources/donaldthumb.png";
        private List<CustomShape> enemies = new List<CustomShape>();
        private List<CustomShape> shipbullets = new List<CustomShape>();
        private List<CustomShape> enemybullets = new List<CustomShape>();
        
        List<CustomShape> barriers = new List<CustomShape>();
        CustomShape[] barriersArray;
        CustomShape ship = new CustomShape();



        double bulletSpeed = 8;
        double speed = 1;
        bool leftPressed;
        bool rightPressed;
        bool isPaused = false;
        public int killCount = 0;
        int difficulty = 1;
        int rows = 3;
        int cols = 8;
        int playerLives = 3;
        int delay = 2000;
        double top = 0.0;
       
        SoundPlayer player = new System.Media.SoundPlayer("Resources/shotSound.wav");
        

        public MainWindow()
        {

            Loaded += delegate
            {
                InitializeComponent();
                Credits.Text = "Writen By:\n Eric Hughes\n Roan Chamberlain\n Jon Depaz";
            };
        }
        private void NewGameClick(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            Credits.Foreground = null;
            window.Close();
            start_button.Visibility = Visibility.Hidden;
            load_button.Visibility = Visibility.Hidden;
            kills.Visibility = Visibility.Visible;
            Lives.Visibility = Visibility.Visible;
            openHS.Visibility = Visibility.Hidden;
            strafeTimer = new DispatcherTimer();
            bulletTimer = new DispatcherTimer();
            enemyBulletTimer = new DispatcherTimer();
            enemyAttackTimer = new DispatcherTimer();
            strafeTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            strafeTimer.Tick += move;
            bulletTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            bulletTimer.Tick += moveBullet;
            enemyAttackTimer.Interval = new TimeSpan(0, 0, 0, 0, delay);
            enemyAttackTimer.Tick += enemyAttack;
            enemyBulletTimer.Interval = new TimeSpan(0, 0, 0, 0, 5);
            enemyBulletTimer.Tick += moveEnemyBullet;
           
           
            string barrierPath = "Resources/barrier.png";
            for (int i = 0; i < 3; i++)
            {
               var left = 1.0;
               var barrier =  new CustomShape();
                barrier.shape = new Rectangle();
                barrier.shape.Width = 100;
                barrier.shape.Height = 50;
                barrier.Health = 10;
                barrier.shape.Fill = new ImageBrush(new BitmapImage(new Uri(barrierPath, UriKind.Relative)));
                barriers.Add(barrier);

               
                Canvas.SetTop(barrier.shape, canvas.ActualHeight - barrier.shape.Height * 2);
                barrier.PositionX = Canvas.GetLeft(barrier.shape);
                barrier.PositionY = Canvas.GetTop(barrier.shape);
                canvas.Children.Add(barrier.shape);
                left =  (barrier.Health * i+1) * barrier.Health;
            }
           
            barriersArray = barriers.ToArray();
            ship.shape = new Rectangle();
            ship.Name = "Ship";
            ship.shape.Width = 50;
            ship.shape.Height = 50;
            Lives.Text = playerLives.ToString();
            String shipPath = "Resources/ship.png";
            String backGroundPath = "Resources/background.gif";
            ship.shape.Fill = new ImageBrush(new BitmapImage(new Uri(shipPath, UriKind.Relative)));
            Canvas.SetLeft(ship.shape, 200);
            Canvas.SetTop(ship.shape, canvas.ActualHeight - ship.shape.Height);
            ship.PositionX = Canvas.GetLeft(ship.shape);
            ship.PositionY = canvas.ActualHeight - ship.shape.Height;
            canvas.Children.Add(ship.shape);

            canvas.Background = new ImageBrush(new BitmapImage(new Uri(backGroundPath, UriKind.Relative)));
            if (isLoadedGame == false)
                createLevel(difficulty);
            else if (isLoadedGame == true)
                createLoadedGame(difficulty);
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
            enemyAttackTimer.Start();
            barriers.Add(barrier1);
            barriers.Add(barrier2);
            barriers.Add(barrier3);
            barriersArray = barriers.ToArray();
            foreach (CustomShape barrier in barriers)
            {
                try
                {

                    canvas.Children.Add(barrier.shape);
                }
                catch (ArgumentException)
                {

                }
            }
            
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
                    ship.PositionX = Canvas.GetLeft(ship.shape);
                }
            }
            if (rightPressed)
            {
                if (x + ship.shape.ActualWidth < canvas.ActualWidth)
                {
                    x += 3;
                    Canvas.SetLeft(ship.shape, x);
                    ship.PositionX = Canvas.GetLeft(ship.shape);
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
            for (int j = 0; j < shipbullets.Count; j++)
            {
                for (  int i = 0; i < barriersArray.Length; i++)
                {
                        if (shipbullets[j].PositionY <= barriersArray[i].PositionY + barriersArray[i].shape.Height
                         &&
                         shipbullets[j].PositionX + shipbullets[j].shape.Width >= barriersArray[i].PositionX
                         &&
                          shipbullets[j].PositionX <= barriersArray[i].PositionX + barriersArray[i].shape.Width)
                        {
                            barriersArray[i].Health--;
                            canvas.Children.Remove(shipbullets[j].shape);
                            shipbullets.Remove(shipbullets[j]);
                            if (barriersArray[i].Health == 0)
                            {
                                canvas.Children.Remove(barriersArray[i].shape);
                                barriersArray[i].PositionX = 0;
                                barriersArray[i].PositionY = 0;
                            }
                        break;
                        }
                 }
            }

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

        private void GameOver()
        {
            
            
                enemyAttackTimer.Stop();
                enemyBulletTimer.Stop();
                strafeTimer.Stop();
                bulletTimer.Stop();
                GameOverWindow GameOverWindow = new GameOverWindow(killCount);
                GameOverWindow.Show();
        }

        private void moveEnemyBullet(object sender, EventArgs e)
        {
           
            Boolean hitship = false;
            for (int j = 0; j < enemybullets.Count; j++)
            {

                if ((enemybullets[j].PositionY + enemybullets[j].shape.Height > ship.PositionY
                    && enemybullets[j].PositionY + enemybullets[j].shape.Height < ship.PositionY + ship.shape.Height)
                        
                    &&

                    (enemybullets[j].PositionX + enemybullets[j].shape.Width > ship.PositionX  
                    && enemybullets[j].PositionX <= ship.PositionX + ship.shape.Width))
                {
                    canvas.Children.Remove(enemybullets[j].shape);
                    enemybullets.Remove(enemybullets[j]);
                    playerLives--;
                    Lives.Text = playerLives.ToString();
                    hitship = true;
                    if (playerLives == 0)
                        canvas.Children.Remove(ship.shape);

                }
                else if(!hitship)
                for (int i = 0; i < barriersArray.Length; i++)
                {
                   
                        if (enemybullets[j].PositionY >= barriersArray[i].PositionY
                            &&
                            enemybullets[j].PositionX + enemybullets[j].shape.Width >= barriersArray[i].PositionX
                            &&
                             enemybullets[j].PositionX <= barriersArray[i].PositionX + barriersArray[i].shape.Width)
                        {
                            canvas.Children.Remove(enemybullets[j].shape);
                            enemybullets.Remove(enemybullets[j]);
                            barriersArray[i].Health--;
                                if (barriersArray[i].Health == 0)
                            {
                                canvas.Children.Remove(barriersArray[i].shape);
                                barriersArray[i].PositionX = 0;
                                barriersArray[i].PositionY = 0;
                            }

                            break;
                        }
                }
                
                else if (enemybullets[j].PositionY > canvas.ActualHeight)
                {
                    canvas.Children.Remove(enemybullets[j].shape);
                    enemybullets.Remove(enemybullets[j]);
                }

            }
            for (int z = 0; z < enemybullets.Count; z++)
            {
                enemybullets[z].PositionY += bulletSpeed;
                Canvas.SetTop(enemybullets[z].shape, enemybullets[z].PositionY);
            }

        }
        



        private void enemyAttack(object sender, EventArgs e)
        {
            double canvaswidth = Math.Round(canvas.ActualWidth);
            List<CustomShape> AvailableEnemies = new List<CustomShape>();
           
            var enemyCount = enemies.Count;

            if (enemyCount > 0)
            {
                var LastEnemy = enemies[enemyCount - 1];
                for (int i = enemyCount - 1; i >= 0; i--)
                {
                    if (enemies[i].PositionY >= LastEnemy.PositionY)
                    {
                        var CurrentEnemy = enemies[i];
                        if (CurrentEnemy.shape.ActualHeight >= LastEnemy.shape.ActualHeight)
                            AvailableEnemies.Add(CurrentEnemy);
                    }
                }
                if (AvailableEnemies.Count != 0)
                {
                    var ChosenEnemyToFire = AvailableEnemies[randomNumber.Next(0, AvailableEnemies.Count - 1)];
                    CustomShape enemybullet = new CustomShape();
                    enemybullet.shape = new Rectangle();
                    enemybullet.shape.Fill = new ImageBrush(new BitmapImage(new Uri(bulletPath, UriKind.Relative)));
                    enemybullet.Name = "EnemyBullet";
                    enemybullet.shape.Width = 5;
                    enemybullet.shape.Height = 10;
                    Canvas.SetTop(enemybullet.shape, ChosenEnemyToFire.PositionY);
                    Canvas.SetLeft(enemybullet.shape, Canvas.GetLeft(ChosenEnemyToFire.shape) + (ChosenEnemyToFire.shape.ActualWidth / 2.0));
                    enemybullets.Add(enemybullet);
                    enemybullet.PositionY = Canvas.GetTop(enemybullet.shape);
                    enemybullet.PositionX = Canvas.GetLeft(enemybullet.shape);
                    canvas.Children.Add(enemybullet.shape);
                    enemyBulletTimer.Start();
                    if (delay > 100)
                        delay -= 50;
                    else
                        delay = 10;
                    enemyAttackTimer.Interval = TimeSpan.FromMilliseconds(delay);
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
                case Key.P:
                    if (isPaused)
                    {
                        strafeTimer.Start();
                        isPaused = !isPaused;
                        paused2.Visibility = Visibility.Hidden;
                        paused.Visibility = Visibility.Hidden;
                        break;
                    }
                    strafeTimer.Stop();
                    isPaused = !isPaused;
                    paused2.Visibility = Visibility.Visible;
                    paused.Visibility = Visibility.Visible;
                    if (MessageBox.Show("Save game?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        saveFile();
                    }
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
            List<String> state = new List<String>();
            String fileName = "gameState.txt";
            state.Add("count:" + enemies.Count);
            for (int i = 0; i < enemies.Count; i++)
            {
                state.Add(i + ":" + enemies[i].PositionX + ":" + enemies[i].PositionY + ":" + enemies[i].shape.Height + ":" + enemies[i].shape.Width + ":" + enemies[i].Health);
            }
            state.Add("--End Enemies");
            state.Add("difficulty:" + difficulty);
            state.Add("Kill count:" + killCount);
            state.Add("Speed:" + speed);
            state.Add("Lives:" + playerLives);
            using (StreamWriter outputFile = new StreamWriter(fileName))
            {
                foreach (string line in state)
                    outputFile.WriteLine(line);
            }
        }

        private void load_button_Click(object sender, RoutedEventArgs e)
        {
            try {
                List<String> loadState = new List<String>();
                isLoadedGame = true;
                int index = -1;
                String[] loadEnemies, loadInfo;
                String fileName = "gameState.txt", relativePath = "Resources/hilaryclintonface.png"; ;

                using (StreamReader inputFile = new StreamReader(fileName))
                {
                    while (inputFile.Peek() >= 0)
                    {
                        String line = inputFile.ReadLine();
                        loadState.Add(line);
                    }
                }
                for (int i = 0; i < loadState.Count; i++)
                {
                    if (loadState[i].Equals("--End Enemies"))
                    {
                        index = i;
                        break;
                    }
                }
                loadEnemies = loadState[0].Split(':');
                int size = int.Parse(loadEnemies[1]);
                for (int i = 1; i < index; i++)
                {
                    loadEnemies = loadState[i].Split(':');
                    CustomShape foe = new CustomShape(); //create the rectangle
                    foe.shape = new Rectangle();
                    foe.shape.Fill = new ImageBrush(new BitmapImage(new Uri(relativePath, UriKind.Relative)));
                    foe.shape.Width = int.Parse(loadEnemies[4]);
                    foe.shape.Height = int.Parse(loadEnemies[3]);
                    foe.PositionX = int.Parse(loadEnemies[1]);
                    foe.PositionY = int.Parse(loadEnemies[2]);
                    foe.Health = int.Parse(loadEnemies[5]);
                    Canvas.SetLeft(foe.shape, foe.PositionX);
                    Canvas.SetTop(foe.shape, foe.PositionY);
                    enemies.Add(foe);
                }
                index++;
                loadInfo = loadState[index].Split(':');
                difficulty = int.Parse(loadInfo[1]);
                index++;
                loadInfo = loadState[index].Split(':');
                killCount = int.Parse(loadInfo[1]);
                index++;
                loadInfo = loadState[index].Split(':');
                speed = int.Parse(loadInfo[1]);
                index++;
                loadInfo = loadState[index].Split(':');
                playerLives = int.Parse(loadInfo[1]);
                NewGameClick(sender, e);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("There is no saved game", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void createLoadedGame(int difficulty)
        {
            foreach (CustomShape foe in enemies)
            {
                canvas.Children.Add(foe.shape);
            }
            strafeTimer.Start();
            kills.Text = Convert.ToString(killCount);
        }

        private void openHS_Click(object sender, RoutedEventArgs e)
        {
            Leaderboards leaderboard = new Leaderboards();
            leaderboard.Show();
        }
    }
}