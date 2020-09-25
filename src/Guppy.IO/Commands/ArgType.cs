using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.IO.Commands
{
    public sealed class ArgType
    {
        #region Static Fields
        public static readonly ArgType Boolean;
        public static readonly ArgType String;
        public static readonly ArgType Int32;
        public static readonly ArgType Single;
        #endregion

        public readonly Func<String, Object> Parse;

        #region Constructor
        public ArgType(Func<String, Object> parse)
        {
            this.Parse = parse;
        }
        #endregion

        #region Static Methods
        static ArgType()
        {
            ArgType.String = new ArgType((s) => s);
            ArgType.Boolean = new ArgType((s) => bool.Parse(s));
            ArgType.Int32 = new ArgType((s) => int.Parse(s));
            ArgType.Single = new ArgType((s) => float.Parse(s));
        }
        #endregion
    }
}
