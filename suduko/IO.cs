using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace suduko
{
    /// <summary>
    /// Handles the Input and Output options
    /// </summary>
    public class IO
    {
        /// <summary>
        /// the path to the Input file - set to default
        /// </summary>
        private static string Path_read = @"";

        /// <summary>
        /// the path to the Output file - set to default
        /// </summary>
        private readonly static string Path_write = @"Output.txt";

        /// <summary>
        /// Input/Output supported options
        /// </summary>
        private readonly static string[] IO_options = {"console", "file"};

        /// <summary>
        /// current Input/Output option - default is "console"
        /// </summary>
        private static string Current_IO = "console";

        /// <summary>
        /// Input: string representing an Input/Output option (where to read from or write to)
        /// </summary>
        /// <param name="option"></param>
        /// <returns>Output: whether this option is supported by the IO class or not</returns>
        public static bool Is_supported_option(string option)
        {
            // check if the option is in the IO_options
            for (int i = 0; i < IO_options.Length; i++)
            {   
                if (option == IO_options[i])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Input: None
        /// </summary>
        /// <returns>Output: returns the available IO options</returns>
        public static string Get_options()
        {
            string return_where = "";
            for (int i = 0; i < IO_options.Length; i++)
            {
                return_where += IO_options[i] + " / ";
            }
            return return_where;
        }

        /// <summary>
        /// Input: gets a string representing where to write to and read from
        /// if IO_option is valid and no errors occure sets Current_IO to the IO_option given by the user otherwise keeps Current_IO set to its default option
        /// </summary>
        /// <param name="IO_option"></param>
        public static void Initiate_IO_option(string IO_option)
        {
            // if the option is not supported uses the default IO option
            if (!Is_supported_option(IO_option))
            {
                IO.Write("Input/Output option not supported, using the default Input/Output option: " + Current_IO + "\n");
                return;
            }

            // if option chosen is file Gets the file path and creates Output file
            if (IO_option == "file")
            {
                IO.Write("Enter the path to the file you want to read the board from: \n");
                Path_read = IO.Read();

                // check no errors occure while setting up file IO option
                try
                {
                    // if the file doesn't exist informs the user and uses the default IO option instead
                    if (!File.Exists(Path_read))
                    {
                        IO.Write("Error: File doesn't exist, using default Input/Output option: " + Current_IO + "\n");
                        return;
                    }

                    // if the file is not a text file informs the user and uses the default IO option instead
                    if (!Path_read.EndsWith(".txt"))
                    {
                        IO.Write("Error: File is not a text file, using default Input/Output option: " + Current_IO + "\n");
                        return;
                    }

                    // if the program doesn't have premission to read from the given txt file informs the user and uses the default IO option instead
                    try
                    {
                        File.ReadAllText(Path_read);
                    }
                    catch
                    {
                        IO.Write("Error: No premission to read from file , using default Input/Output option: " + Current_IO + "\n");
                        return;
                    }

                    // Checks if the outpupt file exists , if it doesn't creates it
                    if (!File.Exists(Path_write))
                    {
                        // Try creating the output file if it doesn't exist , if there is not enough memory to create the file uses the default IO option
                        File.Create(Path_write).Close();
                    }

                    // Checks the input and output files are 2 different files
                    if(Path_write.ToLower() == Path_read.ToLower())
                    {
                        IO.Write("Error: Can't read from output file, using default IO option: " + Current_IO + "\n");
                        return;
                    }

                    // empty the output file
                    File.WriteAllText(Path_write, ""); 

                    IO.Write("Writing to " + Path_write + "\n");
                    IO.Write("Reading from " + Path_read + "\n");
                }
                catch(Exception e)
                {
                    IO.Write("Error occured reading from " + Path_read + " file or writing to/creating " + Path_write + " : " + e.Message + ", using default IO option: " + Current_IO + "\n");
                    return; 
                }
            }
            
            // Set Current_IO to the IO_option given by the user
            Current_IO = IO_option;
        }

        /// <summary>
        /// Input: gets a message (as a string)
        /// writes it to where the Current_IO is set
        /// </summary>
        /// <param name="msg"></param>
        public static void Write(string msg)
        {
            switch (Current_IO)
            {
                case ("console"):
                    Console.Write(msg);
                    break;

                case ("file"):
                    try
                    {
                        File.AppendAllText(Path_write, msg);
                    }
                    catch(Exception e) // error occured while trying to write to output file 
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;

                default:
                    throw new InvalidOperationException("Output option not supported");
            }
        }

        /// <summary>
        /// Input: None
        /// </summary>
        /// <returns>Output: returns input from user where the Current_IO is set</returns>
        public static String Read()
        {
            switch (Current_IO)
            {
                case ("console"):
                    return Console.ReadLine();

                case ("file"):
                    try
                    {
                        return File.ReadAllText(Path_read);
                    }
                    catch(Exception e) // error occured while reading from file
                    {
                        Console.WriteLine(e.Message);
                        return ""; // returns an empty string (file unreadable)
                    }

                default:
                    throw new InvalidOperationException("Inpupt option not supported");
            }
        }

    }

}
