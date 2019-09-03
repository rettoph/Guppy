using Guppy.Loaders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Loaders
{
    /// <summary>
    /// Simple loader used to bind a string to a group.
    /// This is used when selecting a random entity based
    /// on the entity type. For instance, the handle of ship-part:hull
    /// would have entity:shi-part:hull:square and entity:shi-part:hull:triangle
    /// bound to it.
    /// 
    /// When the request random function is called, a random string bound to
    /// the input handle will be returned.
    /// </summary>
    public class RandomStringLoader : Loader<String, String, String[]>
    {
        private Dictionary<String, List<String>> _registeredValues;

        public RandomStringLoader(ILogger logger) : base(logger)
        {
            _registeredValues = new Dictionary<String, List<String>>();
        }

        public void TryRegister(string handle, string value, ushort priority = 100)
        {
            if (!_registeredValues.ContainsKey(handle))
                _registeredValues[handle] = new List<string>();

            _registeredValues[handle].Add(value);
        }

        public String GetRandomValue(String handle, Random rand)
        {
            return this.values[handle].ElementAt(rand.Next(this.values[handle].Length));
        }

        protected override string[] BuildOutput(IGrouping<string, RegisteredValue> registeredValues)
        {
            return registeredValues.Select(rv => rv.Value).ToArray();
        }
    }
}
