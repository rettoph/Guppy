using Guppy.IO.Commands.Services;
using System;
using System.Collections.Generic;
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
        #endregion

        #region Private Fields
        private readonly Func<String, Object> _parser;
        #endregion

        #region Public Fields
        public readonly String Name;
        public readonly String Description;
        #endregion

        #region Constructors
        public ArgType(String name, String description, Func<String, Object> parser)
        {
            this.Name = name;
            this.Description = description;
            this._parser = parser;
        }

        public Object Parse(String value)
        {
            try
            {
                return _parser(value);
            }
            catch (Exception e)
            {
                throw new ArgumentOutOfRangeException($"Unexpected {this.Name} value '{value}'.", e);
            }
        }

        static ArgType()
        {
            ArgType.String = new ArgType("String", "Any combination of characters.", s => s);
            ArgType.Boolean = new ArgType("Boolean", "'true' or 'false' expected.", s => bool.Parse(s));
            ArgType.Int32 = new ArgType("Int32", "Any whole number allowed.", s => int.Parse(s));
            ArgType.Single = new ArgType("Single", "Any number expected.", s => float.Parse(s));
        }
        #endregion
    }
}
