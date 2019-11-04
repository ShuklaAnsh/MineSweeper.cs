using System;

namespace MineSweeper
{
    class Cell
    {
        private int proximity;
        private bool snooped;
        private bool isBomb;
        private bool flagged;
        private Cell[] neighbours;
        public Cell(int row, int col)
        {
            proximity = 0;
            snooped = false;
            isBomb = false;
            flagged = false;
            neighbours = new Cell[8];
        }

        public Cell Left
        {
            get => neighbours[0];
            set => neighbours[0] = value;
        }
        public Cell TopLeft
        {
            get => neighbours[1];
            set => neighbours[1] = value;
        }
        public Cell Top
        {
            get => neighbours[2];
            set => neighbours[2] = value;
        }
        public Cell TopRight
        {
            get => neighbours[3];
            set => neighbours[3] = value;
        }
        public Cell Right
        {
            get => neighbours[4];
            set => neighbours[4] = value;
        }
        public Cell BottomRight
        {
            get => neighbours[5];
            set => neighbours[5] = value;
        }
        public Cell Bottom
        {
            get => neighbours[6];
            set => neighbours[6] = value;
        }
        public Cell BottomLeft
        {
            get => neighbours[7];
            set => neighbours[7] = value;
        }

        public int Proximity
        {
            get => proximity;
            set => proximity = value;
        }

        public bool Snooped
        {
            get => snooped;
            set => snooped = value;
        }

        public bool Flagged
        {
            get => flagged;
            set => flagged = value;
        }

        public bool Bomb
        {
            get => isBomb;
            set => isBomb = value;
        }
    }
}
