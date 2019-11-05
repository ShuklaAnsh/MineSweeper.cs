using System;

namespace MineSweeper
{
    class Program
    {

        static MineField.Mode Mode = MineField.Mode.Flagging;
        static bool ProcessInput(string input, ref Cell.PositionStruct pos)
        {
            string[] inputs = input.Split(" ", 3, StringSplitOptions.RemoveEmptyEntries);
            int x, y;
            if (inputs.Length == 3)
            {
                if (!Int32.TryParse(inputs[0], out x))
                {
                    return false;
                }
                if (!Int32.TryParse(inputs[1], out y))
                {
                    return false;
                }
                if (inputs[2].ToLower().Equals("f"))
                {
                    Mode = MineField.Mode.Flagging;
                }
                else if(inputs[2].ToLower().Equals("s"))
                {
                    Mode = MineField.Mode.Snooping;
                }
                pos.x = x;
                pos.y = y;
                return true;
            }
            return false;
        }
        static void Main(string[] args)
        {
            MineField field = new MineField(rows: 10, cols: 10, numBombs: 20);
            Cell.PositionStruct pos = new Cell.PositionStruct(-1, -1);
            bool malformedInput = false;
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("Bombs: {0} | Moves: {1}", field.Bombs, field.Moves);
                Console.WriteLine(field.ToString());
                if (malformedInput)
                {
                    Console.Write("Incorect Input\n");
                    Console.Write("Proper Syntax: `x y [ f | s ]` for coord (x,y)\n");
                    Console.Write("`x` is a number from the numbers along the top\n");
                    Console.Write("`y` is a number from the numbers along the left\n");
                    Console.Write("`f` is to flag the coordinate\n");
                    Console.Write("`s` is to snoop the coordinate\n");
                }
                Console.Write("Enter Coords: ");
                if (ProcessInput(Console.ReadLine(), ref pos))
                {
                    Console.WriteLine("({0},{1})", pos.x, pos.y);
                    field.HandleSelection(pos, Mode);
                    malformedInput = false;
                }
                else
                {
                    malformedInput = true;
                }
            }
            Console.WriteLine("Game Over!");
        }
    }
}
