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
        public int size { get; set; }
        //public string imgSource { get; set; }

        public static Canvas canvas;
        public static Grid MyGrid;

        private Rectangle ship;
        private int shipSize;
        private int shipWidth;
        private int shipHeight;
        private double? canvasLeft;
        private double? canvasTop;
        private double shipLeft;
        private double shipDefaultLeft;
        private double shipTop;
        private double shipDefaultTop;
        private int shipXStartPosition = -1;
        private int shipYStartPosition = -1;

        private bool _isRectDragInProg;
        private bool _isHorizontal;

        public Ship(int size, int height, int left, int top, string name)
        {
            shipDefaultLeft = shipLeft = left;
            shipDefaultTop = shipTop = top;
            shipWidth = size * 30;
            shipHeight = height;
            shipSize = size;
            ship = new Rectangle();
            ship.Stroke = new SolidColorBrush(Colors.Black);
            ship.Fill = new SolidColorBrush(Colors.Violet);
            ship.Width = size * 30;
            ship.Height = height;
            ship.Name = name;
            _isHorizontal = true;
            ship.AddHandler(Rectangle.PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(ship_MouseLeftButtonDown));
            ship.AddHandler(Rectangle.PreviewMouseRightButtonDownEvent, new MouseButtonEventHandler(ship_Rotate));
            ship.AddHandler(Rectangle.PreviewMouseLeftButtonUpEvent, new MouseButtonEventHandler(ship_MouseLeftButtonUp));
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
                if (shipXStartPosition != -1)
                {
                    modifyShipPosition(true, false);
                }
                placeShipDefaultPosition();
            }
            else
            {
                placeShip(false);
            }
            _isRectDragInProg = false;
            ship.ReleaseMouseCapture();
        }


        private void ship_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isRectDragInProg) return;
            var mousePos = e.GetPosition(canvas);

            double left = mousePos.X - (ship.ActualWidth / 2);
            double top = mousePos.Y - (ship.ActualHeight / 2);
            Canvas.SetLeft(ship, left);
            shipLeft = left;
            Canvas.SetTop(ship, top);
            shipTop = top;
        }

        private void ship_Rotate(object sender, MouseButtonEventArgs e)
        {
            rotateShipFunction();
            if (shipXStartPosition != -1)
            {
                placeShip(true);
            }
        }

        private void rotateShipFunction()
        {
            int height = shipHeight;
            int width = shipWidth;
            ship.Width = height;
            ship.Height = width;
            shipHeight = width;
            shipWidth = height;

            _isHorizontal = !_isHorizontal;
        }

        private void placeShip(bool isRotated)
        {
            int xIndex = 0;
            int yIndex = 0;
            for (int x = 250; x < 550; x += 30)
            {
                if ((x + shipWidth) <= 550)
                {
                    if (shipLeft + 15 > x && shipLeft + 15 < (x + 30))
                    {
                        for (int y = 10; y < 310; y += 30)
                        {
                            if ((y + shipHeight) <= 310)
                            {
                                if (shipTop + 15 > y && shipTop + 15 < (y + 30))
                                {
                                    if (shipXStartPosition != -1)
                                    {
                                        modifyShipPosition(true, isRotated);
                                    }
                                    shipXStartPosition = xIndex;
                                    shipYStartPosition = yIndex;
                                    if (modifyShipPosition(false, false))
                                    {
                                        Canvas.SetLeft(ship, x);
                                        shipLeft = x;
                                        Canvas.SetTop(ship, y);
                                        shipTop = y;
                                    }
                                    else
                                    {
                                        placeShipDefaultPosition();
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                if (shipYStartPosition != -1)
                                {
                                    modifyShipPosition(true, isRotated);
                                }
                                placeShipDefaultPosition();
                                break;
                            }
                            yIndex++;
                        }
                        break;
                    }
                }
                else
                {
                    if (shipXStartPosition != -1)
                    {
                        modifyShipPosition(true, isRotated);
                    }
                    placeShipDefaultPosition();
                    break;
                }
                xIndex++;
            }
        }

        private void placeShipDefaultPosition()
        {
            Canvas.SetTop(ship, shipDefaultTop);
            Canvas.SetLeft(ship, shipDefaultLeft);
            shipXStartPosition = -1;
            shipYStartPosition = -1;
            shipLeft = shipDefaultLeft;
            shipTop = shipDefaultTop;
            if (!_isHorizontal)
            {
                rotateShipFunction();
            }
        }

        private bool modifyShipPosition(bool remove, bool isRotated)
        {
            int number = 0;
            if (!remove)
            {
                number = 1;
            }
            if (_isHorizontal)
            {
                for (int i = 0; i < shipSize; i++)
                {
                    if (remove && isRotated)
                    {
                        Helper.shipsPositions[shipXStartPosition, shipYStartPosition + i] = number;
                    }
                    else if (Helper.shipsPositions[shipXStartPosition + i, shipYStartPosition] == 0 || remove)
                    {
                        if (isRotated)
                        {
                            Helper.shipsPositions[shipXStartPosition, shipYStartPosition + i] = number;
                        }
                        else
                        {
                            Helper.shipsPositions[shipXStartPosition + i, shipYStartPosition] = number;
                        }
                    }
                    else
                    {
                        for (int j = 0; j < i; j++)
                        {
                            Helper.shipsPositions[shipXStartPosition + j, shipYStartPosition] = 0;
                        }
                        return false;
                    }
                }
                return true;
            }
            else
            {
                for (int i = 0; i < shipSize; i++)
                {
                    if (remove && isRotated)
                    {
                        Helper.shipsPositions[shipXStartPosition + i, shipYStartPosition] = number;
                    }
                    else if (Helper.shipsPositions[shipXStartPosition, shipYStartPosition + i] == 0 || remove)
                    {
                        if (isRotated)
                        {
                            Helper.shipsPositions[shipXStartPosition + i, shipYStartPosition] = number;
                        }
                        else
                        {
                            Helper.shipsPositions[shipXStartPosition, shipYStartPosition + i] = number;
                        }
                    }
                    else
                    {
                        for (int j = 0; j < i; j++)
                        {
                            Helper.shipsPositions[shipXStartPosition, shipYStartPosition + j] = 0;
                        }
                        return false;
                    }

                }
                return true;
            }
        }
    }
}
