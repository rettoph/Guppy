using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.CellTypes
{
    internal class FireCellType : BaseCellType
    {
        public override CellTypeEnum Type => CellTypeEnum.Fire;

        public FireCellType() : base(50)
        {

        }

        protected override unsafe CellStepResultEnum Step(ref Cell cell, Grid input, Grid output)
        {
            CellStepResultEnum result = CellStepResultEnum.Inactive;

            for (int i = 0; i < cell.Neighbors.Length; i++)
            {
                ref Cell neighbor = ref output.Cells[cell.Neighbors[i]];

                if (neighbor.Latest.Type == CellTypeEnum.Plant)
                {
                    if (Random.Shared.Next(0, 5) == 0)
                    {
                        this.Update(ref neighbor, this.Type, 0, output);
                        result |= CellStepResultEnum.Active;
                    }
                }
            }

            if (result == CellStepResultEnum.Active)
            {
                this.Update(ref cell, Random.Shared.Next(0, 103) == 0 ? CellTypeEnum.Smolder : CellTypeEnum.Air, 0, output);
                return CellStepResultEnum.Active;
            }

            if (Random.Shared.Next(0, this.MaxInactivityCount - cell.Latest.InactivityCount) == 0)
            {
                this.Update(ref cell, CellTypeEnum.Air, 0, output);

                return CellStepResultEnum.Active;
            }

            if (Random.Shared.Next(0, 10) == 0)
            {
                int deltaX = Random.Shared.Next(-2, 2);
                int deltaY = Random.Shared.Next(-5, 0);

                ref Cell target = ref output.GetCell(cell.X + deltaX, cell.Y + deltaY);

                if (target.Latest.Type == CellTypeEnum.Air)
                {
                    this.Update(ref cell, CellTypeEnum.Air, 0, output);
                    this.Update(ref target, this.Type, 0, output);

                    return CellStepResultEnum.Active;
                }
            }

            return CellStepResultEnum.Inactive;
        }
    }
}