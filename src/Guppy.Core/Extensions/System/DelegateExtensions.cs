using Guppy.Extensions.System.Collections;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Extensions.System
{
    public static class DelegateExtensions
    {
        public static void LogInvocationList(this Delegate d, String name, Service service, Int32 skip = 0)
            => d.LogInvocationList($"{service.GetType().GetPrettyName()}<{service.ServiceConfiguration.Key.Name}>({service.Id}).{name}", service.log, skip);

        public static void LogInvocationList(this Delegate d, String name, ILog logger, Int32 skip = 0)
        {
            if(d != default)
            {
                d.GetInvocationList().Skip(skip).ForEach(d => logger.Warn($"{name} => {d.Method.DeclaringType.GetPrettyName()}.{d.Method.Name}"));
            } 
        }
    }
}
