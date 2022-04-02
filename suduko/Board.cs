using System;
using System.Collections.Generic;
using System.Text;

namespace suduko
{
    /// <summary>
    /// Object representing the game board
    /// </summary>
    public class Board : IBoard
    {
        /// <summary>
        /// matrix representing the game board
        /// </summary>
        private int[,] Game_board;

        /// <summary>
        /// the size of the Game_board (used to initiliaze the Game_board matrix , represents length and width)
        /// </summary>
        private int Size;

        /// <summary>
        /// Input: None
        /// </summary>
        /// <returns>Output: returns size</returns>
        public int Get_size()
        {
            return Size;
        }

        /// <summary>
        /// Input: None
        /// </summary>
        /// <returns>Output: returns Game_board</returns>
        public int[,] Get_game_board()
        {
            return Game_board;
        }

        /// <summary>
        /// Input: a string representing a sudoku board
        /// </summary>
        /// <param name="premade_board"></param>
        /// <returns>Output: sets the Game_board with the premade_board, returns false if the board is invalid and true if its valid and the Game_board was set successfully</returns>
        public bool Set_game_board(string premade_board)
        {
            // checks if the given premade_board is valid as a sudoku board or not
            if (!Check_board_validity(premade_board))
            {
                // Invalid input entered as board (invalid values or board length)
                return false;
            }

            // sets size
            Size = Convert.ToInt32(Math.Sqrt(premade_board.Length));

            // sets the game board
            Game_board = new int[Size, Size];

            // Initialize the Game_board int matrix with the preamde_board string's values
            int string_count = 0;
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Game_board[i, j] = premade_board[string_count] - '0';
                    string_count++;
                }
            }

            // board is valid and was set successfully 
            return true;
        }

        /// <summary>
        /// prints Game_board
        /// </summary>
        public void Print_board()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (j == 0)
                    {
                        IO.Write("|");
                    }
                    IO.Write(Game_board[i, j] + "|");
                }
                IO.Write("\n");
            }
        }

        /// <summary>
        /// Input: gets string representing the board of the game
        /// </summary>
        /// <param name="premade_board_copy"></param>
        /// <returns>Output: returns true if the string will be valid as a sudoku game board and false if not</returns>
        private static bool Check_board_validity(string premade_board_copy)
        {
            int temp_size = Convert.ToInt32(Math.Sqrt(premade_board_copy.Length));

            // check characters validity
            for (int i = 0; i < premade_board_copy.Length; i++)
            {
                if (premade_board_copy[i] - '0' > temp_size || premade_board_copy[i] - '0' < 0)
                {
                    return false;
                }
            }

            // check length validity ( board length must have a whole number as its square root of a square root and its length must be 1 and higher)
            if((Math.Sqrt(Math.Sqrt(premade_board_copy.Length)) % 1 != 0) || (premade_board_copy.Length <= 0))
            {
                return false;
            }

            // board is valid
            return true;
        }
    }
}
