namespace Guppy.Game.MonoGame.Common.Graphics.Effects
{
    public class EffectCode(byte[] bytes)
    {
        private readonly byte[] _bytes = bytes;

        public static implicit operator byte[](EffectCode data)
        {
            return data._bytes;
        }
    }
}