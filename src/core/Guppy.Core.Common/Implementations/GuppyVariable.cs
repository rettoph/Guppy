using System.ComponentModel;

namespace Guppy.Core.Common.Implementations
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class GuppyVariable<TKey, TValue>(TValue value) : IGuppyVariable<TKey, TValue>
        where TKey : GuppyVariable<TKey, TValue>
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
            ThrowIf.Type.IsNotAssignableFrom<GuppyVariable<TKey, TValue>>(typeof(TKey));
            ThrowIf.Type.IsAbstract<TKey>();

            TKey instance = (TKey)(Activator.CreateInstance(typeof(TKey), value) ?? throw new NotImplementedException());
            return instance;
        }
    }
}
