using Draw.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Draw.View
{
    class DrawHelper
    {
        internal const int turnHandleDistance = 31;
        internal const int adornerThickness = 6;
        internal static double turnEllipseDiameter = 10;

        internal static FrameworkElement RectangleControlFactory(Model.Rectangle rectangle)
        {
            Windows.UI.Xaml.Shapes.Rectangle rectangleControl = new Windows.UI.Xaml.Shapes.Rectangle();
            rectangleControl.Stroke = new SolidColorBrush(Colors.Black);
            rectangleControl.Fill = new SolidColorBrush(Colors.Transparent);
            rectangleControl.Width = rectangle.Area.Width;
            rectangleControl.Height = rectangle.Area.Height;
            SetCanvasLocation(rectangleControl, rectangle.Start.X , rectangle.Start.Y ,100);
            return rectangleControl;
        }

        internal static FrameworkElement LineControlFactory(Model.Line line)
        {
            Windows.UI.Xaml.Shapes.Line lineControl = new Windows.UI.Xaml.Shapes.Line();
            lineControl.Stroke = new SolidColorBrush(Colors.Black);
            lineControl.X1 = line.Start.X;
            lineControl.Y1 = line.Start.Y;
            lineControl.X2 = line.End2.X;
            lineControl.Y2 = line.End2.Y;
            return lineControl;
        }


        internal static FrameworkElement EllipseControlFactory(Model.Ellipse ellipse)
        {
            Windows.UI.Xaml.Shapes.Ellipse ellipseControl = new Windows.UI.Xaml.Shapes.Ellipse();
            ellipseControl.Stroke = new SolidColorBrush(Colors.Black);
            ellipseControl.Fill = new SolidColorBrush(Colors.Transparent);
            ellipseControl.Width = ellipse.Area.Width;
            ellipseControl.Height = ellipse.Area.Height;
            SetCanvasLocation(ellipseControl, ellipse.Start.X, ellipse.Start.Y,100);
            return ellipseControl;
        }

        internal static FrameworkElement AdornerControlFactory(Model.Adorner adorner)
        {
            View.Adorner adornerControl = new View.Adorner();
            adornerControl.Width = adorner.Area.Width;
            adornerControl.Height = adorner.Area.Height;
            SetCanvasLocation(adornerControl, adorner.Start.X, adorner.Start.Y,1000);
            return adornerControl;
        }

        public static void SetCanvasLocation(FrameworkElement control, double x, double y, int z)
        {
            Canvas.SetLeft(control, x);
            Canvas.SetTop(control, y);
            Canvas.SetZIndex(control, z);
        }
    }
}
