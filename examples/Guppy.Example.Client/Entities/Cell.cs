using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.Entities
{
    public struct Cell
    {
        public static readonly Cell Null = new Cell(-1, -1, -1)
        {
            Type = CellTypeEnum.Null
        };

        public readonly int Index;
        public readonly short X;
        public readonly short Y;
        public bool Awake;
        public CellTypeEnum Type;
        public byte InactivityCount;
        public bool Updated;

        public Cell(int index, short x, short y)
        {
            this.Index = index;
            this.X = x;
            this.Y = y;
            this.Awake = false;
            this.Type = CellTypeEnum.Air;
            this.InactivityCount = 0;
        }
    }
}
