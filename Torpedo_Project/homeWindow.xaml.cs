using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Torpedo_Project
{
    /// <summary>
    /// Interaction logic for homeWindow.xaml
    /// </summary>
    public partial class homeWindow : Window
    {
        private Ship Ship1;
        private Ship Ship2;
        private Ship Ship3;
        private Ship Ship4;
        private Ship Ship5;
        private Ship Ship6;
        private Ship Ship7;

        public homeWindow()
        {
            InitializeComponent();
            createShipsPosition();
            DrawGrid();
            GenerateShips("klasszikus");
        }

        private void GenerateShips(string type)
        {
            Ship.canvas = canvas;
            Ship.MyGrid = MyGrid;
            Ship1 = new Ship(5, 30, 50, 20, "ship1");
            Ship2 = new Ship(4, 30, 50, 70, "ship2");
            Ship3 = new Ship(3, 30, 50, 120, "ship3");
            Ship4 = new Ship(3, 30, 50, 170, "ship4");
            Ship5 = new Ship(2, 30, 50, 220, "ship5");
            if (type == "nagycsata")
            {
                Ship6 = new Ship(150, 30, 50, 270, "ship6");
                Ship7 = new Ship(150, 30, 50, 320, "ship7");
            }
        }

        private void keyPress_Click(object sender, RoutedEventArgs e)
        {
            var window = GetWindow(this);
            window.KeyDown += HandleKeyPress;
        }
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                MainWindow oldal2 = new MainWindow();
                oldal2.Owner = this.Owner;
                oldal2.Show();
                this.Close();
            }
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow oldal2 = new MainWindow();
            oldal2.Owner = this.Owner;
            oldal2.Show();
            this.Close();
        }

        private void DrawGrid()
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
                    Canvas.SetLeft(rect, 250 + x * 30);
                    Canvas.SetTop(rect, 10 + y * 30);
                    Canvas.SetZIndex(rect, 0);
                    canvas.Children.Add(rect);
                }
            }
        }

        private void createShipsPosition()
        {
            Helper.shipsPositions = new int[10, 10];
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    Helper.shipsPositions[x, y] = 0;
                }
            }
        }
    }
}
