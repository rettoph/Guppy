namespace Guppy.Common
{
    public class Ref
    {
        internal Ref()
        {

        }
    }
    public class Ref<T> : Ref, IRef<T>
    {
        public T Value;

        public Ref(T value)
        {
            Value = value;
        }

        T IRef<T>.Value
        {
            get => this.Value;
            set => this.Value = value;
        }

        public static implicit operator T(Ref<T> @ref)
        {
            return @ref.Value;
        }
    }
}
