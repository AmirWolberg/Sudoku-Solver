using Microsoft.VisualStudio.TestTools.UnitTesting;
using suduko;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace suduko.Tests
{
    /// <summary>
    ///  Runs tests on the sudoku project
    /// </summary>
    [TestClass()]
    public class ProgramTests
    {
        /* Function used by tests */

        /// <summary>
        /// Input: Solved board and its size 
        /// </summary>
        /// <param name="board"></param>
        /// <param name="size"></param>
        /// <returns>Output: whether its been solved correctly or not</returns>
        public static bool Check_success(int[,] board, int size)
        {
            // check arrays - every index + 1 represents a number , whenever a number in the same row/collumn/square is found the value at index [number - 1] turns to true
            bool[] check_row = new bool[size];
            bool[] check_col = new bool[size];
            bool[] check_sqr = new bool[size];

            // reset check arrays
            for (int i = 0; i < size; i++)
            {
                check_row[i] = false;
                check_col[i] = false;
                check_sqr[i] = false;
            }

            // checking the entire board has valid values
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (board[i, j] > size || board[i, j] <= 0)
                    {
                        return false; // the board is not fully solved or has an invalid value
                    }
                }
            }

            // checking there is one of each number every row / col
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (check_row[board[i, j] - 1])
                    {
                        return false; // board not solved correctly - a row has 2 or more of the same value
                    }
                    check_row[board[i, j] - 1] = true;

                    if (check_col[board[j, i] - 1])
                    {
                        return false; // board not solved correctly - a collumn has 2 or more of the same value
                    }
                    check_col[board[j, i] - 1] = true;
                }

                for (int k = 0; k < size; k++)
                {
                    if (!check_row[k])
                    {
                        return false; // board not solved correctly -  a row is missing a value
                    }

                    check_row[k] = false; // reset check_row array

                    if (!check_col[k])
                    {
                        return false; // board not solved correctly -  a collumn is missing a value
                    }

                    check_col[k] = false; // reset check_col array
                }
            }

            // checking there is one of each number every square
            int sqr = Convert.ToInt32(Math.Sqrt(size));
            for (int k = 0; k < sqr; k++) // used to go over all squares in first square row (moving row loop using k)
            {
                for (int n = 0; n < sqr; n++) // used to go over all squares in first square collumn (moving the collumn loop using n) 
                {
                    for (int i = k * sqr; i < (k + 1) * sqr; i++) // inner loops go over each square independly by dividing the board to sqr cubes
                    {
                        for (int j = n * sqr; j < (n + 1) * sqr; j++)
                        {
                            if (check_sqr[board[i, j] - 1])
                            {
                                return false; // board not solved correctly - a square has 2 or more of the same value
                            }
                            check_sqr[board[i, j] - 1] = true;
                        }
                    }

                    for (int m = 0; m < size; m++)
                    {
                        if (!check_sqr[m])
                        {
                            return false; // board not solved correctly -  a square is missing a value
                        }

                        check_sqr[m] = false; // reset check_sqr array
                    }
                }
            }

            // didnt fail any of the checks meaning its been solved correctly 
            return true;

        }

        /// <summary>
        /// Input: string representing game board and the board size
        /// </summary>
        /// <param name="board"></param>
        /// <returns>Output: returns the outcome of the solver test ("Unsolveable" / "Invalid board" / "Solved correctly" / "Solved Incorrectly")</returns>
        public static string Test_act(string board)
        {
            int board_size = Convert.ToInt32(Math.Sqrt(board.Length)); // holds the width/length of the board

            // outcome = ("Unsolveable" / "Invalid board" / a string representing a solved board)
            string outcome = Set_up.Run(board);

            // if the board was solved checks whether the board was solved correctly or not( returns "Solved correctly"/ "Solved incorrectly")
            if (!(outcome == "Invalid board" || outcome == "Unsolveable"))
            {
                // converting outcome which holds the solved board as string into a matrix holding the board to check if it was solved correctly
                int[,]  solved_board = new int[board_size, board_size];
                for (int i = 0; i < board_size; i++)
                {
                    for (int j = 0; j < board_size; j++)
                    {
                        solved_board[i, j] = outcome[i * board_size + j] - '0';
                    }
                }
                // checks if the board was solved correctly and returns "Solved correctly" if it was
                if (Check_success(solved_board, board_size))
                {
                        return ("Solved correctly");
                }
                return ("Solved incorrectly");
            }

            // board was nto solved -> returns "Unsolveable" / "Invalid board"
            return outcome;
        }

        /* Tests run on 4 by 4 sudoku boards */
        /// <summary>
        /// Tests a 4 by 4 empty board
        /// </summary>
        [TestMethod()]
        public void Test_4x4_empty()
        {
            // Arrange
            string result; // holds the outcome of the test ("Unsolveable" / "Invalid board" / "Solved correctly" / "Solved Incorrectly")
            string board = "0000" +
                           "0000" +
                           "0000" +
                           "0000"; // holds the string representation of the board we test
          
            // Act
            result = Test_act(board);
            
            // Assert
            Assert.AreEqual("Solved correctly", result);
        }

        /// <summary>
        /// Tests a 4 by 4 solveable, not empty board
        /// </summary>
        [TestMethod()]
        public void Test_4x4_solveable()
        {
            // Arrange
            string result; // holds the outcome of the test ("Unsolveable" / "Invalid board" / "Solved correctly" / "Solved Incorrectly")
            string board = "1000" +
                           "0200" +
                           "0030" +
                           "0004"; // holds the string representation of the board we test

            // Act
            result = Test_act(board);

            // Assert
            Assert.AreEqual("Solved correctly", result);
        }

        /// <summary>
        /// Tests a 4 by 4 valid but unsolveable board
        /// </summary>
        [TestMethod()]
        public void Test_4x4_unsolveable()
        {
            // Arrange
            string result; // holds the outcome of the test ("Unsolveable" / "Invalid board" / "Solved correctly" / "Solved Incorrectly")
            string board = "1034" +
                           "2000" +
                           "0000" +
                           "0000"; // holds the string representation of the board we test

            // Act
            result = Test_act(board);

            // Assert
            Assert.AreEqual("Unsolveable", result);
        }

        /// <summary>
        /// Tests a 4 by 4 invalid board
        /// </summary>
        [TestMethod()]
        public void Test_4x4_invalid()
        {
            // Arrange
            string result; // holds the outcome of the test ("Unsolveable" / "Invalid board" / "Solved correctly" / "Solved Incorrectly")
            string board = "1034" +
                           "5000" +
                           "0000" +
                           "0000"; // holds the string representation of the board we test

            // Act
            result = Test_act(board);

            // Assert
            Assert.AreEqual("Invalid board", result);
        }


        /* Tests run on 9 by 9 sudoku boards */
        /// <summary>
        /// Tests a 9 by 9 empty board
        /// </summary>
        [TestMethod()]
        public void Test_9x9_empty()
        {
            // Arrange
            string result; // holds the outcome of the test ("Unsolveable" / "Invalid board" / "Solved correctly" / "Solved Incorrectly")
            string board = "000000000" +
                           "000000000" +
                           "000000000" +
                           "000000000" +
                           "000000000" +
                           "000000000" +
                           "000000000" +
                           "000000000" +
                           "000000000"; // holds the string representation of the board we test


            // Act
            result = Test_act(board);

            // Assert
            Assert.AreEqual("Solved correctly", result);
        }

        /// <summary>
        /// Tests a 9 by 9 solveable, not empty board
        /// </summary>
        [TestMethod()]
        public void Test_9x9_solveable()
        {
            // Arrange
            string result; // holds the outcome of the test ("Unsolveable" / "Invalid board" / "Solved correctly" / "Solved Incorrectly")
            string board = "100000000" +
                           "000000000" +
                           "000000500" +
                           "000000000" +
                           "000900000" +
                           "000000000" +
                           "000000000" +
                           "004000000" +
                           "000000020"; // holds the string representation of the board we test

            // Act
            result = Test_act(board);

            // Assert
            Assert.AreEqual("Solved correctly", result);
        }

        /// <summary>
        /// Tests a 9 by 9 valid but unsolveable board
        /// </summary>
        [TestMethod()]
        public void Test_9x9_unsolveable()
        {
            // Arrange
            string result; // holds the outcome of the test ("Unsolveable" / "Invalid board" / "Solved correctly" / "Solved Incorrectly")
            string board = "103456780" +
                           "000000002" +
                           "020000000" +
                           "000000000" +
                           "000000000" +
                           "000000000" +
                           "000000000" +
                           "000000000" +
                           "000000000"; // holds the string representation of the board we test

            // Act
            result = Test_act(board);

            // Assert
            Assert.AreEqual("Unsolveable", result);
        }

        /// <summary>
        /// Tests a 9 by 9 invalid board
        /// </summary>
        [TestMethod()]
        public void Test_9x9_invalid()
        {
            // Arrange
            string result; // holds the outcome of the test ("Unsolveable" / "Invalid board" / "Solved correctly" / "Solved Incorrectly")
            string board = "0;0000000" + 
                           "000000000" +
                           "040002000" +
                           "080000000" +
                           "010010000" +
                           "000010000" +
                           "004560100" +
                           "000000000" +
                           "000000000"; // holds the string representation of the board we test

            // Act
            result = Test_act(board);

            // Assert
            Assert.AreEqual("Invalid board", result); 
        }


        /* Tests run on 16 by 16 sudoku boards */
        /// <summary>
        /// Tests a 16 by 16 empty board
        /// </summary>
        [TestMethod()]
        public void Test_16x16_empty()
        {
            // Arrange
            string result; // holds the outcome of the test ("Unsolveable" / "Invalid board" / "Solved correctly" / "Solved Incorrectly")
            string board = "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000"; // holds the string representation of the board we test


            // Act
            result = Test_act(board);

            // Assert
            Assert.AreEqual("Solved correctly", result);
        }

        /// <summary>
        /// Tests a 16 by 16 solveable, not empty board
        /// </summary>
        [TestMethod()]
        public void Test_16x16_solveable()
        {
            // Arrange
            string result; // holds the outcome of the test ("Unsolveable" / "Invalid board" / "Solved correctly" / "Solved Incorrectly")
            string board = "0100000000002000" +
                           "0;00000000000000" +
                           "0000000:00000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "00000000000000<0" +
                           "00000000000000>0" +
                           "0002000000000000" +
                           "0000004000000000"; // holds the string representation of the board we test


            // Act
            result = Test_act(board);

            // Assert
            Assert.AreEqual("Solved correctly", result);
        }

        /// <summary>
        /// Tests a 16 by 16 valid but unsolveable board
        /// </summary>
        [TestMethod()]
        public void Test_16x16_unsolveable()
        {
            // Arrange
            string result; // holds the outcome of the test ("Unsolveable" / "Invalid board" / "Solved correctly" / "Solved Incorrectly")
            string board = "1023456789:;<>=?" +
                           "00@0000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000"; // holds the string representation of the board we test


            // Act
            result = Test_act(board);

            // Assert
            Assert.AreEqual("Unsolveable", result);
        }

        /// <summary>
        /// Tests a 16 by 16 invalid board
        /// </summary>
        [TestMethod()]
        public void Test_16x16_invalid()
        {
            // Arrange
            string result; // holds the outcome of the test ("Unsolveable" / "Invalid board" / "Solved correctly" / "Solved Incorrectly")
            string board = "0000}00000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000010000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000000000000000" +
                           "0000020000000000" +
                           "0000030000000000" +
                           "0000000000000000" +
                           "0010000000000000" +
                           "0000000000000000" +
                           "1000000000000000"; // holds the string representation of the board we test


            // Act
            result = Test_act(board);

            // Assert
            Assert.AreEqual("Invalid board", result);
        }


        /* Other tests not of a specific size */
        /// <summary>
        /// Tests an empty string as a board
        /// </summary>
        [TestMethod()]
        public void Test_empty_board()
        {
            // Arrange
            string result; // holds the outcome of the test ("Unsolveable" / "Invalid board" / "Solved correctly" / "Solved Incorrectly")
            string board = ""; // holds the string representation of the board we test

            // Act
            result = Test_act(board);

            // Assert
            Assert.AreEqual("Invalid board", result);
        }

        /// <summary>
        /// Tests a board of invalid length
        /// </summary>
        [TestMethod()]
        public void Test_invalid_length()
        {
            // Arrange
            string result; // holds the outcome of the test ("Unsolveable" / "Invalid board" / "Solved correctly" / "Solved Incorrectly")
            string board = "000"; // holds the string representation of the board we test

            // Act
            result = Test_act(board);

            // Assert
            Assert.AreEqual("Invalid board", result);
        }

        /// <summary>
        /// Tests a board initialized with hebrew characters (invalid)
        /// </summary>
        [TestMethod()]
        public void Test_invalid_characters()
        {
            // Arrange
            string result; // holds the outcome of the test ("Unsolveable" / "Invalid board" / "Solved correctly" / "Solved Incorrectly")
            string board = "מה קורה חייל שבודק את זה איך היום שלך?"; // holds the string representation of the board we test

            // Act
            result = Test_act(board);

            // Assert
            Assert.AreEqual("Invalid board", result);
        }

        /// <summary>
        /// Tests a 1x1 board
        /// </summary>
        [TestMethod()]
        public void Test_1x1()
        {
            // Arrange
            string result; // holds the outcome of the test ("Unsolveable" / "Invalid board" / "Solved correctly" / "Solved Incorrectly")
            string board = "0"; // holds the string representation of the board we test

            // Act
            result = Test_act(board);

            // Assert
            Assert.AreEqual("Solved correctly", result);
        }
        
    }
}
