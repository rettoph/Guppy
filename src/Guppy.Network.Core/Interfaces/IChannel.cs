using Guppy.Interfaces;
using Guppy.Network.Lists;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Interfaces
{
    public interface IChannel : IPipe
    {
        #region Properties
        /// <summary>
        /// A short id used internally to differentiate
        /// specific channels.
        /// </summary>
        new Int16 Id { get; internal set; }

        /// <summary>
        /// The current user pipe.
        /// </summary>
        PipeList Pipes { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Update the current <see cref="IChannel"/> 
        /// </summary>
        void TryUpdate();
        #endregion
    }
}
