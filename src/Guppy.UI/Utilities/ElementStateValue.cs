using Guppy.UI.Enums;
using Guppy.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities
{
    /// <summary>
    /// Simple helper object used to contain values based on
    /// an element's current state value.
    /// 
    /// A helper builder method exists within the Element.cs
    /// class under this.BuildStateValue<T>
    /// </summary>
    /// <typeparam name="T">The value type</typeparam>
    public class ElementStateValue<T> : IComparer<ElementState>, IDisposable
    {
        #region Private Fields
        private SortedDictionary<ElementState, T> _values;
        private IElement _owner;
        #endregion

        #region Public Properties
        /// <summary>
        /// The current elements T value based on
        /// state.
        /// </summary>
        public T Current { get; private set; }

        public T this[ElementState state]
        {
            get
            {
                foreach(ElementState vState in _values.Keys)
                    if (vState <= state && (state & vState) != 0 && (~state & vState) == 0)
                        return _values[vState];

                return _values[ElementState.Default];
            }
            set
            {
                _values[state] = value;
                this.Current = this[_owner.State];
            }
        }
        #endregion

        #region Constructpr
        internal ElementStateValue(IElement owner, T defaultValue = default(T))
        {
            _owner = owner;
            _values = new SortedDictionary<ElementState, T>(this);
            _values[ElementState.Default] = defaultValue;

            _owner.OnStateChanged += this.HandleOwnerStateChanged;

            this.Current = this[_owner.State];
        }
        #endregion

        #region IDisposable Implementation
        public void Dispose()
        {
            _owner.OnStateChanged -= this.HandleOwnerStateChanged;
        }
        #endregion

        #region IComparer<ElementState> Implementation
        public int Compare(ElementState x, ElementState y)
        {
            if (x < y)
                return 1;
            if (x > y)
                return -1;

            return 0;
        }
        #endregion

        #region Event Handlers
        private void HandleOwnerStateChanged(IElement sender, ElementState which, bool value)
            => this.Current = this[_owner.State];
        #endregion

        #region Operators
        public static implicit operator T(ElementStateValue<T> elementStateValue)
            => elementStateValue.Current;
        #endregion
    }
}
