﻿namespace Guppy.Core.Network.Common.Identity.Enums
{
    public enum ClaimAccessibility
    {
        Public, // Sent to all peers...
        Protected, // Only sent to the connected peer...
        Private, // Not sent to any peers...
    }
}