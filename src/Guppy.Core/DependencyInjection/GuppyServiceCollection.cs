using Guppy.DependencyInjection.Dtos;
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
        IList<TypeFactoryDto>,
        IList<ServiceConfigurationDto>, 
        IList<BuilderAction>,
        IList<SetupAction>,
        IList<ComponentConfigurationDto>,
        IList<ComponentFilter>
    {
        #region Private Fields
        private List<TypeFactoryDto> _typeFactoryDescriptors;
        private List<BuilderAction> _builderActions;
        private List<ServiceConfigurationDto> _serviceConfigurationDescriptors;
        private List<SetupAction> _setupActions;
        private List<ComponentConfigurationDto> _componentConfigurationDescriptors;
        private List<ComponentFilter> _componentFilters;
        #endregion

        #region Public Properties
        public IReadOnlyCollection<TypeFactoryDto> TypeFactories => _typeFactoryDescriptors;
        public IReadOnlyCollection<BuilderAction> BuilderActions => _builderActions;
        public IReadOnlyCollection<ServiceConfigurationDto> ServiceConfigurations => _serviceConfigurationDescriptors;
        public IReadOnlyCollection<SetupAction> SetupActions => _setupActions;
        public IReadOnlyCollection<ComponentConfigurationDto> ComponentConfigurations => _componentConfigurationDescriptors;
        public IReadOnlyCollection<ComponentFilter> ComponentFilters => _componentFilters;
        #endregion

        #region Constrcutors
        internal GuppyServiceCollection()
        {
            _typeFactoryDescriptors = new List<TypeFactoryDto>();
            _builderActions = new List<BuilderAction>();
            _serviceConfigurationDescriptors = new List<ServiceConfigurationDto>();
            _setupActions = new List<SetupAction>();
            _componentConfigurationDescriptors = new List<ComponentConfigurationDto>();
            _componentFilters = new List<ComponentFilter>();
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
        TypeFactoryDto IList<TypeFactoryDto>.this[int index] { get => _typeFactoryDescriptors[index]; set => _typeFactoryDescriptors[index] = value; }

        int ICollection<TypeFactoryDto>.Count => _typeFactoryDescriptors.Count;

        bool ICollection<TypeFactoryDto>.IsReadOnly => false;

        public void Add(TypeFactoryDto item)
            => _typeFactoryDescriptors.Add(item);

        void ICollection<TypeFactoryDto>.Clear()
            => _typeFactoryDescriptors.Clear();

        bool ICollection<TypeFactoryDto>.Contains(TypeFactoryDto item)
            => _typeFactoryDescriptors.Contains(item);

        void ICollection<TypeFactoryDto>.CopyTo(TypeFactoryDto[] array, int arrayIndex)
            => _typeFactoryDescriptors.CopyTo(array, arrayIndex);

        IEnumerator<TypeFactoryDto> IEnumerable<TypeFactoryDto>.GetEnumerator()
            => _typeFactoryDescriptors.GetEnumerator();

        int IList<TypeFactoryDto>.IndexOf(TypeFactoryDto item)
            => _typeFactoryDescriptors.IndexOf(item);

        void IList<TypeFactoryDto>.Insert(int index, TypeFactoryDto item)
            => _typeFactoryDescriptors.Insert(index, item);

        bool ICollection<TypeFactoryDto>.Remove(TypeFactoryDto item)
            => _typeFactoryDescriptors.Remove(item);

        void IList<TypeFactoryDto>.RemoveAt(int index)
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
        ServiceConfigurationDto IList<ServiceConfigurationDto>.this[int index] { get => _serviceConfigurationDescriptors[index]; set => _serviceConfigurationDescriptors[index] = value; }

        int ICollection<ServiceConfigurationDto>.Count => _serviceConfigurationDescriptors.Count;

        bool ICollection<ServiceConfigurationDto>.IsReadOnly => false;

        public void Add(ServiceConfigurationDto item)
            => _serviceConfigurationDescriptors.Add(item);

        void ICollection<ServiceConfigurationDto>.Clear()
            => _serviceConfigurationDescriptors.Clear();

        bool ICollection<ServiceConfigurationDto>.Contains(ServiceConfigurationDto item)
            => _serviceConfigurationDescriptors.Contains(item);

        void ICollection<ServiceConfigurationDto>.CopyTo(ServiceConfigurationDto[] array, int arrayIndex)
            => _serviceConfigurationDescriptors.CopyTo(array, arrayIndex);

        IEnumerator<ServiceConfigurationDto> IEnumerable<ServiceConfigurationDto>.GetEnumerator()
            => _serviceConfigurationDescriptors.GetEnumerator();

        int IList<ServiceConfigurationDto>.IndexOf(ServiceConfigurationDto item)
            => _serviceConfigurationDescriptors.IndexOf(item);

        void IList<ServiceConfigurationDto>.Insert(int index, ServiceConfigurationDto item)
            => _serviceConfigurationDescriptors.Insert(index, item);

        bool ICollection<ServiceConfigurationDto>.Remove(ServiceConfigurationDto item)
            => _serviceConfigurationDescriptors.Remove(item);

        void IList<ServiceConfigurationDto>.RemoveAt(int index)
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
        ComponentConfigurationDto IList<ComponentConfigurationDto>.this[int index] { get => _componentConfigurationDescriptors[index]; set => _componentConfigurationDescriptors[index] = value; }

        int ICollection<ComponentConfigurationDto>.Count => _componentConfigurationDescriptors.Count;

        bool ICollection<ComponentConfigurationDto>.IsReadOnly => false;

        public void Add(ComponentConfigurationDto item)
            => _componentConfigurationDescriptors.Add(item);

        void ICollection<ComponentConfigurationDto>.Clear()
            => _componentConfigurationDescriptors.Clear();

        bool ICollection<ComponentConfigurationDto>.Contains(ComponentConfigurationDto item)
            => _componentConfigurationDescriptors.Contains(item);

        void ICollection<ComponentConfigurationDto>.CopyTo(ComponentConfigurationDto[] array, int arrayIndex)
            => _componentConfigurationDescriptors.CopyTo(array, arrayIndex);

        IEnumerator<ComponentConfigurationDto> IEnumerable<ComponentConfigurationDto>.GetEnumerator()
            => _componentConfigurationDescriptors.GetEnumerator();

        int IList<ComponentConfigurationDto>.IndexOf(ComponentConfigurationDto item)
            => _componentConfigurationDescriptors.IndexOf(item);

        void IList<ComponentConfigurationDto>.Insert(int index, ComponentConfigurationDto item)
            => _componentConfigurationDescriptors.Insert(index, item);

        bool ICollection<ComponentConfigurationDto>.Remove(ComponentConfigurationDto item)
            => _componentConfigurationDescriptors.Remove(item);

        void IList<ComponentConfigurationDto>.RemoveAt(int index)
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
