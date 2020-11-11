using Guppy.DependencyInjection.Descriptors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.DependencyInjection
{
    public partial class ServiceCollection : IServiceCollection, IList<ServiceFactoryDescriptor>, IList<ServiceConfigurationDescriptor>, IList<ServiceAction>
    {
        #region Private Fields
        private List<ServiceDescriptor> _descriptors;
        private List<ServiceFactoryDescriptor> _factories;
        private List<ServiceConfigurationDescriptor> _configurations;
        private List<ServiceAction> _actions;
        #endregion

        #region Public Properties
        public IReadOnlyCollection<ServiceDescriptor> Descriptors => _descriptors;
        public IReadOnlyCollection<ServiceFactoryDescriptor> Factories => _factories;
        public IReadOnlyCollection<ServiceConfigurationDescriptor> Configurations => _configurations;
        public IReadOnlyCollection<ServiceAction> Actions => _actions;
        #endregion

        #region Constrcutors
        public ServiceCollection()
        {
            _descriptors = new List<ServiceDescriptor>();
            _factories = new List<ServiceFactoryDescriptor>();
            _configurations = new List<ServiceConfigurationDescriptor>();
            _actions = new List<ServiceAction>();
        }
        #endregion

        #region Helper Methods
        public ServiceProvider BuildServiceProvider()
            => new ServiceProvider(this);
        #endregion

        #region IEnumerable Implementation
        IEnumerator IEnumerable.GetEnumerator()
            => throw new NotImplementedException();
        #endregion

        #region ICollection<ServiceFactory> Implementation
        ServiceFactoryDescriptor IList<ServiceFactoryDescriptor>.this[int index] { get => _factories[index]; set => _factories[index] = value; }

        int ICollection<ServiceFactoryDescriptor>.Count => _factories.Count;

        bool ICollection<ServiceFactoryDescriptor>.IsReadOnly => false;

        public void Add(ServiceFactoryDescriptor item)
            => _factories.Add(item);

        void ICollection<ServiceFactoryDescriptor>.Clear()
            => _factories.Clear();

        bool ICollection<ServiceFactoryDescriptor>.Contains(ServiceFactoryDescriptor item)
            => _factories.Contains(item);

        void ICollection<ServiceFactoryDescriptor>.CopyTo(ServiceFactoryDescriptor[] array, int arrayIndex)
            => _factories.CopyTo(array, arrayIndex);

        IEnumerator<ServiceFactoryDescriptor> IEnumerable<ServiceFactoryDescriptor>.GetEnumerator()
            => _factories.GetEnumerator();

        int IList<ServiceFactoryDescriptor>.IndexOf(ServiceFactoryDescriptor item)
            => _factories.IndexOf(item);

        void IList<ServiceFactoryDescriptor>.Insert(int index, ServiceFactoryDescriptor item)
            => _factories.Insert(index, item);

        bool ICollection<ServiceFactoryDescriptor>.Remove(ServiceFactoryDescriptor item)
            => _factories.Remove(item);

        void IList<ServiceFactoryDescriptor>.RemoveAt(int index)
            => _factories.RemoveAt(index);
        #endregion

        #region ICollection<ServiceConfigurationDescriptor> Implementation
        ServiceConfigurationDescriptor IList<ServiceConfigurationDescriptor>.this[int index] { get => _configurations[index]; set => _configurations[index] = value; }

        int ICollection<ServiceConfigurationDescriptor>.Count => _configurations.Count;

        bool ICollection<ServiceConfigurationDescriptor>.IsReadOnly => false;

        public void Add(ServiceConfigurationDescriptor item)
            => _configurations.Add(item);

        void ICollection<ServiceConfigurationDescriptor>.Clear()
            => _configurations.Clear();

        bool ICollection<ServiceConfigurationDescriptor>.Contains(ServiceConfigurationDescriptor item)
            => _configurations.Contains(item);

        void ICollection<ServiceConfigurationDescriptor>.CopyTo(ServiceConfigurationDescriptor[] array, int arrayIndex)
            => _configurations.CopyTo(array, arrayIndex);

        IEnumerator<ServiceConfigurationDescriptor> IEnumerable<ServiceConfigurationDescriptor>.GetEnumerator()
            => _configurations.GetEnumerator();

        int IList<ServiceConfigurationDescriptor>.IndexOf(ServiceConfigurationDescriptor item)
            => _configurations.IndexOf(item);

        void IList<ServiceConfigurationDescriptor>.Insert(int index, ServiceConfigurationDescriptor item)
            => _configurations.Insert(index, item);

        bool ICollection<ServiceConfigurationDescriptor>.Remove(ServiceConfigurationDescriptor item)
            => _configurations.Remove(item);

        void IList<ServiceConfigurationDescriptor>.RemoveAt(int index)
            => _configurations.RemoveAt(index);
        #endregion

        #region ICollection<ServiceAction> Implementation
        ServiceAction IList<ServiceAction>.this[int index] { get => _actions[index]; set => _actions[index] = value; }

        int ICollection<ServiceAction>.Count => _actions.Count;

        bool ICollection<ServiceAction>.IsReadOnly => false;

        public void Add(ServiceAction item)
            => _actions.Add(item);

        void ICollection<ServiceAction>.Clear()
            => _actions.Clear();

        bool ICollection<ServiceAction>.Contains(ServiceAction item)
            => _actions.Contains(item);

        void ICollection<ServiceAction>.CopyTo(ServiceAction[] array, int arrayIndex)
            => _actions.CopyTo(array, arrayIndex);

        IEnumerator<ServiceAction> IEnumerable<ServiceAction>.GetEnumerator()
            => _actions.GetEnumerator();

        int IList<ServiceAction>.IndexOf(ServiceAction item)
            => _actions.IndexOf(item);

        void IList<ServiceAction>.Insert(int index, ServiceAction item)
            => _actions.Insert(index, item);

        bool ICollection<ServiceAction>.Remove(ServiceAction item)
            => _actions.Remove(item);

        void IList<ServiceAction>.RemoveAt(int index)
            => _actions.RemoveAt(index);
        #endregion

        #region IServiceCollection Implementation
        ServiceDescriptor IList<ServiceDescriptor>.this[int index] { get => _descriptors[index]; set => _descriptors[index] = value; }

        int ICollection<ServiceDescriptor>.Count => _descriptors.Count;

        bool ICollection<ServiceDescriptor>.IsReadOnly => false;

        void ICollection<ServiceDescriptor>.Add(ServiceDescriptor item)
            => _descriptors.Add(item);

        void ICollection<ServiceDescriptor>.Clear()
            => _descriptors.Clear();

        bool ICollection<ServiceDescriptor>.Contains(ServiceDescriptor item)
            => _descriptors.Contains(item);

        void ICollection<ServiceDescriptor>.CopyTo(ServiceDescriptor[] array, int arrayIndex)
            => _descriptors.CopyTo(array, arrayIndex);

        IEnumerator<ServiceDescriptor> IEnumerable<ServiceDescriptor>.GetEnumerator()
            => _descriptors.GetEnumerator();

        int IList<ServiceDescriptor>.IndexOf(ServiceDescriptor item)
            => _descriptors.IndexOf(item);

        void IList<ServiceDescriptor>.Insert(int index, ServiceDescriptor item)
            => _descriptors.Insert(index, item);

        bool ICollection<ServiceDescriptor>.Remove(ServiceDescriptor item)
            => _descriptors.Remove(item);

        void IList<ServiceDescriptor>.RemoveAt(int index)
            => _descriptors.RemoveAt(index);
        #endregion
    }
}
