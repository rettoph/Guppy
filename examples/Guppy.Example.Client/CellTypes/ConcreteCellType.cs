using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.CellTypes
{
    internal class ConcreteCellType : BaseCellType
    {
        public override CellTypeEnum Type => CellTypeEnum.Concrete;

        protected override CellStepResultEnum Step(ref Cell cell, Grid input, Grid output)
        {
            cell.Type = CellTypeEnum.Concrete;

            return CellStepResultEnum.Inactive;
        }
    }
}