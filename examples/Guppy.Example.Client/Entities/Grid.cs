using Guppy.Attributes;
using Guppy.Example.Client.CellTypes;
using Guppy.Example.Client.Enum;
using Guppy.Example.Client.Services;
using Guppy.Game.Common;
using Guppy.Game.MonoGame.Primitives;
using Guppy.Game.MonoGame.Utilities.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Guppy.Example.Client.Entities
{
    public class Grid
    {
        private ICellTypeService _cellTypes;

        public short Width { get; }
        public short Height { get; }
        public int Length { get; }
        public Cell[] Cells { get; }

        public Grid Output { get; set; }

        public Grid(short width, short height, ICellTypeService cellTypes) : this(width, height, cellTypes, null)
        {

        }

        private Grid(short width, short height, ICellTypeService cellTypes, Grid? grid)
        {
            this.Width = width;
            this.Height = height;
            this.Length = this.Width * this.Height;

            this.Cells = new Cell[this.Width * this.Height];
            _cellTypes = cellTypes;

            for (short i = 0; i < this.Cells.Length; i++)
            {
                this.Cells[i] = new Cell(i, i % this.Width, i / this.Height);

                this.SetNeighbors(ref this.Cells[i]);
            }

            this.Output = grid ?? new Grid(width, height, cellTypes, this);
        }

        public Grid Update(GameTime gameTime)
        {
            for (int i = 0; i < this.Cells.Length; i++)
            {
                ref Cell input = ref this.Cells[i];
                ref Cell output = ref this.Output.Cells[i];

                output.Type = CellTypeEnum.Air;

                if (input.Type == CellTypeEnum.Air)
                {
                    continue;
                }

                _cellTypes.Update(ref input, ref output);
            }

            return this.Output;
        }

        private void SetNeighbors(ref Cell cell)
        {
            cell.SetNeighbor(CellNeighborEnum.Down, ref this.GetCell(cell.X, cell.Y + 1));
        }

        public ref Cell GetCell(int x, int y)
        {
            int index = this.CalculateIndex(x, y);

            if (index == -1)
            {
                return ref Cell.Void;
            }

            return ref this.Cells[index];
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

            return x + (y * (this.Height));
        }
    }
}
