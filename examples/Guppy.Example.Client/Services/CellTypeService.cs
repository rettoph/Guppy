using Guppy.Example.Client.CellTypes;
using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.Services
{
    public class CellTypeService : ICellTypeService
    {
        private readonly Dictionary<CellTypeEnum, ICellType> _cellTypes;

        public CellTypeService(IEnumerable<ICellType> cellTypes)
        {
            _cellTypes = cellTypes.ToDictionary(x => x.Type, x => x);
        }

        public void Update(ref Cell cell, Grid old, Grid output)
        {
            _cellTypes[cell.Old.Type].Update(ref cell, old, output);
        }
    }
}