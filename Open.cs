using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuoridorProject
{
    public partial class Open : Form
    {
        Board board = new Board();
        Graphics g;

        public Open(int num) { }

        public Open()
        {
            InitializeComponent();
        }

        private void Open_Load(object sender, EventArgs e)
        {
            board.AddPlayer();
            board.AddPlayer();
            board.SetCellBoard();
        }

        private void Open_Paint(object sender, PaintEventArgs e)
        {
            //Bitmap bitmap = new Bitmap("NewFolder1/grey_square.jpg");
            //e.Graphics.DrawImage(bitmap, 60, 10);
            
        }

        //public Graphics GetG()
        //{
        //    return this.g;
        //}

        private void Paint_pb1(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            this.g = g;
            var items = board.GetWalls(); // item1 - walls, item2 - current player
            string currentPlayer = "";
            switch (items.Item2)
            {
                case 0:currentPlayer = "Black";
                    break;
                case 1:currentPlayer = "Red";
                    break;

            }
            int walls = items.Item1;
            using (Font myFont = new Font("Arial", 14))
            {
                e.Graphics.DrawString(currentPlayer+" players' turn!\nYou have "+walls+" walls left."
                    , myFont, Brushes.Green, new Point(550, 2));
            }
            board.PaintBoard(g);
            board.PossibleMoves();
            board.UpdateBoard();
            pictureBox1.Refresh();

        }

        private void Open_Mousedown(object sender, MouseEventArgs e)
        {
            MessageBox.Show(e.X + " " + e.Y);
        }

        private void MouseDown_pb1(object sender, MouseEventArgs e)
        {
            //MessageBox.Show(e.X + " " + e.Y);
            board.PlayerTurn(e.X, e.Y);
            board.InitPossibleMoves();
            board.PossibleMoves();
            board.UpdateBoard();
            pictureBox1.Refresh();
        }

        private void MouseEnter_pb1(object sender, EventArgs e)
        {
            Point screenPosition = MousePosition;
            Point clientPosition = PointToClient(screenPosition);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
