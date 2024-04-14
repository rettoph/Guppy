using Guppy.Engine.Attributes;
using Guppy.Example.Client.Entities;
using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.CellTypes
{
    [AutoLoad]
    internal class SmolderCellType : BaseGravityCellType
    {
        public override CellTypeEnum Type => CellTypeEnum.Smolder;

        public SmolderCellType() : base(CellTypeEnum.Water, 100)
        {
        }

        protected unsafe override CellStepResult Step(ref Cell cell, Grid input, Grid output)
        {
            if (base.Step(ref cell, input, output) == CellStepResult.Active)
            {
                return CellStepResult.Active;
            }

            CellStepResult result = CellStepResult.Inactive;

            for (int i = 0; i < cell.Neighbors.Length; i++)
            {
                ref Cell neighbor = ref output.Cells[cell.Neighbors[i]];

                if (neighbor.Latest.Type == CellTypeEnum.Plant)
                {
                    if (Random.Shared.Next(0, 500) == 0)
                    {
                        this.Update(ref neighbor, CellTypeEnum.Fire, 0, output);
                        result |= CellStepResult.Active;
                    }
                }
            }

            if (result == CellStepResult.Active)
            {
                this.Update(ref cell, Random.Shared.Next(0, 103) == 0 ? CellTypeEnum.Smolder : CellTypeEnum.Air, 0, output);
                return CellStepResult.Active;
            }

            if (Random.Shared.Next(0, this.MaxInactivityCount - cell.Latest.InactivityCount) == 0)
            {
                this.Update(ref cell, CellTypeEnum.Ash, 0, output);

                return CellStepResult.Active;
            }

            return CellStepResult.Inactive;
        }
    }
}
