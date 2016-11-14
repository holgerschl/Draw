using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Draw.Model
{
    class Line : Shape
    {
        public Point End2 { get { return end; } set { end = value; } }
        public Line(Point start, Point end, double rotation, Point rotationCenter) : base(start, end, rotation, rotationCenter)
        {
        }
    }
}
