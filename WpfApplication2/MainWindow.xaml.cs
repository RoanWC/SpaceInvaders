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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
    
        private double _x;
        private double _y;
        private System.Windows.Threading.DispatcherTimer timer;
        
        Ellipse ellipse = null;

   

        public MainWindow()
        {
            InitializeComponent();

            //Initialize the timer class
            _x = 50;
            _y = 50;
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); //Set the interval period here.
            ellipse = CreateAnEllipse(10, 10);
            timer.Tick += timer1_Tick;
        }

        

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer.Start();
            _x += 10;
            PaintCanvas.Children.Add(ellipse);

            
        }

        // Customize your ellipse in this method
        public Ellipse CreateAnEllipse(int height, int width)
        {
            SolidColorBrush fillBrush = new SolidColorBrush() { Color = Colors.Red };
            SolidColorBrush borderBrush = new SolidColorBrush() { Color = Colors.Black };

            return new Ellipse()
            {

                Height = height,
                Width = width,
                StrokeThickness = 1,
                Stroke = borderBrush,
                Fill = fillBrush
            };
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
          
            if (e.Key == Key.Left)
            {
                Canvas.SetLeft(ellipse, _x+=10);
                
            }
            if (e.Key == Key.Right)
            {
                Canvas.SetRight(ellipse, _x -= 10);
            }
        }
    }
}
