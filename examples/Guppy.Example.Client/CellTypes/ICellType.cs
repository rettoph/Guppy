using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.CellTypes
{
    public interface ICellType
    {
        CellTypeEnum Type { get; }

        void Update(ref Cell cell, Grid old, Grid output);
    }
}