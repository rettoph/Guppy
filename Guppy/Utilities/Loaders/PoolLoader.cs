using Guppy.Attributes;
using Guppy.Implementations;
using Guppy.Interfaces;
using Guppy.Utilities;
using Guppy.Utilities.Pools;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Utilities.Loaders
{
    /// <summary>
    /// The pool loader is a loader used to generate
    /// pools of initializable objects automatically. 
    /// Each pool registered via this loader will be 
    /// scoped within the service provider.
    /// </summary>
    [IsLoader]
    public class PoolLoader : Loader<Type, Type, Type>
    {
        internal Dictionary<Type, Type> ValuesTable { get { return this.valuesTable; } }

        public PoolLoader(ILogger logger) : base(logger)
        {
        }

        #region Registration Methods
        public void TryRegister(Type targetType, Type poolType, UInt16 priority = 100)
        {
            ExceptionHelper.ValidateAssignableFrom<Pool>(poolType);

            if (this.registeredValuesList.Where(rc => rc.Handle == targetType && rc.Value == poolType).Count() == 0)
            { // Ensure that this exact pool has not already been registered.
                this.logger.LogTrace($"Registering new Pool<{poolType.Name}>({priority}) => '{targetType.Name}'");

                this.Register(targetType, poolType, priority);
            }
        }
        #endregion

        protected override Type GetOutValue(Type value)
        {
            return value;
        }
    }
}
