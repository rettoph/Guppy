using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.CellTypes
{
    internal class WaterCellType : BaseLiquidCellType
    {
        public override CellTypeEnum Type => CellTypeEnum.Water;

        public WaterCellType() : base(CellTypeEnum.Null)
        {
        }
    }
}