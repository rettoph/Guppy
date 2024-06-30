namespace Guppy.Core.Common.Enums
{
    [Flags]
    public enum ServiceRegistrationFlags
    {
        None = 0,
        RequireAutoLoadAttribute = 1,
        AsSelf = 2,
        AsImplementedInterfaces = 4
    }
}
