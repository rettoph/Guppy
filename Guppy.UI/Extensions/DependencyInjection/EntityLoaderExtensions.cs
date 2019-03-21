using Guppy.Loaders;
using Guppy.UI.Configurations;
using Guppy.UI.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Extensions.DependencyInjection
{
    public static class EntityLoaderExtensions
    {
        public static void AddButton<TButton>(this EntityLoader loader, String handle, String nameHandle, String descriptionHandle, ButtonConfiguration configuration, UInt16 priority = 0)
            where TButton : Button
        {
            loader.Register<TButton>(handle, nameHandle, descriptionHandle, configuration, priority);
        }
    }
}
