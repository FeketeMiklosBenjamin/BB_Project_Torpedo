using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Torpedo_Project
{
    public class Game
    {
        public static int[,] playerShipsPositions = new int[12, 12];
        public static int[,] AIShipsPositions = new int[12, 12];
        public static bool playerTurn;
        public static Canvas gameCanvas;
        public static Grid shipsGrid;
        public static int shipDrowned;
        public static bool playerHit;
        public static bool win = false;

        public Game()
        {
            FillShipsPositionsDatas(playerShipsPositions, ShipStatic.PlayerShipsDatas);
            FillShipsPositionsDatas(AIShipsPositions, ShipStatic.AIShipsDatas);
            playerTurn = SetPlayerTurn();
        }

        private bool SetPlayerTurn()
        {
            Random random = new Random();
            int number = random.Next(0, 2);
            return number == 0;
        }

        private void FillShipsPositionsDatas(int[,] shipsPositions, Dictionary<int, string> shipDatas)
        {
            int shipId;
            int shipType;
            int xPos;
            int yPos;
            bool isHorizontal;
            foreach (var item in shipDatas)
            {
                string[] data = item.Value.Split(';');
                shipId = int.Parse(data[0]);
                shipType = int.Parse(data[1]);
                xPos = int.Parse(data[2]);
                yPos = int.Parse(data[3]);
                isHorizontal = bool.Parse(data[4]);
                for (int i = -1; i <= shipType; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (isHorizontal)
                        {
                            if (shipsPositions[xPos + i, yPos + j] != -1)
                            {
                                shipsPositions[xPos + i, yPos + j] = -2;
                            }
                        }
                        else
                        {
                            if (shipsPositions[xPos + j, yPos + i] != -1)
                            {
                                shipsPositions[xPos + j, yPos + i] = -2;
                            }
                        }
                    }
                }
                for (int i = 0; i < shipType; i++)
                {
                    if (isHorizontal)
                    {
                        shipsPositions[xPos + i, yPos] = shipId;
                    }
                    else
                    {
                        shipsPositions[xPos, yPos + i] = shipId;
                    }
                }
            }
        }

        public bool placeTip(double left, double top)
        {
            shipDrowned = 0;
            if (left <= 350 || left >= 650 || top <= 40 || top >= 340)
            {
                return false;
            }

            int xIndex = 1;
            int yIndex = 1;

            for (int x = 350; x < 650; x += 30)
            {
                if (left > x && left < (x + 30))
                {
                    for (int y = 40; y < 340; y += 30)
                    {
                        if (top > y && top < (y + 30))
                        {
                            int modifyNumber = modifyShipData(true, xIndex, yIndex);
                            if (modifyNumber != 0)
                            {
                                DrawTip(modifyNumber, 350, 40, xIndex, yIndex);
                            }
                            return true;
                        }
                        yIndex++;
                    }
                }
                xIndex++;
            }
            return false;
        }

        public int modifyShipData(bool isAIMatrixModify, int xPos, int yPos)
        {
            shipDrowned = 0;
            if (isAIMatrixModify)
            {
                int CurrentData = AIShipsPositions[xPos, yPos];
                if (CurrentData == 9)
                {
                    return 0;
                }
                else if (CurrentData == -2 || CurrentData == 0)
                {
                    AIShipsPositions[xPos, yPos] = 9;
                    playerTurn = false;
                    playerHit = false;
                    return 1;
                }
                else
                {
                    AIShipsPositions[xPos, yPos] = 9;
                    playerTurn = true;
                    if (theShipIsDrowned(isAIMatrixModify, AIShipsPositions, CurrentData))
                    {
                        shipDrowned = CurrentData;
                    }
                    else
                    {
                        shipDrowned = 0;
                    }
                    playerHit = true;
                    return 2;
                }
            }
            else
            {
                int CurrentData = playerShipsPositions[xPos, yPos];
                if (CurrentData == -2 || CurrentData == 0)
                {
                    playerShipsPositions[xPos, yPos] = 9;
                    playerTurn = true;
                    return 1;
                }
                else
                {
                    playerShipsPositions[xPos, yPos] = 9;
                    playerTurn = false;
                    AI.targetedShip = true;
                    if (theShipIsDrowned(isAIMatrixModify, playerShipsPositions, CurrentData))
                    {
                        foreach (var item in ShipStatic.PlayerShipsDatas)
                        {
                            if (int.Parse(item.Value.Split(";")[0]) == CurrentData)
                            {
                                string[] data = item.Value.Split(';');
                                int shipType = int.Parse(data[1]);
                                int xStartedPos = int.Parse(data[2]);
                                int yStartedPos = int.Parse(data[3]);
                                bool isHorizontal = bool.Parse(data[4]);
                                modifyIfDrowned(shipType, xStartedPos, yStartedPos, isHorizontal);
                            }
                        }
                        shipDrowned = CurrentData;
                        AI.targetedShip = false;
                    }
                    else
                    {
                        AI.targetShipHitCount++;
                        int[] coordinates = new int[2] { xPos, yPos };
                        AI.targetedShipsPositions.Clear();
                        AI.targetedShipsPositions.Add(coordinates[0]);
                        AI.targetedShipsPositions.Add(coordinates[1]);
                        shipDrowned = 0;
                    }
                    return 2;
                }
            }
        }

        public void DrawTip(int modifyNumber, int canvasLeft, int canvasTop, int xPos, int yPos)
        {
            Rectangle tip = new Rectangle();
            tip.Width = 30;
            tip.Height = 30;
            Canvas.SetLeft(tip, canvasLeft + (xPos - 1) * 30);
            Canvas.SetTop(tip, canvasTop + (yPos - 1) * 30);
            Canvas.SetZIndex(tip, 1);
            if (modifyNumber == 1)
            {
                tip.Fill = new SolidColorBrush(Colors.LightBlue);
            }
            else
            {
                tip.Fill = new SolidColorBrush(Colors.Red);
            }
            gameCanvas.Children.Add(tip);
            if (AI.targetedShip == false)
            {
                AI.targetShipHitCount = 0;
            }


        }

        private bool theShipIsDrowned(bool isAIMatrixModify, int[,] shipPosition, int shipType)
        {
            for (int x = 1; x <= 10; x++)
            {
                for (int y = 1; y <= 10; y++)
                {
                    if (shipPosition[x, y] == shipType)
                    {
                        return false;
                    }
                }
            }
            if (!isAIMatrixModify)
            {
                AI.targetedShipsPositions.Clear();
                AI.targetHorizontal = null;
            }
            return true;
        }

        private void modifyIfDrowned(int shipType, int xPos, int yPos, bool isHorizontal)
        {
            for (int i = -1; i <= shipType; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (isHorizontal)
                    {
                        playerShipsPositions[xPos + i, yPos + j] = 9;
                    }
                    else
                    {
                        playerShipsPositions[xPos + j, yPos + i] = 9;
                    }
                }
            }
        }
    }
}
