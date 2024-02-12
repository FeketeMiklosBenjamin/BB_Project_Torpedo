using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Torpedo_Project
{
    public class AI : Game
    {
        public static Canvas gameCanvas;
        public static List<int> playerShipsTypes = new List<int>();
        public static int[,] AITipsHelper = new int[12, 12];

        public static bool targetedShip;
        public static List<int> targetedShipsPositions = new List<int>();
        public static int targetShipHitCount;
        public static bool? targetHorizontal;

        private int randomPercentage;

        public void createShipPosition()
        {
            List<int> datas = new List<int>();
            int shipId = 0;

            foreach (var shipid in ShipStatic.shipsIDs)
            {
                shipId++;
                int xPosition;
                int yPosition;
                bool isHorizontal;
                int shipType = int.Parse(shipid.ToString()[0].ToString());
                do
                {
                    datas.Clear();
                    datas = randomPositionsGenerator(shipType);
                    xPosition = datas[0];
                    yPosition = datas[1];
                    isHorizontal = (datas[2] == 0 ? true : false);
                } while (!ShipStatic.modifyShipPosition(shipType, xPosition, yPosition, isHorizontal, false, false));
                ShipStatic.DrawShips(gameCanvas, shipType, 350, 40, xPosition, yPosition, isHorizontal);
                ShipStatic.AIShipsDatas[shipId] = $"{shipid};{shipType};{xPosition};{yPosition};{isHorizontal}";
            }
        }

        public AI()
        {
            createShipPosition();
        }

        public void placeAITip()
        {
            FillHelperMatrix();
            if (targetedShip)
            {
                int xCoordinate = targetedShipsPositions[0];
                int yCoordinate = targetedShipsPositions[1];
                TargetedShipGuesser(xCoordinate, yCoordinate, targetHorizontal);
            }
            else
            {
                int minNumber = playerShipsTypes[0];
                int maxNumber = playerShipsTypes[playerShipsTypes.Count - 1];
                int randomType = randomTipGenerator(minNumber, maxNumber);
                FilterHelperMatrix(randomType);
            }
        }

        private void TargetedShipGuesser(int xCoordinate, int yCoordinate, bool? Horizontal)
        {
            int modifyNumber;
            if (Horizontal == null || Horizontal == true)
            {
                if (AITipsHelper[xCoordinate - 1, yCoordinate] == 0)
                {
                    modifyNumber = modifyShipData(false, xCoordinate - 1, yCoordinate);
                    DrawTip(modifyNumber, 40, 40, xCoordinate - 1, yCoordinate);
                    if (modifyNumber == 2 && targetedShip == true)
                    {
                        targetHorizontal = true;
                    }
                    return;
                }
                else if (AITipsHelper[xCoordinate + 1, yCoordinate] == 0)
                {
                    modifyNumber = modifyShipData(false, xCoordinate + 1, yCoordinate);
                    DrawTip(modifyNumber, 40, 40, xCoordinate + 1, yCoordinate);
                    if (modifyNumber == 2 && targetedShip == true)
                    {
                        targetHorizontal = true;
                    }
                    return;
                }
            }
            if (Horizontal == null || Horizontal == false)
            {
                if (AITipsHelper[xCoordinate, yCoordinate - 1] == 0)
                {
                    modifyNumber = modifyShipData(false, xCoordinate, yCoordinate - 1);
                    DrawTip(modifyNumber, 40, 40, xCoordinate, yCoordinate - 1);
                    if (modifyNumber == 2 && targetedShip == true)
                    {
                        targetHorizontal = false;
                    }
                    return;
                }
                else
                {
                    modifyNumber = modifyShipData(false, xCoordinate, yCoordinate + 1);
                    DrawTip(modifyNumber, 40, 40, xCoordinate, yCoordinate + 1);
                    if (modifyNumber == 2 && targetedShip == true)
                    {
                        targetHorizontal = false;
                    }
                    return;
                }
            }
            if (Horizontal == true)
            {
                modifyNumber = modifyShipData(false, xCoordinate + targetShipHitCount, yCoordinate);
                DrawTip(modifyNumber, 40, 40, xCoordinate + targetShipHitCount - 1, yCoordinate);
                return;
            }
            else
            {
                modifyNumber = modifyShipData(false, xCoordinate, yCoordinate + targetShipHitCount);
                DrawTip(modifyNumber, 40, 40, xCoordinate, yCoordinate + targetShipHitCount - 1);
                return;
            }
        }

        private void FillHelperMatrix()
        {
            randomPercentage = 0;
            int currentData;
            for (int x = 0; x <= 11; x++)
            {
                for (int y = 0; y <= 11; y++)
                {
                    currentData = playerShipsPositions[x, y];
                    if (currentData == 9 || currentData == -1)
                    {
                        AITipsHelper[x, y] = 1;
                    }
                    else
                    {
                        AITipsHelper[x, y] = 0;
                        randomPercentage++;
                    }
                }
            }
            if (targetedShip)
            {
                int xCoordinate = targetedShipsPositions[0];
                int yCoordinate = targetedShipsPositions[1];
                AITipsHelper[xCoordinate, yCoordinate] = 2;
            }
        }

        private void FilterHelperMatrix(int shipSize)
        {
            int lastXIndex = -1;
            int lastYIndex = -1;
            int modifyNumber;
            for (int x = 0; x <= 11; x++)
            {
                for (int y = 0; y <= 11; y++)
                {
                    if (AITipsHelper[x, y] == 0)
                    {
                        if (TipChecker(x, y, shipSize))
                        {
                            lastXIndex = x;
                            lastYIndex = y;
                            int randomNumber = (int)Math.Round((double)(randomPercentage / 3));
                            if (randomTipGenerator(0, randomNumber) == 1)
                            {
                                modifyNumber = modifyShipData(false, x, y);
                                DrawTip(modifyNumber, 40, 40, x, y);
                                return;
                            }
                        }
                    }
                }
            }
            modifyNumber = modifyShipData(false, lastXIndex, lastYIndex);
            DrawTip(modifyNumber, 40, 40, lastXIndex, lastYIndex);
        }

        private bool TipChecker(int xCoordinate, int yCoordinate, int shipSize)
        {
            int emptySlot = 1;
            for (int i = 0; i < shipSize; i++)
            {
                if (AITipsHelper[xCoordinate - i, yCoordinate] == 0)
                {
                    if (emptySlot == shipSize)
                    {
                        return true;
                    }
                    emptySlot++;
                }
                else
                {
                    break;
                }
            }
            for (int i = 0; i < shipSize; i++)
            {
                if (AITipsHelper[xCoordinate + i, yCoordinate] == 0)
                {
                    if (emptySlot == shipSize)
                    {
                        return true;
                    }
                    emptySlot++;
                }
                else
                {
                    break;
                }
            }

            emptySlot = 1;
            for (int i = 0; i < shipSize; i++)
            {
                if (AITipsHelper[xCoordinate, yCoordinate + i] == 0)
                {
                    if (emptySlot == shipSize)
                    {
                        return true;
                    }
                    emptySlot++;
                }
                else
                {
                    break;
                }
            }
            for (int i = 0; i < shipSize; i++)
            {
                if (AITipsHelper[xCoordinate, yCoordinate - i] == 0)
                {
                    if (emptySlot == shipSize)
                    {
                        return true;
                    }
                    emptySlot++;
                }
                else
                {
                    break;
                }
            }
            return false;
        }

        private int randomTipGenerator(int minValue, int maxValue)
        {
            Random random = new Random();
            int number = random.Next(minValue, maxValue);
            return number;
        }

        private List<int> randomPositionsGenerator(int shipType)
        {
            Random random = new Random();
            int isHorizontal = random.Next(0, 2);
            int randomPositionX;
            int randomPositionY;
            if (isHorizontal == 1)
            {
                randomPositionX = random.Next(1, 11);
                randomPositionY = random.Next(1, shipType + 2);
            }
            else
            {
                randomPositionX = random.Next(1, shipType + 2);
                randomPositionY = random.Next(1, 11);
            }
            List<int> randomDatas = new List<int>() { randomPositionX, randomPositionY, isHorizontal };
            return randomDatas;

        }

    }
}

