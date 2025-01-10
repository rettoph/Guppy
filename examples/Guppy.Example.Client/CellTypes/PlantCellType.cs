using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.CellTypes
{
    internal class PlantCellType : BaseCellType
    {
        public override CellTypeEnum Type => CellTypeEnum.Plant;

        protected override unsafe CellStepResultEnum Step(ref Cell cell, Grid input, Grid output)
        {
            CellStepResultEnum result = CellStepResultEnum.Inactive;

            for (int i = 0; i < cell.Neighbors.Length; i++)
            {
                ref Cell neighbor = ref output.Cells[cell.Neighbors[i]];

                if (neighbor.Latest.Type == CellTypeEnum.Water)
                {
                    if (Random.Shared.Next(0, 10) == 0)
                    {
                        this.Update(ref neighbor, this.Type, 0, output);
                        result |= CellStepResultEnum.Active;
                    }
                }
            }

            if (result == CellStepResultEnum.Active)
            {
                this.Update(ref cell, this.Type, 0, output);
            }


            return result;
        }
    }
}