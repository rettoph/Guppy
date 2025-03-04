using Guppy.Example.Client.CellTypes;
using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.Services
{
    public class CellTypeService(IEnumerable<ICellType> cellTypes) : ICellTypeService
    {
        private readonly Dictionary<CellTypeEnum, ICellType> _cellTypes = cellTypes.ToDictionary(x => x.Type, x => x);

        public void Update(ref Cell cell, Grid old, Grid output)
        {
            this._cellTypes[cell.Old.Type].Update(ref cell, old, output);
        }
    }
}