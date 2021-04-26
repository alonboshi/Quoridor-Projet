using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QuoridorProject.Board;

namespace QuoridorProject
{
    public static class AI
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool Move(Board matrix, Player player)
        {
            int indexBest = 0;   // how many similar best moves
            List<Tuple<int, int>> minList_AI = new List<Tuple<int, int>>(); // the list of the AI's shortest way to the goal. 
            List<List<Tuple<int, int>>> allBest = new List<List<Tuple<int, int>>>(); // A list of all the AI's shortest way with the same count of steps to goal
            int minVal = int.MaxValue; // minimum steps to the goal
            for (int i = 0; i < Board.COL; i++)
            {
                // List of Locations on the wall which represents the shortest way
                List<Tuple<int, int>> tempList = matrix.GetShortestPath(player.x, player.y, player.goalx, i); // way from player's location to col 0-9 in his row goal

                // equal to the minimum
                if (tempList.Count == minVal)
                {
                    // adds to array of differen ways with the same minimum value
                    allBest.Add(tempList);
                    indexBest++;
                }

                // if there is a way and its shorter than the minimum 
                if (tempList.Count != 0 && tempList.Count < minVal)
                {
                    // replaces min way and initializes the list of all same best ways                                                     
                    minList_AI = tempList;
                    minVal = tempList.Count;
                    allBest.Clear();
                    allBest.Add(tempList);
                    indexBest = 1;
                }


            }
            Player opponent = matrix.ChangePlayerWithReturn(matrix.CurrentPlayer()); // the human
            List<Tuple<int, int>> minList_human = null; // the list of the human's shortest way to the goal. 
            int minVal2 = int.MaxValue; // minimum steps to the goal
            int min_goalcell_index = -1; // the index to the goal cell with the shortest way
            for (int i = 0; i < 9; i++)
            {
                // finds the human's shortest way to goal
                List<Tuple<int, int>> tempList = matrix.GetShortestPath(opponent.x, opponent.y, opponent.goalx, i);
                // if there is a way and its shorter than the minimum 
                if (tempList.Count != 0 && tempList.Count < minVal2)
                {
                    minList_human = tempList;
                    minVal2 = tempList.Count;
                    min_goalcell_index = i;
                }

            }
            // if there is no way
            if (minList_AI == null || minList_human == null || min_goalcell_index == -1)
                return false;

            matrix.ChangePlayer(matrix.CurrentPlayer()); // current player changed back to AI

            if (player.walls != 0 && minVal > minVal2) // opponent's way is shorter and we need to put a good block
            {
                int indexWallBest = 0;
                List<Tuple<int, int>> minListWall = null;
                List<Point> wallChangeBest = new List<Point>();
                matrix.InitPossibleWalls();
                matrix.PossibleWalls(opponent);
                // puts a wall and checks how it affected the opponent's way.
                for (int i = 0; i < matrix.index_possibleWalls; i++)
                {
                    List<Vertices> list = matrix.TurnWallTemp(matrix.possibleWalls[i].x, matrix.possibleWalls[i].y, matrix.possibleWalls[i].place);
                    List<Tuple<int, int>> tempList = matrix.GetShortestPath(opponent.x, opponent.y, opponent.goalx, min_goalcell_index);
                    matrix.UndoTurnWallTemp(list, matrix.possibleWalls[i].x, matrix.possibleWalls[i].y, matrix.possibleWalls[i].place);

                    // if the wall made the biggest interuption
                    if (tempList.Count > minVal2)
                    {
                        minVal2 = tempList.Count;
                        minListWall = tempList;
                        wallChangeBest.Clear();
                        Point point;
                        point.x = matrix.possibleWalls[i].x;
                        point.y = matrix.possibleWalls[i].y;
                        point.place = matrix.possibleWalls[i].place;
                        wallChangeBest.Add(point);
                        indexWallBest = 1;
                    }

                    //same interuption as the best
                    else if (tempList.Count == minVal2)
                    {
                        Point point;
                        minListWall = tempList;
                        point.x = matrix.possibleWalls[i].x;
                        point.y = matrix.possibleWalls[i].y;
                        point.place = matrix.possibleWalls[i].place;
                        wallChangeBest.Add(point);
                        indexWallBest++;
                    }
                }
                // if there is a way 
                if (minListWall != null && (wallChangeBest.Count < matrix.index_possibleWalls / 2 || minListWall.Count < 5))
                {
                    //  find the best option to place the wall for the AI itself from all the options in the list.
                    Point THEWALL = Max_Benefit(wallChangeBest, matrix, player, min_goalcell_index);
                    if (matrix.CanReach(THEWALL.x, THEWALL.y, THEWALL.place))
                    {
                        if (matrix.TurnWall(THEWALL.x, THEWALL.y, THEWALL.place))
                        {
                            // succeed 
                            return true;
                        }
                    }
                }
            }

            // if there is optional move
            if (allBest.Count != 0)
            {
                // picks a random step from all the moves that got the same rank
                Random rnd = new Random();
                int randomBest = rnd.Next(0, indexBest);

                // makes a move
                minList_AI = allBest[randomBest];
                matrix.board[player.x, player.y].DeleteImage(player.x, player.y, matrix.numOfPlayers);
                matrix.board[minList_AI[1].Item1, minList_AI[1].Item2].Change(player.num);
                player.x = minList_AI[1].Item1;
                player.y = minList_AI[1].Item2;
                matrix.ChangePlayer(player);
                matrix.InitPossibleMoves();
                matrix.PossibleMoves(null);
                matrix.UpdateBoard();
                return true;
            }
            //makes a move by his first possible move
            int x = matrix.possibleMove[0].x;
            int y = matrix.possibleMove[0].y;
            matrix.board[player.x, player.y].DeleteImage(player.x, player.y, matrix.numOfPlayers);
            matrix.board[x, y].Change(player.num);
            player.x = x;
            player.y = y;
            matrix.ChangePlayer(player);
            matrix.InitPossibleMoves();
            matrix.PossibleMoves(null);
            matrix.UpdateBoard();
            return true;

        }

        public static Point Max_Benefit(List<Point> points, Board matrix, Player player, int min_goalcell_index)
        {
            int indexWallBest = 0;
            List<Tuple<int, int>> minListWall = null;
            List<Point> wallChangeBest = new List<Point>();
            matrix.InitPossibleWalls();
            matrix.PossibleWalls(player);
            int minVal = int.MaxValue;
            for (int i = 0; i < points.Count; i++)
            {
                List<Vertices> list = matrix.TurnWallTemp(points[i].x, points[i].y, points[i].place);
                List<Tuple<int, int>> tempList = matrix.GetShortestPath(player.x, player.y, player.goalx, min_goalcell_index);
                matrix.UndoTurnWallTemp(list, points[i].x, points[i].y, points[i].place);

                if (tempList.Count < minVal)
                {
                    minVal = tempList.Count;
                    minListWall = tempList;
                    wallChangeBest.Clear();
                    Point point;
                    point.x = points[i].x;
                    point.y = points[i].y;
                    point.place = points[i].place;
                    wallChangeBest.Add(point);
                    indexWallBest = 1;
                }
                else if (tempList.Count == minVal)
                {
                    Point point;
                    minListWall = tempList;
                    point.x = points[i].x;
                    point.y = points[i].y;
                    point.place = points[i].place;
                    wallChangeBest.Add(point);
                    indexWallBest++;
                }
            }
            if (minListWall != null && wallChangeBest.Count != 0)
            {
                Random rndWall = new Random();
                int randomWallBest = rndWall.Next(0, indexWallBest);
                return wallChangeBest[randomWallBest];
            }
            return wallChangeBest[0];
        }
    }
}
