using Guppy.UI.Utilities.Units.UnitValues;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities.Units
{
    /// <summary>
    /// Units are dynamic values that change
    /// based on recieved parent values.
    /// 
    /// Units can be used for defining element 
    /// boundries.
    /// </summary>
    public class Unit
    {
        #region Private Fields
        private Int32 _newValue;
        private UnitValue _rawValue;
        #endregion

        #region Public Fields
        public Int32 Value { get; private set; }
        public Unit Parent { get; private set; }
        #endregion

        #region Eventss
        public event EventHandler<Unit> OnUpdated;
        #endregion

        #region Constructors
        public Unit(UnitValue value, Unit parent = null)
        {
            this.SetValue(value);
            this.SetParent(parent);
        }
        public Unit(params UnitValue[] values)
        {
            this.SetValue(values);
            this.SetParent(null);
        }
        #endregion

        /// <summary>
        /// Update the underlying UnitValue used
        /// to generate the current Unit's value
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(UnitValue value)
        {
            _rawValue = value == null ? 0 : value;
            this.Refresh();
        }

        public UnitValue GetRawValue()
        {
            return _rawValue;
        }

        /// <summary>
        /// Update the current unit's parent.
        /// </summary>
        /// <param name="parent"></param>
        public void SetParent(Unit parent)
        {
            if (parent == this)
                throw new Exception("Unable to define parent as self.");

            if(this.Parent != null)
                this.Parent.OnUpdated -= this.HandleParentUpdated;

            this.Parent = parent;
            this.Refresh();

            if (this.Parent != null)
                this.Parent.OnUpdated += this.HandleParentUpdated;
        }

        public void Refresh()
        {
            _newValue = _rawValue.Generate(this.Parent == null ? 0 : this.Parent.Value);
            if(_newValue != this.Value)
            {
                this.Value = _newValue;
                this.OnUpdated?.Invoke(this, this);
            }
        }

        #region Event Handlers
        private void HandleParentUpdated(object sender, Unit e)
        {
            this.Refresh();
        }
        #endregion

        #region Operators
        public static implicit operator Int32(Unit unit)
        {
            return unit.Value;
        }
        public static implicit operator Unit(Int32 value)
        {
            return new Unit(value);
        }
        public static implicit operator Unit(Single amount)
        {
            return new Unit(amount);
        }
        public static implicit operator Unit(UnitValue[] values)
        {
            return new Unit(values);
        }
        #endregion
    }
}
