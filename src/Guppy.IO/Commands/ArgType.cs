using Guppy.IO.Commands.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Guppy.IO.Commands
{
    public class ArgType
    {
        #region Static Fields
        public static readonly ArgType String;
        public static readonly ArgType Boolean;
        public static readonly ArgType Int32;
        public static readonly ArgType Single;
        public static readonly ArgType Byte;
        #endregion

        #region Private Fields
        private readonly Func<String, Object> _parser;
        #endregion

        #region Public Fields
        public readonly String Name;
        private readonly String[] Whitelist;
        #endregion

        #region Constructors
        public ArgType(String name, Func<String, Object> parser, params String[] whitelist)
        {
            _parser = parser;

            this.Name = name;
            this.Whitelist = whitelist;
        }

        public Object Parse(String value)
        {
            try
            {
                if(!this.Whitelist.Any() || this.Whitelist.Contains(value))
                    return _parser(value);
                else
                    throw new ArgumentOutOfRangeException($"Unexpected {this.Name} value.");
            }
            catch (Exception e)
            {
                throw new ArgumentOutOfRangeException($"Unexpected {this.Name} value.", e);
            }
        }

        static ArgType()
        {
            ArgType.String = new ArgType("String", s => s);
            ArgType.Boolean = new ArgType("Boolean", s => bool.Parse(s));
            ArgType.Int32 = new ArgType("Int32", s => int.Parse(s));
            ArgType.Single = new ArgType("Single", s => float.Parse(s));
            ArgType.Byte = new ArgType("Byte", s => byte.Parse(s));
        }
        #endregion
    }
}
