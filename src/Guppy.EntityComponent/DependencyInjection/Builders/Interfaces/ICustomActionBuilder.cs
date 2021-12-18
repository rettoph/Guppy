using Minnow.General.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.EntityComponent.DependencyInjection.Builders.Interfaces
{
    public interface ICustomActionBuilder<TMethodArgs, TFilterArgs> : IOrderable
    {
        /// <summary>
        /// All <see cref="TypeFactoryBuilder"/>s who's <see cref="ITypeFactoryBuilder.Type"/>
        /// is <see cref="Type.IsAssignableFrom(Type)"/> will utilize the defined <see cref="IFactoryActionBuilder"/>.
        /// </summary>
        Type AssignableFactoryType { get; }

        /// <summary>
        /// Construct a new <see cref="FactoryAction{TArgs}"/> instance based on the current
        /// builder configuration.
        /// </summary>
        /// <returns></returns>
        CustomAction<TMethodArgs, TFilterArgs> Build();
    }
}
