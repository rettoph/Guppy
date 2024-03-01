using Guppy.Example.Client.Enums;
using Guppy.Example.Client.Utilities;

namespace Guppy.Example.Client.Entities
{
    public unsafe struct Cell : IDisposable
    {
        public static Cell Null = new Cell(-1, -1, -1)
        {
            Type = CellTypeEnum.Null,
            Updated = true,
        };

        public readonly int Index;
        public readonly short X;
        public readonly short Y;
        public CellTypeEnum Type;
        public byte InactivityCount;
        public NativeArray<int> Neighbors;
        public Cell* OldPtr;
        public ref Cell Old => ref this.OldPtr[0];
        public Cell Latest => this.GetLatest();

        public bool Awake;
        public bool Updated;
        public bool Displaced;

        public Cell(int index, short x, short y)
        {
            this.Index = index;
            this.X = x;
            this.Y = y;
            this.Awake = false;
            this.Type = CellTypeEnum.Air;
            this.InactivityCount = 0;
            this.Neighbors = new NativeArray<int>(5);
        }

        private Cell GetLatest()
        {
            if (this.Updated)
            {
                return this;
            }

            return this.Old;
        }

        public void Dispose()
        {
            this.Neighbors.Dispose();
        }
    }
}
