using System.Linq.Expressions;
using Moq;

namespace Guppy.Tests.Common
{
    public abstract class Mocker
    {
        public abstract object GetInstance();

        public static object GetGenericInstance(Type type, params Type[] genericTypeArguments)
        {
            if (genericTypeArguments.Length > 0)
            {
                type = type.MakeGenericType(genericTypeArguments);
            }

            Type mockerType = typeof(Mocker<>).MakeGenericType(type);
            Mocker mocker = (Mocker)(Activator.CreateInstance(mockerType) ?? throw new NotImplementedException());

            return mocker.GetInstance();
        }
    }

    public class Mocker<T>(Mock<T> mock) : Mocker
        where T : class
    {
        public static readonly Mocker<T> SharedMocker = new();
        public static T SharedInstance => SharedMocker.GetInstance();

        private T? _instance;
        private readonly Mock<T> _mock = mock;

        public Mocker() : this(new Mock<T>())
        {

        }

        public Mocker(object[] args) : this(new Mock<T>(args) { CallBase = true })
        {

        }

        public static Mocker<T> Create()
        {
            Mocker<T> builder = new();

            return builder;
        }

        public Mocker<T> Setup<TResult>(Expression<Func<T, TResult>> expression, TResult result)
        {
            this._mock.Setup(expression).Returns(result);

            return this;
        }

        public Mocker<T> Setup<TResult>(Expression<Func<T, TResult>> expression, InvocationFunc result)
        {
            this._mock.Setup(expression).Returns(result);

            return this;
        }

        public Mocker<T> Setup<TResult>(Expression<Func<T, TResult>> expression, Func<TResult> result)
        {
            this._mock.Setup(expression).Returns(result);

            return this;
        }

        public Mocker<T> Setup<TResult, T1>(Expression<Func<T, TResult>> expression, Func<T1, TResult> result)
        {
            this._mock.Setup(expression).Returns(result);

            return this;
        }

        public Mocker<T> Setup<TResult, T1, T2>(Expression<Func<T, TResult>> expression, Func<T1, T2, TResult> result)
        {
            this._mock.Setup(expression).Returns(result);

            return this;
        }

        public Mocker<T> Setup<TResult, T1, T2, T3>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, TResult> result)
        {
            this._mock.Setup(expression).Returns(result);

            return this;
        }

        public Mocker<T> Setup<TResult, T1, T2, T3, T4>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, TResult> result)
        {
            this._mock.Setup(expression).Returns(result);

            return this;
        }

        public Mocker<T> Setup(Expression<Action<T>> expression, Action callback)
        {
            this._mock.Setup(expression).Callback(callback);

            return this;
        }

        public Mocker<T> Setup(Expression<Action<T>> expression, Delegate callback)
        {
            this._mock.Setup(expression).Callback(callback);

            return this;
        }

        public Mocker<T> Setup<T1>(Expression<Action<T>> expression, Action<T1> callback)
        {
            this._mock.Setup(expression).Callback(callback);

            return this;
        }

        public Mocker<T> Setup<T1, T2>(Expression<Action<T>> expression, Action<T1, T2> callback)
        {
            this._mock.Setup(expression).Callback(callback);

            return this;
        }

        public Mocker<T> Setup<T1, T2, T3>(Expression<Action<T>> expression, Action<T1, T2, T3> callback)
        {
            this._mock.Setup(expression).Callback(callback);

            return this;
        }

        public Mocker<T> Setup<T1, T2, T3, T4>(Expression<Action<T>> expression, Action<T1, T2, T3, T4> callback)
        {
            this._mock.Setup(expression).Callback(callback);

            return this;
        }

        public Mocker<T> Verify(Expression<Action<T>> expression)
        {
            this._mock.Verify(expression);

            return this;
        }

        public Mocker<T> Verify(Expression<Action<T>> expression, string failMessage)
        {
            this._mock.Verify(expression, failMessage);

            return this;
        }


        public Mocker<T> Verify(Expression<Action<T>> expression, Times times, string failMessage)
        {
            this._mock.Verify(expression, times, failMessage);

            return this;
        }

        public Mocker<T> Verify(Expression<Action<T>> expression, Func<Times> times)
        {
            this._mock.Verify(expression, times);

            return this;
        }

        public Mocker<T> Verify<TResult>(Expression<Func<T, TResult>> expression)
        {
            this._mock.Verify(expression);

            return this;
        }

        public Mocker<T> Verify<TResult>(Expression<Func<T, TResult>> expression, string failMessage)
        {
            this._mock.Verify(expression, failMessage);

            return this;
        }

        public Mocker<T> Verify<TResult>(Expression<Func<T, TResult>> expression, Times times, string failMessage)
        {
            this._mock.Verify(expression, times, failMessage);

            return this;
        }

        public void SetInstance(T instance)
        {
            this._instance = instance;
        }

        public Mock<T> AsMock()
        {
            return this._mock;
        }

        public override T GetInstance()
        {
            return this._instance ?? this._mock.Object;
        }

        public Lazy<T> GetLazy()
        {
            return new(this.GetInstance);
        }

        public Lazy<TOut> GetLazy<TOut>()
            where TOut : class
        {
            return new(() => this.GetInstance().As<TOut>());
        }

        public static implicit operator Mock<T>(Mocker<T> mocker)
        {
            return mocker.AsMock();
        }

        public static implicit operator T(Mocker<T> mocker)
        {
            return mocker.GetInstance();
        }
    }
}