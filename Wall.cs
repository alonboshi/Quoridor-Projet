using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoridorProject
{
    class Wall
    {
        public bool[,] horizontal { get; set; }
        public bool[,] vertical { get; set; }

        public Wall()
        {
            this.horizontal = new bool[8, 8];
            this.vertical = new bool[8, 8];
            for (int i = 0; i < horizontal.GetLength(0); i++)
            {
                for (int j = 0; j < horizontal.GetLength(1); j++)
                {
                    horizontal[i, j] = false;

                }
            }
        }
    }
}
