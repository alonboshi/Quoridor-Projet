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
        private Point[] possibleMove = new Point[6];
        private int currentPlayer = 0;
        private Player[] pArr; // array of all players
        private int numOfPlayers; // num of players. at first is 0.
        private Vertices[,] matrix = new Vertices[ROW, COL];
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

        public bool CanReach(int xCell, int yCell,int place)
        {
            bool playerOne = false;
            bool playerTwo = false;
            bool playerThree = false;
            bool playerFour = false;

            // player 1
            if (numOfPlayers < 1)
                playerOne = true;
            else
            {
                for (int i = 0; i < ROW; i++)
                {
                    //Vertices[,] temp = (Vertices[,])this.matrix.Clone();
                    List<Vertices> temp_ver= TurnWallTemp(xCell, yCell, place);
                    if (!(GetShortestPath(pArr[0].x, pArr[0].y, 0, i).Count==0))
                    {
                        UndoTurnWallTemp(temp_ver, xCell, yCell, place);
                        playerOne = true;
                        break;
                    }
                    UndoTurnWallTemp(temp_ver, xCell, yCell, place);
                }
            }

            if (numOfPlayers < 2)
                playerTwo = true;
            else
            {
                for (int i = 0; i < ROW; i++)
                {
                    //Vertices[,] temp = (Vertices[,])this.matrix.Clone();
                    List<Vertices> temp_ver = TurnWallTemp(xCell, yCell, place);
                    if (!(GetShortestPath(pArr[1].x, pArr[1].y, 8, i).Count==0))
                    {
                        UndoTurnWallTemp(temp_ver, xCell, yCell, place);
                        playerTwo = true;
                        break;
                    }
                    UndoTurnWallTemp(temp_ver, xCell, yCell, place);
                }
            }
            if (numOfPlayers < 3)
                playerThree = true;
            if (numOfPlayers < 4)
                playerFour = true;
            
            return playerOne&&playerTwo&&playerThree&&playerFour;
        }

        public Player CurrentPlayer()
        {
            Player p = null;
            for (int i = 0; i < this.numOfPlayers; i++)
            {
                if (pArr[i].num == currentPlayer)
                {
                    p = pArr[i];
                    break;
                }
            }
            return p;
        }
        
        public List<Tuple<int,int>> GetShortestPath(int xStart,int yStart,
            int xEnd,int yEnd)
        {
            List<Tuple<int, int>> shortestPath = new List<Tuple<int, int>>();
            List<Vertices> path = SearchBFS(matrix[xStart, yStart], matrix[xEnd, yEnd]);
            foreach (Vertices item in path)
            {
                shortestPath.Add(new Tuple<int, int>(item.id/ROW,item.id%ROW));
            }
            return shortestPath;
        }

        public List<Vertices> SearchBFS(Vertices start, Vertices finish)
        {
            Vertices startVer, finishVer;
            startVer = start;
            finishVer = finish;

            List<Vertices> dispenser = new List<Vertices>();
            dispenser.Add(startVer);

            Dictionary<Vertices, Vertices> predecessors = new Dictionary<Vertices, Vertices>();
            predecessors.Add(startVer, finishVer);

            while (dispenser.Count!=0)
            {
                Vertices current=dispenser[0];
                dispenser.RemoveAt(0);
                if (current == finishVer)
                    break;

                foreach (Vertices nbr in current.GetNeighbors())
                {
                    if (!predecessors.ContainsKey(nbr))
                    {
                        predecessors.Add(nbr, current);
                        dispenser.Add(nbr);
                    }
                }
            }
            return ConstructPath(predecessors, startVer, finishVer);

        }
        
        private List<Vertices> ConstructPath(Dictionary<Vertices, Vertices> predecessors,
                                     Vertices startVertices, Vertices finishVertices)
        {

            // use predecessors to work backwards from finish to start,
            // all the while dumping everything into a linked list
            List<Vertices> path = new List<Vertices>();

            if (predecessors.ContainsKey(finishVertices))
            {
                Vertices currVertices = finishVertices;
                while (currVertices != startVertices)
                {
                    path.Insert(0, currVertices);
                    currVertices = predecessors[currVertices];
                }
                path.Insert(0, startVertices);
            }

            return path;
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
            Player p = CurrentPlayer();
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
            for (int row_index = 0; row_index < ROW; row_index++)
            {
                for (int col_index = 0; col_index < ROW; col_index++)
                {
                    matrix[row_index, col_index] = new Vertices();
                }
            }

            for (int row_index = 0; row_index < ROW; row_index++)
            {
                for (int col_index = 0; col_index < ROW; col_index++)
                {
                    //if (this.matrix[row_index, col_index].player == Players.EMPTY)
                    //    continue;
                    if (col_index > 0)
                    {
                        this.matrix[row_index, col_index].
                            left = (this.matrix[row_index, col_index - 1]);
                    }
                    if (col_index < ROW - 1)
                    {
                        this.matrix[row_index, col_index].
                            right = (this.matrix[row_index, col_index + 1]);
                    }
                    if (row_index < ROW - 1)
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
            Player p = CurrentPlayer();
            if (p == null)
                return;
            for (int i = 0; i < index_possibleMoves; i++)
            {
                if (possibleMove[i].x == x && possibleMove[i].y == y)
                {
                    board[p.x, p.y].DeleteImage(p.x,p.y,numOfPlayers);
                    board[x, y].Change(p.num);
                    p.x = x;
                    p.y = y;
                    currentPlayer = (p.num == 0) ? 1 : 0;
                }
            }
        }

        public void ShowWall(Point p) 
        { 
            
        }

        public void UndoTurnWallTemp(List<Vertices> temp_ver, int x, int y, int place)
        {
            if (place == 1)
            {
                matrix[x, y].right = temp_ver[0];
                matrix[x + 1, y].right = temp_ver[1];
                matrix[x, y + 1].left = temp_ver[2];
                matrix[x + 1, y + 1].left = temp_ver[0];
            }
            else if (place == 0)
            {
                matrix[x, y].down = temp_ver[0];
                matrix[x, y + 1].down = temp_ver[1];
                matrix[x + 1, y].up = temp_ver[2];
                matrix[x + 1, y + 1].up = temp_ver[3];
            }
        }
        
        public List<Vertices> TurnWallTemp( int x, int y,int place)
        {
            List<Vertices> temp_ver = new List<Vertices>();
            if (place == 1)
            {
                temp_ver.Add(matrix[x, y].right); 
                temp_ver.Add(matrix[x + 1, y].right);
                temp_ver.Add(matrix[x, y+1].right);
                temp_ver.Add(matrix[x+1, y+1].right);
                matrix[x, y].right = null;
                matrix[x + 1, y].right = null;
                matrix[x, y + 1].left = null;
                matrix[x + 1, y + 1].left = null;
                return temp_ver;
            }
            else if(place == 0)
            {
                temp_ver.Add(matrix[x, y].down);
                temp_ver.Add(matrix[x, y+1].down);
                temp_ver.Add(matrix[x+1, y].up);
                temp_ver.Add(matrix[x + 1, y + 1].up);
                matrix[x, y].down = null;
                matrix[x, y + 1].down = null;
                matrix[x + 1, y].up = null;
                matrix[x + 1, y + 1].up = null;
                return temp_ver;
            }
            return temp_ver;
        }



        public bool TurnWall(int x, int y,int place) // place 1 for vertical, 0 for horizontal
        {
            Player p = CurrentPlayer();
            if (p == null)
                return false;
            if (p.walls == 0)
                return false;
            // Vertical wall
            if (place==1)
            {
                if (!walls.IsFreeVertical(x, y) || !walls.IsFreeVertical(x + 1, y) 
                    || walls.IsHorizontal(x,y))
                    return false;
                if (!CanReach(x, y, place))
                    return false;
                walls.vertical[x, y] = 1;
                walls.vertical[x + 1, y ] = 2;
                matrix[x, y].right = null;
                matrix[x+1, y].right = null;
                matrix[x , y+1].left = null;
                matrix[x + 1, y+1].left = null;
                p.walls--;
                currentPlayer = (p.num == 0) ? 1 : 0;
            }
            if (place==0)
            {
                if (!walls.IsFreeHorizontal(x, y)|| !walls.IsFreeHorizontal(x, y+1)
                    || walls.IsVertical(x, y))
                    return false;
                if (!(CanReach(x, y, place)))
                    return false;
                walls.horizontal[x, y] = 1;
                walls.horizontal[x, y + 1] = 2;
                matrix[x, y].down = null;
                matrix[x, y+1].down = null;
                matrix[x+1, y].up = null;
                matrix[x+1, y+1].up = null;
                p.walls--;
                currentPlayer = (p.num == 0) ? 1 : 0;
            }
            return true;
        }

        public void PlayerTurn(int x, int y)
        {
            if (x > 530 || x < 0 || y > 530 || y < 0)
                return;
            if (x % 60 >= 50 && y % 60 >= 50)
                return;
            if (x % 60 >= 50 || y % 60 >= 50)
            {
                // Vertical wall
                if (x % 60 >= 50 && y >= 0 && y <= 460)
                    if (!TurnWall(y / 60, x / 60, 1))
                        return;
                //horizontal wall
                if (y % 60 >= 50 && x >= 0 && x <= 470)
                    if (!TurnWall(y / 60, x / 60, 0))
                        return;
            }
            else
                TurnMove(y / 60, x / 60);
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
                        else {
                            if (matrix[xCell, yCell - 1].up != null)
                            {
                                this.board[xCell-1, yCell - 1].Change_pm(currentPlayer);
                                possibleMove[index_possibleMoves].x = xCell - 1;
                                possibleMove[index_possibleMoves].y = yCell - 1;
                                index_possibleMoves++;
                            }
                            if (matrix[xCell, yCell - 1].down != null)
                            {
                                this.board[xCell+1, yCell - 1].Change_pm(currentPlayer);
                                possibleMove[index_possibleMoves].x = xCell + 1;
                                possibleMove[index_possibleMoves].y = yCell - 1;
                                index_possibleMoves++;
                            }
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
            if (yCell < ROW - 1)
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
                        else
                        {
                            if (matrix[xCell, yCell + 1].up != null)
                            {
                                this.board[xCell - 1, yCell + 1].Change_pm(currentPlayer);
                                possibleMove[index_possibleMoves].x = xCell - 1;
                                possibleMove[index_possibleMoves].y = yCell + 1;
                                index_possibleMoves++;
                            }
                            if (matrix[xCell, yCell + 1].down != null)
                            {
                                this.board[xCell + 1, yCell + 1].Change_pm(currentPlayer);
                                possibleMove[index_possibleMoves].x = xCell + 1;
                                possibleMove[index_possibleMoves].y = yCell + 1;
                                index_possibleMoves++;
                            }
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
            if (xCell < ROW - 1)
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
                        else
                        {
                            if (matrix[xCell+1, yCell].right != null)
                            {
                                this.board[xCell + 1, yCell + 1].Change_pm(currentPlayer);
                                possibleMove[index_possibleMoves].x = xCell + 1;
                                possibleMove[index_possibleMoves].y = yCell + 1;
                                index_possibleMoves++;
                            }
                            if (matrix[xCell+1, yCell ].left != null)
                            {
                                this.board[xCell + 1, yCell - 1].Change_pm(currentPlayer);
                                possibleMove[index_possibleMoves].x = xCell + 1;
                                possibleMove[index_possibleMoves].y = yCell - 1;
                                index_possibleMoves++;
                            }
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
                        else
                        {
                            if (matrix[xCell - 1, yCell].right != null)
                            {
                                this.board[xCell - 1, yCell + 1].Change_pm(currentPlayer);
                                possibleMove[index_possibleMoves].x = xCell -1;
                                possibleMove[index_possibleMoves].y = yCell + 1;
                                index_possibleMoves++;
                            }
                            if (matrix[xCell - 1, yCell].left != null)
                            {
                                this.board[xCell - 1, yCell - 1].Change_pm(currentPlayer);
                                possibleMove[index_possibleMoves].x = xCell - 1;
                                possibleMove[index_possibleMoves].y = yCell - 1;
                                index_possibleMoves++;
                            }
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