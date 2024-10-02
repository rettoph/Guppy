using Guppy.Core.Common.Attributes;
using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.CellTypes
{
    [AutoLoad]
    internal class AshCellType(int maxInactivityCount = 10) : BaseGravityCellType(CellTypeEnum.Null, maxInactivityCount)
    {
        public override CellTypeEnum Type => CellTypeEnum.Ash;
    }
}
