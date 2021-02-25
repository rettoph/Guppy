using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.LayerGroups
{
    public abstract class LayerGroup
    {
        /// <summary>
        /// Detect if the current layer group contains the
        /// recieved group value.
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public abstract Boolean Contains(Int32 group);

        /// <summary>
        /// Detect if there is any overlap between the
        /// current layer group and the recieved layer group.
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public abstract Boolean Overlap(LayerGroup group);

        /// <summary>
        /// Return a single value that falls within the 
        /// current LayerGroup. This will be the deafault
        /// value assigned to entities when they are added
        /// directly into a Layer's EntityList.
        /// </summary>
        /// <returns></returns>
        public abstract Int32 GetValue();
    }
}
