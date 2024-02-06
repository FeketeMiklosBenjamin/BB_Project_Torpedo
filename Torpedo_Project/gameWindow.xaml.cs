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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;

namespace Torpedo_Project
{
    /// <summary>
    /// Interaction logic for gameWindow.xaml
    /// </summary>
    public partial class gameWindow : Window
    {
        //private Dictionary<int, int> shipXDict = new Dictionary<int, int>();
        //private Dictionary<int, int> shipYDict = new Dictionary<int, int>();

        public gameWindow()
        {
            InitializeComponent();
            ShipStatic.DrawCoordinates(gameCanvas, false, 10, 40);
            ShipStatic.DrawCoordinates(gameCanvas, true, 10, 10);
            ShipStatic.DrawGrid(gameCanvas, 40, 40);
            ShipStatic.DrawGrid(gameCanvas, 350, 40);
            ShipStatic.DrawCoordinates(gameCanvas, true, 320, 10);
            ShipStatic.DrawCoordinates(gameCanvas, false, 650, 40);
            startConfig();
        }

        private void startConfig()
        {
            ShowRemaningShips();
            int shipSize;
            int shipStartX;
            int shipStartY;
            bool isHorizontal;
            foreach (var item in ShipStatic.shipsDatas)
            {
                shipSize = int.Parse(item.Value.Split(";")[0]);
                shipStartX = int.Parse(item.Value.Split(";")[1]);
                shipStartY = int.Parse(item.Value.Split(";")[2]);
                isHorizontal = bool.Parse(item.Value.Split(";")[3]);
                DrawShips(shipSize, 40,40, shipStartX, shipStartY, isHorizontal, false);
            }
        }

        private void ShowRemaningShips()
        {
            ship5_Lbl.Content = "1*";
            ship4_Lbl.Content = "1*";
            if (ShipStatic.shipsDatas.Count() == 5)
            {
                ship3_Lbl.Content = "2*";
                ship2_Lbl.Content = "1*";
            }
            else
            {
                ship3_Lbl.Content = "1*";
                ship2_Lbl.Content = "2*";
                ship1_Lbl.Content = "2*";
            }
        }

        private void DrawShips(int size, int canvasLeft, int canvasTop, int startX, int startY, bool isHorizontal, bool isAIShips)
        {
            Rectangle ship = new Rectangle();
            ship.Stroke = new SolidColorBrush(Colors.Black);
            ship.Fill = new SolidColorBrush(Colors.Violet);
            ship.Width = (isHorizontal ? size * 30 : 30);
            ship.Height = (isHorizontal ? 30 : size * 30);
            Canvas.SetLeft(ship, canvasLeft + (startX - 1) * 30);
            Canvas.SetTop(ship, canvasTop + (startY - 1) * 30);
            gameCanvas.Children.Add(ship);
        }
    }
}
