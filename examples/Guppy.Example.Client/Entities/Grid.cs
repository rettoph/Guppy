using Guppy.Example.Client.Enums;
using Guppy.Example.Client.Services;
using Microsoft.Xna.Framework;

namespace Guppy.Example.Client.Entities
{
    public class Grid
    {
        private Cell _nullCell;

        private Grid _output;
        private ICellTypeService _cellTypes;

        public int Width { get; }
        public int Height { get; }
        public int Length { get; }
        public Cell[] Cells { get; }

        public Grid(int width, int height, ICellTypeService cellTypes) : this(width, height, cellTypes, null)
        {

        }

        private Grid(int width, int height, ICellTypeService cellTypes, Grid? grid)
        {
            this.Width = width;
            this.Height = height;
            this.Length = this.Width * this.Height;

            this.Cells = new Cell[this.Width * this.Height];
            _cellTypes = cellTypes;

            for (int i = 0; i < this.Cells.Length; i++)
            {
                this.Cells[i] = new Cell(i, (short)(i % this.Width), (short)(i / this.Width));
            }

            _output = grid ?? new Grid(width, height, cellTypes, this);
        }

        public Grid Update(GameTime gameTime, out int awake)
        {
            awake = 0;
            for (int i = 0; i < this.Cells.Length; i++)
            {
                this.GetPair(i, out CellPair pair);

                if (pair.Input.Awake == false)
                {
                    if (pair.Output.Type == CellTypeEnum.Air)
                    {
                        pair.Output.Type = pair.Input.Type;

                        if (pair.Output.Awake == false)
                        {
                            pair.Output.InactivityCount = pair.Input.InactivityCount;
                            pair.Output.Awake = pair.Input.Awake;
                        }
                    }

                    pair.Input.Type = CellTypeEnum.Air;
                    pair.Input.InactivityCount = 0;
                    pair.Input.Awake = false;

                    continue;
                }

                _cellTypes.Update(ref pair, this, _output);
                pair.Input.Awake = false;
                pair.Input.Type = CellTypeEnum.Air;
                pair.Input.InactivityCount = 0;

                awake++;
                if (pair.Output.InactivityCount > 10 && pair.Output.Awake == true)
                {
                    pair.Output.Awake = false;
                }
            }

            return _output;
        }

        public ref Cell GetCell(int x, int y)
        {
            int index = this.CalculateIndex(x, y);

            if (index == -1)
            {
                return ref _nullCell;
            }

            return ref this.Cells[index];
        }

        public ref Cell GetCell(int index)
        {
            if (index == -1)
            {
                return ref _nullCell;
            }

            return ref this.Cells[index];
        }

        public void GetPair(int index, out CellPair pair)
        {
            CellPair.Create(this, _output, index, out pair);
        }

        public void GetPair(int x, int y, out CellPair pair)
        {
            int index = this.CalculateIndex(x, y);
            CellPair.Create(this, _output, index, out pair);
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

            return x + (y * (this.Width));
        }
    }
}
