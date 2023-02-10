using Guppy.Attributes;
using Guppy.Common;
using Guppy.MonoGame.Constants;
using Guppy.MonoGame.Messages;
using Guppy.MonoGame.Providers;
using Guppy.MonoGame.UI.Constants;
using Guppy.MonoGame.UI.Providers;
using ImGuiNET;
using ImPlotNET;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Num = System.Numerics;

namespace Guppy.MonoGame.UI.GameComponents
{
    internal sealed class DebugMenuComponent : SimpleDrawableGameComponent,
        ISubscriber<Toggle<DebugMenuComponent>>
    {
        private FpsComponent _fps;
        private Menu _menu;
        private ImGuiBatch _imGuiBatch;
        private IntPtr _context;
        private ImFontPtr _font;
        private Num.Vector2 _buttonSize = new Num.Vector2(125, 35);
        private Num.Vector2 _emptyVector2 = new Num.Vector2(0, 0);
        private Num.Vector2 _menuPosition = new Num.Vector2(0, 0);
        private Num.Vector2 _menuSize = new Num.Vector2(0, 0);
        private Num.Vector2 _menuSpacing = new Num.Vector2(12, 12);
        private GameWindow _window;
        private IBus _bus;

        public DebugMenuComponent(
            FpsComponent fps,
            GameWindow window,
            IImGuiBatchProvider batchs,
            IMenuProvider menus,
            IBus bus)
        {
            _menu = menus.Get(MenuConstants.Debug);
            _imGuiBatch = batchs.Get(ImGuiBatchConstants.Debug);
            _context = ImPlot.CreateContext();
            _window = window;
            _fps = fps;
            _bus = bus;
            _font = default!;

            CleanMenuDimensions();

            _window.ClientSizeChanged += HandleClientSizeChanged;

            this.Visible = false;
            this.IsEnabled = false;
        }

        public override void Initialize()
        {
            base.Initialize();

            _font = _imGuiBatch.Fonts[ResourceConstants.DiagnosticsImGuiFont].Ptr;
        }

        public override void Dispose()
        {
            base.Dispose();

            _window.ClientSizeChanged -= HandleClientSizeChanged;
        }


        private void CleanMenuDimensions()
        {
            _menuSize = new Num.Vector2(_buttonSize.X + _menuSpacing.X * 2, _window.ClientBounds.Height);
            _menuPosition = new Num.Vector2(_window.ClientBounds.Width - _menuSize.X, 0);
        }

        public override void Draw(GameTime gameTime)
        {
            ImPlot.SetImGuiContext(_imGuiBatch.Context);
            ImPlot.SetCurrentContext(_context);

            ImGui.PushStyleColor(ImGuiCol.Text, Color.White.ToNumericsVector4());
            ImGui.PushFont(_font);

            ImGui.SetNextWindowPos(_menuPosition);
            ImGui.SetNextWindowSize(_menuSize);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, _menuSpacing);
            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, _menuSpacing);
            if (ImGui.Begin("guppy-debug-menu", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoBackground))
            {
                foreach (MenuItem item in _menu.Items)
                {
                    if (ImGui.Button(item.Label, _buttonSize))
                    {
                        _bus.Enqueue(item.OnClick);
                    }
                }
            }
            ImGui.End();
            ImGui.PopStyleVar(2);
            ImGui.PopFont();
            ImGui.PopStyleColor();
        }

        private void HandleClientSizeChanged(object? sender, EventArgs e)
        {
            CleanMenuDimensions();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public void Process(in Toggle<DebugMenuComponent> message)
        {
            this.Visible = !this.Visible;
        }
    }
}
