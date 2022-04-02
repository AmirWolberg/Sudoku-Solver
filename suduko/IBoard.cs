using System;
using System.Collections.Generic;
using System.Text;

namespace suduko
{
    /// <summary>
    /// Interface with the basic functions any board must have
    /// </summary>
    interface IBoard
    {
        /// <summary>
        /// gets the game board
        /// </summary>
        /// <returns>the game board</returns>
        public abstract int[,] Get_game_board();

        /// <summary>
        /// sets the game board
        /// </summary>
        /// <param name="premade_board"></param>
        /// <returns></returns>
        public abstract bool Set_game_board(string premade_board);

        /// <summary>
        /// prints the game board
        /// </summary>
        public abstract void Print_board();
    }
}
