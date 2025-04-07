using System.Diagnostics.CodeAnalysis;
using Guppy.Core.Common;
using Guppy.Core.Common.Services;

namespace Guppy.Core.Services
{
    public abstract class GuppyVariableService<TVariable>(IEnumerable<TVariable> variables) : IGuppyVariableService<TVariable>
        where TVariable : IGuppyVariable
    {
        private readonly Dictionary<Type, TVariable[]> _variables = variables.GroupBy(x => x.GetType())
            .ToDictionary(x => x.Key, x => x.ToArray());

        public T Get<T>()
            where T : TVariable
        {
            return (T)this._variables[typeof(T)].Last();
        }

        public object Get(Type variableType)
        {
            ThrowIf.Type.IsNotAssignableFrom<IGuppyVariable>(variableType);
            return this._variables[variableType];
        }

        public bool TryGet<T>([MaybeNullWhen(false)] out T variable)
            where T : TVariable
        {
            if (this._variables.TryGetValue(typeof(T), out TVariable[]? variables) == false)
            {
                variable = default;
                return false;
            }

            if (variables.Length == 0)
            {
                variable = default;
                return false;
            }

            variable = (T)variables.Last();
            return true;
        }

        public IEnumerable<T> GetAll<T>() where T : TVariable
        {
            if (this._variables.TryGetValue(typeof(T), out TVariable[]? variables) == false)
            {
                return Enumerable.Empty<T>();
            }

            return variables.Select(x => (T)x);
        }

        public bool TryGet(Type variableType, [MaybeNullWhen(false)] out TVariable? variable)
        {
            ThrowIf.Type.IsNotAssignableFrom<IGuppyVariable>(variableType);
            if (this._variables.TryGetValue(variableType, out TVariable[]? variables) == false)
            {
                variable = default;
                return false;
            }

            if (variables.Length == 0)
            {
                variable = default;
                return false;
            }

            variable = variables.Last();
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

        public Dictionary<Type, TVariable[]> ToDictionary()
        {
            return this._variables.ToDictionary();
        }
    }
}
