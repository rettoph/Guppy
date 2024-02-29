using Guppy.Attributes;
using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Enum;

namespace Guppy.Example.Client.CellTypes
{
    [AutoLoad]
    internal sealed class SandCellType : ICellType
    {
        public CellTypeEnum Type => CellTypeEnum.Sand;

        public void Update(ref Cell input, ref Cell output)
        {
            ref Cell down = ref output[CellNeighborEnum.Down];

            if(down.Type == CellTypeEnum.Air)
            {
                down.Type = input.Type;
                output.Type = CellTypeEnum.Air;
            }
        }
    }
}
