using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy.Client.Entities
{
    /// <summary>
    /// Debuggable entities
    /// </summary>
    public abstract class DebuggableEntity : Entity
    {
        public DebuggableEntity(EntityConfiguration configuration, Scene scene, ILogger logger) : base(configuration, scene, logger)
        {
        }

        public DebuggableEntity(Guid id, EntityConfiguration configuration, Scene scene, ILogger logger) : base(id, configuration, scene, logger)
        {
        }

        public abstract void AddVertices(ref List<VertexPositionColor> vertices);
    }
}
