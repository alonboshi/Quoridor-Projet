using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoridorProject
{
    public class Wall
    {
        //0 - no wall
        //1 - yes . part 1
        //2 - yes . part 2
        public int[,] horizontal { get; set; }
        public int[,] vertical { get; set; }

        public Wall()
        {
            this.horizontal = new int[9, 9];
            this.vertical = new int[9, 9];
            for (int i = 0; i < horizontal.GetLength(0); i++)
            {
                for (int j = 0; j < horizontal.GetLength(1); j++)
                {
                    horizontal[i, j] = 0;
                }
                for (int z = 0; z < vertical.GetLength(1); z++)
                {
                    vertical[i, z] = 0;
                }
            }
        }
        public bool IsFreeHorizontal(int x, int y) // if we can put there horizontal wall
        {
            return horizontal[x, y] == 0;
        }
        public bool IsFreeVertical(int x, int y) // if we can put there vertical wall
        {
            return vertical[x, y] == 0;
        }
        public bool IsHorizontal(int x, int y) // if there is already horizontal wall
        {
            return horizontal[x, y] == 1;
        }
        public bool IsVertical(int x, int y) // if there is already vertical wall
        {
            return vertical[x, y] == 1;
        }
    }
}
