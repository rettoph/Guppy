using Guppy.Interfaces;
using Guppy.UI.Enums;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.UI.Components.Interfaces
{
    /// <summary>
    /// The root most bare bones information required for any
    /// type of element. (Stage, Container, or Child)
    /// </summary>
    public interface IBaseElement
    {
        Boolean Dirty { get; set; }

        /// <summary>
        /// The current hover state.
        /// </summary>
        Boolean Hovered { get; }

        /// <summary>
        /// Returns the current element's bounds.
        /// </summary>
        /// <returns></returns>
        Rectangle GetBounds();

        /// <summary>
        /// Attempt to clean the current element.
        /// This will also call mark all internal
        /// children as dirty.
        /// </summary>
        /// <param name="force"></param>
        void TryClean(Boolean force = false);
    }
}
