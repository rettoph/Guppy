using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.CellTypes
{
    internal abstract class BaseLiquidCellType : BaseGravityCellType
    {
        protected BaseLiquidCellType(CellTypeEnum displaces) : base(displaces)
        {
        }

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

            if (this.TryFlowSide(ref pair, input, output, side * -1) == CellStepResult.Active)
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

            for (int i = 1; i < 20; i++)
            {
                input.GetPair(pair.Input.X + (direction * i), pair.Input.Y, out CellPair side);

                if (side.Either(pair.Input.Type))
                {
                    continue;
                }

                if (side.Both(CellTypeEnum.Air))
                {
                    return this.Update(ref side.Output, pair.Input.Type, 0, output);
                }

                break;
            };

            return CellStepResult.Inactive;
        }
    }
}
