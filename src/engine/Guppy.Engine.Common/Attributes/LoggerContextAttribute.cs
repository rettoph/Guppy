namespace Guppy.Engine.Common.Attributes
{
    public class LoggerContextAttribute : Attribute
    {
        public readonly string Name;

        public LoggerContextAttribute(string name)
        {
            this.Name = name;
        }
    }
}
