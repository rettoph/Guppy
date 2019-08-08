using Guppy.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Attributes
{
    /// <summary>
    /// Mark a given type as an active authenticator
    /// for validating a new incoming user connection
    /// request.
    /// </summary>
    public class IsAuthenticatorAttribute : GuppyAttribute
    {
    }
}
