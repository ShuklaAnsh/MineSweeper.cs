/*
    Contains the entry point (Main Method) for this
    Console based MineSweeper game.
*/

using System;
namespace MineSweeper
{
    /* Class for Program.cs */
    class Program
    {
        // Current input mode ( Flagging or Snooping )
        static MineField.Mode Mode;

        // boolean for if the game is running
        static bool Running;

        /**
            Processes user input from the command line

            Parms:
                string input            - The user input to process
                Cell.PositionStruct pos - Holds the processed coordinates

            Returns:
                True if input was processed correctly
         */
        static bool ProcessInput(string input, ref Cell.PositionStruct pos)
        {
            // expecting input to have 3 args: x, y, and mode
            string[] inputs = input.Split(" ", 3, StringSplitOptions.RemoveEmptyEntries);
            int x, y;
            if (inputs.Length == 3)
            {
                // input 1 is for x position
                if (!Int32.TryParse(inputs[0], out x))
                {
                    return false;
                }
                // input 2 is for y position
                if (!Int32.TryParse(inputs[1], out y))
                {
                    return false;
                }
                // input 3 is for the mode. f for flagging, s for snooping
                if (inputs[2].ToLower().Equals("f"))
                {
                    Mode = MineField.Mode.Flagging;
                }
                else if (inputs[2].ToLower().Equals("s"))
                {
                    Mode = MineField.Mode.Snooping;
                }
                pos.x = x;
                pos.y = y;
                return true;
            }
            // if input is "quit", stop the game
            else if(input.ToLower().Equals("quit"))
            {
                Running = false;
            }

            return false;
        }

        /**
            Entry point function for the game. Contains Game loop and 
            initializes the field.
         */
        static void Main(string[] args)
        {
            // Creates playing field with 15 row, 10 colums, and 20 bombs
            MineField field = new MineField(rows: 15, cols: 10, numBombs: 20);
            // initialize position struct to an invalid coordinate (-1, -1)
            Cell.PositionStruct pos = new Cell.PositionStruct(-1, -1);
            //boolean for if to display the instruction text
            bool displayInst = true;
            Running = true;
            // Game loop that will run untill the game is over
            while (Running)
            {
                // clear the console before every update
                Console.Clear();
                Console.WriteLine("Bombs: {0} | Moves: {1}", field.Bombs, field.Moves);
                // field formats the board string
                Console.WriteLine(field.PrintBoard());
                if (displayInst)
                {
                    Console.WriteLine("Proper Syntax: `x y [ f or s ]` for coord (x,y)");
                    Console.WriteLine("`x` is a number from the numbers along the top");
                    Console.WriteLine("`y` is a number from the numbers along the left");
                    Console.WriteLine("`f` is to flag the coordinate");
                    Console.WriteLine("`s` is to snoop the coordinate");
                    Console.WriteLine("`s` is to snoop the coordinate");
                    Console.WriteLine("`quit` to quit");
                }

                // Input prompt
                Console.Write("Enter x y f/s: ");

                // if input was successfully processed
                if (ProcessInput(Console.ReadLine(), ref pos))
                {
                    Console.WriteLine("({0},{1})", pos.x, pos.y);
                    displayInst = false;
                    // handle the processed input
                    MineField.GameStatus status = field.HandleSelection(pos, Mode);
                    switch (status)
                    {
                        // Lost Game, stop running
                        case MineField.GameStatus.Lose:
                            Running = false;
                            Console.Clear();
                            Console.WriteLine("Bombs: {0} | Moves: {1}", field.Bombs, field.Moves);
                            Console.WriteLine(field.ToString());
                            Console.WriteLine("Game Over! You Lose :(");
                            break;

                        // Won Game, stop running
                        case MineField.GameStatus.Win:
                            Running = false;
                            Console.Clear();
                            Console.WriteLine("Bombs: {0} | Moves: {1}", field.Bombs, field.Moves);
                            Console.WriteLine(field.ToString());
                            Console.WriteLine("Game Over! You Won!! Woo!");
                            break;

                        default:
                            Running = true;
                            break;
                    }
                }
                else
                {
                    displayInst = true;
                }
            }
            Console.Write("Press Any Key to Quit... ");
            Console.ReadLine();
        }
    }
}
