using Guppy.Utilities.Delegaters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    /// <summary>
    /// Unique objects are objects that contain
    /// a unique id, can invoke custom events,
    /// and can be disposed of.
    /// </summary>
    public interface IUnique : IDisposable
    {
        #region Attributes
        Guid Id { get; }
        EventDelegater Events { get; }
        #endregion

        #region Lifecycle Methods
        void TryCreate(IServiceProvider provider);
        #endregion

        #region Helper Methods
        void SetId(Guid id);
        #endregion
    }
}
