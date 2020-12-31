using Guppy.Extensions.System;
using Guppy.IO.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands
{
    public class CommandResponse
    {
        #region Public Fields
        public static CommandResponse Empty = default;

        public readonly CommandResponseType Type;
        public readonly String Message;
        public readonly Exception Exception;
        #endregion

        #region Constructors
        public CommandResponse(CommandResponseType type, String message = null, Exception exception = null)
        {
            this.Type = type;
            this.Message = message;
            this.Exception = exception;
        }
        #endregion

        public override string ToString()
            => $"{this.Message}{(this.Exception?.ToString().AddLeft('\n'))}";

        #region Static Helper Methods
        public static CommandResponse Success(String message = null)
            => new CommandResponse(CommandResponseType.Success, message);

        public static CommandResponse Debug(String message)
            => new CommandResponse(CommandResponseType.Debug, message);

        public static CommandResponse Error(String message = null, Exception exception = null)
            => new CommandResponse(CommandResponseType.Error, message, exception);

        public static CommandResponse Warning(String message = null, Exception exception = null)
            => new CommandResponse(CommandResponseType.Warning, message, exception);
        #endregion
    }
}
