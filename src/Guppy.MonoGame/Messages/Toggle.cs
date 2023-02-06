using Guppy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.MonoGame.Messages
{
    public class Toggle<T> : Message<Toggle<T>>
    {
        public static readonly Toggle<T> Instance = new Toggle<T>();
    }
}
