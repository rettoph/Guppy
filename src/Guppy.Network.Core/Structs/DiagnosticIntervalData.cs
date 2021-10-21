using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Network.Structs
{
    public struct DiagnosticIntervalData
    {
        public readonly UInt32 Flushed;
        public readonly UInt32 Sent;
        public readonly UInt32 Recieved;

        public DiagnosticIntervalData(uint flushed, uint sent, uint read)
        {
            this.Flushed = flushed;
            this.Sent = sent;
            this.Recieved = read;
        }

        public override string ToString()
        {
            return $"Flushed: {this.Flushed}, Sent: {this.Sent}, Read: {this.Recieved}";
        }
    }
}
