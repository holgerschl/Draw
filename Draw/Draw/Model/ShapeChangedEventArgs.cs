using System;

namespace Draw.Model
{
    class ShapeChangedEventArgs: EventArgs
    {
        public Shape Shape { get; private set; }
        public ShapeType Type { get; private set; }
        public bool Removed { get; private set; }

        public ShapeChangedEventArgs(Shape shape, ShapeType type, bool removed)
        {
            Shape = shape;
            Type = type;
            Removed = removed;
        }
    }
}