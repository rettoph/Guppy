using Guppy.Engine.Attributes;
using Guppy.Engine.Enums;
using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.CellTypes
{
    [Service<ICellType>(ServiceLifetime.Scoped, true)]
    public interface ICellType
    {
        CellTypeEnum Type { get; }

        void Update(ref Cell cell, Grid old, Grid output);
    }
}
