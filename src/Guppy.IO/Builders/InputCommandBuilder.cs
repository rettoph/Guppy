using Guppy.CommandLine.Interfaces;
using Guppy.EntityComponent.DependencyInjection;
using Guppy.IO.Enums;
using Guppy.IO.Services;
using Guppy.IO.Structs;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.IO.Builders
{
    public sealed class InputCommandBuilder
    {
        #region Public Fields
        /// <summary>
        /// Unique identifier
        /// </summary>
        public readonly String Handle;
        #endregion

        #region Public Properties
        /// <summary>
        /// The input the current command should be bound to.
        /// </summary>
        public InputButton InputButton { get; set; }

        /// <summary>
        /// The <see cref="InputButton"/> state to <see cref="ICommandData"/> mapping.
        /// </summary>
        public List<(ButtonState state, ICommandData command)> Commands { get; set; }

        public Boolean Lockable { get; set; }
        #endregion

        #region Constructor
        internal InputCommandBuilder(String handle)
        {
            this.Handle = handle;
            this.Commands = new List<(ButtonState state, ICommandData command)>(2);
        }
        #endregion

        #region SetInputButton Methods
        public InputCommandBuilder SetInputButton(Keys keyboardKey)
        {
            this.InputButton = new InputButton(keyboardKey);

            return this;
        }

        public InputCommandBuilder SetInputButton(MouseButton cursorButton)
        {
            this.InputButton = new InputButton(cursorButton);

            return this;
        }
        #endregion

        #region AddCommand Methods
        public InputCommandBuilder AddCommand(ButtonState state, ICommandData command)
        {
            this.Commands.Add((state, command));

            return this;
        }
        #endregion

        #region SetLockable Methods
        public InputCommandBuilder SetLockable(Boolean value)
        {
            this.Lockable = value;

            return this;
        }
        #endregion

        #region Build Methods
        internal InputCommand Build(ServiceProvider provider)
        {
            return provider.GetService<InputCommand>((inputCommand, _, _) =>
            {
                inputCommand.SetContext(
                    handle: this.Handle,
                    input: this.InputButton,
                    commands: this.Commands.ToDictionary(
                        keySelector: bsc => bsc.state,
                        elementSelector: bsc => bsc.command),
                    lockable: this.Lockable);
            });
        }
        #endregion
    }
}
