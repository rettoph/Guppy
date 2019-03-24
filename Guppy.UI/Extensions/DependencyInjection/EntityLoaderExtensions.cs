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
        public static void AddElement<TElement>(
            this EntityLoader loader, 
            String handle, 
            String nameHandle, 
            String descriptionHandle, 
            ElementConfiguration configuration, UInt16 priority = 0)
            where TElement : Element
        {
            loader.Register<TElement>(handle, nameHandle, descriptionHandle, configuration, priority);
        }
    }
}
