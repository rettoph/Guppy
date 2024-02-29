using Guppy.Example.Client.Enum;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Guppy.Example.Client.Entities
{
    public unsafe struct Cell
    {
        public static Cell Void = new Cell()
        {
            Type = CellTypeEnum.Void
        };

        private Cell*[] _neighbors;

        public readonly int Index;
        public readonly int X;
        public readonly int Y;
        public CellTypeEnum Type;

        public ref Cell this[CellNeighborEnum neighbor]
        {
            get => ref Unsafe.AsRef<Cell>(_neighbors[(int)neighbor]);
        }

        public Cell(int index, int x, int y) : this()
        {
            this.Index = index;
            this.X = x;
            this.Y = y;
            this.Type = CellTypeEnum.Air;

            _neighbors = new Cell*[8];
        }

        public void SetNeighbor(CellNeighborEnum neighbor, ref Cell cell)
        {
            _neighbors[(int)neighbor] = (Cell*)Unsafe.AsPointer<Cell>(ref cell);
        }
    }
}
