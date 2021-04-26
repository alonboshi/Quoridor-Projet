using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuoridorProject
{
    /// <summary>
    /// the graph board. each vertice 
    /// </summary>
    public class Vertices
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

        /// <summary>
        /// gets the possible moves and returns the same list converted to vertices
        /// </summary>
        /// <param name="arr"> An array of possible moves</param>
        /// <param name="index"> how many possible moves </param>
        /// <param name="matrix"> the graph board </param>
        /// <returns></returns>
        public List<Vertices> GetNeighbors(Board.Point[] arr,int index,Vertices[,] matrix)
        {

            List<Vertices> neighbors = new List<Vertices>();
            for (int i = 0; i < index; i++)
            {
                neighbors.Add(matrix[arr[i].x, arr[i].y]);
            }
            return neighbors ;
        }

}
}
