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
    [AutoLoad]
    [GuppyFilter<MainGuppy>]
    public class World : IGuppyComponent, IGuppyDrawable, IGuppyUpdateable
    {
        private int _gridIndex;
        private Grid _grid;
        private double _elapsedTime;

        private readonly StaticPrimitiveBatch<VertexPositionColor> _primitiveBatch;
        private readonly Camera2D _camera;
        private readonly ICellTypeService _cellTypes;

        public World(StaticPrimitiveBatch<VertexPositionColor> primitiveBatch, Camera2D camera, ICellTypeService cellTypes)
        {
            _camera = camera;
            _primitiveBatch = primitiveBatch;
            _cellTypes = cellTypes;
        }

        public void Initialize(short width, short height)
        {
            _grid = new Grid(width, height, _cellTypes);

            _primitiveBatch.Initialize(_grid.Length, PrimitiveType.PointList, Enumerable.Range(0, _grid.Length).Select(x => (short)x).ToArray());

            for (short i = 0; i < _grid.Length; i++)
            {
                Cell cell = _grid.Cells[i];

                _primitiveBatch.Vertices[i].Position = new Vector3(cell.X, cell.Y, 0);
            }

            for (int i = 0; i < 1000; i++)
            {
                int index = Random.Shared.Next(0, _grid.Length);
                _grid.Cells[index].Type = CellTypeEnum.Sand;
            }
        }

        public void Initialize(IGuppy guppy)
        {
            this.Initialize(100, 100);
        }

        public void Draw(GameTime gameTime)
        {
            _camera.Update(gameTime);
            for (short i = 0; i < _grid.Length; i++)
            {
                _primitiveBatch.Vertices[i].Color = _grid.Cells[i].Type switch
                {
                    CellTypeEnum.Sand => Color.SandyBrown,
                    _ => Color.Transparent
                };
            }

            _primitiveBatch.Draw(_camera);
        }

        public void Update(GameTime gameTime)
        {
            if ((_elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds) < 100)
            {
                return;
            }
            _elapsedTime -= 100;

            _grid = _grid.Update(gameTime);
        }
    }
}
