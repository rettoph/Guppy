using Guppy.DependencyInjection;
using Guppy.IO.Input.Delegates;
using Guppy.IO.Input.Helpers;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Guppy.IO.Input
{
    public class InputManager : Service
    {
        #region Public Properties
        public InputType Which { get; internal set; }
        public ButtonState State { get; private set; }
        #endregion

        #region Events
        public event OnStateChangedDelegate OnStateChanged;
        public Dictionary<ButtonState, OnStateChangedDelegate> OnState { get; private set; }
        #endregion

        #region Constructors
        internal InputManager()
        {
        }
        #endregion

        #region Lifecyel Methods
        protected override void PreInitialize(ServiceProvider provider)
        {
            base.PreInitialize(provider);

            this.OnState = DictionaryHelper.BuildEnumDictionary<ButtonState, OnStateChangedDelegate>();
        }
        #endregion

        #region Helper Methods
        internal void TrySetState(ButtonState state)
        {
            if(this.State != state)
            {
                this.State = state;
                var args = new InputArgs(this.Which, this.State);

                this.OnStateChanged?.Invoke(this, args);
                this.OnState[state]?.Invoke(this, args);
            }
        }
        #endregion
    }
}
