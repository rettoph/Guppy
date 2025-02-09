using Guppy.Example.Client.Enums;
using Guppy.Game.Input.Common;

namespace Guppy.Example.Client.Messages
{
    public class SelectCellTypeInput(CellTypeEnum type) : InputMessage<SelectCellTypeInput>
    {
        public readonly CellTypeEnum CellType = type;
    }
}