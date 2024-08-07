﻿using Guppy.Core.Common.Attributes;
using Guppy.Core.Common.Enums;
using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.CellTypes
{
    [Service<ICellType>(ServiceLifetime.Scoped, ServiceRegistrationFlags.RequireAutoLoadAttribute)]
    public interface ICellType
    {
        CellTypeEnum Type { get; }

        void Update(ref Cell cell, Grid old, Grid output);
    }
}
