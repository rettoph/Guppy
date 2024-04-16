using Guppy.Example.Client.Enums;
using Guppy.Game.Input.Common;
using Guppy.Messaging;

namespace Guppy.Example.Client.Messages
{
    public class SelectCellTypeInput : Message<SelectCellTypeInput>, IInput
    {
        public readonly CellTypeEnum Type;

        public SelectCellTypeInput(CellTypeEnum type)
        {
            Type = type;
        }
    }
}
