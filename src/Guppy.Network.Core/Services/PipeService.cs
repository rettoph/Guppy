using Guppy.EntityComponent;
using Guppy.EntityComponent.Lists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Network.Services
{
    public sealed class PipeService : ServiceList<Pipe>
    {
        internal Room room { private get; set; }


        #region Helper Methods
        public override bool TryGetById(Guid id, out Pipe item)
        {
            if(!base.TryGetById(id, out item))
            {
                item = this.Create<Pipe>(this.provider, (pipe, _, _) =>
                {
                    pipe.Room = this.room;
                    pipe.SetId(id);
                });
            }

            return true;
        }
        #endregion
    }
}
