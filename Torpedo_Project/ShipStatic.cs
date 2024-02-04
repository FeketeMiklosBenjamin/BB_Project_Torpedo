using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Torpedo_Project
{
    public static class ShipStatic
    {
        public static int[,] shipsPositions;
        public static Canvas canvas;
        public static bool isCheckboxChecked;
        public static Dictionary<int, string> shipsDatas = new Dictionary<int, string>();
    }
}