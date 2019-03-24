using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Guppy.UI.Attributes;
using System.Reflection;

namespace Guppy.UI.Configurations
{
    public class StateElementConfiguration : BaseElementConfiguration
    {
        private ElementConfiguration _parent;

        public StateElementConfiguration()
        {
        }

        protected internal void SetParent(ElementConfiguration parent)
        {
            _parent = parent;
        }

        public override void Initialize(IServiceProvider provider)
        {
            // Get a list of all properties...
            var properties = this.GetType().GetProperties().Where(prop => prop.IsDefined(typeof(ElementPropertyAttribute), false));

            // Update the property value if it is currently null
            foreach (PropertyInfo prop in properties)
                if (prop.GetValue(this) == null)
                    prop.SetValue(this, prop.GetValue(_parent));

            base.Initialize(provider);
        }
    }
}
