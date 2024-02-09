using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Torpedo_Project
{
    public class Game
    {
        public static int[,] playerShipsPositions = new int[12, 12];
        public static int[,] AIShipsPositions = new int[12, 12];

        public Game()
        {
            FillShipsPositionsDatas(playerShipsPositions, ShipStatic.PlayerShipsDatas);
            FillShipsPositionsDatas(AIShipsPositions, ShipStatic.AIShipsDatas);
        }

        private void FillShipsPositionsDatas(int[,] shipsPositions, Dictionary<int, string> shipDatas)
        {
            int shipType;
            int xPos;
            int yPos;
            bool isHorizontal;
            foreach (var item in shipDatas)
            {
                string[] data = item.Value.Split(';');
                shipType = int.Parse(data[0]);
                xPos = int.Parse(data[1]);
                yPos = int.Parse(data[2]);
                isHorizontal = bool.Parse(data[3]);

                if (ShipStatic.isCheckboxChecked)
                {
                    for (int i = -1; i <= shipType; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if (isHorizontal)
                            {
                                shipsPositions[xPos + i, yPos + j] = -1;
                            }
                            else
                            {
                                shipsPositions[xPos + j, yPos + i] = -1;
                            }
                        }
                    }
                }

                for (int i = 0; i < shipType; i++)
                {
                    if (isHorizontal)
                    {
                        shipsPositions[xPos + i, yPos] = shipType;
                    }
                    else
                    {
                        shipsPositions[xPos, yPos + i] = shipType;
                    }
                }
            }
        }
    }
}
