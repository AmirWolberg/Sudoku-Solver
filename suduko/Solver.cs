using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace suduko
{
    /// <summary>
    /// Handles solving a given sudoku board
    /// </summary>
    public class Solver
    {
        /// <summary>
        /// the board to solve, Initialized to 0
        /// </summary>
        private static int[,] Game_board = { { 0 } };

        /// <summary>
        /// the size of the board, Intialized to 1
        /// </summary>
        private static int Size = 1;

        /// <summary>
        /// holds a list for every position on the board in which there are all the possible values for that position , initialized to size x size
        /// </summary>
        private static List<int>[,] Possible_solutions = new List<int>[Size, Size];

        /// <summary>
        /// true if the board is solveable and false if it is not
        /// </summary>
        private static bool Solveable;

        /// <summary>
        /// Input: None
        /// </summary>
        /// <returns>Output: returns Solveable</returns>
        public static bool Get_Solveable()
        {
            return Solveable;
        }

        /// <summary>
        /// Input: None
        /// initiates Game_board/size/Possible_solutions, runs the function that solves the board and sets solveable according to the result
        /// </summary>
        /// <param name="Current_board"></param>
        public static void Run_solver(Board Current_board)
        {
            // initialize everything needed to solve the board (the size of the board, the matrix representing the board and the matrix of lists holding the possible solutions)
            Size = Current_board.Get_size();
            Game_board = Current_board.Get_game_board();
            Possible_solutions = new List<int>[Size, Size];

            // checks if the board can't be solved (if there are duplicate values in any collumn/row/square)
            if (!Check_unique(Game_board))
            {
                Solveable = false;
                return;
            }

            // initialize list that holds all possible values for every position on the board 
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (Game_board[i, j] == 0)
                    {
                        // merges 2 lists 1 holding all available values in row/collumn and one in square
                        List<int> tmp_lst = List_funcs<int>.Merge_lists(Possibilities_sqr(i, j, Game_board), (Possibilities_row_col(i, j, Game_board)));

                        Possible_solutions[i, j] = tmp_lst.Distinct().ToList(); // No duplicates

                        if (Possible_solutions[i,j].Count == 0) // a location on the soduko board has no possible value that can be put in it so the board is unsolveable
                        {
                            Solveable = false;
                            return;
                        }

                        if (Possible_solutions[i,j].Count == 1) // if there is only one possible solution for a given position on the board update the board with it
                        {
                            Game_board[i, j] = Possible_solutions[i, j][0];
                        }

                    }
                }

            }

            // sets Solveable as true and solves the board if its solveable , returns false otherwise
            Solveable = Solve(Game_board, 0, 0);
        }

        /// <summary>
        /// Input: gets a sudoku 
        /// </summary>
        /// <param name="board"></param>
        /// <returns>Output: true if every row collumn and square has at most 1 of every number and false otherwise</returns>
        private static bool Check_unique(int[,] board)
        {
            // check arrays - every index + 1 represents a number , whenever a number in the same row/collumn/square is found the value at index [number - 1] turns to true
            bool[] check_row = new bool[Size];
            bool[] check_col = new bool[Size];
            bool[] check_sqr = new bool[Size];
            int temp;

            // reset check array
            for (int i = 0; i < Size; i++)
            {
                check_row[i] = false;
                check_col[i] = false;
                check_sqr[i] = false;
            }

            // checking there is at most one of each number every row / collumn
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (board[i, j] != 0)
                    {
                        temp = board[i, j] - 1;
                        if (check_row[board[i, j] - 1])
                        {
                            return false; // already found on this row , board invalid return false
                        }

                        check_row[temp] = true;
                    }

                    if (board[j, i] != 0)
                    {
                        temp = board[j, i] - 1;
                        if (check_col[board[j, i] - 1])
                        {
                            return false; // already found on this collumn , board invalid return false
                        }

                        check_col[temp] = true;
                    }
                }

                for (int k = 0; k < Size; k++)
                {
                    check_row[k] = false; // reset check array
                    check_col[k] = false; // reset check array
                }
            }

            // checking there is at most one of each number every square
            int sqr = Convert.ToInt32(Math.Sqrt(Size));
            for (int k = 0; k < sqr; k++) // used to go over all squares in first square row (moving row loop using k)
            {
                for (int n = 0; n < sqr; n++) // used to go over all squares in first square collumn (moving the collumn loop using n) 
                {
                    for (int i = k * sqr; i < (k + 1) * sqr; i++) // inner loops go over each square independly by dividing the board to sqr cubes
                    {
                        for (int j = n * sqr; j < (n + 1) * sqr; j++)
                        {
                            if (board[i, j] != 0)
                            {
                                temp = board[i, j] - 1;
                                if (check_sqr[board[i, j] - 1])
                                {
                                    return false; // already found on this square , board invalid return false
                                }

                                check_sqr[temp] = true;
                            }

                        }
                    }

                    for (int m = 0; m < Size; m++)
                    {
                        check_sqr[m] = false; // reset check array
                    }
                }

            }

            // no duplicate values on all rows/collumns/squares
            return true;
        }

        /// <summary>
        /// Input: gets a point(location) in the game board and the board
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="board"></param>
        /// <returns>Output: returns all available numbers in the square its in in a list</returns>
        private static List<int> Possibilities_sqr(int row, int col, int[,] board)
        {
            // holds all available numbers for given square
            List<int> available = new List<int>();

            // checks which numbers are available in this square and turns the array location equal to that number - 1 to true
            bool[] available_check = new bool[Size];

            // find beggining of the square the point is in
            int sqr = Convert.ToInt32(Math.Sqrt(Size));
            int col_sqr = col - col % sqr, row_sqr = row - row % sqr; // the collumn and row of the start of the square the point is in

            // reset available_check array to false
            for (int i = 0; i < Size; i++)
            {
                available_check[i] = false;
            }

            // go over the square and check which numbers are avilable
            for (int i = row_sqr; i < row_sqr + sqr; i++)
            {
                for (int j = col_sqr; j < col_sqr + sqr; j++)
                {
                    if (board[i, j] != 0)
                    {
                        available_check[board[i, j] - 1] = true;
                    }
                }
            }

            // put all available numbers into the available list
            for (int i = 0; i < Size; i++)
            {
                if (!available_check[i])
                {
                    available.Add(i + 1);
                }
            }

            return available;
        }

        /// <summary>
        /// Input: gets a row and a collumn (a point) in the game board and the board
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="board"></param>
        /// <returns>Output: returns all available numbers in given row/collumn in a list with no duplicates</returns>
        private static List<int> Possibilities_row_col(int row, int col, int[,] board)
        {
            // holds all available numbers for given row
            List<int> available = new List<int>();

            // checks which numbers are available in given row and in given collumn and turns the array locations equal to that number - 1 to true
            bool[] available_check_row = new bool[Size];
            bool[] available_check_col = new bool[Size];

            // reset available_check_row and available_check_col arrays to false
            for (int i = 0; i < Size; i++)
            {
                available_check_row[i] = false;
                available_check_col[i] = false;
            }

            // go over the row and collumn and check which numbers are avilable
            for (int i = 0; i < Size; i++)
            {
                if (board[row, i] != 0)
                {
                    available_check_row[board[row, i] - 1] = true;
                }
                if (board[i, col] != 0)
                {
                    available_check_col[board[i, col] - 1] = true;
                }
            }

            // put all available numbers into the available list
            for (int i = 0; i < Size; i++)
            {
                if (!available_check_row[i])
                {
                    available.Add(i + 1);
                }

                if (!available_check_col[i])
                {
                    available.Add(i + 1);
                }
            }

            available = available.Distinct().ToList(); // remove duplicates
            return available;
        }

        /// <summary>
        /// // Input: a sudoku board and a point on it
        /// </summary>
        /// <param name="board"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>Output: Solves the board using backtracking and returns true if it has been solved and false if its not solveable</returns>
        private static bool Solve(int[,] board, int row, int col)
        {
            /*if we have reached the size-1
               row and size column (0
                 indexed matrix) ,
                we are returning true to avoid further
                backtracking       */
            if (row == Size - 1 && col == Size)
                return true;

            // Check if column value  becomes size ,
            // we move to next row
            // and column start from 0
            if (col == Size)
            {
                row++;
                col = 0;
            }

            // Check if the current position
            // of the grid already
            // contains value >0, we iterate
            // for next column
            if (board[row, col] != 0)
                return Solve(board, row, col + 1);

            // Go over list holding all possible numbers for this position
            foreach (int num in Possible_solutions[row, col])
            {
                // Check if it is safe to place
                // the num (1-size)  in the
                // given row ,col ->we move to next column
                if (Is_available(col, row, board, num))
                {

                    /*  assigning the num in the current
                    (row,col)  position of the grid and
                    assuming our assined num in the position
                    is correct */
                    board[row, col] = num;

                    // Checking for next
                    // possibility with next column
                    if (Solve(board, row, col + 1))
                        return true;
                }

                /* removing the assigned num , since our
              assumption was wrong , and we go for next
              assumption with diff num value   */
                board[row, col] = 0;
            }

            /* removing the assigned num , since our
          assumption was wrong , and we go for next
          assumption with diff num value   */
            board[row, col] = 0;

            return false;

        }

        /// <summary>
        /// Input: point on the board , the board , and a value
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <param name="board"></param>
        /// <param name="num"></param>
        /// <returns>Output: returns true if the board can be placed in the point and false otherwise</returns>
        private static bool Is_available(int col, int row, int[,] board, int num)
        {

            // Check if we find the same num
            // in the similar row/collumn , we
            // return false
            for (int i = 0; i < Size; i++)
            {
                if (board[row, i] == num)
                    return false;

                if (board[i, col] == num)
                    return false;
            }

            // Check if we find the same num
            // in the particular size*size
            // matrix, we return false
            int sqr = Convert.ToInt32(Math.Sqrt(Size));
            int startRow = row - row % sqr, startCol
                                          = col - col % sqr;
            for (int i = 0; i < sqr; i++)
                for (int j = 0; j < sqr; j++)
                    if (board[i + startRow, j + startCol] == num)
                        return false;

            return true;
        }
    }
}
