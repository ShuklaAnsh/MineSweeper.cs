// Contains the class for a cell

using System;

namespace MineSweeper
{
    /*
        The class for a Cell object
        Contains all cell properties and functions
    */
    class Cell
    {
        // a struct definiition. It that holds the cells position coordinates (x,y)
        public struct PositionStruct
        {
            public int x, y;
            public PositionStruct(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
        // private members
        int proximity;
        bool snooped;
        bool isBomb;
        bool flagged;
        Cell[] neighbours;
        
        // public PositionStruct for a cell
        public PositionStruct Position;

        /**
            Constructor for a cell. Initializes its variables

            Parms:
                int x - The x position of the cell
                int y - The y position of the cell
         */
        public Cell(int x, int y)
        {
            this.proximity = 0;
            this.snooped = false;
            this.isBomb = false;
            this.flagged = false;
            this.neighbours = new Cell[8];
            this.Position = new PositionStruct(x, y);
        }

        // public getter for the Cell's neighbours
        public Cell[] Neighbours
        {
            get => neighbours;
        }

        // public getter and setted for the Cell's left neighbour
        public Cell Left
        {
            get => neighbours[0];
            set => neighbours[0] = value;
        }

        // public getter and setted for the Cell's top left neighbour
        public Cell TopLeft
        {
            get => neighbours[1];
            set => neighbours[1] = value;
        }

        // public getter and setted for the Cell's top neighbour
        public Cell Top
        {
            get => neighbours[2];
            set => neighbours[2] = value;
        }

        // public getter and setted for the Cell's top right neighbour
        public Cell TopRight
        {
            get => neighbours[3];
            set => neighbours[3] = value;
        }

        // public getter and setted for the Cell's right neighbour
        public Cell Right
        {
            get => neighbours[4];
            set => neighbours[4] = value;
        }

        // public getter and setted for the Cell's bottom right neighbour
        public Cell BottomRight
        {
            get => neighbours[5];
            set => neighbours[5] = value;
        }

        // public getter and setted for the Cell's bottom neighbour
        public Cell Bottom
        {
            get => neighbours[6];
            set => neighbours[6] = value;
        }
        
        // public getter and setted for the Cell's bottom left neighbour
        public Cell BottomLeft
        {
            get => neighbours[7];
            set => neighbours[7] = value;
        }

        // public getter and setted for the Cell's proximity property
        public int Proximity
        {
            get => proximity;
            set => proximity = value;
        }

        // public getter and setted for the Cell's snooped property
        public bool Snooped
        {
            get => snooped;
            set => snooped = value;
        }

        // public getter and setted for the Cell's flagged property
        public bool Flagged
        {
            get => flagged;
            set => flagged = value;
        }

        // public getter and setted for the Cell's isBomb property
        public bool IsBomb
        {
            get => isBomb;
            set => isBomb = value;
        }

        /**
            Public Function, allows nullable paramaters
            Connects the given cell references to this cell as neighbours

            Parms:
                ref Cell? left         - reference to the cell that is to the left of this cell, can be null
                ref Cell? top_left     - reference to the cell that is to the top left of this cell, can be null
                ref Cell? top          - reference to the cell that is to the top of this cell, can be null
                ref Cell? top_right    - reference to the cell that is to the top right of this cell, can be null
                ref Cell? right        - reference to the cell that is to the right of this cell, can be null
                ref Cell? bottom_right - reference to the cell that is to the bottom right of this cell, can be null
                ref Cell? bottom       - reference to the cell that is to the bottom of this cell, can be null
                ref Cell? bottom_left  - reference to the cell that is to the bottom left of this cell, can be null
         */
        #nullable enable
        public void ConnectNeighbours(
            ref Cell? left, ref Cell? topLeft, ref Cell? top, 
            ref Cell? topRight, ref Cell? right, ref Cell? bottomRight, 
            ref Cell? bottom, ref Cell? bottomLeft
        )
        {
            Left = left;
            if (left != null) left.Right = this;

            TopLeft = topLeft;
            if (topLeft != null) topLeft.BottomRight = this;

            Top = top;
            if (top != null) top.Bottom = this;

            TopRight = topRight;
            if (topRight != null) topRight.BottomLeft = this;

            Right = right;
            if (right != null) right.Left = this;

            BottomRight = bottomRight;
            if (bottomRight != null) bottomRight.TopLeft = this;

            Bottom = bottom;
            if (bottom != null) bottom.Top = this;

            BottomLeft = bottomLeft;
            if (bottomLeft != null) bottomLeft.TopRight = this;
        }

        /**
            Public Function, Overides default object toString() method
            Formats this cell to display its properties

            Returns:
                a string that represents the cell
         */
        public override string ToString()
        {
            string ret = this.GetType() + "\n";
            ret += "Position: (" + Position.x + ", " + Position.y + ")\n";
            ret += "Is a Bomb: " + IsBomb + "\n";
            ret += "is Flagged: " + Flagged + "\n";
            ret += "Snooped: " + Snooped + "\n";
            ret += "Proximity: " + Proximity + "\n";
            int count = 0;
            foreach (var neighbour in neighbours)
            {
                if (neighbour != null)
                {
                    count++;
                }
            }
            ret += "Number of Neighbours: " + count;
            return ret;
        }
    }
}
