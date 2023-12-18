using Guppy.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Common.Services
{
    public interface IObjectTextFilterService
    {
        TextFilterResult Filter(object? instance, string input, int maxDepth = 5, int currentDepth = 0, HashSet<object>? tree = null);
    }
}
