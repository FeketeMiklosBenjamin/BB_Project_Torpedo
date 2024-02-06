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
        private Rectangle ship;
        private int shipId;
        private int shipSize;
        private int shipWidth;
        private int shipHeight;
        private double shipLeft;
        private double shipDefaultLeft;
        private double shipTop;
        private double shipDefaultTop;
        private int shipXStartPosition = -1;
        private int shipYStartPosition = -1;
        private bool isHorizontal;

        private bool _isRectDragInProg;

        public Ship(int size, int height, int left, int top, int id)
        {
            shipId = id;
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
            isHorizontal = true;
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
            shipTop = Canvas.GetTop(ship);
            shipLeft = Canvas.GetLeft(ship);
            if (shipLeft < 266 || shipLeft > 580 || shipTop < 25 || shipTop > 340)
            {
                if (shipXStartPosition != -1)
                {
                    modifyShipPosition(true, false);
                }
                placeShipDefaultPosition();
                changeShipData();
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

            isHorizontal = !isHorizontal;
        }

        private void placeShip(bool isRotated)
        {
            int xIndex = 1;
            int yIndex = 1;
            for (int x = 280; x < 580; x += 30)
            {
                if ((x + shipWidth) <= 580)
                {
                    if (shipLeft + 15 > x && shipLeft + 15 < (x + 30))
                    {
                        for (int y = 40; y < 340; y += 30)
                        {
                            if ((y + shipHeight) <= 340)
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
                                        changeShipData();
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
            changeShipData();
        }

        private void placeShipDefaultPosition()
        {
            Canvas.SetTop(ship, shipDefaultTop);
            Canvas.SetLeft(ship, shipDefaultLeft);
            shipXStartPosition = -1;
            shipYStartPosition = -1;
            shipLeft = shipDefaultLeft;
            shipTop = shipDefaultTop;
            if (!isHorizontal)
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
            if (ShipStatic.isCheckboxChecked)
            {
                if (modifyShipPositionChecked(remove, isRotated))
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (modifyShipPositionNotChecked(remove, isRotated, number))
                {
                    return true;
                }
                return false;
            }
        }

        private bool modifyShipPositionChecked(bool remove, bool isRotated)
        {
            bool hasPlace = true;
            if (!remove)
            {       
                for (int i = -1; i <= shipSize; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (isHorizontal)
                        {
                            if (ShipStatic.shipsPositions[shipXStartPosition + i, shipYStartPosition + j] == 1)
                            {
                                hasPlace = false;
                                return hasPlace;
                            }
                        }
                        else
                        {
                            if (ShipStatic.shipsPositions[shipXStartPosition + j, shipYStartPosition + i] == 1)
                            {
                                hasPlace = false;
                                return hasPlace;
                            }
                        }
                    }
                }
            }
            if (hasPlace == true)
            {
                for (int i = 0; i < shipSize; i++)
                {
                    if ((isHorizontal && remove && isRotated) || (!isHorizontal && remove && !isRotated))
                    {
                        ShipStatic.shipsPositions[shipXStartPosition, shipYStartPosition + i] = 0;
                    }
                    else if ((!isHorizontal && remove && isRotated) || (isHorizontal && remove && !isRotated))
                    {
                        ShipStatic.shipsPositions[shipXStartPosition + i, shipYStartPosition] = 0;
                    }
                    else
                    {
                        if (isHorizontal)
                        {
                            ShipStatic.shipsPositions[shipXStartPosition + i, shipYStartPosition] = 1;
                        }
                        else
                        {
                            ShipStatic.shipsPositions[shipXStartPosition, shipYStartPosition + i] = 1;
                        }
                    }
                }
            }
            return true;
        }

        private bool modifyShipPositionNotChecked(bool remove, bool isRotated, int number)
        {
            for (int i = 0; i < shipSize; i++)
            {
                if (isHorizontal)
                {
                    if (remove && isRotated)
                    {
                        ShipStatic.shipsPositions[shipXStartPosition, shipYStartPosition + i] = number;
                    }
                    else if (ShipStatic.shipsPositions[shipXStartPosition + i, shipYStartPosition] == 0 || remove)
                    {
                        ShipStatic.shipsPositions[shipXStartPosition + i, shipYStartPosition] = number;
                    }
                    else
                    {
                        for (int j = 0; j < i; j++)
                        {
                            ShipStatic.shipsPositions[shipXStartPosition + j, shipYStartPosition] = 0;
                        }
                        return false;
                    }
                }
                else
                {
                    if (remove && isRotated)
                    {
                        ShipStatic.shipsPositions[shipXStartPosition + i, shipYStartPosition] = number;
                    }
                    else if (ShipStatic.shipsPositions[shipXStartPosition, shipYStartPosition + i] == 0 || remove)
                    {
                        if (isRotated)
                        {
                            ShipStatic.shipsPositions[shipXStartPosition + i, shipYStartPosition] = number;
                        }
                        else
                        {
                            ShipStatic.shipsPositions[shipXStartPosition, shipYStartPosition + i] = number;
                        }
                    }
                    else
                    {
                        for (int j = 0; j < i; j++)
                        {
                            ShipStatic.shipsPositions[shipXStartPosition, shipYStartPosition + j] = 0;
                        }
                        return false;
                    }
                }
            }
            return true;
        }

        private void changeShipData()
        {
            if (shipXStartPosition != -1)
            {
                ShipStatic.shipsDatas[shipId] = $"{shipSize};{shipXStartPosition};{shipYStartPosition};{isHorizontal}";
            }
            else
            {
                ShipStatic.shipsDatas.Remove(shipId);
            }
        }

        //private void Draw()
        //{
        //    for (int x = 0; x <= 11; x++)
        //    {
        //        for (int y = 0; y <= 11; y++)
        //        {
        //            Rectangle rect = new Rectangle();
        //            rect.Stroke = new SolidColorBrush(Colors.Black);
        //            if (ShipFunctions.shipsPositions[x, y] == 1)
        //            {
        //                rect.Fill = new SolidColorBrush(Colors.Green);
        //            }
        //            else
        //            {
        //                rect.Fill = new SolidColorBrush(Colors.Yellow);
        //            }
        //            rect.Width = 30;
        //            rect.Height = 30;
        //            Canvas.SetLeft(rect, 550 + x * 30);
        //            Canvas.SetTop(rect, 10 + y * 30);
        //            Canvas.SetZIndex(rect, 0);
        //            ShipFunctions.canvas.Children.Add(rect);
        //        }
        //    }
        //}
    }
}
