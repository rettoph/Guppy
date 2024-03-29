﻿using Guppy.Network.Identity.Services;
using Guppy.Network.Messages;

namespace Guppy.Network.Groups
{
    internal class NotImplementedNetGroup : INetGroup
    {
        public static readonly INetGroup Instance = new NotImplementedNetGroup();

        public byte Id => throw new NotImplementedException();

        public IPeer Peer => throw new NotImplementedException();

        public INetScopeUserService Users => throw new NotImplementedException();

        public INetScope Scope => throw new NotImplementedException();

        byte INetGroup.Id => throw new NotImplementedException();

        IPeer INetGroup.Peer => throw new NotImplementedException();

        INetScopeUserService INetGroup.Users => throw new NotImplementedException();

        INetScope INetGroup.Scope => throw new NotImplementedException();

        private NotImplementedNetGroup()
        {

        }

        public void Attach(INetScope scope)
        {
            throw new NotImplementedException();
        }

        public INetOutgoingMessage<T> CreateMessage<T>(in T body) where T : notnull
        {
            throw new NotImplementedException();
        }

        public void Detach()
        {
            throw new NotImplementedException();
        }

        INetOutgoingMessage<T> INetGroup.CreateMessage<T>(in T body)
        {
            throw new NotImplementedException();
        }

        void INetGroup.Attach(INetScope scope)
        {
            throw new NotImplementedException();
        }

        void INetGroup.Detach()
        {
            throw new NotImplementedException();
        }

        void INetGroup.Process(INetIncomingMessage<UserAction> message)
        {
            throw new NotImplementedException();
        }
    }
}
