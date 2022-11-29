using Guppy.Attributes;
using Guppy.Common;
using Guppy.Common.Collections;
using Guppy.MonoGame.Collections;
using Guppy.MonoGame.Messages.Inputs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Services
{
    internal class DefaultDebuggerService : BaseWindowService, IDebuggerService
    {
        private DrawableCollection _drawables;
        private UpdateableCollection _updateables;

        protected readonly CollectionManager<IDebugger> debuggers;

        public override ToggleWindowInput.Windows Window => ToggleWindowInput.Windows.Debugger;

        public DefaultDebuggerService(IFiltered<IDebugger> debuggers) : this(debuggers, Array.Empty<IManagedCollection>())
        {
        }
        protected DefaultDebuggerService(IFiltered<IDebugger> debuggers, params IManagedCollection[] collections) : base(false)
        {
            this.debuggers = new CollectionManager<IDebugger>(debuggers.Items, collections.Concat(new IManagedCollection[]
            {
                new DrawableCollection(),
                new UpdateableCollection()
            }).ToArray());

            _drawables = this.debuggers.GetManagedCollection<DrawableCollection>();
            _updateables = this.debuggers.GetManagedCollection<UpdateableCollection>();
        }

        public override void Update(GameTime gameTime)
        {
            _updateables.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _drawables.Draw(gameTime);
        }
    }
}
