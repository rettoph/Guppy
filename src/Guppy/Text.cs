using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy
{
    public class Text : Resource<string?>
    {
        public Text(string key, string? defaultValue) : base(key, defaultValue)
        {
        }

        public override string? Export()
        {
            return this.Value;
        }

        public override void Import(string? value)
        {
            this.Value = value;
        }
    }
}
