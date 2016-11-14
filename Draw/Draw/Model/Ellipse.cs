using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Draw.Model
{
    class Ellipse:Shape
    {
        public Ellipse(Point start, Point end, double rotation, Point rotationCenter) : base(start, end, rotation, rotationCenter)
        {
        }
    }
}
