using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Collections
{
    public class DrivenCollection<TDriven> : FrameableCollection<TDriven>
        where TDriven : Driven
    {
        #region Constructor
        public DrivenCollection(IServiceProvider provider) : base(provider)
        {
        }
        #endregion

        #region Collection Methods
        public override bool Add(TDriven item)
        {
            if(base.Add(item))
            {
                item.Events.Add<Creatable>("disposing", this.HandleItemDisposing);

                return true;
            }

            return false;
        }

        public override bool Remove(TDriven item)
        {
            if(base.Remove(item))
            {
                item.Events.Remove<Creatable>("disposing", this.HandleItemDisposing);

                return true;
            }

            return false;
        }
        #endregion

        #region Event Handlers
        private void HandleItemDisposing(object sender, Creatable arg)
        {
            // Auto remove the child on dispose
            this.Remove(sender as TDriven);
        }
        #endregion
    }
}
