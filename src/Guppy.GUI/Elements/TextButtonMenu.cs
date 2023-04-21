using Guppy.MonoGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.GUI.Elements
{
    public class TextButtonMenu : Container<TextButton>
    {
        private readonly Menu _menu;

        public TextButtonMenu(Menu menu, params string[] names) : this(menu, (IEnumerable<string>)names)
        {
        }

        public TextButtonMenu(Menu menu, IEnumerable<string> names) : base(names)
        {
            _menu = menu;

            _menu.OnItemAdded += this.HandleMenuItemAdded;
        }

        protected internal override void Initialize(Stage stage, Element? parent)
        {
            base.Initialize(stage, parent);

            foreach(MenuItem item in _menu.Items)
            {
                this.Add(new TextButton()
                {
                    Text = item.Label,
                    ReleasedMessage = item.OnClick
                });
            }
        }

        private void HandleMenuItemAdded(Menu sender, MenuItem item)
        {
            this.Add(new TextButton()
            {
                Text = item.Label,
                ReleasedMessage = item.OnClick
            });
        }
    }
}
