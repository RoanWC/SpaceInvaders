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
            var top15 = 15;
            using (StreamReader inputfile = new StreamReader("leaderboards.txt"))
            {
                while (inputfile.Peek() >= 0)
                {
                    String line = inputfile.ReadLine();
                    loadHS.Add(line);
                    loadHS.Sort();
                }
            }
                    while (loadHS.Capacity < top15)
            {
                loadHS.Add("unavailable");
            }
            var marginTop = 30;

            for (int i = 0; i < top15; i++)
            {
                Label lb = new Label();
                lb.Height = 25;
                lb.Margin = new Thickness(10, marginTop, 0, 0);
                marginTop += 10;
                lb.VerticalAlignment = VerticalAlignment.Center;
                lb.Width = 20;
                lb.Content = i.ToString();
                lb.Foreground = Brushes.Lime;
                lb.FontSize = 25;
                lb.FontFamily = new FontFamily("OCR A Extended");
                LeaderBoardGrid.Children.Add(lb);
            }
            marginTop = 30;
            for (int i = 0; i < top15; i++)
            {
                TextBlock tb = new TextBlock();
                tb.Height = 20;
                tb.Margin = new Thickness(36, marginTop, 0,0);
                marginTop += 30;
                tb.VerticalAlignment = VerticalAlignment.Center;
                tb.Width = 300;
                tb.Text = loadHS[i];
                tb.Foreground = Brushes.Lime;
                tb.FontSize = 20;
                tb.FontFamily = new FontFamily("OCR A Extended");
                LeaderBoardGrid.Children.Add(tb);

            }

            
                    
                
            

        }
        


          
    }
}
