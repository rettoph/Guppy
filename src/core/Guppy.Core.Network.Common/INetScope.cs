namespace Guppy.Core.Network.Common
{
    public interface INetScope
    {
        INetGroup Group { get; }

        /// <summary>
        /// Automatically create and enqueue a message to be sent
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="body"></param>
        /// <returns></returns>
        INetOutgoingMessage<T> CreateMessage<T>(in T body)
            where T : notnull;


        void Enqueue(INetIncomingMessage message);
        void Enqueue(INetOutgoingMessage message);
    }

    public interface INetScope<T> : INetScope
    {

    }
}