namespace Guppy.Core.Common.Implementations
{
    public abstract class EnvironmentVariable<TKey, TValue>(TValue value) : IEnvironmentVariable<TKey, TValue>
        where TKey : EnvironmentVariable<TKey, TValue>
        where TValue : notnull
    {
        public readonly TValue Value = value;

        public virtual bool Matches(TKey value)
        {
            return this.Value.Equals(value);
        }

        public bool Matches(object value)
        {
            if (value is not TKey casted)
            {
                return false;
            }

            return this.Matches(casted);
        }

        public static TKey Create(TValue value)
        {
            ThrowIf.Type.IsNotAssignableFrom<EnvironmentVariable<TKey, TValue>>(typeof(TKey));
            ThrowIf.Type.IsAbstract<TKey>();

            TKey instance = (TKey)(Activator.CreateInstance(typeof(TKey), value) ?? throw new NotImplementedException());
            return instance;
        }
    }
}
