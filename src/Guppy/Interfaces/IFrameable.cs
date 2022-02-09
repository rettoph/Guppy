using Guppy.EntityComponent.Interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public delegate void Step(GameTime gameTime);

    public interface IFrameable : IEntity
    {
        Boolean Visible { get; set; }
        Boolean Enabled { get; set; }

        /// <summary>
        /// Delegate invoked each frame immidiately before OnDraw.
        /// </summary>
        event Step OnPreDraw;
        /// <summary>
        /// The frame's primary Draw Delegate.
        /// </summary>
        event Step OnDraw;
        /// <summary>
        /// Delegate invoked each frame immidiately after OnDraw
        /// </summary>
        event Step OnPostDraw;

        /// <summary>
        /// Delegate invoked each frame immidiately before OnUpdate.
        /// </summary>
        event Step OnPreUpdate;
        /// <summary>
        /// The frame's primary Update Delegate.
        /// </summary>
        event Step OnUpdate;
        /// <summary>
        /// Delegate invoked each frame immidiately after OnUpdate
        /// </summary>
        event Step OnPostUpdate;

        public event OnEventDelegate<IFrameable, Boolean> OnVisibleChanged;
        public event OnEventDelegate<IFrameable, Boolean> OnEnabledChanged;

        /// <summary>
        /// Public method used to begin the Draw delegate invocation.
        /// </summary>
        /// <param name="gameTime"></param>
        void TryDraw(GameTime gameTime);


        /// <summary>
        /// Public method used to begin the Update delegate invocation.
        /// </summary>
        /// <param name="gameTime"></param>
        void TryUpdate(GameTime gameTime);
    }
}
