using Guppy.Attributes;
using Guppy.GUI;
using Guppy.GUI.Styling;
using Guppy.Providers;
using Guppy.Resources.Providers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Components
{
    [AutoLoad]
    [GuppyFilter<GameLoop>]
    internal sealed class DrawDebugComponent : IGuppyComponent, IGuiComponent, IDisposable
    {
        private readonly IStyler _debugWindowStyler;
        private readonly IGui _gui;
        private readonly IGuppyProvider _guppies;
        private Dictionary<IGuppy, IDebugComponent[]> _components;

        public DrawDebugComponent(IGui gui, IGuppyProvider guppies)
        {
            _components = new Dictionary<IGuppy, IDebugComponent[]>();
            _guppies = guppies;
            _gui = gui;
            _debugWindowStyler = gui.GetStyler(Resources.Styles.DebugWindow);
        }

        public void Initialize(IGuppy guppy)
        {
            _guppies.OnGuppyCreated += this.HandleGuppyCreated;
            _guppies.OnGuppyDestroyed += this.HandleGuppyDestroyed;
        }

        public void Dispose()
        {
            _guppies.OnGuppyCreated -= this.HandleGuppyCreated;
        }

        public void DrawGui(GameTime gameTime)
        {
            _gui.SetNextWindowPos(Vector2.Zero);
            _gui.SetNextWindowSize(_gui.GetMainViewport().Size);

            using (_debugWindowStyler.Apply())
            {                
                if (_gui.Begin($"#{nameof(DrawDebugComponent)}", GuiWindowFlags.NoResize | GuiWindowFlags.NoMove | GuiWindowFlags.NoTitleBar))
                {
                    foreach((IGuppy guppy, IDebugComponent[] components) in _components)
                    {
                        if(components.Length > 0)
                        {
                            if(_gui.CollapsingHeader($"{guppy.GetType().Name} - {guppy.Id}", GuiTreeNodeFlags.DefaultOpen))
                            {
                                _gui.Dummy(Vector2.UnitY);

                                _gui.PushID($"#{guppy.Id}");
                                foreach (IDebugComponent component in components)
                                {
                                    component.RenderDebugInfo(_gui, gameTime);
                                }
                                _gui.PopID();
                            }

                            _gui.Dummy(Vector2.UnitY);
                        }
                    }
                }

                _gui.End();
            }
        }

        private void HandleGuppyCreated(IGuppyProvider sender, IGuppy args)
        {
            _components.Add(args, args.Components.OfType<IDebugComponent>().ToArray());
        }

        private void HandleGuppyDestroyed(IGuppyProvider sender, IGuppy args)
        {
            _components.Remove(args);
        }
    }
}
