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
        

        public homeWindow()
        {
            InitializeComponent();
            ShipStatic.checkShipsPositions = new int[12, 12];
            Ship.canvas = canvas;
            Mode = new List<string>() {"Klasszikus", "Nagycsata"};
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
                ShipStatic.shipsCount = 5;
                Ship3 = new Ship(3, 30, 50, 170, 4);
            }
            else
            {
                ShipStatic.shipsCount = 7;
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
                //if (startGameChecker())
                //{
                    gameWindow oldal3 = new gameWindow();
                    oldal3.Owner = this.Owner;
                    oldal3.Show();
                    this.Close();
                //}
            }
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow oldal2 = new MainWindow();
            oldal2.Owner = this.Owner;
            oldal2.Show();
            this.Close();
        }
        private void DefaultShips()
        {
            ShipStatic.PlayerShipsDatas.Clear();
            canvas.Children.Clear();
            ShipStatic.DrawGrid(canvas, 280, 40);
            ShipStatic.DrawCoordinates(canvas, true, 250, 10);
            ShipStatic.DrawCoordinates(canvas, false, 250, 40);
            GenerateShips(shipCbx.SelectedIndex);
            ShipStatic.SetShipsPositions();
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
            if (ShipStatic.shipsCount == 7)
            {
                numberOfShips = 7;
            }
            if (ShipStatic.PlayerShipsDatas.Count() == numberOfShips)
            {
                foreach (var item in ShipStatic.PlayerShipsDatas)
                {
                    ShipStatic.shipTypes.Add(int.Parse(item.Value.Split(";")[0]));
                }
                ShipStatic.shipTypes.Sort();
                ShipStatic.shipTypes.Reverse();
                return true;
            }
            else
            {
                return false;
            }
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
