using Guppy.Attributes;

namespace Guppy.Resources.ResourceTypes
{
    [AutoLoad]
    internal class StringResourceType : SimpleResourceType<string>
    {
        protected override bool TryResolve(Resource<string> resource, string root, string input, out string value)
        {
            value = input;
            return true;
        }
    }
}
