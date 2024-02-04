using System;
using System.Collections.Generic;
using System.IO;
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
        private List<string> Mode;
        private List<char> CoordinateABC;

        public homeWindow()
        {
            InitializeComponent();
            ShipStatic.shipsPositions = new int[12, 12];
            ShipStatic.canvas = canvas;
            Mode = new List<string>() {"Klasszikus", "Nagycsata"};
            CoordinateABC = new List<char>() {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J'};
            shipCbx.ItemsSource = Mode;
            shipCbx.SelectedIndex = 0;
            DefaultShips();
        }

        private void GenerateShips(int index)
        {
            Ship1 = new Ship(5, 30, 50, 20, 1);
            Ship2 = new Ship(4, 30, 50, 70, 2);
            Ship4 = new Ship(3, 30, 50, 120, 3);
            Ship5 = new Ship(2, 30, 50, 220, 5);
            if (index == 0)
            {
                Ship3 = new Ship(3, 30, 50, 170, 4);
            }
            else
            {
                Ship3 = new Ship(2, 30, 50, 170, 4);
                Ship6 = new Ship(1, 30, 50, 270, 6);
                Ship7 = new Ship(1, 30, 50, 320, 7);
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
            if (e.Key == Key.Enter)
            {
                if (startGameChecker())
                {
                    gameWindow oldal3 = new gameWindow();
                    oldal3.Owner = this.Owner;
                    oldal3.Show();
                    this.Close();
                }
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
                    Canvas.SetLeft(rect, 280 + x * 30);
                    Canvas.SetTop(rect, 40 + y * 30);
                    Canvas.SetZIndex(rect, 0);
                    ShipStatic.canvas.Children.Add(rect);
                }
            }
        }

        private void DrawCoordinates(bool horizontal)
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
                        Canvas.SetLeft(label, 250 + 10 * 30);
                    }
                    else
                    {
                        Canvas.SetLeft(label, 250 + y * 30);
                    }
                    Canvas.SetTop(label, 10);
                }
                else
                {
                    label.Content = CoordinateABC[y];
                    Canvas.SetTop(label, 40 + y * 30);
                    Canvas.SetLeft(label, 250);
                }
                label.Padding = new Thickness(0, 0, 0, 0);
                label.VerticalContentAlignment = VerticalAlignment.Center;
                label.HorizontalContentAlignment = HorizontalAlignment.Center;
                Canvas.SetZIndex(label, 0);
                ShipStatic.canvas.Children.Add(label);
            }
        }

        private void createShipsPosition()
        {
            for (int x = 0; x <= 11; x++)
            {
                for (int y = 0; y <= 11; y++)
                {
                    ShipStatic.shipsPositions[x, y] = 0;
                }
            }
        }
        private void DefaultShips()
        {
            ShipStatic.shipsDatas.Clear();
            canvas.Children.Clear();
            DrawGrid();
            DrawCoordinates(true);
            DrawCoordinates(false);
            GenerateShips(shipCbx.SelectedIndex);
            createShipsPosition();
        }

        private void shipCbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DefaultShips();
        }

        private void shipCkbx_Checked(object sender, RoutedEventArgs e)
        {
            if (shipCkbx.IsChecked == true)
            {
                DefaultShips();
                ShipStatic.isCheckboxChecked = true;
            }
            else 
            {
                DefaultShips();
                ShipStatic.isCheckboxChecked = false; 
            }
        }

        private bool startGameChecker()
        {
            int numberOfShips = 5;
            if (ShipStatic.isCheckboxChecked)
            {
                numberOfShips = 7;
            }
            if (ShipStatic.shipsDatas.Count() == numberOfShips)
            {
                StreamWriter sw = new StreamWriter("ships_positions.txt", false);
                foreach (var item in ShipStatic.shipsDatas)
                {
                    sw.WriteLine($"{item.Key};{item.Value}");
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        private void gameStart_Click(object sender, RoutedEventArgs e)
        {
            if (startGameChecker())
            {
                gameWindow oldal3 = new gameWindow();
                oldal3.Owner = this.Owner;
                oldal3.Show();
                this.Close();
            }
        }
    }
}
