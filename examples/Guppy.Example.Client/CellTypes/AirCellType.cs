using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.CellTypes
{
    internal class AirCellType : BaseCellType
    {
        public override CellTypeEnum Type => CellTypeEnum.Air;

        protected override CellStepResultEnum Step(ref Cell cell, Grid input, Grid output)
        {
            return cell.Type == CellTypeEnum.Air ? CellStepResultEnum.Inactive : CellStepResultEnum.Active;
        }
    }
}