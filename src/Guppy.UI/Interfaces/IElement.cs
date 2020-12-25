using Guppy.Interfaces;
using Guppy.IO.Services;
using Guppy.UI.Delegates;
using Guppy.UI.Enums;
using Guppy.UI.Utilities;
using Guppy.UI.Utilities.Units;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Guppy.UI.Interfaces
{
    public interface IElement : IService, IFrameable
    {
        #region Properties
        /// <summary>
        /// The current interaction state of the element.
        /// </summary>
        ElementState State { get; }

        /// <summary>
        /// The element's outer bounds.
        /// </summary>
        Bounds Bounds { get; }

        /// <summary>
        /// The element's internal padding.
        /// </summary>
        Padding Padding { get; }

        /// <summary>
        /// The local pixel bounds of the outer element.
        /// </summary>
        Rectangle OuterBounds { get; }

        /// <summary>
        /// The local pixel bounds of the inner element.
        /// Calculated with the OuterBounds and Padding.
        /// </summary>
        Rectangle InnerBounds { get; }

        /// <summary>
        /// The current container.
        /// This is used to calculate the
        /// current bounding box.
        /// </summary>
        IContainer Container { get; set; }
        #endregion

        #region Events
        /// <summary>
        /// Invoked when the state value is changed in any way.
        /// </summary>
        event OnStateChangedDelegate OnStateChanged;

        /// <summary>
        /// Allows the binding of specific element state change events.
        /// </summary>
        Dictionary<ElementState, OnStateChangedDelegate> OnState { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Attempt to clean the current element.
        /// 
        /// This will refresh the inner & outer based on the recieved
        /// <paramref name="bounds"/> value.
        /// </summary>
        /// <param name="bounds"></param>
        void TryCleanBounds();

        /// <summary>
        /// Check & update the hovered value of the element specifically.
        /// </summary>
        void TryCleanHovered();
        #endregion
    }
}
