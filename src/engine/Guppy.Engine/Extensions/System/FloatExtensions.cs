namespace System
{
    public static class FloatExtensions
    {
        public static float RoundNearest(this float value, float denomination)
        {
            float result = denomination * MathF.Floor((value / denomination) + 0.5f);

            return result;
        }
    }
}