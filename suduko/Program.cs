using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace suduko
{
    public class Program
    {
        public static void Main()
        {
            Console.SetIn(new StreamReader(Console.OpenStandardInput(8192))); // set console size
            string user_input; // gets the user input 
            bool exit = false; // when true the program stops running
            string credits = "----> By Amir Wolberg <----"; 
            string logo = ("  _________         .___      __             _________      .__                     " + "\n" +
                          " /   _____/__ __  __| _/____ |  | ____ __   /   _____/ ____ |  |___  __ ___________ " + "\n" +
                          " \\_____  \\|  |  \\/ __ |/  _ \\|  |/ /  |  \\  \\_____  \\ /  _ \\|  |\\  \\/ // __ \\_  __ \\" + "\n" +
                          " /        \\  |  / /_/ (  <_> )    <|  |  /  /        (  <_> )  |_\\   /\\  ___/|  | \\/" + "\n" +
                         "/_______  /____/\\____ |\\____/|__|_ \\____/  /_______  /\\____/|____/\\_/  \\___  >__|   " + "\n" +
                        "        \\/           \\/           \\/               \\/                      \\/       "); 


            // While the user wants to solve another board keeps running
            while (!exit)
            {
                // clear Console
                Console.Clear();

                // write the logo and title 
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(logo);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (credits.Length / 2)) + "}", credits + "\n"));

                // change color
                Console.ForegroundColor = ConsoleColor.Gray;
                // initialize Input/Output option
                Console.WriteLine("To exit enter: X ");
                Console.WriteLine("To proceed enter one of the Input/Output options: " + IO.Get_options());
                user_input = Console.ReadLine();
                if (user_input.ToLower() == "x") // exit program
                {
                    Console.WriteLine("Exiting program...");
                    break;
                }
                IO.Initiate_IO_option(user_input);

                // get board from the user
                IO.Write("Enter string of characters representing the board: \n");
                string board = IO.Read(); 

                // run the solver
                IO.Write("\nBoard: " + Set_up.Run(board) + "\n"); // returns Invalid board/Unsolveable/ string representing the solved board
                
                // take care of setting up the rerun of the program
                IO.Initiate_IO_option("console"); // reset IO to default option
                Console.WriteLine("\nDo you want to solve another board? Y/N");
                if (Console.ReadLine().ToLower() != "y") // exit program
                {
                    exit = true; 
                    Console.WriteLine("Exiting program...");
                }
            }
        }
    }
   
}
