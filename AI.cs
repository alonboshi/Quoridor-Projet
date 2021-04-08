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
        public static bool Move(Board matrix, Player player)
        {
            matrix.UpdateBoard();
            int indexBest = 0;   // how many similar best moves
            List<Tuple<int, int>> minList_AI = new List<Tuple<int, int>>(); // the list of the AI's shortest way to the goal. 
            List<List<Tuple<int, int>>> allBest = new List<List<Tuple<int, int>>>(); // A list of all the AI's shortest way with the same count of steps to goal
            int minVal = int.MaxValue; // minimum steps to the goal
            for (int i = 0; i < 9; i++)
            {
                // List of Locations on the wall which represents the most little way
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
                

                Console.WriteLine("Checking my " + i + " way" + "  " + tempList.Count);

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
                Console.WriteLine("Checking opponent " + i + " way" + "  " + tempList.Count);

            }
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
                for (int i = 0; i < matrix.index_possibleWalls; i++)
                {
                    List<Vertices> list = matrix.TurnWallTemp(matrix.possibleWalls[i].x, matrix.possibleWalls[i].y, matrix.possibleWalls[i].place);
                    List<Tuple<int, int>> tempList = matrix.GetShortestPath(opponent.x, opponent.y, opponent.goalx, min_goalcell_index);
                    matrix.UndoTurnWallTemp(list, matrix.possibleWalls[i].x, matrix.possibleWalls[i].y, matrix.possibleWalls[i].place);
                    Console.WriteLine("Calculate wall " + i + "  " + tempList.Count);

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
                if (minListWall != null && wallChangeBest.Count < matrix.index_possibleWalls / 2)
                {
                    Random rndWall = new Random();
                    int randomWallBest = rndWall.Next(0, indexWallBest);
                    Console.WriteLine("rnd: " + randomWallBest + " " + indexWallBest);
                    Point THEWALL = wallChangeBest[randomWallBest];
                    Console.WriteLine(THEWALL.x + " " + THEWALL.y + " " + THEWALL.place);
                    if (matrix.CanReach(THEWALL.x, THEWALL.y, THEWALL.place))
                    {
                        if (matrix.TurnWall(THEWALL.x, THEWALL.y, THEWALL.place))
                        {
                            Console.WriteLine("succeed");
                            return true;
                        }
                    }
                }
            }

            if (allBest.Count != 0)
            {
                Random rnd = new Random();
                int randomBest = rnd.Next(0, indexBest);

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
        /*// tree node class definition
        public class TreeNode
        {
            public Board board;
            public bool is_terminal;
            public bool is_fully_expanded;
            TreeNode parent;
            int visits;
            int score;
            public Dictionary<Point,TreeNode> children;
            // class constructor (create tree node class instance)
            public TreeNode(Board board, TreeNode parent) {
                // init associated board state
                this.board = board;

                // init is node terminal flag
                if (this.board.Iswin() != -1)
                    // we have a terminal node
                    this.is_terminal = true;

                // otherwise
                else
                    // we have a non-terminal node
                    this.is_terminal = false;

                // init is fully expanded flag
                this.is_fully_expanded = this.is_terminal;

                // init parent node if available
                this.parent = parent;

                // init the number of node visits
                this.visits = 0;

                // init the total score of the node
                this.score = 0;

                // init current node's children
                this.children = new Dictionary<Point, TreeNode>();
            }
        }
        private TreeNode root;

        public List<TreeNode> Search(Board initial_state)
        {
            TreeNode node = null;
            // create root node
            this.root = new TreeNode(initial_state, null);

            // walk through 1000 iterations
            for (int iteration = 0; iteration < 1000; iteration++)
                // select a node (selection phase)
                node = Select(this.root);

            // scrore current node (simulation phase)
            int score = Rollout(node.board);

            // backpropagate results
            Backpropagate(node, score);

            // pick up the best move in the current position
            try
            {
                return Get_best_move(this.root, 0);
            }
            catch{}
        }
        // select most promising node
        public TreeNode Select(TreeNode node)
        {
            // make sure that we're dealing with non-terminal nodes
            while (!node.is_terminal)
                // case where the node is fully expanded
                if (node.is_fully_expanded)
                    node = Get_best_move(node, 2);
                // case where the node is not fully expanded 
                else
                    // otherwise expand the node
                    return Expand(node);

            // return node
            return node;
        }
        // expand node
        public TreeNode Expand(TreeNode node) {
            // generate legal states (moves) for the given node
            Point[] states = node.board.possibleMove;
        
            // loop over generated states (moves)
            foreach(Point state in states) { 
                // make sure that current state (move) is not present in child nodes

                if (node.children.(state)) { 
                   // create a new node
                   TreeNode new_node = new TreeNode(state, node)

                     // add child node to parent's node children list (dict)
                     node.children[state] = new_node
                
                    # case when node is fully expanded
                    if len(states) == len(node.children) :
                        node.is_fully_expanded = True
                
                    # return newly created node
                    return new_node

# debugging
        print('Should not get here!!!')
    */}
}
