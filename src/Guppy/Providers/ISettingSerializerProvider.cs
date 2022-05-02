using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Providers
{
    public interface ISettingSerializerProvider
    {
        bool TryGet<T>([MaybeNullWhen(false)] out SettingSerializer<T> serializer);
    }
}
