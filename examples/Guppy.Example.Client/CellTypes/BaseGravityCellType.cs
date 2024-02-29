using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.CellTypes
{
    internal abstract class BaseGravityCellType : BaseCellType
    {
        // public void Update(ref Cell input, Grid output)
        // {
        //     if (this.Step(ref input, output) == false)
        //     {
        //         ref Cell outputCell = ref output.Cells[input.Index];
        // 
        //         outputCell.Update(input.Type);
        //     }
        // }
        // 
        // protected virtual bool Step(ref Cell input, Grid output)
        // {
        //     // Try to move 1 cell down
        //     ref Cell belowInput = ref input.GetNeighbor(0, 1);
        //     ref Cell belowOutput = ref output.GetCell(belowInput.Index);
        //     if (belowInput.Type == CellTypeEnum.Air && belowOutput.Type == CellTypeEnum.Air)
        //     {
        //         output.Cells[input.Index].Update(CellTypeEnum.Air);
        //         belowOutput.Update(input.Type);
        // 
        //         return true;
        //     }
        // 
        //     // Try to move one cell down left/right
        //     int belowSide = Random.Shared.Next(0, 2) == 0 ? -1 : 1;
        //     ref Cell belowSideInput = ref input.GetNeighbor(belowSide, 1);
        //     ref Cell belowSideOutout = ref output.GetCell(belowSideInput.Index);
        //     if (belowSideInput.Type == CellTypeEnum.Air && belowSideOutout.Type == CellTypeEnum.Air)
        //     {
        //         output.Cells[input.Index].Update(CellTypeEnum.Air);
        //         belowSideOutout.Update(input.Type);
        // 
        //         return true;
        //     }
        // 
        //     return false;
        // }

        protected override CellStepResult Step(ref CellPair cell, Grid input, Grid output)
        {
            input.GetPair(cell.Input.X + 0, cell.Input.Y + 1, out CellPair below);
            if (below.Both(CellTypeEnum.Air))
            {
                return this.Update(ref below.Output, cell.Input.Type, 0, output);
            }

            int belowSide = Random.Shared.Next(0, 2) == 0 ? -1 : 1;
            if (this.TryFallSide(ref cell, input, output, belowSide) == CellStepResult.Active)
            {
                return CellStepResult.Active;
            }

            // if (this.TryFallSide(ref cell, input, output, belowSide * -1) == CellStepResult.Active)
            // {
            //     return CellStepResult.Active;
            // }

            return CellStepResult.Inactive;
        }

        private CellStepResult TryFallSide(ref CellPair cell, Grid input, Grid output, int side)
        {
            input.GetPair(cell.Input.X + side, cell.Input.Y + 1, out CellPair belowSide);
            if (belowSide.Both(CellTypeEnum.Air))
            {
                return this.Update(ref belowSide.Output, cell.Input.Type, 0, output);
            }

            return CellStepResult.Inactive;
        }
    }
}
