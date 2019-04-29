using Guppy.Network;
using Guppy.Network.Interfaces;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Network.Extensions.Lidgren;

namespace Pong.Library
{
    public class ChatMessage : NetworkObject
    {
        public String Message { get; private set; }
        public String Sender { get; private set; }
        public Color Color { get; private set; }

        public ChatMessage(String message, String sender, Color color)
        {
            this.Message = message;
            this.Sender = sender;
            this.Color = color;
        }
        public ChatMessage(Guid id) : base(id)
        {
        }

        public override void Read(NetIncomingMessage im)
        {
            base.Read(im);
        }
        public override void Write(NetOutgoingMessage om)
        {
            base.Write(om);

            om.Write(this.Message);
            om.Write(this.Sender);
            om.Write(this.Color);
        }
    }
}
