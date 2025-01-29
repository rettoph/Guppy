using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace Guppy.Core.Common.Utilities
{
    public class GuppyEnvironment(Dictionary<Type, IEnvironmentVariable> variables)
    {
        private readonly ReadOnlyDictionary<Type, IEnvironmentVariable> _variables = new(variables);

        public T Get<T>()
            where T : IEnvironmentVariable<T>
        {
            return (T)this._variables[typeof(T)];
        }

        public object Get(Type environmentVariableType)
        {
            ThrowIf.Type.IsNotAssignableFrom<IEnvironmentVariable>(environmentVariableType);
            return this._variables[environmentVariableType];
        }

        public bool TryGet<T>([MaybeNullWhen(false)] out T? value)
            where T : IEnvironmentVariable<T>
        {
            if (this._variables.TryGetValue(typeof(T), out IEnvironmentVariable? uncasted) == false)
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

        public bool TryGet(Type environmentVariableType, [MaybeNullWhen(false)] out object? value)
        {
            ThrowIf.Type.IsNotAssignableFrom<IEnvironmentVariable>(environmentVariableType);
            if (this._variables.TryGetValue(environmentVariableType, out IEnvironmentVariable? variable) == false)
            {
                value = default;
                return false;
            }

            value = variable;
            return true;
        }

        public bool Has<T>()
            where T : IEnvironmentVariable<T>
        {
            return this._variables.ContainsKey(typeof(T));
        }

        public bool Has(Type environmentVariableType)
        {
            ThrowIf.Type.IsNotAssignableFrom<IEnvironmentVariable>(environmentVariableType);
            return this._variables.ContainsKey(environmentVariableType);
        }

        public bool Matches<T>(T value)
            where T : IEnvironmentVariable<T>
        {
            foreach (IEnvironmentVariable<T> variable in this._variables.Values.OfType<IEnvironmentVariable<T>>())
            {
                if (variable.Matches(value))
                {
                    return true;
                }
            }

            return false;
        }

        public bool Matches(object value)
        {
            foreach ((_, IEnvironmentVariable variable) in this._variables)
            {
                if (variable.Matches(value))
                {
                    return true;
                }
            }

            return false;
        }

        public Dictionary<Type, IEnvironmentVariable> ToDictionary()
        {
            return this._variables.ToDictionary();
        }
    }
}
