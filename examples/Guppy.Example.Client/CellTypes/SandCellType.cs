﻿using Guppy.Example.Client.Enums;

namespace Guppy.Example.Client.CellTypes
{
    internal sealed class SandCellType : BaseGravityCellType
    {
        public override CellTypeEnum Type => CellTypeEnum.Sand;

        public SandCellType() : base(CellTypeEnum.Water)
        {
        }
    }
}