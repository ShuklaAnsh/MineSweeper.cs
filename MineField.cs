using System;

namespace MineSweeper
{
    class MineField
    {
        Cell[,] Field;
        int rows, cols;
        public MineField(int rows, int cols)
        {
            Field = new Cell[rows, cols];
            this.rows = rows;
            this.cols = cols;
            InitCells();
            Console.WriteLine(this);
        }

        void InitCells()
        {
            // Cell c1 = new Cell(0, 0);
            // Cell c2 = new Cell(0, 0);
            // c2.Left = c1;
            // c1.Right = c2;
            // c1.Proximity = 14;
            // System.Console.WriteLine("c2.left: " + c2.Left.Proximity);
            // System.Console.WriteLine("c1: " + c1.Proximity);
            // c1.Position.x = 3;
            // System.Console.WriteLine(c1.Position.x);
            // System.Console.WriteLine(c1.ToString());
            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    Field[row, col] = new Cell(col, row);
                    Console.WriteLine("Inserted Cell at ({0}, {1})", row, col);
                }
            }
        }

        public override string ToString()
        {
            string board = "";
            for (var row = 0; row < rows; row++)
            {
                Console.WriteLine("Row " + row);
                string boardRow = "";
                for (var col = 0; col < cols; col++)
                {
                    Console.WriteLine("Col " + col);
                    string boardCell = Field[row, col].Position.x + "," + Field[row, col].Position.y;
                    boardRow += "[" + boardCell + "] ";
                }
                board += boardRow + "\n";
            }
            return board;
        }
    }
}