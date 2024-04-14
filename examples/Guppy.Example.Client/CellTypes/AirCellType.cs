using Guppy.Engine.Attributes;
using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.CellTypes
{
    [AutoLoad]
    internal class AirCellType : BaseCellType
    {
        public override CellTypeEnum Type => CellTypeEnum.Air;

        protected override CellStepResult Step(ref Cell cell, Grid input, Grid output)
        {
            return cell.Type == CellTypeEnum.Air ? CellStepResult.Inactive : CellStepResult.Active;
        }
    }
}
