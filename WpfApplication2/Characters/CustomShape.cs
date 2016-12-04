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
    class CustomShape : UIElement, IEqualityComparer<CustomShape>
    {
        public string Name { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public double Health { get; set; }
        public Rectangle shape { get; set; }


        public bool Equals(CustomShape x, CustomShape y)
        {
            if ((x == null || GetType() != x.GetType()) || (y == null || GetType() != y.GetType()))
            {
                return false;
            }

            if (x.Name.Equals(y.Name))
                return true;
            else
                return false;
        }

        public int GetHashCode(CustomShape obj)
        {
            return base.GetHashCode();
        }

    }
    }

