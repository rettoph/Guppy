using Guppy.Core.Common.Attributes;
using Guppy.Engine.Common.Enums;
using Guppy.Example.Client.Enums;
using Guppy.Example.Client.Messages;
using Guppy.Example.Client.Services;
using Guppy.Game.Common;
using Guppy.Game.Common.Attributes;
using Guppy.Game.Common.Components;
using Guppy.Game.Common.Enums;
using Guppy.Game.ImGui.Common;
using Guppy.Game.Input.Common;
using Guppy.Game.Input.Common.Constants;
using Guppy.Game.Input.Common.Services;
using Guppy.Game.MonoGame.Common.Primitives;
using Guppy.Game.MonoGame.Common.Utilities.Cameras;
using Guppy.Game.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Example.Client.Entities
{
    [AutoLoad]
    [SceneFilter<MainScene>]
    public class World : ISceneComponent, IUpdatableComponent, IDrawableComponent,
        IInputSubscriber<PlaceSandInput>,
        IInputSubscriber<SelectCellTypeInput>
    {
        private const int InputIndicatorVertices = 30;

        private RenderTarget2D _renderTarget;
        private int _lastScrollValue;
        private CellTypeEnum _inputCellType;
        private bool _inputActive;
        private int _inputRadius;
        private Grid _grid;
        private double _elapsedTime;
        private int _awake;
        private int _inputTypeIndex;
        private CellTypeEnum[] _inputTypes = new[]
        {
            CellTypeEnum.Sand,
            CellTypeEnum.Water,
            CellTypeEnum.Concrete,
            CellTypeEnum.Plant,
            CellTypeEnum.Air
        };

        private readonly StaticPrimitiveBatch<VertexPositionColor> _inputBatch;
        private readonly PointPrimitiveBatch<VertexPositionColor> _gridBatch;
        private readonly Camera2D _camera;
        private readonly ICellTypeService _cellTypes;
        private readonly GameWindow _window;
        private readonly ICursor _mouse;
        private readonly IImGui _imgui;
        private readonly GraphicsDevice _graphics;
        private readonly SpriteBatch _spriteBatch;

        public World(
            SpriteBatch spriteBatch,
            GraphicsDevice graphics,
            IImGui imgui,
            ICursorService cursors,
            PointPrimitiveBatch<VertexPositionColor> gridBatch,
            StaticPrimitiveBatch<VertexPositionColor> primitiveBatch,
            Camera2D camera,
            ICellTypeService cellTypes,
            GameWindow window)
        {
            _imgui = imgui;
            _grid = null!;
            _renderTarget = null!;
            _camera = camera;
            _gridBatch = gridBatch;
            _inputBatch = primitiveBatch;
            _cellTypes = cellTypes;
            _window = window;
            _mouse = cursors.Get(Cursors.Mouse);
            _graphics = graphics;
            _spriteBatch = spriteBatch;

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

        public unsafe void Initialize(int width, int height)
        {
            _grid?.Dispose();
            _renderTarget?.Dispose();

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

            _renderTarget = new RenderTarget2D(_graphics, width, height);
        }

        [SequenceGroup<InitializeComponentSequenceGroup>(InitializeComponentSequenceGroup.Initialize)]
        public void Initialize(IScene scene)
        {
            this.Initialize(_window.ClientBounds.Width / 2, _window.ClientBounds.Height / 2);
        }

        [SequenceGroup<DrawComponentSequenceGroup>(DrawComponentSequenceGroup.Draw)]
        public unsafe void Draw(GameTime gameTime)
        {
            _graphics.SetRenderTarget(_renderTarget);
            _camera.Update(gameTime);
            for (int i = 0; i < _grid.Length; i++)
            {
                _gridBatch.Vertices[i].Color = this.GetColor(_grid.Cells[i].Type);
            }

            _gridBatch.Draw(_camera);

            _inputBatch.Draw(_camera.View, Matrix.CreateTranslation(_mouse.Position.X / 2, _mouse.Position.Y / 2, 0) * _camera.Projection);

            _graphics.SetRenderTarget(null);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_renderTarget, _graphics.Viewport.Bounds, Color.White);
            _spriteBatch.End();
        }

        [SequenceGroup<UpdateComponentSequenceGroup>(UpdateComponentSequenceGroup.Update)]
        public unsafe void Update(GameTime gameTime)
        {
            if (_lastScrollValue != _mouse.Scroll)
            {
                CellTypeEnum inputType = _inputTypes[(_inputTypeIndex++ % _inputTypes.Length)];
                this.SetInput(inputType, 10);
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
                foreach (int index in _grid.GetCellIndices(_mouse.Position / 2, _inputRadius + 1))
                {
                    _grid.Cells[index].Awake = true;
                }

                foreach (int index in _grid.GetCellIndices(_mouse.Position / 2, _inputRadius))
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
                CellTypeEnum.Air => Color.DarkGray,
                CellTypeEnum.Sand => Color.SandyBrown,
                CellTypeEnum.Water => Color.Blue,
                CellTypeEnum.Concrete => Color.Gray,
                CellTypeEnum.Plant => Color.Green,
                CellTypeEnum.Fire => Random.Shared.Next(0, 6) == 0 ? Color.Orange : Color.Red,
                CellTypeEnum.Smolder => Random.Shared.Next(0, 15) == 0 ? Color.Orange : Color.Black,
                CellTypeEnum.Ash => Color.LightGray,
                _ => Color.Pink
            };
        }

        public void Process(in Guid messageId, PlaceSandInput message)
        {
            _inputActive = message.Active;
        }

        public void Process(in Guid messageId, SelectCellTypeInput message)
        {
            this.SetInput(message.Type, _inputRadius);
        }

        private void HandleClientSizeChanged(object? sender, EventArgs e)
        {
            this.Initialize(_window.ClientBounds.Width / 2, _window.ClientBounds.Height / 2);
        }

        public void RenderDebugInfo(GameTime gameTime)
        {
            _imgui.Text($"Awake: {_awake}");
        }
    }
}
