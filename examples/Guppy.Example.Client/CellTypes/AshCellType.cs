using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.CellTypes
{
    internal class AshCellType(int maxInactivityCount = 10) : BaseGravityCellType(CellTypeEnum.Null, maxInactivityCount)
    {
        public override CellTypeEnum Type => CellTypeEnum.Ash;
    }
}