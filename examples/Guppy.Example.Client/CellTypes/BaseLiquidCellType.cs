using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.CellTypes
{
    internal abstract class BaseLiquidCellType : BaseGravityCellType
    {
        protected BaseLiquidCellType(CellTypeEnum displaces) : base(displaces)
        {
        }

        protected override CellStepResult Step(ref Cell cell, Grid input, Grid output)
        {
            if (base.Step(ref cell, input, output) == CellStepResult.Active)
            {
                return CellStepResult.Active;
            }


            // Try to move X cells left or right
            int side = Random.Shared.Next(0, 2) == 0 ? -1 : 1;
            if (this.TryFlowSide(ref cell, input, output, side) == CellStepResult.Active)
            {
                return CellStepResult.Active;
            }

            //if (this.TryFlowSide(ref cell, input, output, side * -1) == CellStepResult.Active)
            //{
            //    return CellStepResult.Active;
            //}

            return CellStepResult.Inactive;
        }

        private CellStepResult TryFlowSide(ref Cell cell, Grid input, Grid output, int direction)
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

            return CellStepResult.Inactive;
        }
    }
}
