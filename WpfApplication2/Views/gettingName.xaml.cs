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
        public gettingName()
        {
            InitializeComponent();
        }

        private void enter(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space)
            {
                textBox.Text = textBox.Text
            }
        }
    }
}
