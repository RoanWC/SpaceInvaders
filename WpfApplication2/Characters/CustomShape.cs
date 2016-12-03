using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApplication2
{
    class CustomShape : UIElement
    {
        private string Name { get; set; }
        public double PositionX { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double PositionY { get; set; }
        public double Health { get; set; }
        public Rectangle shape { get; set; }


    }
    }

