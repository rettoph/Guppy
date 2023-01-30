using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources
{
    public interface IResource
    {
        string Name { get; }

        void Initialize(string path, IServiceProvider provider);
        void Export(string path, IServiceProvider provider);
    }

    public interface IResource<out TValue> : IResource
    {
        TValue Value { get; }
    }

    public interface IResource<out TValue, TJson> : IResource<TValue>
    {
        TJson GetJson();
    }
}
