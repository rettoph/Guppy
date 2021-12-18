using Minnow.General.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.EntityComponent.DependencyInjection.Builders.Interfaces
{
    public interface ITypeFactoryBuilder : IPrioritizable
    {
        public Type Type { get; }

        TypeFactory Build(IEnumerable<CustomAction<TypeFactory, ITypeFactoryBuilder>> allBuilders);
    }
}
