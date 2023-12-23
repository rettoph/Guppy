namespace Guppy.Common
{
    public interface IState
    {
        bool Matches(object? value);
    }
}
