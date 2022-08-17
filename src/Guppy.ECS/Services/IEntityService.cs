using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Services
{
    public interface IEntityService
    {
        Entity Create(EntityKey key);
        Entity Create(EntityKey key, params object[] components);
        Entity Create(uint keyId);
        Entity Create(uint keyId, params object[] components);
    }
}
