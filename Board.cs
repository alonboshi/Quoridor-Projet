using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace QuoridorProject
{
    /// <summary>
    /// 
    /// </summary>
    public class Board
    {
        int choice;

        public Dictionary<int, int> scores = new Dictionary<int, int>();
        public int count = 0;

    public struct Point {
            public int x;
            public int y;
            public int place;
        }
        public string validWall =null;
        public Wall walls = new Wall();
        public int index_possibleMoves = 0;
        public Point[] possibleMove = new Point[6];
        public int index_possibleWalls = 0;
        public List<Point> possibleWalls = new List<Point>();
        public int currentPlayer = 0;
        public Player[] pArr; // array of all players
        public int numOfPlayers; // num of players. at first is 0.
        private Vertices[,] matrix = new Vertices[ROW, COL];
        public const int ROW = 9;
        public const int COL = 9;
        internal Cell[,] board;

        // ---------------------------constructor--------------------------------

        public Board(int choice)
        {
            this.choice = choice;
            pArr = new Player[4]; // max of players - 4
            this.numOfPlayers = 0; // current num of players
            scores.Add(0, 1);
            scores.Add(1, -1);

            for (int row_index = 0; row_index < ROW; row_index++)
            {
                for (int col_index = 0; col_index < ROW; col_index++)
                {
                    matrix[row_index, col_index] = new Vertices();
                }
            }
            SetVerticesMatrix();
        }

        /// <summary>
        /// Function which search through all the players and returns the current player.
        /// </summary>
        /// <returns>The current player</returns>
        public Player CurrentPlayer()
        {
            count++;
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

        /// <summary>
        /// Adds player to the game till maximum 4. 
        /// Every player has its x,y and goalx,goaly and other inviduals fields. 
        /// </summary>
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

        /// <summary>
        /// Sets the Cell matrix and initializes each cell with its image.
        /// </summary>
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

        /// <summary>
        /// finds the current player and return how many walls left
        /// </summary>
        /// <returns>A tuple of the left walls and the  current player's num </returns>
        public Tuple<int, int> GetWalls()
        {
            Player p = CurrentPlayer();
            return Tuple.Create(p.walls, currentPlayer);
        }

        /// <summary>
        /// Sets the Vertices matrix and connect every vertice to its neighbors
        /// </summary>
        private void SetVerticesMatrix()
        {

            for (int row_index = 0; row_index < ROW; row_index++)
            {
                for (int col_index = 0; col_index < ROW; col_index++)
                {
                    if (col_index > 0) // have left neighbor
                    {
                        this.matrix[row_index, col_index].
                            left = (this.matrix[row_index, col_index - 1]);
                    }
                    if (col_index < ROW - 1) // have right neighbor
                    {
                        this.matrix[row_index, col_index].
                            right = (this.matrix[row_index, col_index + 1]);
                    }
                    if (row_index < ROW - 1) // have down neighbor
                    {
                        this.matrix[row_index, col_index].
                            down = (this.matrix[row_index + 1, col_index]);
                    }
                    if (row_index > 0) // have up neighbor
                    {
                        this.matrix[row_index, col_index].
                            up = (this.matrix[row_index - 1, col_index]);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the location of the player's mouse click and active or the wall turn or the move turn.
        /// by the indexes
        /// </summary>
        /// <param name="x">x index</param>
        /// <param name="y">y index</param>
        public int PlayerTurn(int x, int y)
        {
            // outside the area of the board
            if (x > 530 || x < 0 || y > 530 || y < 0)
                return 0;
            // at the middle of vertical and horizontal wall
            if (x % 60 >= 50 && y % 60 >= 50)
                return 0;
            // wall
            if (x % 60 >= 50 || y % 60 >= 50)
            {
                // Vertical wall
                if (x % 60 >= 50 && y >= 0 && y <= 460)
                    if (!TurnWall(y / 60, x / 60, 1))
                        return 0;
                //horizontal wall
                if (y % 60 >= 50 && x >= 0 && x <= 470)
                    if (!TurnWall(y / 60, x / 60, 0))
                        return 0;

                if (this.choice == 1)
                    return 1;
                    //AI.Move(this, CurrentPlayer());
            }
            else
            {
                // move
                return TurnMove(y / 60, x / 60);
                //ChangePlayer(CurrentPlayer());
            }
            UpdateBoard();
            return 0;
            //if (Iswin()!=-1)
            //{
            //    MessageBox.Show("Winner!!!");
            //    InitPossibleMoves();
            //}
        }

        /// <summary>
        /// Function which move the current player to one of its neighbors by its mouse clicks.
        /// </summary>
        /// <param name="x">x index</param>
        /// <param name="y">y index</param>
        public int TurnMove(int x, int y)
        {
            validWall = null ;
            bool check = false;
            Player p = CurrentPlayer();
            if (p == null)
                return 0;
            for (int i = 0; i < index_possibleMoves; i++)
            {
                if (possibleMove[i].x == x && possibleMove[i].y == y)
                {
                    board[p.x, p.y].DeleteImage(p.x,p.y,numOfPlayers);
                    board[x, y].Change(p.num);
                    p.x = x;
                    p.y = y;
                    check = true;
                    ChangePlayer(p);
                    InitPossibleMoves();
                    PossibleMoves(null);
                    UpdateBoard();
                    break;
                }
            }
            if (this.choice == 1 && check)
                return 1;
                //AI.Move(this, CurrentPlayer());
            if (!check)
                validWall = " Illeagal Place to move! ";
            return 0;
        }

        public Point TurnMoveTemp(int x, int y)
        {
            Point res;
            Player p = CurrentPlayer();
            res.x = p.x;
            res.y = p.y;
            res.place = -1;
            p.x = x;
            p.y = y;
            ChangePlayer(p);
            return res;
        }

        public void UndoTurnMoveTemp(Point undo)
        {
            Player p = CurrentPlayer();
            p.x = undo.x;
            p.y = undo.y;
            ChangePlayer(p);
        }

        public void ChangePlayer(Player p) {
            currentPlayer = (p.num == 0) ? 1 : 0;
        }

        public Player ChangePlayerWithReturn(Player p)
        {
            currentPlayer = (p.num == 0) ? 1 : 0;
            return pArr[currentPlayer];
        }

        /// <summary>
        /// Function which places the wall in the given indexes.
        /// </summary>
        /// <param name="x">x index</param>
        /// <param name="y">y index</param>
        /// <param name="place">1 for vertical wall and 0 for horizontal wall.</param>
        /// <returns>Returns true if the indexes were good and false if not.</returns>
        public bool TurnWall(int x, int y, int place) // place 1 for vertical, 0 for horizontal
        {
            validWall = null;
            Player p = CurrentPlayer();
            if (p == null)
                return false;
            if (p.walls == 0)
            {
                validWall = " You ran out of walls! ";
                return false;
            }
            // Vertical wall
            if (place == 1)
            {
                if (!walls.IsFreeVertical(x, y) || !walls.IsFreeVertical(x + 1, y)
                    || walls.IsHorizontal(x, y))
                {
                    validWall = "Illeagal place to place a wall ! ";
                    return false;
                }
                if (!CanReach(x, y, place))
                {
                    validWall = "The wall blocks some player! ";
                    return false;
                }
                walls.vertical[x, y] = 1;
                walls.vertical[x + 1, y] = 2;
                matrix[x, y].right = null;
                if(x!=8)
                    matrix[x + 1, y].right = null;
                if(y!=8)
                    matrix[x, y + 1].left = null;
                if(y!=8&&x!=8)
                    matrix[x + 1, y + 1].left = null;
                p.walls--;
            }
            if (place == 0)
            {
                if (!walls.IsFreeHorizontal(x, y) || !walls.IsFreeHorizontal(x, y + 1)
                    || walls.IsVertical(x, y))
                {
                    validWall = "Illeagal place to place a wall ! ";
                    return false;
                }
                if (!(CanReach(x, y, place)))
                {
                    validWall = "The wall blockes some player! ";
                    return false;
                }
                walls.horizontal[x, y] = 1;
                walls.horizontal[x, y + 1] = 2;
                matrix[x, y].down = null;
                if (y != 8)
                    matrix[x, y + 1].down = null;
                if (x != 8)
                    matrix[x + 1, y].up = null;
                if (y != 8 && x != 8)
                    matrix[x + 1, y + 1].up = null;
                p.walls--;
            }
            //if (choice == 2)
             ChangePlayer(p);
            return true;
        }

        public void ShowWall(Point p) 
        { 
            
        }

        /// <summary>
        /// Function which places the wall in the given indexes. just temporary.
        /// </summary>
        /// <param name="x">x index</param>
        /// <param name="y">y index</param>
        /// <param name="place">1 for vertical wall and 0 for horizontal wall.</param>
        /// <returns>Returns a list of the previous vertices that being interupted by the new wall</returns>
        public List<Vertices> TurnWallTemp( int x, int y,int place)
        {
            List<Vertices> temp_ver = new List<Vertices>();
            if (place == 1)
            {

                temp_ver.Add(matrix[x, y].right);
                if (x + 1 != 9)
                    temp_ver.Add(matrix[x + 1, y].right);
                if (y + 1 != 9)
                    temp_ver.Add(matrix[x, y + 1].right);
                if (y + 1 != 9 && x + 1 != 9)
                    temp_ver.Add(matrix[x + 1, y + 1].right);
                matrix[x, y].right = null;
                if (x + 1 != 9)
                    matrix[x + 1, y].right = null;
                if (y + 1 != 9)
                    matrix[x, y + 1].left = null;
                if (y + 1 != 9 && x + 1 != 9)
                    matrix[x + 1, y + 1].left = null;
                return temp_ver;
            }
            else if (place == 0)
            {
                temp_ver.Add(matrix[x, y].down);
                if (y + 1 != 9)
                    temp_ver.Add(matrix[x, y + 1].down);
                if (x + 1 != 9)
                    temp_ver.Add(matrix[x + 1, y].up);
                if (y + 1 != 9 && x + 1 != 9)
                    temp_ver.Add(matrix[x + 1, y + 1].up);
                matrix[x, y].down = null;
                if (y + 1 != 9)
                    matrix[x, y + 1].down = null;
                if (x + 1 != 9)
                    matrix[x + 1, y].up = null;
                if (y + 1 != 9 && x + 1 != 9)
                    matrix[x + 1, y + 1].up = null;
                return temp_ver;
            }
            return temp_ver;
        }

        /// <summary>
        /// Function which deletes the previous wall that was just temorary.
        /// </summary>
        /// <param name="temp_ver">
        /// A list of the vertices that need to turn back to what they were before the new wall
        /// </param>
        /// <param name="x">x index</param>
        /// <param name="y">y index</param>
        /// <param name="place">1 for vertical wall and 0 for horizontal wall.</param>
        public void UndoTurnWallTemp(List<Vertices> temp_ver, int x, int y, int place)
        {
            if (place == 1)
            {
                matrix[x, y].right = temp_ver[0];
                if (x + 1 != 9&&temp_ver.Count>=2)
                    matrix[x + 1, y].right = temp_ver[1];
                if (y + 1 != 9 && temp_ver.Count >= 3)
                    matrix[x, y + 1].left = temp_ver[2];
                if (y + 1 != 9 && x + 1 != 9 && temp_ver.Count >= 4)
                    matrix[x + 1, y + 1].left = temp_ver[3];
            }
            else if (place == 0)
            {
                matrix[x, y].down = temp_ver[0];
                if (y + 1 != 9 && temp_ver.Count >= 2)
                    matrix[x, y + 1].down = temp_ver[1];
                if (x + 1 != 9 && temp_ver.Count >= 3)
                    matrix[x + 1, y].up = temp_ver[2];
                if (y + 1 != 9 && x + 1 != 9 && temp_ver.Count >= 4)
                    matrix[x + 1, y + 1].up = temp_ver[3];
            }
        }     

        /// <summary>
        /// Function that checks if in a spesific location, there is another player.  
        /// </summary>
        /// <param name="x">row index</param>
        /// <param name="y">col index</param>
        /// <returns>Returns true if in the given location, there is a player, otherwise false.</returns>
        public bool AnotherPlayerPos(int x, int y)
        {
            for (int i = 0; i <numOfPlayers; i++)
            {
                if (pArr[i].x == x && pArr[i].y == y)
                    return true;
            }
            return false;
        }

        public bool InLimits(int x,int y)
        {
            return x >= 0 && x <= 8 && y >= 0 && y <= 8;
        }

        /// <summary>
        /// A function that finds all of the cuurent player's possible moves. and adds them to the array possibleMoves
        /// </summary>
        internal void PossibleMoves(Vertices current)
        {
            int yCell = 0;
            int xCell = 0;
            if (current == null)
            {
                this.index_possibleMoves = 0;
                yCell = pArr[currentPlayer].y;
                xCell = pArr[currentPlayer].x;
            }
            else {
                this.index_possibleMoves = 0;
                yCell = current.id%ROW;
                xCell = current.id/COL;
            }
            if (yCell > 0)
            {
                if (InLimits(xCell,yCell)&&this.matrix[xCell, yCell].left != null )
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
                if (InLimits(xCell, yCell) && this.matrix[xCell, yCell].right != null)
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
                if (InLimits(xCell, yCell) && this.matrix[xCell, yCell].down != null)
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
                if (InLimits(xCell, yCell) && this.matrix[xCell, yCell].up != null)
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
        }

        /// <summary>
        /// Initialize the possibleMoves array to empty.
        /// </summary>
        public void InitPossibleMoves()
        {
            for (int i = 0; i < index_possibleMoves; i++)
            {
                // initialize all the previous possible moves
                int x = possibleMove[i].x;
                int y = possibleMove[i].y;
                this.board[x, y].DeleteImage(x, y, numOfPlayers);
            }
            this.index_possibleMoves = 0;
        }

        public void PossibleWalls(Player player)
        {
            for (int i = player.x - 1; i < player.x + 1; i++)
            {
                for (int j = player.y - 2; j < player.y + 2; j++)
                {
                    if (i <= 8 && i >= 0 && j <= 8 && j >= 0)
                    {
                        if (i != 8 && walls.IsFreeVertical(i, j) && walls.IsFreeVertical(i + 1, j))
                        {
                            Point point;
                            point.x = i;
                            point.y = j;
                            point.place = 1;
                            possibleWalls.Add(point);
                            index_possibleWalls++;
                        }
                        if (j != 8 && walls.IsFreeHorizontal(i, j) && walls.IsFreeHorizontal(i, j + 1))
                        {
                            Point point;
                            point.x = i;
                            point.y = j;
                            point.place = 0;
                            possibleWalls.Add(point);
                            index_possibleWalls++;
                        }
                    }
                }

            }
            for (int i = player.y - 1; i < player.y + 1; i++)
            {
                for (int j = player.x - 2; j < player.x + 2; j++)
                {
                    if (i <= 8 && i >= 0 && j <= 8 && j >= 0)
                    {
                        if (i != 8 && walls.IsFreeVertical(i, j) && walls.IsFreeVertical(i + 1, j))
                        {
                            Point point;
                            point.x = i;
                            point.y = j;
                            point.place = 1;
                            possibleWalls.Add(point);
                            index_possibleWalls++;
                        }
                        if (j != 8 && walls.IsFreeHorizontal(i, j) && !walls.IsFreeHorizontal(i, j + 1))
                        {
                            Point point;
                            point.x = i;
                            point.y = j;
                            point.place = 0;
                            possibleWalls.Add(point);
                            index_possibleWalls++;
                        }
                    }
                }
            }


            //int length = this.walls.vertical.GetLength(0);
            //int width = this.walls.vertical.GetLength(1);
            //for (int i = 0; i < length; i++)
            //{
            //    for (int j = 0; j < width; j++)
            //    {
            //        if (i != 8 && walls.IsFreeVertical(i, j) && walls.IsFreeVertical(i + 1, j))
            //        {
            //            Point point;
            //            point.x = i;
            //            point.y = j;
            //            point.place = 1;
            //            possibleWalls.Add(point);
            //            index_possibleWalls++;
            //        }
            //        if (j != 8 && walls.IsFreeHorizontal(i, j) && !walls.IsFreeHorizontal(i, j + 1))
            //        {
            //            Point point;
            //            point.x = i;
            //            point.y = j;
            //            point.place = 0;
            //            possibleWalls.Add(point);
            //            index_possibleWalls++;
            //        }
            //    }
            //}
        }

        public void InitPossibleWalls()
        {
            this.possibleWalls.Clear();
            index_possibleWalls = 0;
        }

        /// <summary>
        /// For each player it updates its place and changes the image in the view board by its place.
        /// </summary>
        public void UpdateBoard()
        {
            SetVerticesMatrix();
            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {
                    if (walls.IsVertical(i,j))
                    {
                        matrix[i, j].right = null;
                        if (i != 8)
                            matrix[i + 1, j].right = null;
                        if (j != 8)
                            matrix[i, j + 1].left = null;
                        if (j != 8 && i != 8)
                            matrix[i + 1, j + 1].left = null;
                    }
                    if (walls.IsHorizontal(i,j))
                    {
                        matrix[i, j].down = null;
                        if (j != 8)
                            matrix[i, j + 1].down = null;
                        if (i != 8)
                            matrix[i + 1, j].up = null;
                        if (j != 8 && i != 8)
                            matrix[i + 1, j + 1].up = null;
                    }
                }
            }
            for (int i = 0; i < numOfPlayers; i++)
            {
                board[pArr[i].x, pArr[i].y].Change(pArr[i].num);
            }
        }

        /// <summary>
        /// The function gets a location of placing a new wall on the board and checks if this wall w'ont 
        /// block any player from its location to its goal.
        /// </summary>
        /// <param name="xCell">Row index</param>
        /// <param name="yCell">Col index</param>
        /// <param name="place">1 for vertical wall, 0 for horizontal wall</param>
        /// <returns>
        /// Boolean var which true means that the wall can be placed in the given location
        /// and falce if not.
        /// </returns>
        public bool CanReach(int xCell, int yCell, int place)
        {
            UpdateBoard();

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
                    // Adding the wall to the board to check if there is not problem.
                    List<Vertices> temp_ver = TurnWallTemp(xCell, yCell, place);
                    if (!(GetShortestPath(pArr[0].x, pArr[0].y, 0, i).Count == 0))
                    {
                        // UnAdding the wall
                        UndoTurnWallTemp(temp_ver, xCell, yCell, place);
                        playerOne = true;
                        break;
                    }
                    // UnAdding the wall
                    UndoTurnWallTemp(temp_ver, xCell, yCell, place);
                }
            }

            // player 2
            if (numOfPlayers < 2)
                playerTwo = true;
            else
            {
                for (int i = 0; i < ROW; i++)
                {
                    // Adding the wall to the board to check if there is not problem.
                    List<Vertices> temp_ver = TurnWallTemp(xCell, yCell, place);
                    if (!(GetShortestPath(pArr[1].x, pArr[1].y, 8, i).Count == 0))
                    {
                        // UnAdding the wall
                        UndoTurnWallTemp(temp_ver, xCell, yCell, place);
                        playerTwo = true;
                        break;
                    }
                    // UnAdding the wall
                    UndoTurnWallTemp(temp_ver, xCell, yCell, place);
                }
            }
            //------------------------------------------------------------------------------------
            // player 3
            if (numOfPlayers < 3)
                playerThree = true;
            //------------------------------------------------------------------------------------
            // player 4
            if (numOfPlayers < 4)
                playerFour = true;

            return playerOne && playerTwo && playerThree && playerFour;
        }

        /// <summary>
        /// Function which gets a location of the player and a location of its goal and returns a list 
        /// of the way to the goal location.
        /// </summary>
        /// <param name="xStart">Row index of the player's location</param>
        /// <param name="yStart">Col index of the player's location</param>
        /// <param name="xEnd">Row index of the player's goal location</param>
        /// <param name="yEnd">Col index of the player's goal location</param>
        /// <returns>
        /// A list that shows the way to the goal location.
        /// If there is no way, the list will be empty.
        /// </returns>
        public List<Tuple<int, int>> GetShortestPath(int xStart, int yStart,
            int xEnd, int yEnd)
        {
            List<Tuple<int, int>> shortestPath = new List<Tuple<int, int>>();
            List<Vertices> path = SearchBFS(matrix[xStart, yStart], matrix[xEnd, yEnd]);
            foreach (Vertices item in path)
            {
                shortestPath.Add(new Tuple<int, int>(item.id / ROW, item.id % ROW));
            }
            return shortestPath;
        }

        private List<Vertices> SearchBFS(Vertices start, Vertices finish)
        {
            Vertices startVer, finishVer;
            startVer = start;
            finishVer = finish;

            List<Vertices> dispenser = new List<Vertices>();
            dispenser.Add(startVer);

            Dictionary<Vertices, Vertices> predecessors = new Dictionary<Vertices, Vertices>();
            predecessors.Add(startVer, finishVer);

            while (dispenser.Count != 0)
            {
                Vertices current = dispenser[0];
                dispenser.RemoveAt(0);
                if (current == finishVer)
                    break;
                InitPossibleMoves();
                PossibleMoves(current);
                List<Vertices> cur = current.GetNeighbors(possibleMove,index_possibleMoves,matrix);
                foreach (Vertices nbr in cur)
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

        public int Iswin()
        {
            for (int i = 0; i < numOfPlayers; i++)
            {
                if (pArr[i].x == pArr[i].goalx || pArr[i].y == pArr[i].goaly)
                    return i;
            }
            return -1;
        }

        public void PaintBoard(Graphics g)
        {
           
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

        public void bestMove()
        {
            // AI to make its turn
            int bestScore = int.MinValue;
            int x = -1, y = -1;
            for (int i = 0; i < index_possibleMoves; i++)
            {
                Point undo=TurnMoveTemp(possibleMove[i].x, possibleMove[i].y);
                count++;
                int score = minimax(0, false);
                UndoTurnMoveTemp(undo);
                if (score > bestScore)
                {
                    bestScore = score;
                    x = possibleMove[i].x;
                    y = possibleMove[i].y;
                }
              
            }
            TurnMove(x, y);
            ChangePlayer(CurrentPlayer());
        }

        public int minimax(int depth, bool isMaximizing)
        {
            int result = Iswin();
            if (result!=-1)
            {
                return scores[result];
            }

            if (isMaximizing)
            {
                int bestScore = int.MinValue;
                for (int i = 0; i < index_possibleMoves; i++)
                {
                    Point undo = TurnMoveTemp(possibleMove[i].x, possibleMove[i].y);
                    int score = minimax(depth + 1, false);
                    UndoTurnMoveTemp(undo);
                    bestScore = Math.Max(score, bestScore);
                }
                return bestScore;
            }
            else
            {
                int bestScore = int.MinValue;
                for (int i = 0; i < index_possibleMoves; i++)
                {
                    Point undo = TurnMoveTemp(possibleMove[i].x, possibleMove[i].y);
                    int score = minimax(depth + 1, true);
                    UndoTurnMoveTemp(undo);
                    bestScore = Math.Min(score, bestScore);
                }
                return bestScore;
            }
        }
    }
}