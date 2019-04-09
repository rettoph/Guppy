using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Utilities.Units.UnitValues
{
    /// <summary>
    /// Unit values are the literal unit mutators.
    /// 
    /// These are used to generate one time use Int32
    /// based on recieved parent information.
    /// </summary>
    public abstract class UnitValue
    {
        public abstract Int32 Generate(Int32 parent);
        protected internal abstract UnitValue Flip();

        #region Operators
        public static implicit operator UnitValue(Single amount)
        {
            return new PercentUnitValue(amount);
        }
        public static implicit operator UnitValue(Int32 value)
        {
            return new PixelUnitValue(value);
        }
        public static implicit operator UnitValue(UnitValue[] values)
        {
            return new NestedUnitValue(values);
        }
        public static UnitValue operator -(UnitValue uv)
        {
            return uv.Flip();
        }
        #endregion
    }
}
