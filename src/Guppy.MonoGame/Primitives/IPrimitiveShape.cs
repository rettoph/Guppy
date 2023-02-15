using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Primitives
{
    public interface IPrimitiveShape<TVertexType>
        where TVertexType : struct, IVertexType
    {
        public int Length { get; }


        void Transform(int index, in Color color, ref Matrix transformation, out TVertexType output);
    }
}
