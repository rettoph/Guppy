namespace Guppy.Core.Common
{
    public abstract class Ref : IRef
    {
        public abstract Type Type { get; }

        public object? Value => this.GetValue();

        internal Ref()
        {

        }

        protected abstract object? GetValue();
    }
    public class Ref<T> : Ref, IRef<T>
    {
        public new T Value;

        public Ref(T value)
        {
            Value = value;
        }

        public override Type Type => typeof(T);

        T IRef<T>.Value
        {
            get => this.Value;
            set => this.Value = value;
        }

        protected override object? GetValue()
        {
            return this.Value;
        }

        public static implicit operator T(Ref<T> @ref)
        {
            return @ref.Value;
        }
    }
}
