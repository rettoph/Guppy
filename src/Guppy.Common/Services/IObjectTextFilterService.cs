using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Services
{
    public interface IObjectTextFilterService
    {
        bool Filter(object? instance, string input, int maxDepth = 5, int currentDepth = 0);
    }
}
