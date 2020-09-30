using Guppy.Interfaces;
using Guppy.UI.Delegates;
using Guppy.UI.Enums;

namespace Guppy.UI.Interfaces
{
    public interface IElement : IService, IFrameable
    {
        #region Properties
        ElementState State { get; }
        #endregion

        #region Events
        event OnStateChangedDelegate OnStateChanged;
        #endregion
    }
}
