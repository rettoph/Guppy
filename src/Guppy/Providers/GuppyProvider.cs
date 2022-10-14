using Guppy.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Providers
{
    internal sealed class GuppyProvider : IGuppyProvider, IEnumerable<IScoped<IGuppy>>
    {
        private IServiceProvider _provider;
        private IList<IScoped<IGuppy>> _guppies;

        public GuppyProvider(IServiceProvider provider)
        {
            _guppies = new List<IScoped<IGuppy>>();
            _provider = provider;
        }

        IScoped<T> IGuppyProvider.Create<T>()
        {
            var guppy = new Scoped<T>(_provider);

            _guppies.Add(guppy);

            return guppy;
        }

        IEnumerable<IScoped<T>> IGuppyProvider.All<T>()
        {
            foreach(IScoped<IGuppy> guppy in _guppies)
            {
                if(guppy is IScoped<T> casted)
                {
                    yield return casted;
                }
            }
        }

        public IEnumerator<IScoped<IGuppy>> GetEnumerator()
        {
            return _guppies.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
