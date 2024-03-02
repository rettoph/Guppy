using Guppy.Attributes;
using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.CellTypes
{
    [AutoLoad]
    internal class PlantCelType : BaseCellType
    {
        public override CellTypeEnum Type => CellTypeEnum.Plant;

        protected unsafe override CellStepResult Step(ref Cell cell, Grid input, Grid output)
        {
            CellStepResult result = CellStepResult.Inactive;

            for (int i = 0; i < cell.Neighbors.Length; i++)
            {
                ref Cell neighbor = ref output.Cells[cell.Neighbors[i]];

                if (neighbor.Latest.Type == CellTypeEnum.Water)
                {
                    if (Random.Shared.Next(0, 10) == 0)
                    {
                        this.Update(ref neighbor, this.Type, 0, output);
                        result |= CellStepResult.Active;
                    }
                }
            }

            if (result == CellStepResult.Active)
            {
                this.Update(ref cell, this.Type, 0, output);
            }


            return result;
        }
    }
}
