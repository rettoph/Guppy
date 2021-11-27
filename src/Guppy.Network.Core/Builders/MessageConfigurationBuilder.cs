using Guppy.Network.Configurations;
using System;

namespace Guppy.Network.Builders
{
    public class MessageConfigurationBuilder : DataConfigurationBuilder<MessageConfigurationBuilder, MessageConfiguration>
    {
        #region Private Fields
        private String _name;
        #endregion

        #region Public Properties
        public String Name
        {
            get => _name;
            set => this.SetName(value);
        }
        #endregion

        #region SeName Methods
        public MessageConfigurationBuilder SetName(String name)
        {
            _name = name;

            return this;
        }
        #endregion

        protected override MessageConfiguration Build(byte[] idBytes)
        {
            return new MessageConfiguration(
                this.Id.Value,
                idBytes,
                this.Name,
                this.DataType,
                this.DataWriter,
                this.DataReader);
        }
    }
}
