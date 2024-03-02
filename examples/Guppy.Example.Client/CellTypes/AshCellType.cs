using Guppy.Attributes;
using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.CellTypes
{
    [AutoLoad]
    internal class AshCellType : BaseGravityCellType
    {
        public override CellTypeEnum Type => CellTypeEnum.Ash;

        public AshCellType(int maxInactivityCount = 10) : base(CellTypeEnum.Null, maxInactivityCount)
        {
        }
    }
}
