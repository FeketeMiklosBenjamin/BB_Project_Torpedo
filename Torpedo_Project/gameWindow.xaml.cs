using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace Torpedo_Project
{
    /// <summary>
    /// Interaction logic for gameWindow.xaml
    /// </summary>
    public partial class gameWindow : Window
    {
        private AI aIClass;
        private Game gameClass;
        private DispatcherTimer picTimer;

        public gameWindow()
        {
            InitializeComponent();
            ShipStatic.SetShipsPositions();
            ShipStatic.DrawCoordinates(gameCanvas, false, 10, 40);
            ShipStatic.DrawCoordinates(gameCanvas, true, 10, 10);
            ShipStatic.DrawGrid(gameCanvas, 40, 40);
            ShipStatic.DrawGrid(gameCanvas, 350, 40);
            ShipStatic.DrawCoordinates(gameCanvas, true, 320, 10);
            ShipStatic.DrawCoordinates(gameCanvas, false, 650, 40);
            startConfig();
            AI.gameCanvas = gameCanvas;
            AI.playerShipsTypes = ShipStatic.AIShipsTypes;
            Game.gameCanvas = gameCanvas;
            Game.shipsGrid = shipsGrid;
            aIClass = new AI();
            gameClass = new Game();
            gameCanvas.AddHandler(Canvas.PreviewMouseRightButtonDownEvent, new MouseButtonEventHandler(canvas_MouseRightButtonDown));
            if (!Game.playerTurn)
            {
                AITip();
            }
        }
        private void randomPic()
        {

        }
        private void startConfig()
        {
            ShowRemainingShips();
            int shipSize;
            int shipStartX;
            int shipStartY;
            bool isHorizontal;
            foreach (var item in ShipStatic.PlayerShipsDatas)
            {
                shipSize = int.Parse(item.Value.Split(";")[1]);
                shipStartX = int.Parse(item.Value.Split(";")[2]);
                shipStartY = int.Parse(item.Value.Split(";")[3]);
                isHorizontal = bool.Parse(item.Value.Split(";")[4]);
                ShipStatic.DrawShips(gameCanvas, shipSize, 40,40, shipStartX, shipStartY, isHorizontal);
            }
        }

        private void ShowRemainingShips()
        {
            ship5_Lbl.Content = "1*";
            ship4_Lbl.Content = "1*";
            if (ShipStatic.PlayerShipsDatas.Count() == 5)
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

        private void ChangeRemainingShips(int shipType)
        {
            switch (shipType)
            {
                case 1:
                    ship1_Lbl.Content = $"{ShipStatic.AIShipsTypes.Where(x => x == 1).Count()}*";
                    break;
                case 2:
                    ship2_Lbl.Content = $"{ShipStatic.AIShipsTypes.Where(x => x == 2).Count()}*";
                    break;
                case 3:
                    ship3_Lbl.Content = $"{ShipStatic.AIShipsTypes.Where(x => x == 3).Count()}*";
                    break;
                case 4:
                    ship4_Lbl.Content = $"{ShipStatic.AIShipsTypes.Where(x => x == 4).Count()}*";
                    break;
                case 5:
                    ship5_Lbl.Content = $"{ShipStatic.AIShipsTypes.Where(x => x == 5).Count()}*";
                    break;
            }
        }

        private void canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!Game.playerTurn || Game.playerWins != null)
            {
                return;
            }
            var mousePos = e.GetPosition(gameCanvas);

            double left = mousePos.X;
            double top = mousePos.Y;

            if (gameClass.placeTip(left, top))
            {
                if (Game.shipDrowned != 0)
                {
                    int shipType = int.Parse(Game.shipDrowned.ToString()[0].ToString());
                    ShipStatic.AIShipsTypes.Remove(shipType);
                    ChangeRemainingShips(shipType);
                    if (Game.playerWins == true)
                    {
                        DrawWinLabel(true);
                        return;
                    }
                }

                if (Game.shipDrowned != 0)
                {
                    DrawTipResult(Game.playerHit, true);
                }
                else
                {   
                    DrawTipResult(Game.playerHit, false);
                }

                if (!Game.playerTurn)
                {
                    AITip();
                    if (Game.playerWins == false)
                    {
                        DrawWinLabel(false);
                        return;
                    }
                }
            }
        }

        private void AITip()
        {
            do
            {
                aIClass.placeAITip();
                if (Game.shipDrowned != 0)
                {
                    int shipType = int.Parse(Game.shipDrowned.ToString()[0].ToString());
                    AI.playerShipsTypes.Remove(shipType);
                }
            } while (!Game.playerTurn);
        }

        private void DrawTipResult(bool isHit, bool isDrowned)
        {
            resultGrid.Children.Clear();
            Label label = new Label();
            label.Width = 160;
            label.Height = 40;
            label.FontSize = 20;
            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            if (isHit && isDrowned)
            {
                pic1.Visibility = Visibility.Visible;
                picTimer = new DispatcherTimer();
                picTimer.Interval = TimeSpan.FromSeconds(5);
                picTimer.Tick += Timer_Tick;
                picTimer.Start();
                label.Content = "Talált és süllyedt!";
                label.Foreground = new SolidColorBrush(Colors.Green);
            }
            else if (isHit)
            {
                label.Content = "Talált!";
                label.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                label.Content = "Nem talált!";
                label.Foreground = new SolidColorBrush(Colors.Red);
            }
            resultGrid.Children.Add(label);
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            pic1.Visibility = Visibility.Hidden;
            picTimer.Stop();
        }
        private void DrawWinLabel(bool playerWins)
        {
            resultGrid.Children.Clear();
            Label label = new Label();
            label.Width = 150;
            label.Height = 40;
            label.FontSize = 20;
            label.HorizontalContentAlignment = HorizontalAlignment.Center;
            if (playerWins)
            {
                
                label.Content = "Győztél!";
                label.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                label.Content = "Vesztettél!";
                label.Foreground = new SolidColorBrush(Colors.Red);
            }
            resultGrid.Children.Add(label);
        }
    }
}
