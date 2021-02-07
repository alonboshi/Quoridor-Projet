using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoridorProject
{
    class Vertices
    {
        public enum Players { EMPTY, BLACK, WHITE, RED, BLUE };
        public Players player { get; set; }
        public static int id_ver = 0;
        public int id { get; set; }
        public Vertices up { get; set; }
        public Vertices down { get; set; }
        public Vertices right { get; set; }
        public Vertices left { get; set; }
        public char text { get; set; }

        public Vertices()
        {
            this.player = Players.EMPTY;
            this.down = null;
            this.left = null;
            this.right = null;
            this.up = null;
            this.id = id_ver++;
        }
    }
}
