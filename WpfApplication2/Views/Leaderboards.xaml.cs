using SpaceInvaders;
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
                    if (!line.Equals(""))
                        loadHS.Add(line);
                    
                }
            }
                    while (loadHS.Capacity <= top15)
            {
                loadHS.Add("unavailable");
            }
            sortScores(loadHS);

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
            Names.Text = "Rank";
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
                RowDefinition row = new RowDefinition();
                
                LeaderBoardGrid.RowDefinitions.Add(row);
                TextBlock rank = new TextBlock();
                rank.Height = 20;
                rank.VerticalAlignment = VerticalAlignment.Center;
                rank.HorizontalAlignment = HorizontalAlignment.Left;
                if (i > 14)
                    rank.Text = "15";
                else
                    rank.Text = i.ToString();
               
                rank.Foreground = Brushes.Lime;
                rank.FontSize = 20;
                rank.FontFamily = new FontFamily("OCR A Extended");
                Grid.SetRow(rank, i);
                Grid.SetColumn(rank, 0);
                                 
                TextBlock highscores = new TextBlock();
                highscores.Height = 20;
                highscores.VerticalAlignment = VerticalAlignment.Center;
                highscores.HorizontalAlignment = HorizontalAlignment.Left;
                if (i > 14)
                    highscores.Text = loadHS[14];
                else
                    highscores.Text = loadHS[i-1];
                highscores.Foreground = Brushes.Lime;
                highscores.FontSize = 20;
                highscores.FontFamily = new FontFamily("OCR A Extended");
                Grid.SetRow(highscores, i);
                Grid.SetColumn(highscores, 1);
                LeaderBoardGrid.Children.Add(rank);
                LeaderBoardGrid.Children.Add(highscores);

                
            }

            RowDefinition menuRow1 = new RowDefinition();
            RowDefinition menuRow2 = new RowDefinition();
            LeaderBoardGrid.RowDefinitions.Add(menuRow1);
            LeaderBoardGrid.RowDefinitions.Add(menuRow2);
            Button BackToMainMenuBtn = new Button();
            BackToMainMenuBtn.Height = 40;
            BackToMainMenuBtn.Width = 100;
            BackToMainMenuBtn.Click += BackToMainMenuBtn_Click;
            BackToMainMenuBtn.Background = Brushes.Transparent;
            BackToMainMenuBtn.VerticalAlignment = VerticalAlignment.Center;
            BackToMainMenuBtn.HorizontalAlignment = HorizontalAlignment.Left;
            BackToMainMenuBtn.Content = "MainMenu";
            BackToMainMenuBtn.Foreground = Brushes.Lime;
            BackToMainMenuBtn.FontSize = 20;
            BackToMainMenuBtn.SetValue(Grid.RowProperty, 16);
            BackToMainMenuBtn.SetValue(Grid.ColumnProperty, 0);
            BackToMainMenuBtn.FontFamily = new FontFamily("OCR A Extended");
            Grid.SetRow(BackToMainMenuBtn, 18);
            Grid.SetColumn(BackToMainMenuBtn, 0);
            LeaderBoardGrid.Children.Add(BackToMainMenuBtn);
            LeaderBoardWindow.Content =LeaderBoardGrid;
            
        }

        private void BackToMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        private static void sortScores(List<String> list)
        {
            if (list == null)
                throw new ArgumentNullException("List is null for " + list.ToString());
            
            int biggest;
            for (int i = 0; i < list.Count; i++)
            {
                biggest = i; 
                

                for (int j = i + 1; j < list.Count; j++)
                {
                    var current = list[j].Substring(list[j].LastIndexOf(" ") + 1);
                    var biggestscore = list[biggest].Substring(list[biggest].LastIndexOf(" ") + 1);
                    if (int.Parse(current).CompareTo(int.Parse(biggestscore)) >0)
                    {
                        biggest = j;
                    }
                  
                }
                if (biggest != i)
                    swap(list, biggest, i);
            }
        }

        static void swap(List<String>array, int index1, int index2)
        {
            String temp = array[index1];
            array[index1] = array[index2];
            array[index2] = temp;
        }

        private static Boolean isNullElement(String[] list)
        {

            for (int i = 0; i < list.Length; i++)
                if (list[i] == null)
                    return false;
            return true;
        }

    }      
    
}
