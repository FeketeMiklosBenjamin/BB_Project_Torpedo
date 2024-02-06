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
        public static int[,] shipsPositions;
        public static bool isCheckboxChecked;
        public static int shipsCount;
        public static Dictionary<int, string> shipsDatas = new Dictionary<int, string>();
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

    }
}