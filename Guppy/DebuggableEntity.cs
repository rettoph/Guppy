using System;
using System.Collections.Generic;
using System.Text;
using Guppy.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework.Graphics;

namespace Guppy
{
    /// <summary>
    /// Special entity that have debuggable attributes
    /// such as vertice drawing
    /// </summary>
    public abstract class DebuggableEntity : Entity
    {
        public DebuggableEntity(EntityConfiguration configuration, Scene scene, IServiceProvider provider, ILogger logger) : base(configuration, scene, provider, logger)
        {
        }

        public DebuggableEntity(Guid id, EntityConfiguration configuration, Scene scene, IServiceProvider provider, ILogger logger) : base(id, configuration, scene, provider, logger)
        {
        }

        public abstract void AddDebugVertices(ref List<VertexPositionColor> vertices);
    }
}
