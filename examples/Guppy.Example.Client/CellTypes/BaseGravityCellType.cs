using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.CellTypes
{
    internal abstract class BaseGravityCellType(CellTypeEnum displaces, int maxInactivityCount = 10) : BaseCellType(maxInactivityCount)
    {
        public CellTypeEnum Displaces { get; set; } = displaces | CellTypeEnum.Air;

        protected override CellStepResultEnum Step(ref Cell cell, Grid input, Grid output)
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
            if (this.TryFallSide(ref cell, input, output, belowSide) == CellStepResultEnum.Active)
            {
                return CellStepResultEnum.Active;
            }

            // if (this.TryFallSide(ref cell, input, output, belowSide * -1) == CellStepResult.Active)
            // {
            //     return CellStepResult.Active;
            // }

            return CellStepResultEnum.Inactive;
        }

        protected virtual CellStepResultEnum Displace(ref Cell first, ref Cell second, byte inactivityCount, Grid grid)
        {
            CellTypeEnum placeholder = first.Latest.Type;

            CellStepResultEnum a = this.Update(ref first, second.Latest.Type, inactivityCount, grid);
            first.Displaced = true;

            CellStepResultEnum b = this.Update(ref second, placeholder, inactivityCount, grid);
            second.Displaced = true;

            return a == CellStepResultEnum.Active || b == CellStepResultEnum.Active ? CellStepResultEnum.Active : CellStepResultEnum.Inactive;
        }

        protected bool CanDisplace(ref Cell cell) => (cell.Displaced == false || cell.Type == CellTypeEnum.Air) && this.Displaces.HasFlag(cell.Latest.Type);

#pragma warning disable IDE0060 // Remove unused parameter
        private CellStepResultEnum TryFallSide(ref Cell cell, Grid input, Grid output, int side)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            ref Cell belowSide = ref output.GetCell(cell.X + side, cell.Y + 1);
            if (this.CanDisplace(ref belowSide))
            {
                return this.Displace(ref cell, ref belowSide, 0, output);
            }

            return CellStepResultEnum.Inactive;
        }
    }
}