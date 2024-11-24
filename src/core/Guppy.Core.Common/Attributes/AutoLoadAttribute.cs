namespace Guppy.Core.Common.Attributes
{
    /// <summary>
    /// Custom attribute to indicate custom actions should be taken
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AutoLoadAttribute : Attribute
    {
        public AutoLoadAttribute()
        {
        }
    }
}
