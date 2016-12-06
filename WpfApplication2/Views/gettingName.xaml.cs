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
using System.Windows.Shapes;

namespace WpfApplication2.Views
{
    /// <summary>
    /// Interaction logic for gettingName.xaml
    /// </summary>
    public partial class gettingName : Window
    {
        List<String> people = new List<string>();
        String playerName;
        String playerNameAndScore;
        string filename = "leaderboards.txt";
        int killCount;


        public gettingName()
        {
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

    }
}
