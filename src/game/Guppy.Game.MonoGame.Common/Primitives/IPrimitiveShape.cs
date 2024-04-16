using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Game.MonoGame.Common.Primitives
{
    public interface IPrimitiveShape<TVertexType>
        where TVertexType : struct, IVertexType
    {
        public int Length { get; }


        void Transform(int index, in Color color, ref Matrix transformation, out TVertexType output);
    }
}
