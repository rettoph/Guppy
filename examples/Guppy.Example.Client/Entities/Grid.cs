using Guppy.Attributes;
using Guppy.Example.Client.CellTypes;
using Guppy.Example.Client.Enum;
using Guppy.Game.Common;
using Guppy.Game.MonoGame.Primitives;
using Guppy.Game.MonoGame.Utilities.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Example.Client.Entities
{
    [AutoLoad]
    [GuppyFilter<MainGuppy>]
    public class Grid : IGuppyComponent, IGuppyDrawable, IGuppyUpdateable
    {
        private Cell[] _cells;
        private float _elapsedTime;
        private Dictionary<CellTypeEnum, ICellType> _cellTypes;

        private readonly StaticPrimitiveBatch<VertexPositionColor> _primitiveBatch;
        private readonly Camera2D _camera;

        public short Width { get; private set; }
        public short Height { get; private set; }

        public Grid(StaticPrimitiveBatch<VertexPositionColor> primitiveBatch, Camera2D camera, IEnumerable<ICellType> cellTypes)
        {
            _cells = Array.Empty<Cell>();
            _camera = camera;
            _primitiveBatch = primitiveBatch;
            _cellTypes = cellTypes.ToDictionary(x => x.Type, x => x);
        }

        public void Initialize(short width, short height)
        {
            this.Width = width;
            this.Height = height;

            _cells = new Cell[this.Width * this.Height];
            _primitiveBatch.Initialize(_cells.Length, PrimitiveType.PointList, Enumerable.Range(0, _cells.Length).Select(x => (short)x).ToArray());

            for (short i = 0; i < _cells.Length; i++)
            {
                _cells[i] = new Cell(i, i % this.Width, i / this.Height);
                _primitiveBatch.Vertices[i].Position = new Vector3(_cells[i].X, _cells[i].Y, 0);

                this.SetNeighbors(ref _cells[i]);
            }

            for (int i = 0; i < 1000; i++)
            {
                int index = Random.Shared.Next(0, _cells.Length);
                _cells[index].Type = CellTypeEnum.Sand;
            }
        }

        public void Initialize(IGuppy guppy)
        {
            this.Initialize(100, 100);
        }

        public void Draw(GameTime gameTime)
        {
            _camera.Update(gameTime);
            for (short i = 0; i < _cells.Length; i++)
            {
                _primitiveBatch.Vertices[i].Color = _cells[i].Type switch
                {
                    CellTypeEnum.Sand => Color.SandyBrown,
                    _ => Color.Transparent
                };
            }

            _primitiveBatch.Draw(_camera);

            //_test.Begin(_camera);
            //_test.DrawLine(new VertexPositionColor() { Position = new Vector3(0, 0, 0), Color = Color.Red }, new VertexPositionColor() { Position = new Vector3(100, 1000, 0), Color = Color.Red });
            //_test.End();
        }

        public void Update(GameTime gameTime)
        {
            if ((_elapsedTime += gameTime.ElapsedGameTime.Milliseconds) < 500)
            {
                return;
            }
            _elapsedTime -= 500;

            for (int i = 0; i < _cells.Length; i++)
            {
                ref Cell cell = ref _cells[i];
                if (cell.Type == CellTypeEnum.Air)
                {
                    continue;
                }

                _cellTypes[cell.Type].Update(ref cell, this);
            }
        }

        private void SetNeighbors(ref Cell cell)
        {
            cell.SetNeighbor(CellNeighborEnum.Down, ref this.GetCell(cell.X, cell.Y + 1));
        }

        private ref Cell GetCell(int x, int y)
        {
            int index = this.CalculateIndex(x, y);

            if (index == -1)
            {
                return ref Cell.Void;
            }

            return ref _cells[index];
        }

        private int CalculateIndex(int x, int y)
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
