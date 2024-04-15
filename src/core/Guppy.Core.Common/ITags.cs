namespace Guppy.Core.Common
{
    public interface ITags
    {
        bool IsRoot { get; }

        bool Has(object tag);
    }
}
