using Guppy.Core.Messaging.Common;
using Guppy.Example.Client.Enums;
using Guppy.Game.Input.Common;

namespace Guppy.Example.Client.Messages
{
    public class SelectCellTypeInput(CellTypeEnum type) : Message<SelectCellTypeInput>, IInput
    {
        public readonly CellTypeEnum CellType = type;
    }
}
