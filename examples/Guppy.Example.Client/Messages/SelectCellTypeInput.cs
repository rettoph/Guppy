using Guppy.Core.Messaging.Common;
using Guppy.Example.Client.Enums;
using Guppy.Game.Input.Common;

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
