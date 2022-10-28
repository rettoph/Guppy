using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.ECS.Services
{
    public interface IWorldService
    {
        World Instance { get; }
    }
}
