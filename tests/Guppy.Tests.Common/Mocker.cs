using System.Linq.Expressions;
using Moq;

namespace Guppy.Tests.Common
{
    public abstract class Mocker
    {
        protected abstract object GetInstance();

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

    public class Mocker<T> : Mocker
        where T : class
    {
        private Mock<T>? _mock;
        private T? _object;

        public Mock<T> Mock
        {
            get
            {
                return this._mock ??= this.BuildMock();
            }
            set
            {
                this._mock = value;
            }
        }
        public T Object
        {
            get
            {
                if (this._object is not null)
                {
                    return this._object;
                }

                return this.Mock.Object;
            }
            set => this._object = value;
        }

        protected virtual Mock<T> BuildMock()
        {
            return new Mock<T>();
        }

        protected override object GetInstance()
        {
            return this.Object;
        }

        public Mocker<T> SetupReturn<TResult>(Expression<Func<T, TResult>> expression, TResult result)
        {
            this.Mock.Setup(expression).Returns(result);

            return this;
        }

        public Mocker<T> SetupReturn<TResult>(Expression<Func<T, TResult>> expression, InvocationFunc result)
        {
            this.Mock.Setup(expression).Returns(result);

            return this;
        }

        public Mocker<T> SetupReturn<TResult>(Expression<Func<T, TResult>> expression, Func<TResult> result)
        {
            this.Mock.Setup(expression).Returns(result);

            return this;
        }

        public Mocker<T> SetupReturn<TResult, T1>(Expression<Func<T, TResult>> expression, Func<T1, TResult> result)
        {
            this.Mock.Setup(expression).Returns(result);

            return this;
        }

        public Mocker<T> SetupReturn<TResult, T1, T2>(Expression<Func<T, TResult>> expression, Func<T1, T2, TResult> result)
        {
            this.Mock.Setup(expression).Returns(result);

            return this;
        }

        public Mocker<T> SetupReturn<TResult, T1, T2, T3>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, TResult> result)
        {
            this.Mock.Setup(expression).Returns(result);

            return this;
        }

        public Mocker<T> SetupReturn<TResult, T1, T2, T3, T4>(Expression<Func<T, TResult>> expression, Func<T1, T2, T3, T4, TResult> result)
        {
            this.Mock.Setup(expression).Returns(result);

            return this;
        }

        public Mocker<T> SetupCallback(Expression<Action<T>> expression, Action callback)
        {
            this.Mock.Setup(expression).Callback(callback);

            return this;
        }

        public Mocker<T> SetupCallback(Expression<Action<T>> expression, Delegate callback)
        {
            this.Mock.Setup(expression).Callback(callback);

            return this;
        }

        public Mocker<T> SetupCallback<T1>(Expression<Action<T>> expression, Action<T1> callback)
        {
            this.Mock.Setup(expression).Callback(callback);

            return this;
        }

        public Mocker<T> SetupCallback<T1, T2>(Expression<Action<T>> expression, Action<T1, T2> callback)
        {
            this.Mock.Setup(expression).Callback(callback);

            return this;
        }

        public Mocker<T> SetupCallback<T1, T2, T3>(Expression<Action<T>> expression, Action<T1, T2, T3> callback)
        {
            this.Mock.Setup(expression).Callback(callback);

            return this;
        }

        public Mocker<T> SetupCallback<T1, T2, T3, T4>(Expression<Action<T>> expression, Action<T1, T2, T3, T4> callback)
        {
            this.Mock.Setup(expression).Callback(callback);

            return this;
        }

        public Mocker<T> Verify(Expression<Action<T>> expression)
        {
            this.Mock.Verify(expression);

            return this;
        }

        public Mocker<T> Verify(Expression<Action<T>> expression, string failMessage)
        {
            this.Mock.Verify(expression, failMessage);

            return this;
        }


        public Mocker<T> Verify(Expression<Action<T>> expression, Times times, string failMessage)
        {
            this.Mock.Verify(expression, times, failMessage);

            return this;
        }

        public Mocker<T> Verify(Expression<Action<T>> expression, Func<Times> times)
        {
            this.Mock.Verify(expression, times);

            return this;
        }

        public Mocker<T> Verify<TResult>(Expression<Func<T, TResult>> expression)
        {
            this.Mock.Verify(expression);

            return this;
        }

        public Mocker<T> Verify<TResult>(Expression<Func<T, TResult>> expression, string failMessage)
        {
            this.Mock.Verify(expression, failMessage);

            return this;
        }

        public Mocker<T> Verify<TResult>(Expression<Func<T, TResult>> expression, Times times, string failMessage)
        {
            this.Mock.Verify(expression, times, failMessage);

            return this;
        }
    }
}