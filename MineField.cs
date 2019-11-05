/*
    Contains the logic for the board
    Handles cell selections and board output
*/

using System;

namespace MineSweeper
{
    class MineField
    {

        // enum for the game modes available for selecting a cell
        public enum Mode
        {
            Flagging, Snooping
        }

        // enum for game status (Game ended or not)
        public enum GameStatus
        {
            Running, Win, Lose
        }

        // 2D Cell array to contain our board grid
        Cell[,] Field;
        // private int members for number of rows, 
        // cols, cells snooped, and cells flagged
        int Rows, Cols, Snooped, Flags;
        // public int members for number of bombs and moves used
        public int Bombs, Moves;

        /**
            Public function
            Constructor for the MineField class.
            Calls to initialize the cells

            Parms:
                int rows     - The number of rows in the field
                int cols     - The number of cols in the field
                int numBombs - The number of bombs in the field
         */
        public MineField(int rows, int cols, int numBombs)
        {
            Field = new Cell[cols, rows];
            Rows = rows;
            Cols = cols;
            Bombs = numBombs;
            InitCells();
        }

        /**
            Initialize the cells by populating the 2D Cell array (Field)
            and calls to initialize the cell's neighbours
         */
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

        /**
            Calls to initializes cell neighbours by connecting the cell to its neighbours
            depending on the cells position

            Note: 
                Cell pairing to neighbours may have redundancy, but no
                significant loss due to processing.
         */
        void InitCellNeighbours()
        {
            // a Cell set to null for generalized border conditions
            Cell borderCell = null;
            for (var x = 0; x < Cols; x++)
            {
                for (var y = 0; y < Rows; y++)
                {
                    var cell = Field[x, y];

                    // Handle neighbours if cell is not at a corner, (CornerBoundaries handles corners)
                    if (!CornerBoundaries(ref cell))
                    {
                        // Left Border
                        if (x == 0)
                        {
                            // Cells relative to cell at current (x, y)
                            var top = Field[x, y - 1];
                            var topRight = Field[x + 1, y - 1];
                            var right = Field[x + 1, y];
                            var bottomRight = Field[x + 1, y + 1];
                            var bottom = Field[x, y + 1];
                            cell.ConnectNeighbours(ref borderCell, ref borderCell, ref top, ref topRight, ref right, ref bottomRight, ref bottom, ref borderCell);
                        }
                        // Top Border
                        else if (y == 0)
                        {
                            var left = Field[x - 1, y];
                            var right = Field[x + 1, y];
                            var bottomRight = Field[x + 1, y + 1];
                            var bottom = Field[x, y + 1];
                            var bottomLeft = Field[x - 1, y + 1];
                            cell.ConnectNeighbours(ref left, ref borderCell, ref borderCell, ref borderCell, ref right, ref bottomRight, ref bottom, ref bottomLeft);
                        }
                        // Right Border
                        else if (x == (Cols - 1))
                        {
                            var left = Field[x - 1, y];
                            var topLeft = Field[x - 1, y - 1];
                            var top = Field[x, y - 1];
                            var bottom = Field[x, y + 1];
                            var bottomLeft = Field[x - 1, y + 1];
                            cell.ConnectNeighbours(ref left, ref topLeft, ref top, ref borderCell, ref borderCell, ref borderCell, ref bottom, ref bottomLeft);
                        }
                        // Bottom Border
                        else if (y == (Rows - 1))
                        {
                            var left = Field[x - 1, y];
                            var topLeft = Field[x - 1, y - 1];
                            var top = Field[x, y - 1];
                            var topRight = Field[x + 1, y - 1];
                            var right = Field[x + 1, y];
                            cell.ConnectNeighbours(ref left, ref topLeft, ref top, ref topRight, ref right, ref borderCell, ref borderCell, ref borderCell);
                        }
                        // Middle of the board (not on the edges or corners)
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
                            cell.ConnectNeighbours(ref left, ref topLeft, ref top, ref topRight, ref right, ref bottomRight, ref bottom, ref bottomLeft);
                        }
                    }
                }
            }
        }

        /**
            Handles neighbours when current cell is at a corner
            Calls to connect the cell to its neighbours
            (TopLeft, TopRight, BottomRight, BottomLeft)

            Parms:
                ref Cell cell - Reference to the current cell

            Returns:
                True if cell's position was at a corner
         */
        bool CornerBoundaries(ref Cell cell)
        {
            int x = cell.Position.x;
            int y = cell.Position.y;
            Cell borderCell = null;
            // Top Left Border
            if ((x == 0) && (y == 0))
            {

                // Cells relative to cell at current (x, y)
                var right = Field[x + 1, y];
                var bottomRight = Field[x + 1, y + 1];
                var bottom = Field[x, y + 1];
                cell.ConnectNeighbours(ref borderCell, ref borderCell, ref borderCell, ref borderCell, ref right, ref bottomRight, ref bottom, ref borderCell);
                return true;
            }
            // Top Right Border
            else if ((x == (Cols - 1)) && (y == 0))
            {
                var left = Field[x - 1, y];
                var bottom = Field[x, y + 1];
                var bottomLeft = Field[x - 1, y + 1];
                cell.ConnectNeighbours(ref left, ref borderCell, ref borderCell, ref borderCell, ref borderCell, ref borderCell, ref bottom, ref bottomLeft);
                return true;
            }
            // Bottom Right Border
            else if ((x == (Cols - 1)) && (y == (Rows - 1)))
            {
                var left = Field[x - 1, y];
                var topLeft = Field[x - 1, y - 1];
                var top = Field[x, y - 1];
                cell.ConnectNeighbours(ref left, ref topLeft, ref top, ref borderCell, ref borderCell, ref borderCell, ref borderCell, ref borderCell);
                return true;
            }
            // Bottom Left Border
            else if ((x == 0) && (y == (Rows - 1)))
            {
                var top = Field[x, y - 1];
                var topRight = Field[x + 1, y - 1];
                var right = Field[x + 1, y];
                cell.ConnectNeighbours(ref borderCell, ref borderCell, ref top, ref topRight, ref right, ref borderCell, ref borderCell, ref borderCell);
                return true;
            }
            return false;
        }

        /**
            Initializes the games bombs by settings random cells as bombs
            Called on the first move.

            Ensures that the starting cell is not a bomb.

            Parms:
                ref Cell startingCell - the first cell selected in the game
         */
        void InitBombs(ref Cell startingCell)
        {
            Random rng = new Random();
            // Pick random cells to be bombs
            for (int i = 0; i < Bombs; i++)
            {
                // random number between 0 and (Cols - 1)
                int randCol = rng.Next(0, Cols);
                int randRow = rng.Next(0, Rows);
                Cell randCell = Field[randCol, randRow];
                // if the random cell is already a bomb or the starting cell, skip
                if (randCell.IsBomb || randCell.Position.Equals(startingCell.Position))
                {
                    i--;
                    continue;
                }
                randCell.IsBomb = true;
            }
            // Set the proximities of each Cell to bombs
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

        /**
            Handles the processed user selection depending on mode
            Calls to run bfs to find open areas (areas with cells with no proximity)
            relative to selected cell

            Parms:
                Cell.PositionStruct pos - The coordinates for the selected cell
                Mode mode               - The mode of selection (Flagging or Snooping)

            Returns:
                GameStatus for if the game is running or ended (ended = win or lose)
         */
        public GameStatus HandleSelection(Cell.PositionStruct pos, Mode mode)
        {
            // selectedCell initialized to invalid coordinate (-1,-1)
            Cell selectedCell = new Cell(-1, -1);
            // try to get selected cell, catch exception if invalid position
            try
            {
                selectedCell = Field[pos.x, pos.y];
            }
            catch (IndexOutOfRangeException)
            {
                Console.Write("Bad Coordinates. Press Enter to Continue...");
                Console.ReadLine();
                return GameStatus.Running;
            }

            // If this is the first selection of the game, initialize the bombs
            if (Moves == 0)
            {
                InitBombs(ref selectedCell);
            }
            // if the cell has already been snooped, selection handling is over
            else if (selectedCell.Snooped)
            {
                return GameStatus.Running;
            }

            Moves++;
            // if the selection mode is set to flagging, toggle flag on cell
            if (mode == Mode.Flagging)
            {
                // Toggle off if on
                if (selectedCell.Flagged)
                {
                    selectedCell.Flagged = false;
                    Flags--;
                    Bombs++;
                }
                // Toggle on if off
                else
                {
                    selectedCell.Flagged = true;
                    Flags++;
                    Bombs--;
                }
            }
            // else, selection in snooping mode
            else
            {
                if (selectedCell.Flagged)
                {
                    Moves--;
                    return GameStatus.Running;
                }
                if (selectedCell.IsBomb)
                {
                    return GameStatus.Lose;
                }
                // if the cell is not flagged or a bomb, run bfs
                else
                {
                    BreadthFirstSearch(selectedCell);
                }
            }

            // if the number of bombs reaches 0, check to see if game has been won
            if (Bombs == 0)
            {
                int numCells = Rows * Cols;
                int numCheckedCells = Snooped + Flags;
                if ((numCheckedCells == numCells) && CheckFlags())
                {
                    return GameStatus.Win;
                }
            }
            return GameStatus.Running;
        }

        /**
            Recursive BFS algorithm for uncovering areas by
            snooping the given cell and its neighbours

            Parms:
                Cell cell - The cell to start the search on 
         */
        void BreadthFirstSearch(Cell cell)
        {
            // Base case: if the cell is snooped or flagged, end
            if (cell.Snooped || cell.Flagged)
            {
                return;
            }
            // Snoop cell
            cell.Snooped = true;
            Snooped++;
            // Another ending case: if you've reached a cell with a proximity, end
            if (cell.Proximity > 0)
            {
                return;
            }
            else
            {
                // Call BFS on the cell's neighbours
                foreach (var neighbour in cell.Neighbours)
                {
                    if (neighbour != null) BreadthFirstSearch(neighbour);
                }
            }
        }

        /**
            Checks if all the flagged cells are also not bombs

            Used to see if the game has been won.
            Called when the number of bombs reaches 0.

            Returns:
                false if a flagged cell is actually not a bomb
         */
        bool CheckFlags()
        {
            foreach (var cell in Field)
            {
                if (cell.Flagged && !cell.IsBomb) return false;
            }
            return true;
        }

        /**
            Public Function
            Formats the board to display its current state

            Returns:
                a string that represents the board in its current state
         */
        public string PrintBoard()
        {
            string board = "   ";
            for (var x = 0; x < Cols; x++)
            {
                // pad the x-axis depending on if x is double digits
                board += (x > 9) ? x + " " : " " + x + " ";
            }
            board += "\n";
            for (var y = 0; y < Rows; y++)
            {

                // pad the y-axis depending on if y is double digits
                string boardRow = (y > 9) ? y + " " : " " + y + " ";
                for (var x = 0; x < Cols; x++)
                {
                    var cell = Field[x, y];
                    string boardCell = " ";
                    // if the cell has been snooped, display its proximity
                    if (cell.Snooped) boardCell = cell.Proximity.ToString();
                    // if the cell is flagged, display a '#' to represent a flag
                    else if (cell.Flagged) boardCell = "#";
                    boardRow += "[" + boardCell + "]";
                }
                board += boardRow + "\n";
            }
            return board;
        }

        /**
            Public Function, Overides default object toString() method
            Formats the board to display bombs and cells with proximities

            Returns:
                a string that represents the board
         */
        public override string ToString()
        {
            string board = "   ";
            for (var x = 0; x < Cols; x++)
            {
                board += (x > 9) ? x + " " : " " + x + " ";
            }
            board += "\n";
            for (var y = 0; y < Rows; y++)
            {
                string boardRow = (y > 9) ? y + " " : " " + y + " ";
                for (var x = 0; x < Cols; x++)
                {
                    var cell = Field[x, y];
                    string boardCell = cell.IsBomb ? "*" : cell.Proximity.ToString();
                    boardRow += "[" + boardCell + "]";
                }
                board += boardRow + "\n";
            }
            return board;
        }
    }
}