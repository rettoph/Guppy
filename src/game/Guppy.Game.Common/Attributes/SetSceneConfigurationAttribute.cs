namespace Guppy.Game.Common.Attributes
{
    public abstract class SetSceneConfigurationAttribute : Attribute
    {
        public abstract void SetValue(ISceneConfiguration configuration);
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SetSceneConfigurationAttribute<T>(string key, T value) : SetSceneConfigurationAttribute
        where T : notnull
    {
        public readonly string Key = key;
        public readonly T Value = value;

        public override void SetValue(ISceneConfiguration configuration) => configuration.Set<T>(this.Key, this.Value);
    }
}