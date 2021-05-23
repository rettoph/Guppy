using Guppy.DependencyInjection.Descriptors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    public partial class ServiceCollection : IServiceCollection, IList<TypeFactoryDescriptor>, IList<ServiceAction>, IList<ServiceConfigurationDescriptor>, IList<ComponentConfigurationDescriptor>, IList<ComponentFilter>
    {
        #region Private Fields
        private List<ServiceDescriptor> _serviceDescriptors;
        private List<TypeFactoryDescriptor> _typeFactoryDescriptors;
        private List<ServiceConfigurationDescriptor> _serviceConfigurationDescriptors;
        private List<ServiceAction> _serviceActions;
        private List<ComponentConfigurationDescriptor> _componentConfigurationDescriptor;
        private List<ComponentFilter> _componentFilters;
        #endregion

        #region Public Properties
        public IReadOnlyCollection<ServiceDescriptor> ServiceDescriptors => _serviceDescriptors;
        public IReadOnlyCollection<TypeFactoryDescriptor> TypeFactories => _typeFactoryDescriptors;
        public IReadOnlyCollection<ServiceConfigurationDescriptor> ServiceConfigurations => _serviceConfigurationDescriptors;
        public IReadOnlyCollection<ServiceAction> ServiceActions => _serviceActions;
        public IReadOnlyCollection<ComponentConfigurationDescriptor> ComponentConfigurationDescriptors => _componentConfigurationDescriptor;
        public IReadOnlyCollection<ComponentFilter> ComponentFilters => _componentFilters;
        #endregion

        #region Constrcutors
        public ServiceCollection()
        {
            _serviceDescriptors = new List<ServiceDescriptor>();
            _typeFactoryDescriptors = new List<TypeFactoryDescriptor>();
            _serviceConfigurationDescriptors = new List<ServiceConfigurationDescriptor>();
            _serviceActions = new List<ServiceAction>();
            _componentConfigurationDescriptor = new List<ComponentConfigurationDescriptor>();
            _componentFilters = new List<ComponentFilter>();
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

        #region IServiceCollection Implementation
        ServiceDescriptor IList<ServiceDescriptor>.this[int index] { get => _serviceDescriptors[index]; set => _serviceDescriptors[index] = value; }

        int ICollection<ServiceDescriptor>.Count => _serviceDescriptors.Count;

        bool ICollection<ServiceDescriptor>.IsReadOnly => false;

        void ICollection<ServiceDescriptor>.Add(ServiceDescriptor item)
            => _serviceDescriptors.Add(item);

        void ICollection<ServiceDescriptor>.Clear()
            => _serviceDescriptors.Clear();

        bool ICollection<ServiceDescriptor>.Contains(ServiceDescriptor item)
            => _serviceDescriptors.Contains(item);

        void ICollection<ServiceDescriptor>.CopyTo(ServiceDescriptor[] array, int arrayIndex)
            => _serviceDescriptors.CopyTo(array, arrayIndex);

        IEnumerator<ServiceDescriptor> IEnumerable<ServiceDescriptor>.GetEnumerator()
            => _serviceDescriptors.GetEnumerator();

        int IList<ServiceDescriptor>.IndexOf(ServiceDescriptor item)
            => _serviceDescriptors.IndexOf(item);

        void IList<ServiceDescriptor>.Insert(int index, ServiceDescriptor item)
            => _serviceDescriptors.Insert(index, item);

        bool ICollection<ServiceDescriptor>.Remove(ServiceDescriptor item)
            => _serviceDescriptors.Remove(item);

        void IList<ServiceDescriptor>.RemoveAt(int index)
            => _serviceDescriptors.RemoveAt(index);
        #endregion

        #region ICollection<TypeFactoryDescriptor> Implementation
        TypeFactoryDescriptor IList<TypeFactoryDescriptor>.this[int index] { get => _typeFactoryDescriptors[index]; set => _typeFactoryDescriptors[index] = value; }

        int ICollection<TypeFactoryDescriptor>.Count => _typeFactoryDescriptors.Count;

        bool ICollection<TypeFactoryDescriptor>.IsReadOnly => false;

        public void Add(TypeFactoryDescriptor item)
            => _typeFactoryDescriptors.Add(item);

        void ICollection<TypeFactoryDescriptor>.Clear()
            => _typeFactoryDescriptors.Clear();

        bool ICollection<TypeFactoryDescriptor>.Contains(TypeFactoryDescriptor item)
            => _typeFactoryDescriptors.Contains(item);

        void ICollection<TypeFactoryDescriptor>.CopyTo(TypeFactoryDescriptor[] array, int arrayIndex)
            => _typeFactoryDescriptors.CopyTo(array, arrayIndex);

        IEnumerator<TypeFactoryDescriptor> IEnumerable<TypeFactoryDescriptor>.GetEnumerator()
            => _typeFactoryDescriptors.GetEnumerator();

        int IList<TypeFactoryDescriptor>.IndexOf(TypeFactoryDescriptor item)
            => _typeFactoryDescriptors.IndexOf(item);

        void IList<TypeFactoryDescriptor>.Insert(int index, TypeFactoryDescriptor item)
            => _typeFactoryDescriptors.Insert(index, item);

        bool ICollection<TypeFactoryDescriptor>.Remove(TypeFactoryDescriptor item)
            => _typeFactoryDescriptors.Remove(item);

        void IList<TypeFactoryDescriptor>.RemoveAt(int index)
            => _typeFactoryDescriptors.RemoveAt(index);
        #endregion

        #region ICollection<ServiceAction> Implementation
        ServiceAction IList<ServiceAction>.this[int index] { get => _serviceActions[index]; set => _serviceActions[index] = value; }

        int ICollection<ServiceAction>.Count => _serviceActions.Count;

        bool ICollection<ServiceAction>.IsReadOnly => false;

        public void Add(ServiceAction item)
            => _serviceActions.Add(item);

        void ICollection<ServiceAction>.Clear()
            => _serviceActions.Clear();

        bool ICollection<ServiceAction>.Contains(ServiceAction item)
            => _serviceActions.Contains(item);

        void ICollection<ServiceAction>.CopyTo(ServiceAction[] array, int arrayIndex)
            => _serviceActions.CopyTo(array, arrayIndex);

        IEnumerator<ServiceAction> IEnumerable<ServiceAction>.GetEnumerator()
            => _serviceActions.GetEnumerator();

        int IList<ServiceAction>.IndexOf(ServiceAction item)
            => _serviceActions.IndexOf(item);

        void IList<ServiceAction>.Insert(int index, ServiceAction item)
            => _serviceActions.Insert(index, item);

        bool ICollection<ServiceAction>.Remove(ServiceAction item)
            => _serviceActions.Remove(item);

        void IList<ServiceAction>.RemoveAt(int index)
            => _serviceActions.RemoveAt(index);
        #endregion

        #region ICollection<ServiceConfigurationDescriptor> Implementation
        ServiceConfigurationDescriptor IList<ServiceConfigurationDescriptor>.this[int index] { get => _serviceConfigurationDescriptors[index]; set => _serviceConfigurationDescriptors[index] = value; }

        int ICollection<ServiceConfigurationDescriptor>.Count => _serviceConfigurationDescriptors.Count;

        bool ICollection<ServiceConfigurationDescriptor>.IsReadOnly => false;

        public void Add(ServiceConfigurationDescriptor item)
            => _serviceConfigurationDescriptors.Add(item);

        void ICollection<ServiceConfigurationDescriptor>.Clear()
            => _serviceConfigurationDescriptors.Clear();

        bool ICollection<ServiceConfigurationDescriptor>.Contains(ServiceConfigurationDescriptor item)
            => _serviceConfigurationDescriptors.Contains(item);

        void ICollection<ServiceConfigurationDescriptor>.CopyTo(ServiceConfigurationDescriptor[] array, int arrayIndex)
            => _serviceConfigurationDescriptors.CopyTo(array, arrayIndex);

        IEnumerator<ServiceConfigurationDescriptor> IEnumerable<ServiceConfigurationDescriptor>.GetEnumerator()
            => _serviceConfigurationDescriptors.GetEnumerator();

        int IList<ServiceConfigurationDescriptor>.IndexOf(ServiceConfigurationDescriptor item)
            => _serviceConfigurationDescriptors.IndexOf(item);

        void IList<ServiceConfigurationDescriptor>.Insert(int index, ServiceConfigurationDescriptor item)
            => _serviceConfigurationDescriptors.Insert(index, item);

        bool ICollection<ServiceConfigurationDescriptor>.Remove(ServiceConfigurationDescriptor item)
            => _serviceConfigurationDescriptors.Remove(item);

        void IList<ServiceConfigurationDescriptor>.RemoveAt(int index)
            => _serviceConfigurationDescriptors.RemoveAt(index);
        #endregion

        #region ICollection<ComponentConfigurationDescriptor> Implementation
        ComponentConfigurationDescriptor IList<ComponentConfigurationDescriptor>.this[int index] { get => _componentConfigurationDescriptor[index]; set => _componentConfigurationDescriptor[index] = value; }

        int ICollection<ComponentConfigurationDescriptor>.Count => _componentConfigurationDescriptor.Count;

        bool ICollection<ComponentConfigurationDescriptor>.IsReadOnly => false;

        public void Add(ComponentConfigurationDescriptor item)
            => _componentConfigurationDescriptor.Add(item);

        void ICollection<ComponentConfigurationDescriptor>.Clear()
            => _componentConfigurationDescriptor.Clear();

        bool ICollection<ComponentConfigurationDescriptor>.Contains(ComponentConfigurationDescriptor item)
            => _componentConfigurationDescriptor.Contains(item);

        void ICollection<ComponentConfigurationDescriptor>.CopyTo(ComponentConfigurationDescriptor[] array, int arrayIndex)
            => _componentConfigurationDescriptor.CopyTo(array, arrayIndex);

        IEnumerator<ComponentConfigurationDescriptor> IEnumerable<ComponentConfigurationDescriptor>.GetEnumerator()
            => _componentConfigurationDescriptor.GetEnumerator();

        int IList<ComponentConfigurationDescriptor>.IndexOf(ComponentConfigurationDescriptor item)
            => _componentConfigurationDescriptor.IndexOf(item);

        void IList<ComponentConfigurationDescriptor>.Insert(int index, ComponentConfigurationDescriptor item)
            => _componentConfigurationDescriptor.Insert(index, item);

        bool ICollection<ComponentConfigurationDescriptor>.Remove(ComponentConfigurationDescriptor item)
            => _componentConfigurationDescriptor.Remove(item);

        void IList<ComponentConfigurationDescriptor>.RemoveAt(int index)
            => _componentConfigurationDescriptor.RemoveAt(index);
        #endregion

        #region ICollection<ComponentFilter> Implementation
        ComponentFilter IList<ComponentFilter>.this[int index] { get => _componentFilters[index]; set => _componentFilters[index] = value; }

        int ICollection<ComponentFilter>.Count => _componentFilters.Count;

        bool ICollection<ComponentFilter>.IsReadOnly => false;

        public void Add(ComponentFilter item)
            => _componentFilters.Add(item);

        void ICollection<ComponentFilter>.Clear()
            => _componentFilters.Clear();

        bool ICollection<ComponentFilter>.Contains(ComponentFilter item)
            => _componentFilters.Contains(item);

        void ICollection<ComponentFilter>.CopyTo(ComponentFilter[] array, int arrayIndex)
            => _componentFilters.CopyTo(array, arrayIndex);

        IEnumerator<ComponentFilter> IEnumerable<ComponentFilter>.GetEnumerator()
            => _componentFilters.GetEnumerator();

        int IList<ComponentFilter>.IndexOf(ComponentFilter item)
            => _componentFilters.IndexOf(item);

        void IList<ComponentFilter>.Insert(int index, ComponentFilter item)
            => _componentFilters.Insert(index, item);

        bool ICollection<ComponentFilter>.Remove(ComponentFilter item)
            => _componentFilters.Remove(item);

        void IList<ComponentFilter>.RemoveAt(int index)
            => _componentFilters.RemoveAt(index);
        #endregion
    }
}
