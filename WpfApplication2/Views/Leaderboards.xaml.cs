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
            using (StreamReader inputfile = new StreamReader("leaderboards.txt"))
            {
                while (inputfile.Peek() >= 0)
                {
                    String line = inputfile.ReadLine();
                    loadHS.Add(line);
                    loadHS.Sort();
                }
            }
                    while (loadHS.Capacity < 15)
            {
                loadHS.Add("unavailable");
            }

                    HStextBlock1.Text = loadHS[1];
                    HStextBlock2.Text = loadHS[2];
                    HStextBlock3.Text = loadHS[3];
                    HStextBlock4.Text = loadHS[4];
                    HStextBlock5.Text = loadHS[5];
                    HStextBlock6.Text = loadHS[6];
                    HStextBlock7.Text = loadHS[7];
                    HStextBlock8.Text = loadHS[8];
                    HStextBlock9.Text = loadHS[9];
                    HStextBlock10.Text = loadHS[10];
                    HStextBlock11.Text = loadHS[11];
                    HStextBlock12.Text = loadHS[12];
                    HStextBlock13.Text = loadHS[13];
                    HStextBlock14.Text = loadHS[14];
                    HStextBlock15.Text = loadHS[15];
                
            

        }
        


          
    }
}
