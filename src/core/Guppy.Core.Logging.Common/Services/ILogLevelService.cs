using Guppy.Core.Logging.Common.Enums;

namespace Guppy.Core.Logging.Common.Services
{
    public interface ILogLevelService
    {
        LogLevelEnum GetLogLevel(Type context);
    }
}
