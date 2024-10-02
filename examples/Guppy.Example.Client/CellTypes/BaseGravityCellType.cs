using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.CellTypes
{
    internal abstract class BaseGravityCellType(CellTypeEnum displaces, int maxInactivityCount = 10) : BaseCellType(maxInactivityCount)
    {
        public CellTypeEnum Displaces { get; set; } = displaces | CellTypeEnum.Air;

        protected override CellStepResult Step(ref Cell cell, Grid input, Grid output)
        {
            ref Cell below = ref output.GetCell(cell.X + 0, cell.Y + 1);

            // input.GetPair(cell.Input.X + 0, cell.Input.Y + 1, out CellPair below);
            if (below.Latest.Type == CellTypeEnum.Null)
            {
                return this.Update(ref cell, CellTypeEnum.Air, 0, output);
            }

            if (this.CanDisplace(ref below))
            {
                return this.Displace(ref cell, ref below, 0, output);
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

        protected virtual CellStepResult Displace(ref Cell first, ref Cell second, byte inactivityCount, Grid grid)
        {
            CellTypeEnum placeholder = first.Latest.Type;

            CellStepResult a = this.Update(ref first, second.Latest.Type, inactivityCount, grid);
            first.Displaced = true;

            CellStepResult b = this.Update(ref second, placeholder, inactivityCount, grid);
            second.Displaced = true;

            return a == CellStepResult.Active || b == CellStepResult.Active ? CellStepResult.Active : CellStepResult.Inactive;
        }

        protected bool CanDisplace(ref Cell cell)
        {
            return (cell.Displaced == false || cell.Type == CellTypeEnum.Air) && this.Displaces.HasFlag(cell.Latest.Type);
        }

        private CellStepResult TryFallSide(ref Cell cell, Grid input, Grid output, int side)
        {
            ref Cell belowSide = ref output.GetCell(cell.X + side, cell.Y + 1);
            if (this.CanDisplace(ref belowSide))
            {
                return this.Displace(ref cell, ref belowSide, 0, output);
            }

            return CellStepResult.Inactive;
        }
    }
}
