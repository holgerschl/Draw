using Draw.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Draw.Model
{
    class DrawModel
    {
        private readonly List<Shape> _rectangles = new List<Shape>();
        private readonly List<Shape> _lines = new List<Shape>();
        private readonly List<Shape> _ellipses = new List<Shape>();
        private Point startPosition, offset;
        private KeyValuePair<Shape, ShapeType> currentShape;
        private bool shapeDrawingActive = false;
        private Adorner adorner;

        public DrawModel()
        {
        }

        public event EventHandler<ShapeChangedEventArgs> ShapeChanged;

        private void OnShapeChanged(Shape shape, ShapeType type, bool removed)
        {
            EventHandler<ShapeChangedEventArgs> shapeChanged = ShapeChanged;
            if (shapeChanged != null)
            {
                shapeChanged(this, new ShapeChangedEventArgs(shape, type, removed));
            }
        }

        public event EventHandler<ModusChangedEventArgs> ModusChanged;

        private void OnModusChanged(Modus modus)
        {
            EventHandler<ModusChangedEventArgs> modusChanged = ModusChanged;
            if (modusChanged != null)
            {
                modusChanged(this, new ModusChangedEventArgs(modus));
            }
        }

        internal void NewShape(Point position, ShapeType shapeType)
        {
            shapeDrawingActive = true;
            switch (shapeType)
            {
                case ShapeType.Rectangle:
                    startPosition = position;
                    Shape rectangle = new Rectangle(startPosition, startPosition, 0, startPosition);
                    currentShape = new KeyValuePair<Shape, ShapeType>(rectangle, ShapeType.Rectangle);
                    _rectangles.Add(rectangle);
                    OnShapeChanged(rectangle, ShapeType.Rectangle, false);
                    break;
                case ShapeType.Line:
                    startPosition = position;
                    Shape line = new Line(startPosition, startPosition, 0, startPosition);
                    currentShape = new KeyValuePair<Shape, ShapeType>(line, ShapeType.Line);
                    _lines.Add(line);
                    OnShapeChanged(line, ShapeType.Line, false);
                    break;
                case ShapeType.Ellipse:
                    startPosition = position;
                    Shape ellipse = new Ellipse(startPosition, startPosition, 0, startPosition);
                    currentShape = new KeyValuePair<Shape, ShapeType>(ellipse, ShapeType.Ellipse);
                    _ellipses.Add(ellipse);
                    OnShapeChanged(ellipse, ShapeType.Ellipse, false);
                    break;
                default:
                    break;
            }
        }

        internal void DrawShape(Point position)
        {
            if (shapeDrawingActive)
            {
                currentShape.Key.Start = startPosition;
                double shapeCenterX = startPosition.X + currentShape.Key.Area.Width / 2;
                double shapeCenterY = startPosition.Y + currentShape.Key.Area.Height / 2; ;
                if (currentShape.Value == ShapeType.Line)
                {
                    ((Line)currentShape.Key).End2 = position;
                    if (((Line)currentShape.Key).End2.Y < ((Line)currentShape.Key).Start.Y && ((Line)currentShape.Key).End2.X >= ((Line)currentShape.Key).Start.X)
                    {
                        shapeCenterY = startPosition.Y - currentShape.Key.Area.Height / 2;
                    }

                    else if (((Line)currentShape.Key).End2.X < ((Line)currentShape.Key).Start.X && ((Line)currentShape.Key).End2.Y >= ((Line)currentShape.Key).Start.Y)
                    {
                        shapeCenterX = startPosition.X - currentShape.Key.Area.Width / 2;
                    }

                }
                else
                {
                    currentShape.Key.End = position;
                }
                currentShape.Key.RotationCenter = new Point(shapeCenterX, shapeCenterY);
                OnShapeChanged(currentShape.Key, currentShape.Value, false);
            }
        }

        internal void SetShape(Point position)
        {
            DrawShape(position);
            shapeDrawingActive = false;
        }

        internal void SelectShape(Shape shape, Point position)
        {
            if (shape != null)
            {
                if (_rectangles.Contains(shape))
                {
                    currentShape = new KeyValuePair<Shape, ShapeType>(shape, ShapeType.Rectangle);
                }
                if (_lines.Contains(shape))
                {
                    currentShape = new KeyValuePair<Shape, ShapeType>(shape, ShapeType.Line);
                }
                if (_ellipses.Contains(shape))
                {
                    currentShape = new KeyValuePair<Shape, ShapeType>(shape, ShapeType.Ellipse);
                }
                startPosition = shape.Start;
                shapeDrawingActive = true;

                offset = new Point(position.X - startPosition.X, position.Y - startPosition.Y);
            }
            else
                shapeDrawingActive = false;
        }

        internal void RemoveAdorner()
        {
            if (adorner != null)
            {
                OnShapeChanged(adorner, ShapeType.Adorner, true);
                adorner = null;
            }
        }

        internal void DropShape(Point position)
        {
            if (shapeDrawingActive)
            {
                MoveAdorner(position);
                shapeDrawingActive = false;
            }
        }

        private void MoveAdorner(Point position)
        {

            double adornerTopLeftX = currentShape.Key.Start.X - DrawHelper.adornerThickness;
            double adornerTopLeftY = currentShape.Key.Start.Y - DrawHelper.turnHandleDistance;
            double adornerBottomRightX = currentShape.Key.End.X + DrawHelper.adornerThickness;
            double adornerBottomRightY = currentShape.Key.End.Y + DrawHelper.adornerThickness;
            double adornerCenterX = currentShape.Key.RotationCenter.X + DrawHelper.adornerThickness;
            double adornerCenterY = currentShape.Key.RotationCenter.Y + DrawHelper.turnHandleDistance;
            if (currentShape.Value == ShapeType.Line)
            {
                if (currentShape.Key.Start.Y > currentShape.Key.End.Y && currentShape.Key.Start.X <= currentShape.Key.End.X)
                {
                    adornerTopLeftY = currentShape.Key.End.Y - DrawHelper.turnHandleDistance;
                    adornerBottomRightY = currentShape.Key.Start.Y + DrawHelper.adornerThickness;
                }
                else if (currentShape.Key.Start.X > currentShape.Key.End.X && currentShape.Key.Start.Y <= currentShape.Key.End.Y)
                {
                    adornerTopLeftX = currentShape.Key.End.X - DrawHelper.adornerThickness;
                    adornerBottomRightX = currentShape.Key.Start.X + DrawHelper.adornerThickness;
                }
            }
            if (adorner == null)
            {
                adorner = new Adorner(new Point(adornerTopLeftX, adornerTopLeftY),
                        new Point(adornerBottomRightX, adornerBottomRightY), 0,
                        new Point(adornerCenterX, adornerCenterY));
            }
            else
            {
                adorner.Start = new Point(adornerTopLeftX, adornerTopLeftY);
                adorner.End = new Point(adornerBottomRightX, adornerBottomRightY);
                adorner.RotationCenter = new Point(adornerCenterX, adornerCenterY);
            }
            adorner.Rotation = currentShape.Key.Rotation;
            OnShapeChanged(adorner, ShapeType.Adorner, false);
        }

        internal void AdornerResize(Modus direction, Point position)
        {

        }

        internal void MoveShape(Point position)
        {
            if (shapeDrawingActive)
            {
                double shapeTopLeftX = position.X - offset.X;
                double shapeTopLeftY = position.Y - offset.Y;
                double shapeBottomRightX = position.X + currentShape.Key.Area.Width - offset.X;
                double shapeBottomRightY = position.Y + currentShape.Key.Area.Height - offset.Y;
                double shapeCenterX = position.X + currentShape.Key.Area.Width / 2 - offset.X;
                double shapeCenterY = position.Y + currentShape.Key.Area.Height / 2 - offset.Y;
                if (currentShape.Value == ShapeType.Line)
                {
                    if (((Line)currentShape.Key).End2.Y < ((Line)currentShape.Key).Start.Y && ((Line)currentShape.Key).End2.X >= ((Line)currentShape.Key).Start.X)
                    {
                        shapeBottomRightY = position.Y - offset.Y - currentShape.Key.Area.Height;
                        shapeCenterY = position.Y - offset.Y - currentShape.Key.Area.Height / 2;
                    }

                    else if (((Line)currentShape.Key).End2.X < ((Line)currentShape.Key).Start.X && ((Line)currentShape.Key).End2.Y >= ((Line)currentShape.Key).Start.Y)
                    {
                        shapeBottomRightX = position.X - offset.X - currentShape.Key.Area.Width;
                        shapeCenterX = position.X - offset.X - currentShape.Key.Area.Width / 2;
                    }
                    ((Line)currentShape.Key).End2 = new Point(shapeBottomRightX, shapeBottomRightY);
                }
                else
                    currentShape.Key.End = new Point(shapeBottomRightX, shapeBottomRightY);
                currentShape.Key.Start = new Point(shapeTopLeftX, shapeTopLeftY);
                currentShape.Key.RotationCenter = new Point(shapeCenterX, shapeCenterY);
                OnShapeChanged(currentShape.Key, currentShape.Value, false);
                MoveAdorner(position);
            }
        }

        internal void SetProjectedWidthAndHeight(Point position)
        {
            offset = new Point(position.X - startPosition.X, position.Y - startPosition.Y);
        }

        internal void ResizeShape(Point position, Modus modus)
        {
            switch (modus)
            {
                case Modus.TopLeft:
                    if (position.X < currentShape.Key.End.X && position.Y < currentShape.Key.End.Y)
                        ResizeTopLeft(position);
                    else if (position.X >= currentShape.Key.End.X && position.Y >= currentShape.Key.End.Y)
                        modus = Modus.BottomRight;
                    else if (position.X >= currentShape.Key.End.X && position.Y < currentShape.Key.End.Y)
                        modus = Modus.BottomLeft;
                    else if (position.X < currentShape.Key.End.X && position.Y >= currentShape.Key.End.Y)
                        modus = Modus.TopRight;
                    OnModusChanged(modus);
                    break;
                case Modus.BottomRight:
                    if (position.X > currentShape.Key.Start.X && position.Y > currentShape.Key.Start.Y)
                        ResizeBottomRight(position);
                    else if (position.X <= currentShape.Key.Start.X && position.Y <= currentShape.Key.Start.Y)
                        modus = Modus.TopLeft;
                    else if (position.X <= currentShape.Key.Start.X && position.Y > currentShape.Key.Start.Y)
                        modus = Modus.BottomLeft;
                    else if (position.X > currentShape.Key.Start.X && position.Y <= currentShape.Key.Start.Y)
                        modus = Modus.TopRight;
                    OnModusChanged(modus);
                    break;
                case Modus.TopRight:
                    if (position.X > currentShape.Key.Start.X && position.Y < currentShape.Key.End.Y)
                        ResizeTopRight(position);
                    else if (position.X <= currentShape.Key.Start.X && position.Y >= currentShape.Key.End.Y)
                        modus = Modus.BottomLeft;
                    else if (position.X > currentShape.Key.Start.X && position.Y >= currentShape.Key.End.Y)
                        modus = Modus.BottomRight;
                    else if (position.X <= currentShape.Key.Start.X && position.Y < currentShape.Key.End.Y)
                        modus = Modus.TopLeft;
                    OnModusChanged(modus);
                    break;
                case Modus.BottomLeft:
                    if (position.X < currentShape.Key.End.X && position.Y > currentShape.Key.Start.Y)
                        ResizeBottomLeft(position);
                    else if (position.X >= currentShape.Key.End.X && position.Y <= currentShape.Key.Start.Y)
                        modus = Modus.TopRight;
                    else if (position.X < currentShape.Key.End.X && position.Y <= currentShape.Key.Start.Y)
                        modus = Modus.TopLeft;
                    else if (position.X >= currentShape.Key.End.X && position.Y > currentShape.Key.Start.Y)
                        modus = Modus.BottomRight;
                    OnModusChanged(modus);
                    break;
                case Modus.TopCenter:
                    if (position.Y < currentShape.Key.End.Y)
                        ResizeTopCenter(position);
                    else if (position.Y >= currentShape.Key.End.Y)
                        modus = Modus.BottomCenter;
                    OnModusChanged(modus);
                    break;
                case Modus.BottomCenter:
                    if (position.Y > currentShape.Key.Start.Y)
                        ResizeBottomCenter(position);
                    else if (position.Y <= currentShape.Key.Start.Y)
                        modus = Modus.TopCenter;
                    OnModusChanged(modus);
                    break;
                case Modus.CenterLeft:
                    if (position.X < currentShape.Key.End.X)
                        ResizeCenterLeft(position);
                    else if (position.X >= currentShape.Key.End.X)
                        modus = Modus.CenterRight;
                    OnModusChanged(modus);
                    break;
                case Modus.CenterRight:
                    if (position.X > currentShape.Key.Start.X)
                        ResizeCenterRight(position);
                    else if (position.X <= currentShape.Key.Start.X)
                        modus = Modus.CenterLeft;
                    OnModusChanged(modus);
                    break;
            }
            MoveAdorner(position);
        }

        private void ResizeCenterRight(Point position)
        {
            double positionCenterRightX = position.X;
            currentShape.Key.End = new Point(positionCenterRightX, currentShape.Key.End.Y);
            //currentShape.Key.RotationCenter = new Point(currentShape.Key.Start.X + currentShape.Key.Area.Width / 2, currentShape.Key.Start.Y + currentShape.Key.Area.Height / 2);
            OnShapeChanged(currentShape.Key, currentShape.Value, false);
        }

        private void ResizeCenterLeft(Point position)
        {
            double positionCenterLeftX = position.X;
            currentShape.Key.Start = new Point(positionCenterLeftX, currentShape.Key.Start.Y);
            //currentShape.Key.RotationCenter = new Point(currentShape.Key.Start.X + currentShape.Key.Area.Width / 2, currentShape.Key.Start.Y + currentShape.Key.Area.Height / 2);
            OnShapeChanged(currentShape.Key, currentShape.Value, false);
        }

        private void ResizeBottomCenter(Point position)
        {
            double positionBottomCenterY = position.Y;
            currentShape.Key.End = new Point(currentShape.Key.End.X, positionBottomCenterY);
            //currentShape.Key.RotationCenter = new Point(currentShape.Key.Start.X + currentShape.Key.Area.Width / 2, currentShape.Key.Start.Y + currentShape.Key.Area.Height / 2);
            OnShapeChanged(currentShape.Key, currentShape.Value, false);
        }

        private void ResizeTopCenter(Point position)
        {
            double positionTopCenterY = position.Y;
            currentShape.Key.Start = new Point(currentShape.Key.Start.X, positionTopCenterY);
            //currentShape.Key.RotationCenter = new Point(currentShape.Key.Start.X + currentShape.Key.Area.Width / 2, currentShape.Key.Start.Y + currentShape.Key.Area.Height / 2);
            OnShapeChanged(currentShape.Key, currentShape.Value, false);
        }

        private void ResizeBottomLeft(Point position)
        {
            double positionBottomLeftX = position.X;
            double positionBottomLeftY = position.Y;
            currentShape.Key.Start = new Point(positionBottomLeftX, currentShape.Key.Start.Y);
            currentShape.Key.End = new Point(currentShape.Key.End.X, positionBottomLeftY);
            //currentShape.Key.RotationCenter = new Point(currentShape.Key.Start.X + currentShape.Key.Area.Width / 2, currentShape.Key.Start.Y + currentShape.Key.Area.Height / 2);
            OnShapeChanged(currentShape.Key, currentShape.Value, false);
        }

        private void ResizeTopRight(Point position)
        {
            double positionTopRightX = position.X;
            double positionTopRightY = position.Y;
            currentShape.Key.Start = new Point(currentShape.Key.Start.X, positionTopRightY);
            currentShape.Key.End = new Point(positionTopRightX, currentShape.Key.End.Y);
            //currentShape.Key.RotationCenter = new Point(currentShape.Key.Start.X + currentShape.Key.Area.Width / 2, currentShape.Key.Start.Y + currentShape.Key.Area.Height / 2);
            OnShapeChanged(currentShape.Key, currentShape.Value, false);
        }

        private void ResizeBottomRight(Point position)
        {
            double currentShapeWidth = currentShape.Key.Area.Width;
            double currentShapeHeight = currentShape.Key.Area.Height;
            double positionBottomRightX = position.X;
            double positionBottomRightY = position.Y;
            currentShape.Key.End = new Point(positionBottomRightX, positionBottomRightY);
            //currentShape.Key.RotationCenter = new Point(currentShape.Key.Start.X + currentShape.Key.Area.Width / 2, currentShape.Key.Start.Y + currentShape.Key.Area.Height / 2);
            OnShapeChanged(currentShape.Key, currentShape.Value, false);
        }


        private void ResizeTopLeft(Point position)
        {
            double currentShapeWidth = currentShape.Key.Area.Width;
            double currentShapeHeight = currentShape.Key.Area.Height;
            double positionTopLeftX = position.X - (
                (currentShape.Key.Area.Width / 2) * (1 - Math.Cos(Math.PI / 180 * currentShape.Key.Rotation))
                + (currentShape.Key.Area.Height / 2) * Math.Sin(Math.PI / 180 * currentShape.Key.Rotation));
            double positionTopLeftY = position.Y + (
                (currentShape.Key.Area.Width / 2) * Math.Sin(Math.PI / 180 * currentShape.Key.Rotation)
                - (currentShape.Key.Area.Height / 2) * (1 - Math.Cos(Math.PI / 180 * currentShape.Key.Rotation)));
            double angle = currentShape.Key.Rotation;
            currentShape.Key.Start = new Point(positionTopLeftX, positionTopLeftY);
            currentShape.Key.End = new Point(positionTopLeftX + currentShape.Key.Area.Width
                - ((currentShapeHeight - currentShape.Key.Area.Height) / 2 * Math.Sin(Math.PI / 180 * currentShape.Key.Rotation))
                , positionTopLeftY + currentShape.Key.Area.Height
                + ((currentShapeWidth - currentShape.Key.Area.Width) / 2 * Math.Sin(Math.PI / 180 * currentShape.Key.Rotation))
                );
            currentShape.Key.RotationCenter = new Point(currentShape.Key.Start.X + currentShape.Key.Area.Width / 2, currentShape.Key.Start.Y + currentShape.Key.Area.Height / 2);
            OnShapeChanged(currentShape.Key, currentShape.Value, false);
        }

        internal void TurnShape(Point position)
        {
            Point turnCenter = currentShape.Key.RotationCenter;
            double angle = 180 / Math.PI * Math.Atan2(position.Y - turnCenter.Y, position.X - turnCenter.X) + 90;
            currentShape.Key.Rotation = angle;
            OnShapeChanged(currentShape.Key, currentShape.Value, false);
            MoveAdorner(position);
        }

    }
}
