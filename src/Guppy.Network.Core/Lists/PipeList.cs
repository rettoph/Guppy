using Guppy.Network.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Lists
{
    public class PipeList : BaseNetworkList<IPipe>
    {
        #region Create Methods
        /// <summary>
        /// Get or create a new <see cref="IPipe"/> with the given <paramref name="id"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IPipe GetOrCreate(Guid id)
        {
            var lane = this.GetById(id);
            lane ??= this.Create<IPipe>(this.provider, (lane, p, c) =>
            {
                lane.Id = id;
            });

            return lane;
        }
        #endregion
    }
}
