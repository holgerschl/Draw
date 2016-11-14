using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Draw.Model
{
    class Shape
    {
        internal Point end;
        internal Point start;
        public virtual Point End
        {
            get
            {
                return end;
            }

            set
            {
                end = value;
                if (value.X < start.X && value.Y >= start.Y)
                {
                    Point temp = start;
                    start = new Point(value.X, start.Y);
                    end = new Point(temp.X, value.Y);
                }
                if (value.Y < start.Y && value.X >= start.X)
                {
                    Point temp = start;
                    start = new Point(start.X, value.Y);
                    end = new Point(value.X, temp.Y);
                }
                if (value.X < start.X && value.Y < start.Y)
                {
                    Point temp = start;
                    start = value;
                    end = temp;
                }
            }
        }

        public Point Start
        {
            get
            {
                return start;
            }

            set
            {
                start = value;
            }
        }

        public double Rotation { get; set; }
        public Point RotationCenter { get; set; }

        public Shape(Point start, Point end, double rotation, Point rotationCenter)
        {
            this.start = start;
            this.end = end;
            Rotation = rotation;
            RotationCenter = rotationCenter;
        }
        public Rect Area
        {
            get { return new Rect(start, end); }
        }

    }
}
