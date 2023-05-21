﻿using Guppy.Attributes;
using Guppy.Commands.Extensions;
using Guppy.Common;
using Guppy.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Guppy.Commands.Attributes
{
    public class CommandAttribute : GuppyConfigurationAttribute
    {
        public readonly string? Name;
        public readonly string? Description;
        public readonly Type? Parent;

        public CommandAttribute(Type? parent = null, string? name = null, string? description = null)
        {
            this.Parent = parent;
            this.Name = name;
            this.Description = description;
        }

        protected override void Configure(GuppyConfiguration configuration, Type classType)
        {
            Command command = new Command(
                type: classType,
                parent: this.Parent,
                name: this.Name ?? classType.Name.LowerCaseFirstLetter(),
                description: this.Description,
                options: FactoryAttribute<Option>.GetAll(classType));

            configuration.Services.AddSingleton<Command>(command);
        }
    }

    public class CommandAttribute<TParent> : CommandAttribute
    {
        public CommandAttribute(string? name = null, string? description = null) : base(typeof(TParent), name, description)
        {
        }
    }
}
