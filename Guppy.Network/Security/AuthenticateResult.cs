using Guppy.Network.Security.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Security
{
    public class AuthenticateResult
    {
        public readonly AuthenticateResultType Type;
        public readonly String Message;
        public readonly DateTime CreatedAt;

        private AuthenticateResult(AuthenticateResultType type, String message)
        {
            this.Type = type;
            this.Message = message;
            this.CreatedAt = DateTime.Now;
        }

        public static AuthenticateResult Success(String message)
        {
            return new AuthenticateResult(AuthenticateResultType.Success, message);
        }

        public static AuthenticateResult Failure(String message)
        {
            return new AuthenticateResult(AuthenticateResultType.Failure, message);
        }
    }
}
