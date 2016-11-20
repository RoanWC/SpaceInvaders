using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApplication2
{
    class Enemy : Shape
    {
        private new string Name { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        private double Health { get; set; }
        private double size;

        public double Size
        {
            get { return (double)this.GetValue(SizeProperty); }
            set { this.SetValue(SizeProperty, value); }
        }
        public static readonly DependencyProperty SizeProperty = DependencyProperty.
            Register("Size", typeof(Double), typeof(Enemy));
        protected override Geometry DefiningGeometry
        {
            get
            {

                Point p1 = new Point(0.0d, 0.0d);
                Point p2 = new Point(this.Size, 0.0d);
                Point p3 = new Point(this.Size / 2, -this.Size);

                List<PathSegment> segments = new List<PathSegment>(3);
                segments.Add(new LineSegment(p1, true));
                segments.Add(new LineSegment(p2, true));
                segments.Add(new LineSegment(p3, true));

                List<PathFigure> figures = new List<PathFigure>(1);
                PathFigure pf = new PathFigure(p1, segments, true);
                figures.Add(pf);

                Geometry g = new PathGeometry(figures, FillRule.EvenOdd, null);

                return g;
            }
        }
    }
}
