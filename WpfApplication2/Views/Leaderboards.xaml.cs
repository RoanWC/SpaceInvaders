﻿using System;
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
using WpfApplication2.Views;

namespace WpfApplication2.Views
{
    /// <summary>
    /// Interaction logic for Leaderboards.xaml
    /// </summary>
    public partial class Leaderboards : Window
    {
        public Leaderboards()
        {
            InitializeComponent();
        }
        gettingName gettingName = new gettingName();   
    }
}