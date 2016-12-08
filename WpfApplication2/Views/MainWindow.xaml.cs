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

using WpfApplication2.Views;
/*
 Space invaders written by eric hughes roan chamberlain jon depaz

    Game is created using 3 main classes 
    CustomShape which inherits from UIELement to populate the screen 
    The custom shape is used as the enemies, ship and barriers

    The MainWindow class has lists of enemies, ship bullets, enemy bullets 
    multiple timers to animate the movement of the enemies, ship bullets, enemy bullets and
    the decremented delay in between each enemy attack.

    enemies at the first row have 3 health and decremented by 1 towards the bottom of the canvas

    if the player manages to kill all the enemies for that level, difficulty is incremented 
    which increments the rows, columns and speed of the enemies strafe and descent 

    the barriers have 10 health each and are decremented 1 each time an enemy hits the barrier 
    no changes as the player progresses through the levels

    the score is decided by the enemies, hillary = 3, bill = 2, obama = 1
    the scores for each kill are multiplied by the difficulty as the player progresses

    the player can pause and is prompted to save progress or continue. the player can pause game 
    save quit game and load to their checkpoint

    the player has 3 lives and once their lives are equal 0 they are asked to save their scores 
    on the leaderboards which are sorted highest first and only display top 15
     */
namespace SpaceInvaders
{
    public partial class MainWindow : Window
    {


        Boolean isLoadedGame = false;
        Random randomNumber = new Random();

        // timers for movement
        private DispatcherTimer strafeTimer;
        private DispatcherTimer bulletTimer;
        private DispatcherTimer enemyBulletTimer;
        private DispatcherTimer enemyAttackTimer;

        // string paths for resources
        string barrierPath = "Resources/barrier.png";
        string bulletPath = "Resources/donaldthumb.png";
        // lists to hold enemies, shipbullets and enemybullets
        private List<CustomShape> enemies = new List<CustomShape>();
        private List<CustomShape> shipbullets = new List<CustomShape>();
        private List<CustomShape> enemybullets = new List<CustomShape>();
        private List<CustomShape> barriers = new List<CustomShape>();
        //converted barriers to an array since the size never changes
        //can implement more barriers in the future with list 
        CustomShape[] barriersArray;
        //ship is declared as a field since needs full scope but not in a list since only 1 ship
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
        int counter = 0;
        double top = 3.0;
        SoundPlayer player = new System.Media.SoundPlayer("Resources/shotSound.wav");
        MediaPlayer song = new System.Windows.Media.MediaPlayer();

        public MainWindow()
        {

            Loaded += delegate
            {
                InitializeComponent();
                Credits.Text = "Writen By:\n Eric Hughes\n Roan Chamberlain\n Jon Depaz";
            };
        }
        //creates new game once user clicks new game 
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
            //timers are built and set
            strafeTimer = new DispatcherTimer();
            bulletTimer = new DispatcherTimer();
            enemyBulletTimer = new DispatcherTimer();
            enemyAttackTimer = new DispatcherTimer();
            strafeTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            strafeTimer.Tick += move;
            bulletTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            bulletTimer.Tick += moveShipBullet;
            enemyAttackTimer.Interval = new TimeSpan(0, 0, 0, 0, delay);
            enemyAttackTimer.Tick += enemyAttack;
            enemyBulletTimer.Interval = new TimeSpan(0, 0, 0, 0, 5);
            enemyBulletTimer.Tick += moveEnemyBullet;
            // build ship 
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
            // add ship to canvas and save X and Y position in CustomShape properties PositionX and PositionY
            canvas.Children.Add(ship.shape);
            //start music
            song.Open(new Uri("Resources/oyvey.wav", UriKind.Relative)); //Open the file for a media playback
            song.MediaEnded += song_MediaEnded;
            song.Play();
            canvas.Background = new ImageBrush(new BitmapImage(new Uri(backGroundPath, UriKind.Relative)));

            // if game is new or not if false create brand new barriers
            if (!isLoadedGame)
            {
                CreateBarriers();
                createLevel(difficulty);
            }
            else if (isLoadedGame)
                createLoadedGame(difficulty);
        }

        void song_MediaEnded(object sender, EventArgs e)
        {
            song.Position = TimeSpan.FromSeconds(0);
            //song.Play();
        }
        private void CreateBarriers()
        {
            var left = 10.0;
            for (int i = 0; i < 3; i++)
            {
                var barrier = new CustomShape();
                barrier.shape = new Rectangle();
                barrier.shape.Width = 100;
                barrier.shape.Height = 50;
                barrier.Health = 10;
                barrier.shape.Fill = new ImageBrush(new BitmapImage(new Uri(barrierPath, UriKind.Relative)));
                barriers.Add(barrier);

                Canvas.SetTop(barrier.shape, canvas.ActualHeight - barrier.shape.Height * 2);
                if (i > 0)
                    left = (barrier.Health * barrier.Health) * (i + i);
                Canvas.SetLeft(barrier.shape, left);
                barrier.PositionX = Canvas.GetLeft(barrier.shape);
                barrier.PositionY = Canvas.GetTop(barrier.shape);
                canvas.Children.Add(barrier.shape);
            }
            barriersArray = barriers.ToArray();
        }
        // create level depending on difficulty
        public void createLevel(int difficulty)
        {

            if (difficulty > 1)
            {
                rows++;
                speed += 0.5;

            }
            if (difficulty > 3)
                cols++;
            var FoeYSpacing = 0.0;
            var FoeXSpacing = 1.0;
            var str = "Resources/hilaryclintonface.png," + "Resources/billclinton.png," + "Resources/obama.png";
            var enemyNames = "Hilary,Bill,Obama";
            var PicturesArray = str.Split(',');
            var enemyNamesArray = enemyNames.Split(',');
            var health = 3;
            // rows are dependent on difficulty
            // build list of enemies
            for (int i = 0; i < rows; i++)
            {

                if (counter > 2)
                    counter = 0;

                for (int j = 0; j < cols; j++)
                {

                    CustomShape foe = new CustomShape(); //create the rectangle
                    foe.shape = new Rectangle();
                    foe.Name = enemyNamesArray[counter];
                    foe.shape.Fill = new ImageBrush(new BitmapImage(new Uri(PicturesArray[counter], UriKind.Relative)));
                    foe.shape.Width = 50.0;
                    foe.shape.Height = 50.0;
                    foe.PositionX = FoeXSpacing;
                    foe.PositionY = FoeYSpacing;
                    foe.Health = health;
                    Canvas.SetLeft(foe.shape, FoeXSpacing);
                    FoeXSpacing += foe.shape.Width;
                    Canvas.SetTop(foe.shape, FoeYSpacing);

                    enemies.Add(foe);
                }
                // restart counter for resources array
                counter++;

                health--;
                FoeXSpacing = 0.0;
                // once outer loop is finished increase the PositionY of enemy so they dont overlap
                FoeYSpacing += enemies[i].shape.Height;
            }
            foreach (CustomShape foe in enemies)
            {
                canvas.Children.Add(foe.shape);
            }
            // end of build enemies
            //build barriers
            CreateBarriers();
            //start strafe timer which invokes move method
            strafeTimer.Start();
            //start enemy delay in between enemy attacks. set to 2000 milliseconds 
            enemyAttackTimer.Start();

        }
        // move is invoked each time the enemy moves
        public void move(object sender, EventArgs e)
        {
            // if all enemies are destroyed create new level with new difficulty
            if (enemies.Count == 0)
            {

                foreach (var item in shipbullets)
                {
                    canvas.Children.Remove(item.shape);
                }
                strafeTimer.Stop();
                top = 0.0;
                bulletTimer.Stop();
                enemyAttackTimer.Stop();
                enemyBulletTimer.Stop();
                enemies.Clear();
                enemybullets.Clear();
                shipbullets.Clear();
                createLevel(++difficulty);
            }
            Boolean diretionChanged = false;
            // check for left and right side collisions
            for (int i = 0; i < enemies.Count; i++)
            {
                double canvaswidth = Math.Round(canvas.ActualWidth);
                // check enemies right side is > than canvas change speed and update positions for 
                // remainder in list with inner for loop
                if (enemies[i].PositionX + enemies[i].shape.Width >= canvaswidth)
                {
                    speed = -speed;

                    diretionChanged = true;
                    for (int j = 0; j < enemies.Count; j++)
                    {
                        enemies[j].PositionY += top;
                        Canvas.SetTop(enemies[j].shape, enemies[j].PositionY);
                    }
                }
                // same logic but for left side of canvas and change speed
                else if (enemies[i].PositionX < 0)
                {
                    speed = -speed;

                    diretionChanged = true;
                    for (int j = 0; j < enemies.Count; j++)
                    {
                        enemies[j].PositionY += top;
                        Canvas.SetTop(enemies[j].shape, enemies[j].PositionY);
                    }
                }
                // check if enemies have descended past the ship call GameOver()
                else if (enemies[i].PositionY + enemies[i].shape.ActualHeight > ship.PositionY)
                    GameOver();
                // update method when loaded is called 
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
                // if no collissions on canvas update positions normally
                else
                {
                    enemies[i].PositionX += speed;
                    Canvas.SetLeft(enemies[i].shape, enemies[i].PositionX);
                }
            }
            // update ships position for players input on keyboard
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
        // give points for particular kill to player and display to canvas
        public void updateKillCount(String name)
        {
            var score = 0;
            if (name.Equals("Hilary"))
                score = 3 * difficulty;
            else if (name.Equals("Bill"))
                score = 2 * difficulty;
            else if (name.Equals("Obama"))
                score = 1 * difficulty;
            killCount += score;
            kills.Text = Convert.ToString(killCount);
        }
        // timer method for shipbullets
        // same logic as enemy detection but slightly different 
        // also checks collisions with barriers and removes them if their health is 0
        public void moveShipBullet(object sender, EventArgs e)
        {
            int enemyCount = enemies.Count;
            for (int j = 0; j < shipbullets.Count; j++)
            {
                for (int i = 0; i < barriersArray.Length; i++)
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
            //(int i = size-1; i >= 0; i--
            for (int i = enemies.Count-1; i >= 0; i--)
            {
                for (int j = shipbullets.Count-1; j >=0; j--)
                {
                    try
                    {
                        if ((shipbullets[j].PositionY <= enemies[i].PositionY + enemies[i].shape.Height && shipbullets[j].PositionY >= enemies[i].PositionY)
                        &&
                        (shipbullets[j].PositionX + shipbullets[j].shape.Width > enemies[i].PositionX &&
                         shipbullets[j].PositionX <= enemies[i].PositionX + enemies[i].shape.Width))
                        {
                            enemies[i].Health--;
                            canvas.Children.Remove(shipbullets[j].shape);
                            shipbullets.Remove(shipbullets[j]);

                            if (enemies[i].Health == 0)
                            {
                                canvas.Children.Remove(enemies[i].shape);
                                updateKillCount(enemies[i].Name);
                                enemies.Remove(enemies[i]);


                            }

                        }
                        else if (shipbullets[j].PositionY < 0)
                        {
                            canvas.Children.Remove(shipbullets[j].shape);
                            shipbullets.Remove(shipbullets[j]);
                        }
                    }
                    catch (ArgumentException) { }
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
            song.Stop();
            song.Position = TimeSpan.FromSeconds(0);
            enemyBulletTimer.Stop();
            strafeTimer.Stop();
            bulletTimer.Stop();
            GameOverWindow GameOverWindow = new GameOverWindow(killCount);
            GameOverWindow.Show();
            this.Close();
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
                    {
                        canvas.Children.Remove(ship.shape);
                        GameOver();
                    }

                }
                else if (!hitship)
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
                    if (delay > 500)
                        delay -= 50;
                    else
                        delay = 500;
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
                        enemyAttackTimer.Start();
                        enemyBulletTimer.Start();
                        bulletTimer.Start();
                        song.Play();
                        isPaused = !isPaused;
                        paused2.Visibility = Visibility.Hidden;
                        paused.Visibility = Visibility.Hidden;
                        break;
                    }
                    strafeTimer.Stop();
                    enemyAttackTimer.Stop();
                    enemyBulletTimer.Stop();
                    bulletTimer.Stop();
                    song.Pause();
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
            // List hold all the info needed to save
            List<String> state = new List<String>();
            // File to save the game info
            String fileName = "gameState.txt";
            state.Add("count:" + enemies.Count);
            // Adds each enemies health, coordinates, dimensions and name to the list
            for (int i = 0; i < enemies.Count; i++)
            {
                state.Add(i + ":" + enemies[i].PositionX + ":" + enemies[i].PositionY + ":" + enemies[i].shape.Height + ":" + enemies[i].shape.Width + ":" + enemies[i].Health + ":" + enemies[i].Name);
            }
            // End of enemy info
            state.Add("--End Enemies");
            state.Add("difficulty:" + difficulty); // saves the difficulty
            state.Add("Kill count:" + killCount); // saves the user score
            state.Add("Speed:" + speed); // saves the speed for enemies
            state.Add("Lives:" + playerLives); // save remaining lives
            // Adds each barrier health and coordinates to the list
            for (int i = 0; i < barriersArray.Length; i++)
            {
                state.Add("Barrier" + i + ":" + barriersArray[i].Health + ":" + barriersArray[i].PositionX + ":" +
                    barriersArray[i].PositionY + ":" + Canvas.GetTop(barriersArray[i].shape) + ":" + Canvas.GetLeft(barriersArray[i].shape));
            }
            using (StreamWriter outputFile = new StreamWriter(fileName))
            {
                // for every element in the list, write it to it's own line in the file
                foreach (string line in state)
                    outputFile.WriteLine(line);
            }
        }

        private void load_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // list to hold data
                List<String> loadState = new List<String>();
                isLoadedGame = true;
                int index = -1;
                // loadEnemies for enemy info, loadInfo for other info
                String[] loadEnemies, loadInfo;
                String fileName = "gameState.txt";

                using (StreamReader inputFile = new StreamReader(fileName))
                {
                    while (inputFile.Peek() >= 0)
                    {
                        // add every line to the loadState list
                        String line = inputFile.ReadLine();
                        loadState.Add(line);
                    }
                }
                for (int i = 0; i < loadState.Count; i++)
                {
                    // find the index where the enemy info ends
                    if (loadState[i].Equals("--End Enemies"))
                    {
                        index = i;
                        break;
                    }
                }
                loadEnemies = loadState[0].Split(':');
                int size = int.Parse(loadEnemies[1]);
                String hPath = "Resources/hilaryclintonface.png",
                       bPath = "Resources/billclinton.png",
                       oPath = "Resources/obama.png";
                for (int i = 1; i < index; i++)
                {
                    // split the line by a delimiter 
                    loadEnemies = loadState[i].Split(':');
                    CustomShape foe = new CustomShape(); //create the rectangle
                    foe.shape = new Rectangle();
                    // load needed info to properly "re-draw" the enemies on the canvas
                    foe.shape.Width = int.Parse(loadEnemies[4]);
                    foe.shape.Height = int.Parse(loadEnemies[3]);
                    foe.PositionX = int.Parse(loadEnemies[1]);
                    foe.PositionY = int.Parse(loadEnemies[2]);
                    foe.Health = int.Parse(loadEnemies[5]);
                    // depending on the name of the enemy, a different picture will be used
                    foe.Name = loadEnemies[6];
                    if (foe.Name.Equals("Hilary"))
                        foe.shape.Fill = new ImageBrush(new BitmapImage(new Uri(hPath, UriKind.Relative)));
                    else if (foe.Name.Equals("Bill"))
                        foe.shape.Fill = new ImageBrush(new BitmapImage(new Uri(bPath, UriKind.Relative)));
                    else
                        foe.shape.Fill = new ImageBrush(new BitmapImage(new Uri(oPath, UriKind.Relative)));
                    Canvas.SetLeft(foe.shape, foe.PositionX);
                    Canvas.SetTop(foe.shape, foe.PositionY);
                    enemies.Add(foe); // add each enemy to the list
                }
                // split the next line with delimiter, load the data and increment index
                index++;
                loadInfo = loadState[index].Split(':');
                difficulty = int.Parse(loadInfo[1]);
                index++;
                loadInfo = loadState[index].Split(':');
                killCount = int.Parse(loadInfo[1]);
                index++;
                loadInfo = loadState[index].Split(':');
                speed = Double.Parse(loadInfo[1]);
                index++;
                loadInfo = loadState[index].Split(':');
                playerLives = int.Parse(loadInfo[1]);
                index++;
                int k = 0;
                // build barrier list
                for (int i = 0; i < 3; i++)
                {
                    var barrier = new CustomShape();
                    barrier.shape = new Rectangle();
                    barrier.shape.Width = 100;
                    barrier.shape.Height = 50;
                    barrier.Health = 10;
                    barrier.shape.Fill = new ImageBrush(new BitmapImage(new Uri(barrierPath, UriKind.Relative)));
                    barriers.Add(barrier);
                }
                while (k < barriers.Count)
                {
                    loadInfo = loadState[index].Split(':');
                    barriers[k].Health = double.Parse(loadInfo[1]);
                    barriers[k].PositionX = double.Parse(loadInfo[2]);
                    barriers[k].PositionY = double.Parse(loadInfo[3]);
                    Canvas.SetTop(barriers[k].shape, double.Parse(loadInfo[4]));
                    Canvas.SetLeft(barriers[k].shape, double.Parse(loadInfo[5]));
                    // only draw the barrier on the canvas if the health is > 0, when health = 0, the barrier is destroyed
                    if (barriers[k].Health > 0)
                        canvas.Children.Add(barriers[k].shape);
                    k++;
                    // increment index so we can load info from the next line
                    index++;
                    //
                }
                barriersArray = barriers.ToArray();
                NewGameClick(sender, e);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("There is no saved game", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void createLoadedGame(int difficulty)
        {
            // adds each enemy based on their coordinates so the enemies are in their proper positions prior to the save
            foreach (CustomShape foe in enemies)
            {
                canvas.Children.Add(foe.shape);
            }
            // start the enemy move timer
            strafeTimer.Start();
            // update kill count
            kills.Text = Convert.ToString(killCount);
        }

        private void openHS_Click(object sender, RoutedEventArgs e)
        {
            Leaderboards leaderboard = new Leaderboards();
            this.Close();
            leaderboard.Show();

        }
    }
}