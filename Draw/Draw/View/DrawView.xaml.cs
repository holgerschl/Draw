using Draw.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Draw.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DrawView : Page
    {
        private ShapeType shapeType;

        public DrawView()
        {
            this.InitializeComponent();
        }

        private void line_Click(object sender, RoutedEventArgs e)
        {
            shapeType = ShapeType.Line;
            line.BorderBrush = new SolidColorBrush(Colors.Yellow);
            rectangle.BorderBrush = new SolidColorBrush(Colors.LightGray);
            circle.BorderBrush = new SolidColorBrush(Colors.LightGray);
            select.BorderBrush = new SolidColorBrush(Colors.LightGray);
            viewModel.RemoveAdorner();
        }

        private void rectangle_Click(object sender, RoutedEventArgs e)
        {
            shapeType = ShapeType.Rectangle;
            line.BorderBrush = new SolidColorBrush(Colors.LightGray);
            rectangle.BorderBrush = new SolidColorBrush(Colors.Yellow);
            circle.BorderBrush = new SolidColorBrush(Colors.LightGray);
            select.BorderBrush = new SolidColorBrush(Colors.LightGray);
            viewModel.RemoveAdorner();
        }

        private void circle_Click(object sender, RoutedEventArgs e)
        {
            shapeType = ShapeType.Ellipse;
            line.BorderBrush = new SolidColorBrush(Colors.LightGray);
            rectangle.BorderBrush = new SolidColorBrush(Colors.LightGray);
            circle.BorderBrush = new SolidColorBrush(Colors.Yellow);
            select.BorderBrush = new SolidColorBrush(Colors.LightGray);
            viewModel.RemoveAdorner();
        }

        private void select_Click(object sender, RoutedEventArgs e)
        {
            shapeType = ShapeType.Selector;
            line.BorderBrush = new SolidColorBrush(Colors.LightGray);
            rectangle.BorderBrush = new SolidColorBrush(Colors.LightGray);
            circle.BorderBrush = new SolidColorBrush(Colors.LightGray);
            select.BorderBrush = new SolidColorBrush(Colors.Yellow);
        }

        private void ItemsControl_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (shapeType == ShapeType.Selector)
            {
                viewModel.SelectShape(e, e.GetCurrentPoint(itemsControl).Position);
            }
            else
                viewModel.NewShape(e.GetCurrentPoint(itemsControl).Position, shapeType);
        }

        private void ItemsControl_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (shapeType == ShapeType.Selector)
            {
                viewModel.MoveShape( e, e.GetCurrentPoint(itemsControl).Position);
            }
            else

                viewModel.DrawShape( e.GetCurrentPoint(itemsControl).Position);
        }

        private void ItemsControl_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (shapeType == ShapeType.Selector)
            {
                viewModel.DropShape(e.GetCurrentPoint(itemsControl).Position);
            }
            else
                viewModel.SetShape( e.GetCurrentPoint(itemsControl).Position);
        }
    }
}
