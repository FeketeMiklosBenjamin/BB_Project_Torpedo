using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Torpedo_Project
{
    public class Ship
    {
        public int width { get; set; }
        public int height { get; set; }
        public int left { get; set; }
        public int top { get; set; }
        public string name { get; set; }
        //public string imgSource { get; set; }

        public static Canvas canvas;
        public static Grid MyGrid;

        private Rectangle ship;
        private double? canvasLeft;
        private double? canvasTop;
        private double shipLeft;
        private double shipTop;

        private bool _isRectDragInProg;

        public Ship(int width, int height, int left, int top, string name)
        {
            shipLeft = left;
            shipTop = top;
            ship = new Rectangle();
            ship.Stroke = new SolidColorBrush(Colors.Black);
            ship.Fill = new SolidColorBrush(Colors.Violet);
            ship.Width = width;
            ship.Height = height;
            ship.Name = name;
            ship.AddHandler(Rectangle.PreviewMouseDownEvent, new MouseButtonEventHandler(ship_MouseLeftButtonDown));
            ship.AddHandler(Rectangle.PreviewMouseUpEvent, new MouseButtonEventHandler(ship_MouseLeftButtonUp));
            ship.AddHandler(Rectangle.PreviewMouseMoveEvent, new MouseEventHandler(ship_MouseMove));
            Canvas.SetLeft(ship, left);
            Canvas.SetTop(ship, top);
            Canvas.SetZIndex(ship, 1);
            canvas.Children.Add(ship);

        }

        private void ship_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isRectDragInProg = true;
            ship.CaptureMouse();
        }


        private void ship_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            canvasTop = Canvas.GetTop(ship);
            canvasLeft = Canvas.GetLeft(ship);
            if (canvasLeft < 236 || canvasLeft > 550 || canvasLeft == null || canvasTop < 0 || canvasTop > 310 || canvasTop == null)
            {
                Canvas.SetTop(ship, shipTop);
                Canvas.SetLeft(ship, shipLeft);
            }
            _isRectDragInProg = false;
            ship.ReleaseMouseCapture();
        }


        private void ship_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isRectDragInProg) return;

            // get the position of the mouse relative to the Canvas
            var mousePos = e.GetPosition(canvas);

            // center the rect on the mouse
            double left = mousePos.X - (ship.ActualWidth / 2);
            double top = mousePos.Y - (ship.ActualHeight / 2);
            Canvas.SetLeft(ship, left);
            Canvas.SetTop(ship, top);
        }

        //private void ship_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (_buttonPosition == null)
        //        _buttonPosition = (sender as Rectangle).TransformToAncestor(MyGrid).Transform(new System.Windows.Point(0, 0));
        //    var mousePosition = Mouse.GetPosition(MyGrid);
        //    deltaX = mousePosition.X - _buttonPosition.Value.X;
        //    deltaY = mousePosition.Y - _buttonPosition.Value.Y;
        //    _isMoving = true;

        //}

        //private void ship_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        //{
        //    canvasLeft = Canvas.GetLeft(ship);
        //    canvasTop = Canvas.GetTop(ship);
        //    if (canvasLeft == null || canvasTop == null)
        //    {
        //        canvasLeft = -1;
        //        canvasTop = -1;
        //    }
        //    _currentTT = (sender as Rectangle).RenderTransform as TranslateTransform;
        //    _isMoving = false;

        //}

        //private void ship_PreviewMouseMove(object sender, MouseEventArgs e)
        //{
        //    if (!_isMoving) return;


        //    var mousePoint = Mouse.GetPosition(MyGrid);

        //    var offsetX = (_currentTT == null ? _buttonPosition.Value.X : _buttonPosition.Value.X - _currentTT.X) + deltaX - mousePoint.X;
        //    var offsetY = (_currentTT == null ? _buttonPosition.Value.Y : _buttonPosition.Value.Y - _currentTT.Y) + deltaY - mousePoint.Y;

        //    (sender as Rectangle).RenderTransform = new TranslateTransform(-offsetX, -offsetY);
        //}
    }
}
