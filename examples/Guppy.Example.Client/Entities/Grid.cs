using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Guppy.Example.Client.Enums;
using Guppy.Example.Client.Services;
using Guppy.Example.Client.Utilities;
using Microsoft.Xna.Framework;

namespace Guppy.Example.Client.Entities
{
    public unsafe class Grid : IDisposable
    {
        private bool _disposed;
        private readonly Grid _output;
        private readonly ICellTypeService _cellTypes;
        private Cell _nullCell = Cell.Null;

        public int Width { get; }
        public int Height { get; }
        public int Length { get; }
        public Cell* Cells;
        private readonly int[] _cellUpdateOrder;

        public Grid(int width, int height, ICellTypeService cellTypes) : this(width, height, cellTypes, null)
        {

        }

        private Grid(int width, int height, ICellTypeService cellTypes, Grid? grid)
        {
            this._cellTypes = cellTypes;

            this.Width = width + (width % 2);
            this.Height = height + (height % 2);
            this.Length = this.Width * this.Height;

            this.Cells = (Cell*)Marshal.AllocHGlobal(this.Length * sizeof(Cell));
            this._cellUpdateOrder = new int[this.Length];

            for (int i = 0; i < this.Length; i++)
            {
                this.Cells[i] = new Cell(i, (short)(i % this.Width), (short)(i / this.Width));
            }

            this._output = grid ?? new Grid(width, height, cellTypes, this);

            for (int i = 0; i < this.Length; i++)
            {
                this.ConfigureCell(ref this.Cells[i]);
            }

            this.SetCellUpdateOrder();
        }



#pragma warning disable IDE0060 // Remove unused parameter
        public Grid Update(GameTime gameTime, out int awake)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            awake = 0;
            for (int i = 0; i < this._cellUpdateOrder.Length; i++)
            {
                awake += this.Update(this._cellUpdateOrder[i]);
            }

            return this._output;
        }

        private unsafe int Update(int index)
        {
            ref Cell cell = ref this._output.Cells[index];

            try
            {
                if (cell.Old.Awake == false)
                {
                    if (cell.Type == CellTypeEnum.Air)
                    {
                        cell.Type = cell.Old.Type;

                        if (cell.Awake == false)
                        {
                            cell.InactivityCount = cell.Old.InactivityCount;
                            cell.Awake = cell.Old.Awake;
                        }
                    }

                    return 0;
                }

                this._cellTypes.Update(ref cell, this, this._output);

                return 1;
            }
            finally
            {
                cell.Old.Displaced = false;
                cell.Old.Updated = false;
                cell.Old.Awake = false;
                cell.Old.Type = CellTypeEnum.Air;
                cell.Old.InactivityCount = 0;

                cell.Updated = true;
            }
        }

        public ref Cell GetCell(int x, int y)
        {
            int index = this.CalculateIndex(x, y);

            if (index == -1)
            {
                return ref this._nullCell;
            }

            return ref this.Cells[index];
        }

        public ref Cell GetCell(int index)
        {
            if (index == -1)
            {
                return ref this._nullCell;
            }

            return ref this.Cells[index];
        }

        public IEnumerable<int> GetCellIndices(Vector2 position, int radius)
        {
            int posX = (int)position.X;
            int posY = (int)position.Y;
            for (int x = -radius; x < radius; x++)
            {
                for (int y = -radius; y < radius; y++)
                {
                    if ((x * x) + (y * y) <= radius * radius)
                    {
                        Cell cell = this.GetCell(posX + x, posY + y);

                        if (cell.Index != -1)
                        {
                            yield return cell.Index;
                        }
                    }
                }
            }
        }

        public int CalculateIndex(int x, int y)
        {
            if (x < 0 || x >= this.Width)
            {
                return -1;
            }

            if (y < 0 || y >= this.Height)
            {
                return -1;
            }

            return x + (y * this.Width);
        }

        private unsafe void ConfigureCell(ref Cell cell)
        {
            int[] neighbors = [
                this.CalculateIndex(cell.X - 1, cell.Y - 1),
                this.CalculateIndex(cell.X + 0, cell.Y - 1),
                this.CalculateIndex(cell.X + 1, cell.Y - 1),

                this.CalculateIndex(cell.X - 1, cell.Y + 0),
                this.CalculateIndex(cell.X + 1, cell.Y + 0),

                this.CalculateIndex(cell.X - 1, cell.Y + 1),
                this.CalculateIndex(cell.X + 0, cell.Y + 1),
                this.CalculateIndex(cell.X - 1, cell.Y + 1),
            ];

            neighbors = neighbors.Where(x => x != -1).ToArray();

            cell.Neighbors = new NativeArray<int>(neighbors.Length);
            cell.OldPtr = (Cell*)Unsafe.AsPointer(ref this._output.Cells[cell.Index]);

            for (int i = 0; i < neighbors.Length; i++)
            {
                cell.Neighbors[i] = neighbors[i];
            }
        }


        /// <summary>
        /// This method is a mess but the short of it is:
        /// We dont want to update cells in order from 0 on. This results in cells being updated left to right one row at a time.
        /// Such an update method causes particle movement to favor the left side of the screen, which looks weird. Water will rush to the left
        /// first every time.
        /// 
        /// Additionally this method updates top to bottom, which means sand cant fall until the frame after whatever is beneath it has fallen.
        /// Makes sand space itself out 1 pixel while falling
        /// 
        /// This method take all possible indexes and generates an array if incidec sorted such that:
        /// Rows are done from the bottom up
        /// Cells within each row are weaved half going left to right the other half going right to left.
        /// Woven into each other
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void SetCellUpdateOrder()
        {
            // Creates a sort of woven array of numbers interlacing front to back togehter
            // 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 => (0, 9), (2, 7), (4, 5), (6, 3), (8, 1)
            var updateIds = Enumerable.Zip(
                Enumerable.Range(0, this.Width / 2).Reverse().Select(x => x * 2).Where(x => x < this.Width),
                Enumerable.Range(0, this.Width / 2).Select(x => (x * 2) + 1).Where(x => x < this.Width)
            ).ToArray();
            var updateIdsReverse = updateIds.Reverse().ToArray();

            int index = 0;
            for (int y = this.Height - 1; y >= 0; y--)
            {
                for (int x = 0; x < this.Width / 2; x++)
                {
                    if (y % 2 == 0)
                    {
                        this._cellUpdateOrder[index++] = this.CalculateIndex(updateIds[x].First, y);
                        this._cellUpdateOrder[index++] = this.CalculateIndex(updateIds[x].Second, y);
                    }
                    else
                    {
                        this._cellUpdateOrder[index++] = this.CalculateIndex(updateIdsReverse[x].First, y);
                        this._cellUpdateOrder[index++] = this.CalculateIndex(updateIdsReverse[x].Second, y);
                    }
                }
            }

            if (this._cellUpdateOrder.Any(x => x == -1))
            {
                throw new Exception();
            }

            if (this._cellUpdateOrder.Distinct().Count() != this._cellUpdateOrder.Length)
            {
                var test = this._cellUpdateOrder.GroupBy(x => x).OrderByDescending(x => x.Count()).ToArray();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                this._disposed = true;

                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    this._output.Dispose();

                    for (int i = 0; i < this.Length; i++)
                    {
                        this.Cells[i].Dispose();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                Marshal.FreeHGlobal((nint)this.Cells);
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Grid()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}