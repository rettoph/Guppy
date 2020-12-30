using Guppy.Extensions.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guppy.Extensions.System
{
    public static class DelegateExtensions
    {
        public static Action<String> LogInvocation { get; set; } = s => Console.WriteLine(s);

        public static void LogInvocationList(this Delegate d, String name, Int32 skip = 0)
        {
            if(d != default)
            {
                var c = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                d.GetInvocationList().Skip(skip).ForEach(d => DelegateExtensions.LogInvocation($"{name} => {d.Method.DeclaringType.GetPrettyName()}.{d.Method.Name}"));
                Console.ForegroundColor = c;
            } 
        }
    }
}
