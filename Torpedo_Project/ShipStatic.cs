using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Torpedo_Project
{
    public static class ShipStatic
    {
        public static int[,] checkShipsPositions;
        public static bool isCheckboxChecked;
        public static int shipsCount;
        public static List<int> AIShipsTypes = new List<int>();
        public static List<int> shipsIDs = new List<int>();
        public static Dictionary<int, string> PlayerShipsDatas = new Dictionary<int, string>();
        public static Dictionary<int, string> AIShipsDatas = new Dictionary<int, string>();
        public static List<char> CoordinateABC = new List<char>() { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };

        public static void DrawGrid(Canvas canvas, int startLeft, int startTop)
        {
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    Rectangle rect = new Rectangle();
                    rect.Stroke = new SolidColorBrush(Colors.Black);
                    rect.Fill = new SolidColorBrush(Colors.Blue);
                    rect.Width = 30;
                    rect.Height = 30;
                    Canvas.SetLeft(rect, startLeft + x * 30);
                    Canvas.SetTop(rect, startTop + y * 30);
                    Canvas.SetZIndex(rect, 0);
                    canvas.Children.Add(rect);
                }
            }
        }
        public static void DrawCoordinates(Canvas canvas, bool horizontal, int startLeft, int startTop)
        {
            for (int y = 0; y < 10; y++)
            {
                Label label = new Label();
                label.Width = 30;
                label.Height = 30;
                label.FontSize = 18;
                label.FontWeight = FontWeights.Bold;
                if (horizontal)
                {
                    label.Content = y.ToString();
                    if (y == 0)
                    {
                        Canvas.SetLeft(label, startLeft + 10 * 30);
                    }
                    else
                    {
                        Canvas.SetLeft(label, startLeft + y * 30);
                    }
                    Canvas.SetTop(label, startTop);
                }
                else
                {
                    label.Content = CoordinateABC[y];
                    Canvas.SetTop(label, startTop + y * 30);
                    Canvas.SetLeft(label, startLeft);
                }
                label.Padding = new Thickness(0, 0, 0, 0);
                label.VerticalContentAlignment = VerticalAlignment.Center;
                label.HorizontalContentAlignment = HorizontalAlignment.Center;
                Canvas.SetZIndex(label, 0);
                canvas.Children.Add(label);
            }
        }

        public static void DrawShips(Canvas canvas, int size, int canvasLeft, int canvasTop, int startX, int startY, bool isHorizontal)
        {
            Rectangle ship = new Rectangle();
            ship.Stroke = new SolidColorBrush(Colors.Black);
            ship.Fill = new SolidColorBrush(Colors.Violet);
            ship.Width = (isHorizontal ? size * 30 : 30);
            ship.Height = (isHorizontal ? 30 : size * 30);
            Canvas.SetLeft(ship, canvasLeft + (startX - 1) * 30);
            Canvas.SetTop(ship, canvasTop + (startY - 1) * 30);
            Canvas.SetZIndex(ship, 1);
            canvas.Children.Add(ship);
        }

        public static void SetShipsPositions()
        {
            for (int x = 0; x <= 11; x++)
            {
                for (int y = 0; y <= 11; y++)
                {
                    if (x == 0 || y == 0 || x == 11 || y == 11)
                    {
                        Game.playerShipsPositions[x, y] = -1;
                        Game.AIShipsPositions[x, y] = -1;
                        checkShipsPositions[x, y] = 0;
                    }
                    else
                    {
                        Game.playerShipsPositions[x, y] = 0;
                        Game.AIShipsPositions[x, y] = 0;
                        checkShipsPositions[x, y] = 0;
                    }
                }
            }
        }

        public static bool modifyShipPosition(int shipSize, int xPos, int yPos, bool isHorizontal, bool remove, bool isRotated)
        {
            if (!remove)
            {
                for (int i = -1; i <= shipSize; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (isHorizontal)
                        {
                            if (xPos + i > 11 || yPos + j > 11)
                            {
                                return false;
                            }
                            if (checkShipsPositions[xPos + i, yPos + j] == 1)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (xPos + j > 11 || yPos + i > 11)
                            {
                                return false;
                            }
                            if (checkShipsPositions[xPos + j, yPos + i] == 1)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < shipSize; i++)
            {
                if ((isHorizontal && remove && isRotated) || (!isHorizontal && remove && !isRotated))
                {
                    checkShipsPositions[xPos, yPos + i] = 0;
                }
                else if ((!isHorizontal && remove && isRotated) || (isHorizontal && remove && !isRotated))
                {
                    checkShipsPositions[xPos + i, yPos] = 0;
                }
                else
                {
                    if (isHorizontal)
                    {
                        checkShipsPositions[xPos + i, yPos] = 1;
                    }
                    else
                    {
                        checkShipsPositions[xPos, yPos + i] = 1;
                    }
                }
            }
            return true;
        }

    }
}