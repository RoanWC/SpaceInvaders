using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using SpaceInvaders;

namespace WpfApplication2.Views
{
    /// <summary>
    /// Interaction logic for gettingName.xaml
    /// </summary>
    public partial class GameOverWindow : Window
    {
        List<String> people = new List<string>();
        String playerName;
        String playerNameAndScore;
        string filename = "leaderboards.txt";
        int killCount;
        private StreamWriter fileWriter;


        public GameOverWindow(int kills)
        {
            this.killCount = kills;
            InitializeComponent();
        }



        private void enter(object sender, KeyEventArgs e)
        {
        
            String filename = "leaderboards.txt";
            
           

          playerName = getNameTextbox.Text;
            playerNameAndScore = killCount + playerName;
            people.Add(playerNameAndScore);
            people.Sort();
        }

        private void saveHighscore_Click(object sender, RoutedEventArgs e)
        {

            using (StreamWriter outputFile = new StreamWriter(filename))
            {
                playerNameAndScore = playerName + " " + killCount; 
                outputFile.WriteLine(playerNameAndScore);
            }
        }
    }
}
