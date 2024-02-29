using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.CellTypes
{
    internal abstract class BaseLiquidCellType : BaseGravityCellType
    {
        protected override CellStepResult Step(ref CellPair pair, Grid input, Grid output)
        {
            if (base.Step(ref pair, input, output) == CellStepResult.Active)
            {
                return CellStepResult.Active;
            }


            // Try to move X cells left or right
            int side = Random.Shared.Next(0, 2) == 0 ? 1 : -1;
            if (this.TryFlowSide(ref pair, input, output, side) == CellStepResult.Active)
            {
                return CellStepResult.Active;
            }

            return CellStepResult.Inactive;
        }

        private CellStepResult TryFlowSide(ref CellPair pair, Grid input, Grid output, int direction)
        {
            if (pair.Input.InactivityCount == 0)
            {
                return CellStepResult.Inactive;
            }

            int? openIndex = null;
            int yRadius = 0;

            for (int i = 1; i < 20; i++)
            {
                yRadius++;
                input.GetPair(pair.Input.X + (direction * i), pair.Input.Y, out CellPair side);

                if (
                       (side.Input.Type != CellTypeEnum.Air && side.Input.Type != pair.Input.Type)
                    || (side.Output.Type != CellTypeEnum.Air && side.Output.Type != pair.Input.Type)
                )
                {
                    break;
                }

                if (side.Both(CellTypeEnum.Air))
                {
                    openIndex = side.Output.Index;
                }

            }

            if (openIndex.HasValue == false)
            {
                return CellStepResult.Inactive;
            }

            return this.Update(ref output.Cells[openIndex.Value], pair.Input.Type, 0, output);
        }
    }
}
