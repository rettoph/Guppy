using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.CellTypes
{
    internal abstract class BaseGravityCellType : BaseCellType
    {
        public CellTypeEnum Displaces { get; set; }

        protected BaseGravityCellType(CellTypeEnum displaces)
        {
            this.Displaces = displaces | CellTypeEnum.Air;
        }

        protected override CellStepResult Step(ref CellPair cell, Grid input, Grid output)
        {
            input.GetPair(cell.Input.X + 0, cell.Input.Y + 1, out CellPair below);
            if (below.Input.Type == CellTypeEnum.Null)
            {
                return this.Update(ref cell.Output, CellTypeEnum.Air, 0, output);
            }

            if (below.BothIn(this.Displaces))
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

        protected virtual CellStepResult Displace(ref CellPair start, ref CellPair end, byte inactivityCount, Grid grid)
        {
            CellTypeEnum placeholder = start.OutputType;
            CellStepResult a = this.Update(ref start.Output, end.OutputType, inactivityCount, grid);
            CellStepResult b = this.Update(ref end.Output, placeholder, inactivityCount, grid);

            return a == CellStepResult.Active || b == CellStepResult.Active ? CellStepResult.Active : CellStepResult.Inactive;
        }

        private CellStepResult TryFallSide(ref CellPair cell, Grid input, Grid output, int side)
        {
            input.GetPair(cell.Input.X + side, cell.Input.Y + 1, out CellPair belowSide);
            if (belowSide.BothIn(this.Displaces))
            {
                return this.Displace(ref cell, ref belowSide, 0, output);
            }

            return CellStepResult.Inactive;
        }
    }
}
