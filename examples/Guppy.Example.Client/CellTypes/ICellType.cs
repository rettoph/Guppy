using Guppy.Attributes;
using Guppy.Enums;
using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Enum;

namespace Guppy.Example.Client.CellTypes
{
    [Service<ICellType>(ServiceLifetime.Scoped, true)]
    public interface ICellType
    {
        CellTypeEnum Type { get; }

        void Update(ref Cell cell, Grid grid);
    }
}
