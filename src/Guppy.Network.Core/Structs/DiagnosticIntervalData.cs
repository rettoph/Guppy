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

        public readonly UInt32 TotalFlushed;
        public readonly UInt32 TotalSent;
        public readonly UInt32 TotalRecieved;

        public DiagnosticIntervalData(uint flushed, uint sent, uint read, uint totalFlushed, uint totalSent, uint totalRecieved)
        {
            this.Flushed = flushed;
            this.Sent = sent;
            this.Recieved = read;

            this.TotalFlushed = totalFlushed;
            this.TotalSent = totalSent;
            this.TotalRecieved = totalRecieved;
        }

        public override string ToString()
        {
            return $"Flushed: {this.Flushed}, Sent: {this.Sent}, Read: {this.Recieved}";
        }
    }
}
