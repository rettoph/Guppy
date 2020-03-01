using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.Interfaces
{
    public interface IScene : IAsyncable
    {
        IServiceScope Scope { get; set; }
    }
}
