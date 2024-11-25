namespace Guppy.Core.Common.Attributes
{
    /// <summary>
    /// Custom attribute to indicate custom actions should be taken
    /// </summary>
    [Obsolete]
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AutoLoadAttribute : Attribute
    {
        public AutoLoadAttribute()
        {
        }
    }
}
