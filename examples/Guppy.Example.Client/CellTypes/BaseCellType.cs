using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.CellTypes
{
    internal abstract class BaseCellType : ICellType
    {
        protected BaseCellType(int maxInactivityCount = 10)
        {
            this.MaxInactivityCount = maxInactivityCount;
        }

        public abstract CellTypeEnum Type { get; }
        public int MaxInactivityCount { get; set; }

        public void Update(ref CellPair cell, Grid input, Grid output)
        {
            if (cell.Output.Updated)
            {
                return;
            }

            if (this.Step(ref cell, input, output) == CellStepResult.Inactive)
            {
                cell.Output.Type = cell.Input.Type;
                cell.Output.InactivityCount = ++cell.Input.InactivityCount;
                cell.Output.Awake = true;
            }
            else
            {
                this.WakeupNearbyCells(ref cell.Output, output);
            }

            if (cell.Output.InactivityCount > 10 && cell.Output.Awake == true)
            {
                cell.Output.Awake = false;
            }
        }

        protected virtual CellStepResult Update(ref Cell cell, CellTypeEnum type, byte inactivityCount, Grid grid)
        {
            if (cell.Type == CellTypeEnum.Null || type == CellTypeEnum.Null)
            {
                return CellStepResult.Inactive;
            }

            cell.Type = type;
            cell.InactivityCount = inactivityCount;
            cell.Awake = true;
            cell.Updated = true;

            if (inactivityCount == 0)
            {
                this.WakeupNearbyCells(ref cell, grid);
            }

            return CellStepResult.Active;
        }

        protected virtual void WakeupNearbyCells(ref Cell cell, Grid grid)
        {
            this.Wakeup(cell.X - 1, cell.Y - 1, grid);
            this.Wakeup(cell.X + 0, cell.Y - 1, grid);
            this.Wakeup(cell.X + 1, cell.Y - 1, grid);

            this.Wakeup(cell.X - 1, cell.Y + 0, grid);
            this.Wakeup(cell.X + 1, cell.Y + 0, grid);

            this.Wakeup(cell.X - 1, cell.Y + 1, grid);
            this.Wakeup(cell.X + 0, cell.Y + 1, grid);
            this.Wakeup(cell.X + 1, cell.Y + 1, grid);
        }

        private void Wakeup(int x, int y, Grid grid)
        {
            ref Cell cell = ref grid.GetCell(x, y);
            if (cell.Type != CellTypeEnum.Null)
            {
                cell.Awake = true;
                cell.InactivityCount = 0;
            }

            //grid.GetPair(x, y, out CellPair pair);
            //
            //if (pair.Input.Type != CellTypeEnum.Null)
            //{
            //    pair.Input.Awake = true;
            //    pair.Input.InactivityCount = 0;
            //}
            //
            //if (pair.Output.Type != CellTypeEnum.Null)
            //{
            //    pair.Output.Awake = true;
            //    pair.Output.InactivityCount = 0;
            //}
        }

        protected abstract CellStepResult Step(ref CellPair cell, Grid input, Grid output);
    }
}
