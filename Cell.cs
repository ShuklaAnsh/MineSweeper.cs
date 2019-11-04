using System;

namespace MineSweeper
{
    class Cell
    {
        public struct PositionStruct {
            public int x, y;
            public PositionStruct(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
        int proximity;
        bool snooped;
        bool isBomb;
        bool flagged;
        Cell[] neighbours;
        public PositionStruct Position;
        public Cell(int x, int y)
        {
            this.proximity = 0;
            this.snooped = false;
            this.isBomb = false;
            this.flagged = false;
            this.neighbours = new Cell[8];
            this.Position = new PositionStruct(x, y);
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

        public bool IsBomb
        {
            get => isBomb;
            set => isBomb = value;
        }

        public override string ToString()
        {
            string ret = this.GetType() + "\n" ;
            ret += "Position: (" + Position.x + ", " + Position.y + ")\n";
            ret += "Is a Bomb: " + IsBomb + "\n";
            ret += "is Flagged: " + Flagged + "\n";
            ret += "Snooped: " + Snooped + "\n";
            ret += "Proximity: " + Proximity + "\n";
            int count = 0;
            foreach (var neighbour in neighbours)
            {
                if(neighbour != null)
                {
                    count++;
                }
            }
            ret += "Number of Neighbours: " + count;
            return ret;
        }
    }
}
