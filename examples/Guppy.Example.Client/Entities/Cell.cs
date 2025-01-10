using Guppy.Example.Client.Enums;
using Guppy.Example.Client.Utilities;

namespace Guppy.Example.Client.Entities
{
    public unsafe struct Cell(int index, short x, short y) : IDisposable
    {
        public static readonly Cell Null = new(-1, -1, -1)
        {
            Type = CellTypeEnum.Null,
            Updated = true,
        };

        public readonly int Index = index;
        public readonly short X = x;
        public readonly short Y = y;
        public CellTypeEnum Type = CellTypeEnum.Air;
        public byte InactivityCount = 0;
        public NativeArray<int> Neighbors = new(5);
        public Cell* OldPtr;
        public ref Cell Old => ref this.OldPtr[0];
        public Cell Latest => this.GetLatest();

        public bool Awake = false;
        public bool Updated;
        public bool Displaced;

        private Cell GetLatest()
        {
            if (this.Updated)
            {
                return this;
            }

            return this.Old;
        }

        public readonly void Dispose()
        {
            this.Neighbors.Dispose();
        }
    }
}