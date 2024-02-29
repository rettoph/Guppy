using Guppy.Example.Client.CellTypes;
using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Enum;

namespace Guppy.Example.Client.Services
{
    public class CellTypeService : ICellTypeService
    {
        private readonly Dictionary<CellTypeEnum, ICellType> _cellTypes;

        public CellTypeService(IEnumerable<ICellType> cellTypes)
        {
            _cellTypes = cellTypes.ToDictionary(x => x.Type, x => x);
        }

        public void Update(ref Cell input, ref Cell output)
        {
            _cellTypes[input.Type].Update(ref input, ref output);
        }
    }
}