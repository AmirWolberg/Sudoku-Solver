using System;
using System.Collections.Generic;
using System.Text;

namespace suduko
{
    /// <summary>
    /// Sets up the program -> initializes Board and runs solver
    /// </summary>
    public class Set_up
    {
        /// <summary>
        /// the current_board we run the solver and program on
        /// </summary>
        private static Board Current_board;

        /// <summary>
        /// Input: None
        /// </summary>
        /// <returns>Output: returns current_board</returns>
        public static Board Get_current_board()
        {
            return Current_board;
        }

        /// <summary>
        /// Input: string representing a sudoku board
        /// </summary>
        /// <param name="board"></param>
        /// <returns>Output: initialize the board and run the solver on it , if the board is invalid returns "Invalid board" ,
        /// if the board is unsolveable returns "Unsolveable" and if the board was solved returns a string representing the solved board</returns>
        public static string Run(string board)
        {
            // sets the current board as a new Board object
            Current_board = new Board();

            // if the board is valid
            if (Current_board.Set_game_board(board))
            {
                // prints the unsolved board as a matrix
                Current_board.Print_board();

                // Solves the board , changes the Current_board to its solved version
                Solver.Run_solver(Current_board);

                // if the board is not solveable
                if (!Solver.Get_Solveable()) 
                {
                    return ("Unsolveable");
                }
                
                // if the board is solveable
                else 
                {
                    // printing the board as a matrix
                    IO.Write("\n");
                    Current_board.Print_board();

                    // Converting board to string (the format we got it in)
                    var builder = new StringBuilder();
                    int[,] matrix_board = Current_board.Get_game_board();
                    for(int i =0; i<Current_board.Get_size(); i++)
                    {
                        for(int j=0; j<Current_board.Get_size(); j++)
                        {
                            builder.Append(Convert.ToChar(matrix_board[i, j] + '0'));
                        }
                    }
                    string string_board = builder.ToString();

                    // board was solved -> returns solved board as string
                    return (string_board);
                }
            }

            // if the board is not valid
            return ("Invalid board"); ;
        }
    }
}
