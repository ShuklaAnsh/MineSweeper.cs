using System;

namespace MineSweeper
{
    class MineField
    {
        public enum Mode
        {
            Flagging, Snooping
        }

        Cell[,] Field;
        int Rows, Cols, Snooped;
        public int Bombs, Moves, Flags;
        public MineField(int rows, int cols, int numBombs)
        {
            Field = new Cell[cols, rows];
            Rows = rows;
            Cols = cols;
            Bombs = numBombs;
            InitCells();
        }

        void InitCells()
        {
            for (var y = 0; y < Rows; y++)
            {
                for (var x = 0; x < Cols; x++)
                {
                    Field[x, y] = new Cell(x, y);
                }
            }
            InitCellNeighbours();
        }

        void InitCellNeighbours()
        {
            Cell borderCell = null;
            for (var x = 0; x < Cols; x++)
            {
                for (var y = 0; y < Rows; y++)
                {
                    var cell = Field[x, y];

                    if (!CornerBoundaries(ref cell, x, y))
                    {
                        // Left Border
                        if (x == 0)
                        {
                            var top = Field[x, y - 1];
                            var topRight = Field[x + 1, y - 1];
                            var right = Field[x + 1, y];
                            var bottomRight = Field[x + 1, y + 1];
                            var bottom = Field[x, y + 1];
                            cell.addNeighbours(ref borderCell, ref borderCell, ref top, ref topRight, ref right, ref bottomRight, ref bottom, ref borderCell);
                        }
                        // Top Border
                        else if (y == 0)
                        {
                            var left = Field[x - 1, y];
                            var right = Field[x + 1, y];
                            var bottomRight = Field[x + 1, y + 1];
                            var bottom = Field[x, y + 1];
                            var bottomLeft = Field[x - 1, y + 1];
                            cell.addNeighbours(ref left, ref borderCell, ref borderCell, ref borderCell, ref right, ref bottomRight, ref bottom, ref bottomLeft);
                        }
                        // Right Border
                        else if (x == (Cols - 1))
                        {
                            var left = Field[x - 1, y];
                            var topLeft = Field[x - 1, y - 1];
                            var top = Field[x, y - 1];
                            var bottom = Field[x, y + 1];
                            var bottomLeft = Field[x - 1, y + 1];
                            cell.addNeighbours(ref left, ref topLeft, ref top, ref borderCell, ref borderCell, ref borderCell, ref bottom, ref bottomLeft);
                        }
                        // Bottom Border
                        else if (y == (Rows - 1))
                        {
                            var left = Field[x - 1, y];
                            var topLeft = Field[x - 1, y - 1];
                            var top = Field[x, y - 1];
                            var topRight = Field[x + 1, y - 1];
                            var right = Field[x + 1, y];
                            cell.addNeighbours(ref left, ref topLeft, ref top, ref topRight, ref right, ref borderCell, ref borderCell, ref borderCell);
                        }
                        // Middle
                        else
                        {
                            var left = Field[x - 1, y];
                            var topLeft = Field[x - 1, y - 1];
                            var top = Field[x, y - 1];
                            var topRight = Field[x + 1, y - 1];
                            var right = Field[x + 1, y];
                            var bottomRight = Field[x + 1, y + 1];
                            var bottom = Field[x, y + 1];
                            var bottomLeft = Field[x - 1, y + 1];
                            cell.addNeighbours(ref left, ref topLeft, ref top, ref topRight, ref right, ref bottomRight, ref bottom, ref bottomLeft);
                        }
                    }
                }
            }
        }

        bool CornerBoundaries(ref Cell cell, int x, int y)
        {
            // Top Left Border
            Cell borderCell = null;
            if ((x == 0) && (y == 0))
            {
                var right = Field[x + 1, y];
                var bottomRight = Field[x + 1, y + 1];
                var bottom = Field[x, y + 1];
                cell.addNeighbours(ref borderCell, ref borderCell, ref borderCell, ref borderCell, ref right, ref bottomRight, ref bottom, ref borderCell);
                return true;
            }
            // Top Right Border
            else if ((x == (Cols - 1)) && (y == 0))
            {
                var left = Field[x - 1, y];
                var bottom = Field[x, y + 1];
                var bottomLeft = Field[x - 1, y + 1];
                cell.addNeighbours(ref left, ref borderCell, ref borderCell, ref borderCell, ref borderCell, ref borderCell, ref bottom, ref bottomLeft);
                return true;
            }
            // Bottom Right Border
            else if ((x == (Cols - 1)) && (y == (Rows - 1)))
            {
                var left = Field[x - 1, y];
                var topLeft = Field[x - 1, y - 1];
                var top = Field[x, y - 1];
                cell.addNeighbours(ref left, ref topLeft, ref top, ref borderCell, ref borderCell, ref borderCell, ref borderCell, ref borderCell);
                return true;
            }
            // Bottom Left Border
            else if ((x == 0) && (y == (Rows - 1)))
            {
                var top = Field[x, y - 1];
                var topRight = Field[x + 1, y - 1];
                var right = Field[x + 1, y];
                cell.addNeighbours(ref borderCell, ref borderCell, ref top, ref topRight, ref right, ref borderCell, ref borderCell, ref borderCell);
                return true;
            }
            return false;
        }

        void InitBombs(ref Cell startingCell)
        {
            Random rng = new Random();
            for (int i = 0; i < Bombs; i++)
            {
                int randCol = rng.Next(0, Cols - 1);
                int randRow = rng.Next(0, Rows - 1);
                Cell randCell = Field[randRow, randCol];
                if (randCell.IsBomb || randCell.Position.Equals(startingCell.Position))
                {
                    i--;
                    continue;
                }
                randCell.IsBomb = true;
            }
            foreach (var cell in Field)
            {
                foreach (var neighbour in cell.Neighbours)
                {
                    if (neighbour != null && neighbour.IsBomb)
                    {
                        cell.Proximity++;
                    }
                }
            }
        }

        public void HandleSelection(Cell.PositionStruct pos, Mode mode)
        {
            Cell selectedCell = new Cell(-1, -1);
            try 
            {
                selectedCell = Field[pos.x, pos.y];
            }
            catch (IndexOutOfRangeException)
            {
                Console.Write("Bad Coordinates. Press Enter to Continue...");
                Console.ReadLine();
                return;
            }
            if(Moves == 0)
            {
                InitBombs(ref selectedCell);
            }
            else if(selectedCell.Snooped)
            {
                return;
            }

            Moves++;

            if(mode == Mode.Flagging)
            {
                if(selectedCell.Flagged)
                {
                    selectedCell.Flagged = false;
                    Flags--;
                    Bombs++;
                }
                else
                {
                    selectedCell.Flagged = true;
                    Flags++;
                    Bombs--;
                }
            }
            else
            {
                if(selectedCell.IsBomb)
                {
                    //GameOver -> Lose
                    return;
                }
                else
                {
                    BreadthFirstSearch(selectedCell);
                }
            }

            if (Bombs == 0)
            {
                //check if win
            }
        }

        void BreadthFirstSearch(Cell cell)
        {
            if(cell.Snooped || cell.Flagged)
            {
                return;
            }
            cell.Snooped = true;
            Snooped++;

            if(cell.Proximity > 0)
            {
                return;
            }
            else
            {
                foreach (var neighbour in cell.Neighbours)
                {
                    if(neighbour != null) BreadthFirstSearch(neighbour);
                }
            }
        }
        
        public override string ToString()
        {
            string board = "  ";
            for (var x = 0; x < Cols; x++)
            {
                board += " " + x + " ";
            }
            board += "\n";
            for (var y = 0; y < Rows; y++)
            {
                string boardRow = y + " ";
                for (var x = 0; x < Cols; x++)
                {
                    var cell = Field[x, y];
                    string boardCell = cell.IsBomb ? "x" : cell.Proximity.ToString();
                    boardRow += "[" + boardCell + "]";
                }
                board += boardRow + "\n";
            }
            return board;
        }
    }
}