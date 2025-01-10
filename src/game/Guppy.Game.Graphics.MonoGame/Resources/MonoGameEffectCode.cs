using Guppy.Game.Graphics.Common.Resources;

namespace Guppy.Game.Graphics.MonoGame.Resources
{
    public class MonoGameEffectCode(byte[] data) : IEffectCode
    {
        public byte[] Data { get; } = data;
    }
}