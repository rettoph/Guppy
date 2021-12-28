using Guppy.CommandLine.Interfaces;
using Guppy.EntityComponent.DependencyInjection;
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
    public sealed class InputCommandServiceBuilder
    {
        #region Private Fields
        private List<InputCommandBuilder> _inputCommands = new List<InputCommandBuilder>();
        #endregion

        #region RegisterInputCommand Methods
        public InputCommandBuilder RegisterInputCommand(String handle)
        {
            InputCommandBuilder inputCommand = new InputCommandBuilder(handle);
            _inputCommands.Add(inputCommand);

            return inputCommand;
        }
        #endregion

        #region Build Methods
        internal InputCommandService Build(ServiceProvider provider)
        {
            Dictionary<String, InputCommand> inputCommands = _inputCommands.Select(icb => icb.Build(provider)).ToDictionaryByValue(keySelector: ic => ic.Handle);

            return new InputCommandService(inputCommands);
        }
        #endregion
    }
}
