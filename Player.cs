using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QuoridorProject.Vertices;

namespace QuoridorProject
{
    class Player
    {
        public int x { get; set; }
        public int y { get; set; }
        public int goalx { get; set; }
        public int goaly { get; set; }
        public int num { get; set; }
        public int walls { get; set; }
        public Players player { get; set; }

        public Player()
        { }
        public Player(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.walls = 10;
        }
    }
}
