using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Core.Common.Systems;
using Guppy.Core.Messaging.Common.Enums;
using Guppy.Example.Client.Enums;
using Guppy.Example.Client.Messages;
using Guppy.Example.Client.Services;
using Guppy.Example.Client.Utilities;
using Guppy.Game.Common.Enums;
using Guppy.Game.Common.Systems;
using Guppy.Game.Graphics.Common;
using Guppy.Game.ImGui.Common;
using Guppy.Game.Input.Common;
using Guppy.Game.Input.Common.Constants;
using Guppy.Game.Input.Common.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Example.Client.Entities
{
    public class World : ISceneSystem, IInitializeSystem, IUpdateSystem, IDrawSystem,
        IInputSubscriber<PlaceSandInput>,
        IInputSubscriber<SelectCellTypeInput>
    {
        private const int _inputIndicatorVertices = 30;

        private RenderTarget2D _renderTarget;
        private int _lastScrollValue;
        private CellTypeEnum _inputCellType;
        private bool _inputActive;
        private int _inputRadius;
        private Grid _grid;
        private double _elapsedTime;
        private int _awake;
        private int _inputTypeIndex;
        private readonly CellTypeEnum[] _inputTypes = [
            CellTypeEnum.Sand,
            CellTypeEnum.Water,
            CellTypeEnum.Concrete,
            CellTypeEnum.Plant,
            CellTypeEnum.Air
        ];

        private readonly StaticPrimitiveBatch<VertexPositionColor> _inputBatch;
        private readonly PointPrimitiveBatch<VertexPositionColor> _gridBatch;
        private readonly ICamera2D _camera;
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
            ICamera2D camera,
            ICellTypeService cellTypes,
            GameWindow window)
        {
            this._imgui = imgui;
            this._grid = null!;
            this._renderTarget = null!;
            this._camera = camera;
            this._gridBatch = gridBatch;
            this._inputBatch = primitiveBatch;
            this._cellTypes = cellTypes;
            this._window = window;
            this._mouse = cursors.Get(Cursors.Mouse);
            this._graphics = graphics;
            this._spriteBatch = spriteBatch;

            this._camera.Center = false;
            this._window.ClientSizeChanged += this.HandleClientSizeChanged;

            short[] indices = new short[_inputIndicatorVertices * 2];
            for (short i = 0; i < _inputIndicatorVertices; i++)
            {
                indices[(i * 2)] = i;
                indices[(i * 2) + 1] = (short)((i + 1) % _inputIndicatorVertices);
            }
            this._inputBatch.Initialize(_inputIndicatorVertices, PrimitiveType.LineList, indices);
            this.SetInput(CellTypeEnum.Sand, 10);
        }

        public unsafe void Initialize(int width, int height)
        {
            this._grid?.Dispose();
            this._renderTarget?.Dispose();

            this._grid = new Grid(width, height, this._cellTypes);

            this._gridBatch.Initialize(this._grid.Length);

            for (int i = 0; i < this._grid.Length; i++)
            {
                Cell cell = this._grid.Cells[i];

                this._gridBatch.Vertices[i].Position = new Vector3(cell.X, cell.Y, 0);
            }
            //for (int i = 0; i < 10000; i++)
            //{
            //    int index = Random.Shared.Next(0, _grid.Length / 2);
            //    _grid.Cells[index].CurrentType = CellTypeEnum.Sand;
            //}

            this._grid.Cells[0].Type = CellTypeEnum.Sand;
            this._grid.Cells[0].Awake = true;

            this._renderTarget = new RenderTarget2D(this._graphics, width, height);
        }

        [SequenceGroup<InitializeSequenceGroupEnum>(InitializeSequenceGroupEnum.Initialize)]
        public void Initialize()
        {
            this.Initialize(this._window.ClientBounds.Width, this._window.ClientBounds.Height);
        }

        [SequenceGroup<DrawSequenceGroupEnum>(DrawSequenceGroupEnum.Draw)]
        public unsafe void Draw(GameTime gameTime)
        {
            this._graphics.SetRenderTarget(this._renderTarget);
            this._camera.Update(gameTime);
            for (int i = 0; i < this._grid.Length; i++)
            {
                this._gridBatch.Vertices[i].Color = World.GetColor(this._grid.Cells[i].Type);
            }

            this._gridBatch.Draw(this._camera);

            this._inputBatch.Draw(this._camera.View, Matrix.CreateTranslation(this._mouse.Position.X, this._mouse.Position.Y, 0) * this._camera.Projection);

            this._graphics.SetRenderTarget(null);

            this._spriteBatch.Begin();
            this._spriteBatch.Draw(this._renderTarget, this._graphics.Viewport.Bounds, Color.White);
            this._spriteBatch.End();
        }

        [SequenceGroup<UpdateSequenceGroupEnum>(UpdateSequenceGroupEnum.Update)]
        public unsafe void Update(GameTime gameTime)
        {
            if (this._lastScrollValue != this._mouse.Scroll)
            {
                CellTypeEnum inputType = this._inputTypes[(this._inputTypeIndex++ % this._inputTypes.Length)];
                this.SetInput(inputType, 10);
            }

            this._lastScrollValue = this._mouse.Scroll;

            if ((this._elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds) < 20)
            {
                return;
            }
            this._elapsedTime -= 20;

            this._grid = this._grid.Update(gameTime, out this._awake);

            if (this._inputActive)
            {
                foreach (int index in this._grid.GetCellIndices(this._mouse.Position, this._inputRadius + 1))
                {
                    this._grid.Cells[index].Awake = true;
                }

                foreach (int index in this._grid.GetCellIndices(this._mouse.Position, this._inputRadius))
                {
                    this._grid.Cells[index].Type = this._inputCellType;
                    this._grid.Cells[index].InactivityCount = 0;
                    this._grid.Cells[index].Awake = true;
                }
            }
        }

        private void SetInput(CellTypeEnum cellType, int radius)
        {
            this._inputCellType = cellType;
            this._inputRadius = radius;

            for (int i = 0; i < _inputIndicatorVertices; i++)
            {
                float radians = MathF.PI * 2 / _inputIndicatorVertices * i;
                this._inputBatch.Vertices[i].Position = new Vector3(
                    x: MathF.Cos(radians) * radius,
                    y: MathF.Sin(radians) * radius,
                    z: 0);

                this._inputBatch.Vertices[i].Color = World.GetColor(this._inputCellType);
            }
        }

        private static Color GetColor(CellTypeEnum cellType)
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

        [SequenceGroup<SubscriberSequenceGroupEnum>(SubscriberSequenceGroupEnum.Process)]
        public void Process(in int messageId, PlaceSandInput message)
        {
            this._inputActive = message.Active;
        }

        [SequenceGroup<SubscriberSequenceGroupEnum>(SubscriberSequenceGroupEnum.Process)]
        public void Process(in int messageId, SelectCellTypeInput message)
        {
            this.SetInput(message.CellType, this._inputRadius);
        }

        private void HandleClientSizeChanged(object? sender, EventArgs e)
        {
            this.Initialize(this._window.ClientBounds.Width / 2, this._window.ClientBounds.Height / 2);
        }
    }
}