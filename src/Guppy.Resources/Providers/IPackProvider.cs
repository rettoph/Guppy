using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources.Providers
{
    public interface IPackProvider
    {
        IEnumerable<Pack> GetAll();
        Pack GetById(Guid id);
    }
}
