﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace QuoridorProject
{
    public partial class Open : Form
    {
        int choice;
        Board board;
        Graphics g;
        
        public Open(int num) {
            InitializeComponent();
            this.choice = num;
            this.board = new Board(num);
        }

        public Open()
        {
         //   InitializeComponent();

        }

        private void Open_Load(object sender, EventArgs e)
        {
            if (this.choice != 3)
                button1.Hide();
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
                case 0:
                    currentPlayer = "Black";
                    break;
                case 1:
                    currentPlayer = "Red";
                    break;

            }
            int walls = items.Item1;
            using (Font myFont = new Font("Arial", 14))
            {
                e.Graphics.DrawString(currentPlayer + " players' turn!\nYou have " + walls + " walls left.\n\n"+board.validWall
                    , myFont, Brushes.Black, new Point(550, 2));
            }
            if (board.Iswin() != -1)
            {
                string str = "";
                switch (board.CurrentPlayer().num)
                {
                    case 0:
                        str=("Red's Player won!!");
                        break;
                    case 1:
                        str=("Black's Player won!!");
                        break;
                }

                DialogResult dialogResult = MessageBox.Show("Would you like to exit?", str , MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    this.Close();
                    System.Windows.Forms.Application.Exit();
                }
                else if (dialogResult == DialogResult.No)
                {
                    
                Thread.Sleep(5000);
                 System.Windows.Forms.Application.Exit();
                    //do something else
                }

                /*pictureBox1.Refresh();
                string message = "You did not enter a server name. Cancel this operation?";
                string caption = "Error Detected in Input";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;
                // Displays the MessageBox.
                result = MessageBox.Show(this,message, caption, buttons);
                if (result == DialogResult.Yes)
                {
                    // Closes the parent form.
                    this.Close();
                }*/
            }
            board.PaintBoard(g);
            board.PossibleMoves(null);
            board.UpdateBoard();
            pictureBox1.Refresh();
        }

        private void Open_Mousedown(object sender, MouseEventArgs e)
        {
            MessageBox.Show(e.X + " " + e.Y);
        }

        private void MouseDown_pb1(object sender, MouseEventArgs e)
        {
            int check = 0;
            if (choice == 2)
            {
                board.PlayerTurn(e.X, e.Y);
            }
            else if (choice == 1)
            {
                check = board.PlayerTurn(e.X, e.Y);
            }
            
            board.InitPossibleMoves();
            board.PossibleMoves(null);
            board.UpdateBoard();
            pictureBox1.Refresh();
            if (check == 1)
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                AI.Move(board, board.CurrentPlayer());
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                Console.WriteLine(ts.Seconds);
                board.InitPossibleMoves();
                board.PossibleMoves(null);
                board.UpdateBoard();
                pictureBox1.Refresh();
            }
        }

        private void MouseEnter_pb1(object sender, EventArgs e)
        {
            //    Point screenPosition = MousePosition;
            //    Point clientPosition = PointToClient(screenPosition);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e) // AI VS AI start button
        {
            if (choice == 3)
                while (board.Iswin() == -1)
                {
                    board.InitPossibleMoves();
                    board.PossibleMoves(null);
                    board.UpdateBoard();
                    pictureBox1.Refresh();
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();
                    AI.Move(board, board.CurrentPlayer());
                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;
                    Console.WriteLine(ts.Seconds);
                }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
            //this.Close();
            //Choice ch = new Choice();
            //ch.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            Open op = new Open(choice);
            op.Show();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            this.Close();
            Open op = new Open(choice);
            op.Show();
        }
    }
}
