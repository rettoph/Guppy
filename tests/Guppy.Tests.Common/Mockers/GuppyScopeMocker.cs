using Guppy.Core.Common;

namespace Guppy.Tests.Common.Mockers
{
    public class GuppyScopeMocker : Mocker<GuppyScopeMocker, IGuppyScope>
    {
        public GuppyScopeMocker SetupResolve<T>(Func<T> service)
            where T : class
        {
            this.SetupReturn(x => x.Resolve<T>(), () => service());
            return this;
        }
    }
}
