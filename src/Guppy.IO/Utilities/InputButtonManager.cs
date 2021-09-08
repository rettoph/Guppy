using Guppy.DependencyInjection;
using Guppy.Events.Delegates;
using Guppy.IO.Structs;
using Guppy.IO.Utilities;
using Guppy.Utilities;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Services
{
    public class InputButtonManager : Service
    {
        #region Public Properties
        public InputButton Which { get; internal set; }
        public ButtonState State { get; private set; }
        #endregion

        #region Events
        public event OnEventDelegate<InputButtonManager, InputButtonArgs> OnStateChanged;
        public Dictionary<ButtonState, OnEventDelegate<InputButtonManager, InputButtonArgs>> OnState { get; private set; }
        #endregion

        #region Constructors
        internal InputButtonManager()
        {
        }
        #endregion

        #region Lifecyel Methods
        protected override void PreInitialize(GuppyServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.OnState = DictionaryHelper.BuildEnumDictionary<ButtonState, OnEventDelegate<InputButtonManager, InputButtonArgs>>();
        }
        #endregion

        #region Helper Methods
        internal void TrySetState(ButtonState state)
        {
            if (this.State != state)
            {
                this.State = state;
                var args = new InputButtonArgs(this.Which, this.State);

                this.OnStateChanged?.Invoke(this, args);
                this.OnState[state]?.Invoke(this, args);
            }
        }
        #endregion
    }
}
