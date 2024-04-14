using Guppy.Engine.Attributes;
using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.CellTypes
{
    [AutoLoad]
    internal class WaterCellType : BaseLiquidCellType
    {
        public override CellTypeEnum Type => CellTypeEnum.Water;

        public WaterCellType() : base(CellTypeEnum.Null)
        {
        }
    }
}
