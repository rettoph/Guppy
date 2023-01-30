using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources
{
    public abstract class Resource<TValue, TJson> : IResource<TValue, TJson>
    {
        public string Name { get; }
        public virtual TValue Value { get; set; }

        public Resource(string name)
        {
            this.Name = name;
            this.Value = default!;
        }

        public abstract void Initialize(string path, IServiceProvider services);
        public abstract void Export(string path, IServiceProvider services);

        public abstract TJson GetJson();
    }

    public abstract class Resource<T> : IResource<T, T>
    {
        public string Name { get; }
        public T Value { get; set; }

        public Resource(string name, T value)
        {
            this.Name = name;
            this.Value = value;
        }

        public virtual void Initialize(string path, IServiceProvider services)
        {
            //
        }

        public virtual void Export(string path, IServiceProvider services)
        {
            //
        }

        public T GetJson()
        {
            return this.Value;
        }
    }
}
