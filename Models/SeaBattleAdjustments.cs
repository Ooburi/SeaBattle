using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleTelegramBot.Models
{
    
    public static class SeaBattleAdjustments
    {
        public static void PrepareMove(string letter, string number, ref int i, ref int j)
        {
            j = Convert.ToInt32(number);
            j--;
            switch (letter.ToLower())
            {
                case "a":
                    i = 0;
                    break;
                case "b":
                    i = 1;
                    break;
                case "c":
                    i = 2;
                    break;
                case "d":
                    i = 3;
                    break;
                case "e":
                    i = 4;
                    break;
                case "f":
                    i = 5;
                    break;
                case "g":
                    i = 6;
                    break;
                case "h":
                    i = 7;
                    break;
                case "i":
                    i = 8;
                    break;
                case "j":
                    i = 9;
                    break;
            }
        }
        public enum Status
        {
            NotInGame,
            MyTurn,
            Wait,
            Looser,
            Winner
        }
        public static string ToDisplay(string ships)
        {
            string firstRow = "<pre> |1|2|3|4|5|6|7|8|9|10|\n";
            string[] rows = ships.Split("\n");
            for(int i=0; i < rows.Length; i++)
            {
                rows[i] = String.Join("|", rows[i].ToCharArray()) +" |\n";
            }
            string secondRow = "A|"+rows[0];
            string Row3 = "B|" + rows[1];
            string Row4 = "C|" + rows[2];
            string Row5 = "D|" + rows[3];
            string Row6 = "E|" + rows[4];
            string Row7 = "F|" + rows[5];
            string Row8 = "G|" + rows[6];
            string Row9 = "H|" + rows[7];
            string Row10 = "I|" + rows[8];
            string Row11 = "J|" + rows[9];
            return firstRow + secondRow + Row3 + Row4 + Row5 + Row6 + Row7 + Row8 + Row9 + Row10 + Row11 +"</pre>";
        }
        public static string JoinShips(int[][] ships)
        {
            string shipsCollapsed = "";
            for (int i = 0; i < ships.Length; i++)
            {
                for (int j = 0; j < ships[i].Length; j++)
                {
                    shipsCollapsed += ships[i][j].ToString();
                }
                shipsCollapsed += "\n";
            }
            return shipsCollapsed;
        }
        /// <summary>
        /// Must be with numbers only
        /// </summary>
        /// <param name="ships"></param>
        /// <returns></returns>
        public static int[][] SplitShips(string ships)
        {
            int[][] shipsSplitted = new int[10][];
            string[] rows = ships.Split("\n");
            for(int i = 0; i < rows.Length-1; i++)
            {
                shipsSplitted[i] = new int[10];
                for(int j = 0; j < 10; j++)
                {
                    string s = rows[i][j].ToString();
                     int.TryParse(s, out shipsSplitted[i][j]);
                }
            }
            return shipsSplitted;
        }
        public static string GetEmptyField()
        {
            return "          \n" + "          \n" + "          \n" + "          \n" + "          \n" + "          \n" + "          \n" + "          \n" + "          \n" + "          \n";
        }
        public static string ReplaceShipsNumberToSymbols(string ships)
        {
            ships=ships.Replace('0', ' ');
            ships=ships.Replace('1', '#');
            ships=ships.Replace('2', '*');
            ships=ships.Replace('3', '.');
            return ships;
        }
        public static string ReplaceShipsSymbolsToNumber(string ships)
        {

            ships=ships.Replace(' ','0');
            ships=ships.Replace('#','1');
            ships=ships.Replace('*','2');
            ships=ships.Replace('.','3');
            return ships;
        }
        public static string GetShips()
        {
            int[][] ships = getRandomSheeps();
            return ReplaceShipsNumberToSymbols(JoinShips(ships));
        }
        public static int[][] getRandomSheeps()
        {
            int[][][] ShipVariants = new int[4][][];

            ShipVariants[0] = new int[][]
            {
             new int[] { 1, 0, 0, 1, 0, 0, 0, 0, 1, 0 },
             new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
             new int[] { 1, 0, 0, 1, 1, 1, 0, 0, 0, 0 },
             new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
             new int[] { 0, 0, 0, 0, 1, 1, 1, 0, 0, 0 },
             new int[] { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
             new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
             new int[] { 0, 1, 0, 0, 0, 0, 0, 0, 1, 0 },
             new int[] { 0, 1, 0, 0, 1, 0, 1, 0, 0, 0 },
             new int[] { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }
            };

            ShipVariants[1] = new int[][]
            {
             new int[] { 0, 1, 0, 1, 0, 0, 0, 0, 1, 0 },
             new int[] { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
             new int[] { 0, 1, 0, 1, 1, 1, 0, 0, 0, 1 },
             new int[] { 0, 1, 0, 0, 0, 0, 0, 0, 0, 1 },
             new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
             new int[] { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
             new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
             new int[] { 0, 1, 0, 0, 0, 0, 0, 0, 1, 0 },
             new int[] { 0, 1, 0, 0, 1, 0, 0, 0, 0, 0 },
             new int[] { 0, 0, 0, 0, 1, 0, 0, 0, 0, 1 }
            };
            ShipVariants[2] = new int[][]
            {
             new int[] { 0, 0, 0, 1, 0, 0, 1, 0, 0, 0 },
             new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
             new int[] { 0, 0, 0, 1, 1, 1, 0, 0, 0, 1 },
             new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
             new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
             new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
             new int[] { 1, 1, 1, 1, 0, 0, 0, 0, 1, 0 },
             new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
             new int[] { 0, 0, 0, 0, 1, 0, 1, 0, 0, 0 },
             new int[] { 0, 1, 1, 0, 1, 0, 0, 0, 0, 1 }
            };
            ShipVariants[3] = new int[][]
            {
             new int[] { 0, 1, 0, 1, 0, 0, 1, 0, 0, 0 },
             new int[] { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
             new int[] { 0, 1, 0, 0, 0, 0, 0, 0, 0, 1 },
             new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
             new int[] { 1, 0, 0, 0, 1, 0, 0, 0, 0, 1 },
             new int[] { 1, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
             new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
             new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
             new int[] { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
             new int[] { 0, 1, 1, 0, 0, 0, 0, 0, 0, 1 }
            };

            return ShipVariants[new Random().Next(0, 4)];
        }
    }
}
