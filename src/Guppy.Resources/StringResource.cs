using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Resources
{
    public class StringResource : Resource<string>
    {
        public StringResource(string name, string value) : base(name, value)
        {
        }
    }
}
