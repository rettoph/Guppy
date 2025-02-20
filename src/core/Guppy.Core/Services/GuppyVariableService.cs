using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Guppy.Core.Common;
using Guppy.Core.Common.Providers;
using Guppy.Core.Common.Services;

namespace Guppy.Core.Services
{
    public abstract class GuppyVariableService<TVariable>(IEnumerable<TVariable> variables) : IGuppyVariableService<TVariable>, IEnumerable<TVariable>
        where TVariable : IGuppyVariable
    {
        private readonly Dictionary<Type, TVariable> _variables = variables.GroupBy(x => x.GetType())
            .ToDictionary(x => x.Key, x => x.Last());

        public T Get<T>()
            where T : TVariable
        {
            return (T)this._variables[typeof(T)];
        }

        public object Get(Type variableType)
        {
            ThrowIf.Type.IsNotAssignableFrom<IGuppyVariable>(variableType);
            return this._variables[variableType];
        }

        public bool TryGet<T>([MaybeNullWhen(false)] out T? value)
            where T : TVariable
        {
            if (this._variables.TryGetValue(typeof(T), out TVariable? uncasted) == false)
            {
                value = default;
                return false;
            }

            if (uncasted is not T casted)
            {
                value = default;
                return false;
            }

            value = casted;
            return true;
        }

        public bool TryGet(Type variableType, [MaybeNullWhen(false)] out TVariable? value)
        {
            ThrowIf.Type.IsNotAssignableFrom<IGuppyVariable>(variableType);
            if (this._variables.TryGetValue(variableType, out TVariable? variable) == false)
            {
                value = default;
                return false;
            }

            value = variable;
            return true;
        }

        public bool Has<T>()
            where T : TVariable
        {
            return this._variables.ContainsKey(typeof(T));
        }

        public bool Has(Type variableType)
        {
            ThrowIf.Type.IsNotAssignableFrom<IGuppyVariable>(variableType);
            return this._variables.ContainsKey(variableType);
        }

        public bool Matches<T>(T value)
            where T : TVariable
        {
            foreach (T variable in this._variables.Values.OfType<T>())
            {
                if (variable.Matches(value))
                {
                    return true;
                }
            }

            return false;
        }

        public bool Matches(TVariable value)
        {
            foreach ((_, TVariable variable) in this._variables)
            {
                if (variable.Matches(value))
                {
                    return true;
                }
            }

            return false;
        }

        public Dictionary<Type, TVariable> ToDictionary()
        {
            return this._variables.ToDictionary();
        }

        public IEnumerator<TVariable> GetEnumerator()
        {
            return this._variables.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        T? IGuppyVariableProvider<TVariable>.Get<T>() where T : default
        {
            this.TryGet<T>(out T? value);
            return value;
        }
    }
}
