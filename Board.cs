using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QuoridorProject.Vertices;

namespace QuoridorProject
{
    class Board
    {
        public struct Point {
            public int x;
            public int y;
        }
        private Wall walls = new Wall();
        private int index_possibleMoves = 0;
        private Point[] possibleMove = new Point[4];
        private int currentPlayer = 0;
        private Player[] pArr; // array of all players
        private int numOfPlayers; // num of players. at first is 0.
        private Vertices[,] matrix = new Vertices[9, 9];
        private const int ROW = 9;
        private const int COL = 9;
        public Cell[,] board;
        public Board()
        {
            // pe = PaintEventArgs;
            pArr = new Player[4];
            this.numOfPlayers = 0;
            
            //for (int i = 0; i < pArr.Length; i++)
            //{
            //    pArr[i] = new Player();
            //}
            SetVerticesMatrix();

        }

        public void SetCellBoard()
        {
            this.board = new Cell[ROW, COL];
            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {
                    this.board[i, j] = new Cell(i, j, numOfPlayers);
                }
            }
            for (int i = 0; i < numOfPlayers; i++)
            {
                board[pArr[i].x, pArr[i].y].Change(pArr[i].num);
            }
        }

        public Tuple<int, int> GetWalls()
        {
            Player p = null;
            for (int i = 0; i < numOfPlayers; i++)
            {
                if (pArr[i].num == currentPlayer)
                {
                    p = pArr[i];
                    break;
                }

            }
            return Tuple.Create(p.walls, currentPlayer);
        }

        public void AddPlayer()
        {
            if (this.numOfPlayers == 0)
            {
                // player at the bottom
                Player p = new Player(8, 4)
                {
                    goalx = 0,
                    goaly = -1,
                    num = 0
                };
                pArr[0] = p;
                //p.player = Players.BLACK;
                this.numOfPlayers++;
            }
            else if (this.numOfPlayers == 1)
            {
                // player at the top
                Player p = new Player(0, 4)
                {
                    goalx = 8,
                    goaly = -1,
                    num = 1
                };
                pArr[1] = p;
                //p.player = Players.WHITE;
                this.numOfPlayers++;
            }
            else if (this.numOfPlayers == 2)
            {
                // player at the right
                Player p = new Player(4, 8)
                {
                    goalx = -1,
                    goaly = 0,
                    num = 2
                };
                pArr[2] = p;
                //p.player = Players.RED;
                this.numOfPlayers++;
            }
            else if (this.numOfPlayers == 3)
            {
                // player at the left
                Player p = new Player(4, 0)
                {
                    goalx = -1,
                    goaly = 8,
                    num = 3
                };
                pArr[3] = p;
                //p.player = Players.BLUE;
                this.numOfPlayers++;
            }
        }

        private void SetVerticesMatrix()
        {
            for (int row_index = 0; row_index < 9; row_index++)
            {
                for (int col_index = 0; col_index < 9; col_index++)
                {
                    matrix[row_index, col_index] = new Vertices();
                }
            }

            for (int row_index = 0; row_index < 9; row_index++)
            {
                for (int col_index = 0; col_index < 9; col_index++)
                {
                    //if (this.matrix[row_index, col_index].player == Players.EMPTY)
                    //    continue;
                    if (col_index > 0)
                    {
                        this.matrix[row_index, col_index].
                            left = (this.matrix[row_index, col_index - 1]);
                    }
                    if (col_index < 9 - 1)
                    {
                        this.matrix[row_index, col_index].
                            right = (this.matrix[row_index, col_index + 1]);
                    }
                    if (row_index < 9 - 1)
                    {
                        this.matrix[row_index, col_index].
                            down = (this.matrix[row_index + 1, col_index]);
                    }
                    if (row_index > 0)
                    {
                        this.matrix[row_index, col_index].
                            up = (this.matrix[row_index - 1, col_index]);
                    }
                }
            }
        }

        public void UpdateBoard()
        {
            //Player p = null;
            for (int i = 0; i < numOfPlayers; i++)
            {
                //if (pArr[i].num == currentPlayer)
                //    p = pArr[i];
                board[pArr[i].x, pArr[i].y].Change(pArr[i].num);
            }
            //if (p == null)
            //    return;
            //board[p.x, p.y].Change(p.num);

        }

        public void TurnMove(int x, int y)
        {
            Player p = null;
            for (int i = 0; i < numOfPlayers; i++)
            {
                if (pArr[i].num == currentPlayer)
                    p = pArr[i];
            }
            if (p == null)
                return;
            int xCell = y / 60;
            int yCell = x / 60;
            for (int i = 0; i < index_possibleMoves; i++)
            {
                if (possibleMove[i].x == xCell && possibleMove[i].y == yCell)
                {
                    board[p.x, p.y].DeleteImage(p.x,p.y,numOfPlayers);
                    board[xCell, yCell].Change(p.num);
                    p.x = xCell;
                    p.y = yCell;
                    currentPlayer = (p.num == 0) ? 1 : 0;
                }
            }
        }

        public void ShowWall(Point p) 
        { 
            
        }

        public void TurnWall(int x, int y)
        {
            Player p = null;
            for (int i = 0; i < numOfPlayers; i++)
            {
                if (pArr[i].num == currentPlayer)
                    p = pArr[i];
            }
            if (p == null)
                return;
            if (p.walls == 0)
                return;
            // Vertical wall
            if (x % 60 >= 50 && y >= 0 && y <= 460)
            {
                int xCell = y / 60;
                int yCell = x / 60;
                if (!walls.IsFreeVertical(xCell, yCell) || !walls.IsFreeVertical(xCell + 1, yCell) 
                    || walls.IsHorizontal(xCell,yCell))
                    return;
                walls.vertical[xCell, yCell] = 1;
                walls.vertical[xCell + 1, yCell ] = 2;
                matrix[xCell, yCell].right = null;
                matrix[xCell+1, yCell].right = null;
                matrix[xCell , yCell+1].left = null;
                matrix[xCell + 1, yCell+1].left = null;
                p.walls--;
                currentPlayer = (p.num == 0) ? 1 : 0;
            }
            if (y % 60 >= 50 && x >= 0 && x <= 470)
            {
                int xCell = y / 60;
                int yCell = x / 60;
                if (!walls.IsFreeHorizontal(xCell, yCell)|| !walls.IsFreeHorizontal(xCell, yCell+1)
                    || walls.IsVertical(xCell, yCell))
                    return;
                walls.horizontal[xCell, yCell] = 1;
                walls.horizontal[xCell, yCell + 1] = 2;
                matrix[xCell, yCell].down = null;
                matrix[xCell, yCell+1].down = null;
                matrix[xCell+1, yCell].up = null;
                matrix[xCell+1, yCell+1].up = null;
                p.walls--;
                currentPlayer = (p.num == 0) ? 1 : 0;
            }
        }

        public void PlayerTurn(int x, int y)
        {
            if (x > 530 || x < 0 || y > 530 || y < 0)
            {
                return;
            }
            if (x % 60 >= 50 && y % 60 >= 50)
                return;        
            if (x % 60 >= 50 || y % 60 >= 50)
                TurnWall(x,y);
            else
                TurnMove(x, y);
            
            UpdateBoard();
            if (Iswin())
            {
                MessageBox.Show("Winner!!!");
                InitPossibleMoves();
            }
        }

        public void InitPossibleMoves()
        {
            for (int i = 0; i < index_possibleMoves; i++)
            {
                // initialize all the previous possible moves
                int x = possibleMove[i].x;
                int y = possibleMove[i].y;
                this.board[x,y ].DeleteImage(x,y,numOfPlayers);
            }
            this.index_possibleMoves = 0;
        }

        public bool AnotherPlayerPos(int x, int y)
        {
            for (int i = 0; i <numOfPlayers; i++)
            {
                if (pArr[i].x == x && pArr[i].y == y)
                    return true;
            }
            return false;
        }

        public void PossibleMoves()
        {
            this.index_possibleMoves = 0;
            int yCell = pArr[currentPlayer].y;
            int xCell = pArr[currentPlayer].x;
            if (yCell > 0)
            {
                if (this.matrix[xCell, yCell].left != null)
                {
                    if (AnotherPlayerPos(xCell, yCell-1))
                    {
                        if (matrix[xCell, yCell - 1].left != null)
                        {
                            this.board[xCell, yCell - 2].Change_pm(currentPlayer);
                            possibleMove[index_possibleMoves].x = xCell;
                            possibleMove[index_possibleMoves].y = yCell - 2;
                            index_possibleMoves++;
                        }
                    }
                    else
                    {
                        this.board[xCell, yCell - 1].Change_pm(currentPlayer);
                        possibleMove[index_possibleMoves].x = xCell;
                        possibleMove[index_possibleMoves].y = yCell - 1;
                        index_possibleMoves++;
                    }
                }
            }
            if (yCell < 9 - 1)
            {
                if (this.matrix[xCell, yCell].right != null)
                {
                    if (AnotherPlayerPos(xCell, yCell + 1))
                    {
                        if (matrix[xCell, yCell + 1].right != null)
                        {
                            this.board[xCell, yCell + 2].Change_pm(currentPlayer);
                            possibleMove[index_possibleMoves].x = xCell;
                            possibleMove[index_possibleMoves].y = yCell + 2;
                            index_possibleMoves++;
                        }
                    }
                    else
                    {
                        this.board[xCell, yCell + 1].Change_pm(currentPlayer);
                        possibleMove[index_possibleMoves].x = xCell;
                        possibleMove[index_possibleMoves].y = yCell + 1;
                        index_possibleMoves++;
                    }
                }
            }
            if (xCell < 9 - 1)
            {
                if (this.matrix[xCell, yCell].down != null)
                {
                    if (AnotherPlayerPos(xCell + 1, yCell))
                    {
                        if (matrix[xCell+ 1, yCell ].down != null)
                        {
                            this.board[xCell + 2, yCell].Change_pm(currentPlayer);
                            possibleMove[index_possibleMoves].x = xCell+2;
                            possibleMove[index_possibleMoves].y = yCell ;
                            index_possibleMoves++;
                        }
                    }
                    else
                    {
                        this.board[xCell + 1, yCell].Change_pm(currentPlayer);
                        possibleMove[index_possibleMoves].x = xCell + 1;
                        possibleMove[index_possibleMoves].y = yCell;
                        index_possibleMoves++;
                    }
                }
            }
            if (xCell > 0)
            {
                if (this.matrix[xCell, yCell].up != null)
                {
                    if (AnotherPlayerPos(xCell - 1, yCell))
                    {
                        if (matrix[xCell - 1, yCell].up != null)
                        {
                            this.board[xCell - 2, yCell].Change_pm(currentPlayer);
                            possibleMove[index_possibleMoves].x = xCell - 2;
                            possibleMove[index_possibleMoves].y = yCell;
                            index_possibleMoves++;
                        }
                    }
                    else
                    {
                        this.board[xCell - 1, yCell].Change_pm(currentPlayer);
                        possibleMove[index_possibleMoves].x = xCell - 1;
                        possibleMove[index_possibleMoves].y = yCell;
                        index_possibleMoves++;
                    }
                }
            }
            UpdateBoard();
        }

        public bool Iswin()
        {
            return (pArr[currentPlayer].x == pArr[currentPlayer].goalx
                 || pArr[currentPlayer].y == pArr[currentPlayer].goaly);
        }

        public void PaintBoard(Graphics g)
        {
            //while (true)
            //{
                int x = 0;
                int y = 0;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    this.board[i, j].PaintCell(g, x, y);
                    bool isHor = walls.IsHorizontal(i, j);
                    bool isVer = walls.IsVertical(i, j);
                    if (isHor)
                    {
                        y += 50;
                        board[i, j].PaintHorizontal(g, x, y);
                        y -= 50;
                    }
                    if (isVer)
                    {
                        x += 50;
                        board[i, j].PaintVertical(g, x, y);
                        x -= 50;
                    }
                    x += 60;
                }
                x = 0;
                y += 60;
            }
        }
    }
}