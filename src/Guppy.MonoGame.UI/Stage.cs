using Guppy.MonoGame.UI.Elements;
using Guppy.MonoGame.UI.Providers;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Timers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.UI
{
    public class Stage : Container<IElement>, IContainer<IElement>, IPublicContainer<IElement>
    {
        private readonly IStyleSheetProvider _styleSheets;

        public IStyleSheet StyleSheet { get; private set; }

        public Stage(IStyleSheetProvider styleSheets)
        {
            _styleSheets = styleSheets;

            this.StyleSheet = null!;
        }

        public void Initialize(string styleSheetName)
        {
            var styleSheet = _styleSheets.Get(styleSheetName);

            this.Initialize(styleSheet);
        }

        public void Initialize(IStyleSheet styleSheet)
        {
            this.StyleSheet = styleSheet;

            base.Initialize(this);
        }

        public new void Add(IElement child)
        {
            base.Add(child);
        }

        public new void Remove(IElement child)
        {
            base.Remove(child);
        }

        public new void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public new void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Initialize(IContainer container, out IStyleProvider styles, out Selector? parent)
        {
            styles = this.StyleSheet.GetProvider(this.Selector);
            parent = null;
        }
    }
}
