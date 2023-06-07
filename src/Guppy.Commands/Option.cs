﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Commands
{
    public sealed class Option
    {
        public readonly PropertyInfo PropertyInfo;
        public readonly string[] Names;
        public readonly string? Description;
        public readonly bool Required;

        public Option(PropertyInfo propertyInfo, string[] names, string? description, bool required)
        {
            this.PropertyInfo = propertyInfo;
            this.Names = names;
            this.Description = description;
            this.Required = required;
        }
    }
}