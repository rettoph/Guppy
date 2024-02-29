using Guppy.Attributes;
using Guppy.Example.Client.Enums;
using Guppy.Example.Client.Messages;
using Guppy.Example.Client.Services;
using Guppy.Game.Common;
using Guppy.Game.Components;
using Guppy.Game.ImGui;
using Guppy.Game.Input;
using Guppy.Game.Input.Constants;
using Guppy.Game.Input.Providers;
using Guppy.Game.MonoGame.Primitives;
using Guppy.Game.MonoGame.Utilities.Cameras;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Example.Client.Entities
{
    [AutoLoad]
    [GuppyFilter<MainGuppy>]
    public class World : IGuppyComponent, IGuppyDrawable, IGuppyUpdateable, IInputSubscriber<PlaceSandInput>, IDebugComponent
    {
        private const int InputIndicatorVertices = 30;

        private int _lastScrollValue;
        private CellTypeEnum _inputCellType;
        private bool _inputActive;
        private int _inputRadius;
        private Grid _grid;
        private double _elapsedTime;
        private int _awake;

        private readonly StaticPrimitiveBatch<VertexPositionColor> _inputBatch;
        private readonly PointPrimitiveBatch<VertexPositionColor> _gridBatch;
        private readonly Camera2D _camera;
        private readonly ICellTypeService _cellTypes;
        private readonly GameWindow _window;
        private readonly ICursor _mouse;
        private readonly IImGui _imgui;

        public World(IImGui imgui, ICursorProvider cursors, PointPrimitiveBatch<VertexPositionColor> gridBatch, StaticPrimitiveBatch<VertexPositionColor> primitiveBatch, Camera2D camera, ICellTypeService cellTypes, GameWindow window)
        {
            _imgui = imgui;
            _grid = null!;
            _camera = camera;
            _gridBatch = gridBatch;
            _inputBatch = primitiveBatch;
            _cellTypes = cellTypes;
            _window = window;
            _mouse = cursors.Get(Cursors.Mouse);

            _camera.Center = false;
            _window.ClientSizeChanged += this.HandleClientSizeChanged;

            short[] indices = new short[InputIndicatorVertices * 2];
            for (short i = 0; i < InputIndicatorVertices; i++)
            {
                indices[(i * 2)] = i;
                indices[(i * 2) + 1] = (short)((i + 1) % InputIndicatorVertices);
            }
            _inputBatch.Initialize(InputIndicatorVertices, PrimitiveType.LineList, indices);
            this.SetInput(CellTypeEnum.Sand, 10);
        }

        public void Initialize(int width, int height)
        {
            _grid = new Grid(width, height, _cellTypes);

            _gridBatch.Initialize(_grid.Length);

            for (int i = 0; i < _grid.Length; i++)
            {
                Cell cell = _grid.Cells[i];

                _gridBatch.Vertices[i].Position = new Vector3(cell.X, cell.Y, 0);
            }
            //for (int i = 0; i < 10000; i++)
            //{
            //    int index = Random.Shared.Next(0, _grid.Length / 2);
            //    _grid.Cells[index].CurrentType = CellTypeEnum.Sand;
            //}

            _grid.Cells[0].Type = CellTypeEnum.Sand;
            _grid.Cells[0].Awake = true;
        }

        public void Initialize(IGuppy guppy)
        {
            this.Initialize(_window.ClientBounds.Width, _window.ClientBounds.Height);
            //this.Initialize(1, 10);
        }

        public void Draw(GameTime gameTime)
        {
            _camera.Update(gameTime);
            for (int i = 0; i < _grid.Length; i++)
            {
                _gridBatch.Vertices[i].Color = this.GetColor(_grid.Cells[i].Type);
            }

            _gridBatch.Draw(_camera);

            _inputBatch.Draw(_camera.View, Matrix.CreateTranslation(_mouse.Position.X, _mouse.Position.Y, 0) * _camera.Projection);
        }

        public void Update(GameTime gameTime)
        {
            if (_lastScrollValue != _mouse.Scroll)
            {
                this.SetInput(_inputCellType == CellTypeEnum.Sand ? CellTypeEnum.Water : CellTypeEnum.Sand, 10);
            }

            _lastScrollValue = _mouse.Scroll;

            if ((_elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds) < 20)
            {
                return;
            }
            _elapsedTime -= 20;

            _grid = _grid.Update(gameTime, out _awake);

            if (_inputActive)
            {
                foreach (int index in _grid.GetCellIndices(_mouse.Position, _inputRadius))
                {
                    _grid.Cells[index].Type = _inputCellType;
                    _grid.Cells[index].InactivityCount = 0;
                    _grid.Cells[index].Awake = true;
                }
            }
        }

        private void SetInput(CellTypeEnum cellType, int radius)
        {
            _inputCellType = cellType;
            _inputRadius = radius;

            for (int i = 0; i < InputIndicatorVertices; i++)
            {
                float radians = ((MathF.PI * 2) / InputIndicatorVertices) * i;
                _inputBatch.Vertices[i].Position = new Vector3(
                    x: MathF.Cos(radians) * radius,
                    y: MathF.Sin(radians) * radius,
                    z: 0);

                _inputBatch.Vertices[i].Color = this.GetColor(_inputCellType);
            }
        }

        private Color GetColor(CellTypeEnum cellType)
        {
            return cellType switch
            {
                CellTypeEnum.Sand => Color.SandyBrown,
                CellTypeEnum.Water => Color.Blue,
                _ => Color.Transparent
            };
        }

        public void Process(in Guid messageId, PlaceSandInput message)
        {
            _inputActive = message.Active;
        }

        private void HandleClientSizeChanged(object? sender, EventArgs e)
        {
            this.Initialize(_window.ClientBounds.Width, _window.ClientBounds.Height);
        }

        public void RenderDebugInfo(GameTime gameTime)
        {
            _imgui.Text($"Awake: {_awake}");
        }
    }
}
