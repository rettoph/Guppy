using Guppy.Interfaces;
using Guppy.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy
{
    public class Configurable : Orderable, IConfigurable
    {
        #region Private Fields 
        private LoadedString _name;
        private LoadedString _description;
        #endregion

        #region Public Attributes
        public String Handle { get; set; }
        public String Name { get => _name; set => _name.Set(value); }
        public String Description { get => _description; set => _description.Set(value); }
        #endregion

        #region Lifecycle Methods
        protected override void Create(IServiceProvider provider)
        {
            base.Create(provider);

            _name = provider.GetRequiredService<LoadedString>();
            _description = provider.GetRequiredService<LoadedString>();
        }
        #endregion
    }
}
