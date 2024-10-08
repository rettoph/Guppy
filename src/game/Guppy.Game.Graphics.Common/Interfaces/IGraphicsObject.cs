using Guppy.Game.Graphics.Common.Enums;

namespace Guppy.Game.Graphics.Common.Interfaces
{
    public interface IGraphicsObject
    {
        GraphicsObjectStatusEnum Status { get; }
    }

    public interface IGraphicsObject<T> : IGraphicsObject
    {
        T Value { get; }
    }
}
