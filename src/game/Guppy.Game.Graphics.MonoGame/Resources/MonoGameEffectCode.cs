using Guppy.Game.Graphics.Common.Assets;

namespace Guppy.Game.Graphics.MonoGame.Assets
{
    public class MonoGameEffectCode(byte[] data) : IEffectCode
    {
        public byte[] Data { get; } = data;
    }
}