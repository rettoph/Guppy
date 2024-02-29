using Guppy.Attributes;
using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.CellTypes
{
    [AutoLoad]
    internal class ConcreteCellType : BaseCellType
    {
        public override CellTypeEnum Type => CellTypeEnum.Concrete;

        protected override CellStepResult Step(ref CellPair cell, Grid input, Grid output)
        {
            cell.Output.Type = CellTypeEnum.Concrete;

            return CellStepResult.Inactive;
        }
    }
}
