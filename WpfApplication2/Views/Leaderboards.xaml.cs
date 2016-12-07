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
                    while (loadHS.Capacity <= top15)
            {
                loadHS.Add("unavailable");
            }

            Grid LeaderBoardGrid = new Grid();
            LeaderBoardGrid.Width = 657;
            LeaderBoardGrid.HorizontalAlignment = HorizontalAlignment.Left;
            LeaderBoardGrid.VerticalAlignment = VerticalAlignment.Top;
            LeaderBoardGrid.ShowGridLines = false;
            LeaderBoardGrid.Background = new SolidColorBrush(Colors.Transparent);

            ColumnDefinition gridCol1 = new ColumnDefinition();
            ColumnDefinition gridCol2 = new ColumnDefinition();
            gridCol1.Width = new GridLength(25, GridUnitType.Star);
            gridCol2.Width = new GridLength(75, GridUnitType.Star);
            LeaderBoardGrid.ColumnDefinitions.Add(gridCol1);
            LeaderBoardGrid.ColumnDefinitions.Add(gridCol2);

            
            // Add first column header
            TextBlock Names = new TextBlock();
            Names.Text = "Names";
            Names.Foreground = Brushes.Lime;
            Names.FontSize = 30;
            Names.FontFamily = new FontFamily("OCR A Extended");
            Names.VerticalAlignment = VerticalAlignment.Top;
            Names.HorizontalAlignment = HorizontalAlignment.Left;
            Grid.SetRow(Names, 0);
            Grid.SetColumn(Names, 0);

            // Add second column header
            TextBlock HighScores = new TextBlock();
            HighScores.Text = "HighScores";
            HighScores.Foreground = Brushes.Lime;
            HighScores.FontSize = 30;
            HighScores.FontFamily = new FontFamily("OCR A Extended");
            HighScores.VerticalAlignment = VerticalAlignment.Top;
            HighScores.HorizontalAlignment = HorizontalAlignment.Left;
            Grid.SetRow(HighScores, 0);
            Grid.SetColumn(HighScores, 1);
            // add column headers to grid
            LeaderBoardGrid.Children.Add(Names);
            LeaderBoardGrid.Children.Add(HighScores);
            
            for (int i = 1; i < 16; i++)
            {
                RowDefinition gridRow1 = new RowDefinition();
                
                LeaderBoardGrid.RowDefinitions.Add(gridRow1);
                TextBlock names = new TextBlock();
                names.Height = 20;
                names.VerticalAlignment = VerticalAlignment.Center;
                names.HorizontalAlignment = HorizontalAlignment.Left;
                names.Text = (i).ToString();
                names.Foreground = Brushes.Lime;
                names.FontSize = 20;
                names.FontFamily = new FontFamily("OCR A Extended");
                Grid.SetRow(names, i);
                Grid.SetColumn(names, 0);
            LeaderBoardGrid.Children.Add(names);

                TextBlock highscores = new TextBlock();
                highscores.Height = 20;
                highscores.VerticalAlignment = VerticalAlignment.Center;
                highscores.HorizontalAlignment = HorizontalAlignment.Left;
                highscores.Text = loadHS[i-1];
                highscores.Foreground = Brushes.Lime;
                highscores.FontSize = 20;
                highscores.FontFamily = new FontFamily("OCR A Extended");
                Grid.SetRow(highscores, i);
                Grid.SetColumn(highscores, 1);
                    
                LeaderBoardGrid.Children.Add(highscores);

                
            }

            LeaderBoardWindow.Content =LeaderBoardGrid;
            
                    
                
            

        }
        


          
    }
}
