using System;
using System.Collections.Generic;
using System.Text;
using Guppy.UI.Styles;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using Guppy.UI.Enums;
using Guppy.UI.Utilities;
using Guppy.UI.Entities;

namespace Guppy.UI.Elements
{
    /// <summary>
    /// Forms are special containers designed
    /// to hold inputs. When an active input gets
    /// tabbed, the form will automatically mark
    /// the next input as active.
    /// </summary>
    public class Form : Container
    {
        public Form(UnitRectangle outerBounds, Element parent, Stage stage, Style style = null) : base(outerBounds, parent, stage, style)
        {
            this.Stage.TextInput += this.HandleTextInput;
        }

        private List<TextInput> getInputs()
        {
            List<Element> elements = new List<Element>();
            this.GetChildren(elements);

            // Return a list of all text input objects
            return elements.Where(e => e is TextInput).Select(e => e as TextInput).ToList();
        }

        private void HandleTextInput(object sender, TextInputEventArgs e)
        {
            if(e.Key == Keys.Tab)
            {
                var inputs = this.getInputs();
                TextInput oldActive;

                if((oldActive = inputs.FirstOrDefault(i => i.State == ElementState.Active)) != default(TextInput))
                { // If there is currently an active input...
                    oldActive.setState(ElementState.Normal);
                    if (inputs.IndexOf(oldActive) + 1 < inputs.Count())
                        inputs[inputs.IndexOf(oldActive) + 1]?.setState(ElementState.Active, ElementState.Normal);
                    else
                        inputs[0]?.setState(ElementState.Active, ElementState.Normal);
                }

                inputs.Clear();
                inputs = null;
            }
        }

        public override void Dispose()
        {
            this.Stage.TextInput -= this.HandleTextInput;

            base.Dispose();
        }
    }
}
