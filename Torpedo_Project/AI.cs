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
    public class AI
    {
        public static Canvas gameCanvas;
        public void createShipPosition()
        {
            List<int> datas = new List<int>();
            int shipId = 0;

            foreach (var shiptype in ShipStatic.shipTypes)
            {
                shipId++;
                int xPosition;
                int yPosition;
                bool isHorizontal;
                do
                {
                    datas.Clear();
                    datas = randomGenerator(shiptype);
                    xPosition = datas[0];
                    yPosition = datas[1];
                    isHorizontal = (datas[2] == 0 ? true : false);
                } while (!ShipStatic.modifyShipPositionChecked(shiptype, xPosition, yPosition, isHorizontal, false, false));
                ShipStatic.DrawShips(gameCanvas, shiptype, 350, 40, xPosition, yPosition, isHorizontal);
                ShipStatic.AIShipsDatas[shipId] = $"{shiptype};{xPosition};{yPosition};{isHorizontal}";

            }
        }

        public AI()
        {
            createShipPosition();
        }

        private List<int> randomGenerator(int shipType)
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
            List<int> randomDatas = new List<int>() { randomPositionX, randomPositionY, isHorizontal};
            return randomDatas;

        }

    }
}
