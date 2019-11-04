using System;

namespace MineSweeper
{
    class Program
    {
        static void Main(string[] args)
        {
            Cell c1 = new Cell(0, 0);
            Cell c2 = new Cell(0, 0);
            c2.Left = c1;
            c1.Proximity = 14;
            System.Console.WriteLine("c2.left: " + c2.Left.Proximity);
            System.Console.WriteLine("c1: " + c1.Proximity);
        }
    }
}
