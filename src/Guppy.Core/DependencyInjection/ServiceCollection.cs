using Guppy.DependencyInjection.Contexts;
using Guppy.DependencyInjection.Actions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.DependencyInjection
{
    public partial class ServiceCollection : IServiceCollection, 
        IList<TypeFactoryContext>,
        IList<ServiceConfigurationContext>, 
        IList<BuilderAction>,
        IList<SetupAction>,
        IList<ComponentConfigurationContext>,
        IList<ComponentFilter>
    {
        #region Private Fields
        private List<ServiceDescriptor> _serviceDescriptors;
        private List<TypeFactoryContext> _typeFactoryDescriptors;
        private List<BuilderAction> _builderActions;
        private List<ServiceConfigurationContext> _serviceConfigurationDescriptors;
        private List<SetupAction> _setupActions;
        private List<ComponentConfigurationContext> _componentConfigurationDescriptors;
        private List<ComponentFilter> _componentFilters;
        #endregion

        #region Public Properties
        public IReadOnlyCollection<TypeFactoryContext> TypeFactories => _typeFactoryDescriptors;
        public IReadOnlyCollection<BuilderAction> BuilderActions => _builderActions;
        public IReadOnlyCollection<ServiceConfigurationContext> ServiceConfigurations => _serviceConfigurationDescriptors;
        public IReadOnlyCollection<SetupAction> SetupActions => _setupActions;
        public IReadOnlyCollection<ComponentConfigurationContext> ComponentConfigurations => _componentConfigurationDescriptors;
        public IReadOnlyCollection<ComponentFilter> ComponentFilters => _componentFilters;
        #endregion

        #region Constrcutors
        internal ServiceCollection()
        {
            _serviceDescriptors = new List<ServiceDescriptor>();
            _typeFactoryDescriptors = new List<TypeFactoryContext>();
            _builderActions = new List<BuilderAction>();
            _serviceConfigurationDescriptors = new List<ServiceConfigurationContext>();
            _setupActions = new List<SetupAction>();
            _componentConfigurationDescriptors = new List<ComponentConfigurationContext>();
            _componentFilters = new List<ComponentFilter>();
        }
        #endregion

        #region Helper Methods
        public ServiceProvider BuildServiceProvider()
        {
            return new ServiceProvider(this);
        }
        #endregion

        #region IServiceCollection Implementation
        ServiceDescriptor IList<ServiceDescriptor>.this[int index] { get => _serviceDescriptors[index]; set => throw new NotImplementedException(); }

        int ICollection<ServiceDescriptor>.Count => _serviceDescriptors.Count;

        bool ICollection<ServiceDescriptor>.IsReadOnly => throw new NotImplementedException();

        void ICollection<ServiceDescriptor>.Add(ServiceDescriptor item)
        {
            _serviceDescriptors.Add(item);
            item.ConvertServiceDescriptor(this);
        }

        void ICollection<ServiceDescriptor>.Clear()
            => throw new NotImplementedException();

        bool ICollection<ServiceDescriptor>.Contains(ServiceDescriptor item)
            => throw new NotImplementedException();

        void ICollection<ServiceDescriptor>.CopyTo(ServiceDescriptor[] array, int arrayIndex)
            => throw new NotImplementedException();

        IEnumerator<ServiceDescriptor> IEnumerable<ServiceDescriptor>.GetEnumerator()
            => _serviceDescriptors.GetEnumerator();

        int IList<ServiceDescriptor>.IndexOf(ServiceDescriptor item)
            => throw new NotImplementedException();

        void IList<ServiceDescriptor>.Insert(int index, ServiceDescriptor item)
            => throw new NotImplementedException();

        bool ICollection<ServiceDescriptor>.Remove(ServiceDescriptor item)
            => throw new NotImplementedException();

        void IList<ServiceDescriptor>.RemoveAt(int index)
            => throw new NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator()
            => throw new NotImplementedException();
        #endregion

        #region IList<TypeFactoryDescriptor> Implementation
        TypeFactoryContext IList<TypeFactoryContext>.this[int index] { get => _typeFactoryDescriptors[index]; set => _typeFactoryDescriptors[index] = value; }

        int ICollection<TypeFactoryContext>.Count => _typeFactoryDescriptors.Count;

        bool ICollection<TypeFactoryContext>.IsReadOnly => false;

        public void Add(TypeFactoryContext item)
            => _typeFactoryDescriptors.Add(item);

        void ICollection<TypeFactoryContext>.Clear()
            => _typeFactoryDescriptors.Clear();

        bool ICollection<TypeFactoryContext>.Contains(TypeFactoryContext item)
            => _typeFactoryDescriptors.Contains(item);

        void ICollection<TypeFactoryContext>.CopyTo(TypeFactoryContext[] array, int arrayIndex)
            => _typeFactoryDescriptors.CopyTo(array, arrayIndex);

        IEnumerator<TypeFactoryContext> IEnumerable<TypeFactoryContext>.GetEnumerator()
            => _typeFactoryDescriptors.GetEnumerator();

        int IList<TypeFactoryContext>.IndexOf(TypeFactoryContext item)
            => _typeFactoryDescriptors.IndexOf(item);

        void IList<TypeFactoryContext>.Insert(int index, TypeFactoryContext item)
            => _typeFactoryDescriptors.Insert(index, item);

        bool ICollection<TypeFactoryContext>.Remove(TypeFactoryContext item)
            => _typeFactoryDescriptors.Remove(item);

        void IList<TypeFactoryContext>.RemoveAt(int index)
            => _typeFactoryDescriptors.RemoveAt(index);
        #endregion

        #region IList<TypeBuilderAction> Implementation
        BuilderAction IList<BuilderAction>.this[int index] { get => _builderActions[index]; set => _builderActions[index] = value; }

        int ICollection<BuilderAction>.Count => _builderActions.Count;

        bool ICollection<BuilderAction>.IsReadOnly => false;

        public void Add(BuilderAction item)
            => _builderActions.Add(item);

        void ICollection<BuilderAction>.Clear()
            => _builderActions.Clear();

        bool ICollection<BuilderAction>.Contains(BuilderAction item)
            => _builderActions.Contains(item);

        void ICollection<BuilderAction>.CopyTo(BuilderAction[] array, int arrayIndex)
            => _builderActions.CopyTo(array, arrayIndex);

        IEnumerator<BuilderAction> IEnumerable<BuilderAction>.GetEnumerator()
            => _builderActions.GetEnumerator();

        int IList<BuilderAction>.IndexOf(BuilderAction item)
            => _builderActions.IndexOf(item);

        void IList<BuilderAction>.Insert(int index, BuilderAction item)
            => _builderActions.Insert(index, item);

        bool ICollection<BuilderAction>.Remove(BuilderAction item)
            => _builderActions.Remove(item);

        void IList<BuilderAction>.RemoveAt(int index)
            => _builderActions.RemoveAt(index);
        #endregion

        #region IList<ServiceConfigurationDescriptor> Implementation
        ServiceConfigurationContext IList<ServiceConfigurationContext>.this[int index] { get => _serviceConfigurationDescriptors[index]; set => _serviceConfigurationDescriptors[index] = value; }

        int ICollection<ServiceConfigurationContext>.Count => _serviceConfigurationDescriptors.Count;

        bool ICollection<ServiceConfigurationContext>.IsReadOnly => false;

        public void Add(ServiceConfigurationContext item)
            => _serviceConfigurationDescriptors.Add(item);

        void ICollection<ServiceConfigurationContext>.Clear()
            => _serviceConfigurationDescriptors.Clear();

        bool ICollection<ServiceConfigurationContext>.Contains(ServiceConfigurationContext item)
            => _serviceConfigurationDescriptors.Contains(item);

        void ICollection<ServiceConfigurationContext>.CopyTo(ServiceConfigurationContext[] array, int arrayIndex)
            => _serviceConfigurationDescriptors.CopyTo(array, arrayIndex);

        IEnumerator<ServiceConfigurationContext> IEnumerable<ServiceConfigurationContext>.GetEnumerator()
            => _serviceConfigurationDescriptors.GetEnumerator();

        int IList<ServiceConfigurationContext>.IndexOf(ServiceConfigurationContext item)
            => _serviceConfigurationDescriptors.IndexOf(item);

        void IList<ServiceConfigurationContext>.Insert(int index, ServiceConfigurationContext item)
            => _serviceConfigurationDescriptors.Insert(index, item);

        bool ICollection<ServiceConfigurationContext>.Remove(ServiceConfigurationContext item)
            => _serviceConfigurationDescriptors.Remove(item);

        void IList<ServiceConfigurationContext>.RemoveAt(int index)
            => _serviceConfigurationDescriptors.RemoveAt(index);
        #endregion

        #region IList<ServiceSetupAction> Implementation
        SetupAction IList<SetupAction>.this[int index] { get => _setupActions[index]; set => _setupActions[index] = value; }

        int ICollection<SetupAction>.Count => _setupActions.Count;

        bool ICollection<SetupAction>.IsReadOnly => false;

        public void Add(SetupAction item)
            => _setupActions.Add(item);

        void ICollection<SetupAction>.Clear()
            => _setupActions.Clear();

        bool ICollection<SetupAction>.Contains(SetupAction item)
            => _setupActions.Contains(item);

        void ICollection<SetupAction>.CopyTo(SetupAction[] array, int arrayIndex)
            => _setupActions.CopyTo(array, arrayIndex);

        IEnumerator<SetupAction> IEnumerable<SetupAction>.GetEnumerator()
            => _setupActions.GetEnumerator();

        int IList<SetupAction>.IndexOf(SetupAction item)
            => _setupActions.IndexOf(item);

        void IList<SetupAction>.Insert(int index, SetupAction item)
            => _setupActions.Insert(index, item);

        bool ICollection<SetupAction>.Remove(SetupAction item)
            => _setupActions.Remove(item);

        void IList<SetupAction>.RemoveAt(int index)
            => _setupActions.RemoveAt(index);
        #endregion

        #region ICollection<ComponentConfigurationDescriptor> Implementation
        ComponentConfigurationContext IList<ComponentConfigurationContext>.this[int index] { get => _componentConfigurationDescriptors[index]; set => _componentConfigurationDescriptors[index] = value; }

        int ICollection<ComponentConfigurationContext>.Count => _componentConfigurationDescriptors.Count;

        bool ICollection<ComponentConfigurationContext>.IsReadOnly => false;

        public void Add(ComponentConfigurationContext item)
            => _componentConfigurationDescriptors.Add(item);

        void ICollection<ComponentConfigurationContext>.Clear()
            => _componentConfigurationDescriptors.Clear();

        bool ICollection<ComponentConfigurationContext>.Contains(ComponentConfigurationContext item)
            => _componentConfigurationDescriptors.Contains(item);

        void ICollection<ComponentConfigurationContext>.CopyTo(ComponentConfigurationContext[] array, int arrayIndex)
            => _componentConfigurationDescriptors.CopyTo(array, arrayIndex);

        IEnumerator<ComponentConfigurationContext> IEnumerable<ComponentConfigurationContext>.GetEnumerator()
            => _componentConfigurationDescriptors.GetEnumerator();

        int IList<ComponentConfigurationContext>.IndexOf(ComponentConfigurationContext item)
            => _componentConfigurationDescriptors.IndexOf(item);

        void IList<ComponentConfigurationContext>.Insert(int index, ComponentConfigurationContext item)
            => _componentConfigurationDescriptors.Insert(index, item);

        bool ICollection<ComponentConfigurationContext>.Remove(ComponentConfigurationContext item)
            => _componentConfigurationDescriptors.Remove(item);

        void IList<ComponentConfigurationContext>.RemoveAt(int index)
            => _componentConfigurationDescriptors.RemoveAt(index);
        #endregion

        #region ICollection<ComponentConfigurationDescriptor> Implementation
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
