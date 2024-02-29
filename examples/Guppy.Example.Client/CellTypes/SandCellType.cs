using Guppy.Attributes;
using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Enum;

namespace Guppy.Example.Client.CellTypes
{
    [AutoLoad]
    internal sealed class SandCellType : ICellType
    {
        public CellTypeEnum Type => CellTypeEnum.Sand;

        public void Update(ref Cell cell, Grid grid)
        {
            throw new NotImplementedException();
        }
    }
}
