namespace Guppy.Game.MonoGame.Graphics.Effects
{
    public class EffectCode
    {
        private byte[] _bytes;

        public EffectCode(byte[] bytes)
        {
            _bytes = bytes;
        }

        public static implicit operator byte[](EffectCode data)
        {
            return data._bytes;
        }
    }
}