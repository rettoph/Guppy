using Guppy.DependencyInjection.Builders;
using Guppy.DependencyInjection.Actions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Guppy.Extensions.DependencyInjection;

namespace Guppy.DependencyInjection
{
    public partial class GuppyServiceCollection :
        IList<TypeFactoryBuilder>,
        IList<ServiceConfigurationBuilder>, 
        IList<BuilderAction>,
        IList<SetupAction>,
        IList<ComponentConfigurationBuilder>,
        IList<ComponentFilterBuilder>
    {
        #region Private Fields
        private List<TypeFactoryBuilder> _typeFactoryDescriptors;
        private List<BuilderAction> _builderActions;
        private List<ServiceConfigurationBuilder> _serviceConfigurationDescriptors;
        private List<SetupAction> _setupActions;
        private List<ComponentConfigurationBuilder> _componentConfigurationDescriptors;
        private List<ComponentFilterBuilder> _componentFilters;
        #endregion

        #region Public Properties
        public IReadOnlyCollection<TypeFactoryBuilder> TypeFactories => _typeFactoryDescriptors;
        public IReadOnlyCollection<BuilderAction> BuilderActions => _builderActions;
        public IReadOnlyCollection<ServiceConfigurationBuilder> ServiceConfigurations => _serviceConfigurationDescriptors;
        public IReadOnlyCollection<SetupAction> SetupActions => _setupActions;
        public IReadOnlyCollection<ComponentConfigurationBuilder> ComponentConfigurations => _componentConfigurationDescriptors;
        public IReadOnlyCollection<ComponentFilterBuilder> ComponentFilters => _componentFilters;
        #endregion

        #region Constrcutors
        internal GuppyServiceCollection()
        {
            _typeFactoryDescriptors = new List<TypeFactoryBuilder>();
            _builderActions = new List<BuilderAction>();
            _serviceConfigurationDescriptors = new List<ServiceConfigurationBuilder>();
            _setupActions = new List<SetupAction>();
            _componentConfigurationDescriptors = new List<ComponentConfigurationBuilder>();
            _componentFilters = new List<ComponentFilterBuilder>();
        }
        #endregion

        #region Helper Methods
        public GuppyServiceProvider BuildServiceProvider()
        {
            return new GuppyServiceProvider(this);
        }
        #endregion

        #region IEnumerable Implementation
        IEnumerator IEnumerable.GetEnumerator()
            => throw new NotImplementedException();
        #endregion

        #region IList<TypeFactoryDescriptor> Implementation
        TypeFactoryBuilder IList<TypeFactoryBuilder>.this[int index] { get => _typeFactoryDescriptors[index]; set => _typeFactoryDescriptors[index] = value; }

        int ICollection<TypeFactoryBuilder>.Count => _typeFactoryDescriptors.Count;

        bool ICollection<TypeFactoryBuilder>.IsReadOnly => false;

        public void Add(TypeFactoryBuilder item)
            => _typeFactoryDescriptors.Add(item);

        void ICollection<TypeFactoryBuilder>.Clear()
            => _typeFactoryDescriptors.Clear();

        bool ICollection<TypeFactoryBuilder>.Contains(TypeFactoryBuilder item)
            => _typeFactoryDescriptors.Contains(item);

        void ICollection<TypeFactoryBuilder>.CopyTo(TypeFactoryBuilder[] array, int arrayIndex)
            => _typeFactoryDescriptors.CopyTo(array, arrayIndex);

        IEnumerator<TypeFactoryBuilder> IEnumerable<TypeFactoryBuilder>.GetEnumerator()
            => _typeFactoryDescriptors.GetEnumerator();

        int IList<TypeFactoryBuilder>.IndexOf(TypeFactoryBuilder item)
            => _typeFactoryDescriptors.IndexOf(item);

        void IList<TypeFactoryBuilder>.Insert(int index, TypeFactoryBuilder item)
            => _typeFactoryDescriptors.Insert(index, item);

        bool ICollection<TypeFactoryBuilder>.Remove(TypeFactoryBuilder item)
            => _typeFactoryDescriptors.Remove(item);

        void IList<TypeFactoryBuilder>.RemoveAt(int index)
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
        ServiceConfigurationBuilder IList<ServiceConfigurationBuilder>.this[int index] { get => _serviceConfigurationDescriptors[index]; set => _serviceConfigurationDescriptors[index] = value; }

        int ICollection<ServiceConfigurationBuilder>.Count => _serviceConfigurationDescriptors.Count;

        bool ICollection<ServiceConfigurationBuilder>.IsReadOnly => false;

        public void Add(ServiceConfigurationBuilder item)
            => _serviceConfigurationDescriptors.Add(item);

        void ICollection<ServiceConfigurationBuilder>.Clear()
            => _serviceConfigurationDescriptors.Clear();

        bool ICollection<ServiceConfigurationBuilder>.Contains(ServiceConfigurationBuilder item)
            => _serviceConfigurationDescriptors.Contains(item);

        void ICollection<ServiceConfigurationBuilder>.CopyTo(ServiceConfigurationBuilder[] array, int arrayIndex)
            => _serviceConfigurationDescriptors.CopyTo(array, arrayIndex);

        IEnumerator<ServiceConfigurationBuilder> IEnumerable<ServiceConfigurationBuilder>.GetEnumerator()
            => _serviceConfigurationDescriptors.GetEnumerator();

        int IList<ServiceConfigurationBuilder>.IndexOf(ServiceConfigurationBuilder item)
            => _serviceConfigurationDescriptors.IndexOf(item);

        void IList<ServiceConfigurationBuilder>.Insert(int index, ServiceConfigurationBuilder item)
            => _serviceConfigurationDescriptors.Insert(index, item);

        bool ICollection<ServiceConfigurationBuilder>.Remove(ServiceConfigurationBuilder item)
            => _serviceConfigurationDescriptors.Remove(item);

        void IList<ServiceConfigurationBuilder>.RemoveAt(int index)
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
        ComponentConfigurationBuilder IList<ComponentConfigurationBuilder>.this[int index] { get => _componentConfigurationDescriptors[index]; set => _componentConfigurationDescriptors[index] = value; }

        int ICollection<ComponentConfigurationBuilder>.Count => _componentConfigurationDescriptors.Count;

        bool ICollection<ComponentConfigurationBuilder>.IsReadOnly => false;

        public void Add(ComponentConfigurationBuilder item)
            => _componentConfigurationDescriptors.Add(item);

        void ICollection<ComponentConfigurationBuilder>.Clear()
            => _componentConfigurationDescriptors.Clear();

        bool ICollection<ComponentConfigurationBuilder>.Contains(ComponentConfigurationBuilder item)
            => _componentConfigurationDescriptors.Contains(item);

        void ICollection<ComponentConfigurationBuilder>.CopyTo(ComponentConfigurationBuilder[] array, int arrayIndex)
            => _componentConfigurationDescriptors.CopyTo(array, arrayIndex);

        IEnumerator<ComponentConfigurationBuilder> IEnumerable<ComponentConfigurationBuilder>.GetEnumerator()
            => _componentConfigurationDescriptors.GetEnumerator();

        int IList<ComponentConfigurationBuilder>.IndexOf(ComponentConfigurationBuilder item)
            => _componentConfigurationDescriptors.IndexOf(item);

        void IList<ComponentConfigurationBuilder>.Insert(int index, ComponentConfigurationBuilder item)
            => _componentConfigurationDescriptors.Insert(index, item);

        bool ICollection<ComponentConfigurationBuilder>.Remove(ComponentConfigurationBuilder item)
            => _componentConfigurationDescriptors.Remove(item);

        void IList<ComponentConfigurationBuilder>.RemoveAt(int index)
            => _componentConfigurationDescriptors.RemoveAt(index);
        #endregion

        #region ICollection<ComponentFilterBuilder> Implementation
        ComponentFilterBuilder IList<ComponentFilterBuilder>.this[int index] { get => _componentFilters[index]; set => _componentFilters[index] = value; }

        int ICollection<ComponentFilterBuilder>.Count => _componentFilters.Count;

        bool ICollection<ComponentFilterBuilder>.IsReadOnly => false;

        public void Add(ComponentFilterBuilder item)
            => _componentFilters.Add(item);

        void ICollection<ComponentFilterBuilder>.Clear()
            => _componentFilters.Clear();

        bool ICollection<ComponentFilterBuilder>.Contains(ComponentFilterBuilder item)
            => _componentFilters.Contains(item);

        void ICollection<ComponentFilterBuilder>.CopyTo(ComponentFilterBuilder[] array, int arrayIndex)
            => _componentFilters.CopyTo(array, arrayIndex);

        IEnumerator<ComponentFilterBuilder> IEnumerable<ComponentFilterBuilder>.GetEnumerator()
            => _componentFilters.GetEnumerator();

        int IList<ComponentFilterBuilder>.IndexOf(ComponentFilterBuilder item)
            => _componentFilters.IndexOf(item);

        void IList<ComponentFilterBuilder>.Insert(int index, ComponentFilterBuilder item)
            => _componentFilters.Insert(index, item);

        bool ICollection<ComponentFilterBuilder>.Remove(ComponentFilterBuilder item)
            => _componentFilters.Remove(item);

        void IList<ComponentFilterBuilder>.RemoveAt(int index)
            => _componentFilters.RemoveAt(index);
        #endregion
    }
}
