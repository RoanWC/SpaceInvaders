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
using WpfApplication2.Views;

namespace WpfApplication2.Views
{
    /// <summary>
    /// Interaction logic for Leaderboards.xaml
    /// </summary>
    public partial class Leaderboards : Window
    {
        List<String> loadHS = new List<string>();
        string filename = "leaderboards.txt";
        


        public Leaderboards()
        {
            InitializeComponent();
            using(StreamReader inputfile = new StreamReader("leaderboards.txt"))
            {
                while (inputfile.Peek() >= 0)
                {
                    String line = inputfile.ReadLine();
                    loadHS.Add(line);
                }
            }

        }
        


          
    }
}
