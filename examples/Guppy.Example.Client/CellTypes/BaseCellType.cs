using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.CellTypes
{
    internal abstract class BaseCellType(int maxInactivityCount = 10) : ICellType
    {
        public abstract CellTypeEnum Type { get; }
        public int MaxInactivityCount { get; set; } = maxInactivityCount;

        public void Update(ref Cell cell, Grid old, Grid output)
        {
            if (cell.Updated)
            {
                return;
            }

            if (this.Step(ref cell, old, output) == CellStepResultEnum.Inactive)
            {
                cell.Type = cell.Old.Type;
                cell.InactivityCount = (byte)(cell.Old.InactivityCount + 1);
                cell.Awake = true;
                cell.Updated = true;
            }
            else
            {
                this.WakeupNearbyCells(ref cell, output);
            }

            if (cell.InactivityCount > this.MaxInactivityCount && cell.Awake == true)
            {
                cell.Awake = false;
            }
        }

        protected virtual CellStepResultEnum Update(ref Cell cell, CellTypeEnum type, byte inactivityCount, Grid grid)
        {
            if (cell.Type == CellTypeEnum.Null || type == CellTypeEnum.Null)
            {
                return CellStepResultEnum.Inactive;
            }

            cell.Type = type;
            cell.InactivityCount = inactivityCount;
            cell.Awake = true;
            cell.Updated = true;

            if (inactivityCount == 0)
            {
                this.WakeupNearbyCells(ref cell, grid);
            }

            return CellStepResultEnum.Active;
        }

        protected unsafe virtual void WakeupNearbyCells(ref Cell cell, Grid grid)
        {
            for (int i = 0; i < cell.Neighbors.Length; i++)
            {
                int neighborIndex = cell.Neighbors[i];
                Wakeup(ref grid.Cells[neighborIndex]);
            }
        }

        private static void Wakeup(ref Cell cell)
        {
            cell.Awake = true;
            cell.InactivityCount = 0;
        }

        protected abstract CellStepResultEnum Step(ref Cell cell, Grid input, Grid output);
    }
}