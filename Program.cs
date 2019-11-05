using System;

namespace MineSweeper
{
    class Program
    {
        static void Main(string[] args)
        {
            MineField field = new MineField(rows: 10, cols: 10, numBombs: 20);
        }
    }
}
