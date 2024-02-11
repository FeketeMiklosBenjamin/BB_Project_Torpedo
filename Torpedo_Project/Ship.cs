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
        private int Id;
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

        public Ship(int size, int height, int left, int top, int id, int shipID)
        {
            Id = id;
            shipId = shipID;
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
            return ShipStatic.modifyShipPosition(shipSize, shipXStartPosition, shipYStartPosition, isHorizontal, remove, isRotated);
        }

        private void changeShipData()
        {
            if (shipXStartPosition != -1)
            {
                ShipStatic.PlayerShipsDatas[Id] = $"{shipId};{shipSize};{shipXStartPosition};{shipYStartPosition};{isHorizontal}";
            }
            else
            {
                ShipStatic.PlayerShipsDatas.Remove(Id);
            }
        }
    }
}
