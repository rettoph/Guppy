using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.CellTypes
{
    internal abstract class BaseLiquidCellType(CellTypeEnum displaces) : BaseGravityCellType(displaces)
    {
        protected override CellStepResultEnum Step(ref Cell cell, Grid input, Grid output)
        {
            if (base.Step(ref cell, input, output) == CellStepResultEnum.Active)
            {
                return CellStepResultEnum.Active;
            }


            // Try to move X cells left or right
            int side = Random.Shared.Next(0, 2) == 0 ? -1 : 1;
            if (this.TryFlowSide(ref cell, input, output, side) == CellStepResultEnum.Active)
            {
                return CellStepResultEnum.Active;
            }

            //if (this.TryFlowSide(ref cell, input, output, side * -1) == CellStepResult.Active)
            //{
            //    return CellStepResult.Active;
            //}

            return CellStepResultEnum.Inactive;
        }

#pragma warning disable IDE0060 // Remove unused parameter
        private CellStepResultEnum TryFlowSide(ref Cell cell, Grid input, Grid output, int direction)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            for (int i = 1; i < 15; i++)
            {
                ref Cell side = ref output.GetCell(cell.X + (direction * i), cell.Y);

                if ((side.Updated == true && side.Type == this.Type) || (side.Updated == false && side.Old.Type == this.Type))
                {
                    continue;
                }

                if (this.CanDisplace(ref side))
                {
                    return this.Displace(ref cell, ref side, 0, output);
                }

                break;
            };

            return CellStepResultEnum.Inactive;
        }
    }
}