using Guppy.Core.Commands.Common.Attributes;
using System.Reflection;

namespace Guppy.Core.Commands.Common.Contexts
{
    public sealed class OptionContext : IOptionContext
    {
        public string[] Names { get; }
        public string? Description { get; }
        public bool Required { get; }
        public PropertyInfo PropertyInfo { get; }

        private OptionContext(string[] names, string? description, bool required, PropertyInfo propertyInfo)
        {
            this.Names = names;
            this.Description = description;
            this.Required = required;
            this.PropertyInfo = propertyInfo;
        }

        public static IOptionContext[] CreateAll<TCommand>()
        {
            List<IOptionContext> contexts = new List<IOptionContext>();

            IEnumerable<PropertyInfo> propertyInfos = typeof(TCommand).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => x.HasCustomAttribute<OptionAttribute>(true));
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                OptionAttribute optionAttribute = propertyInfo.GetCustomAttribute<OptionAttribute>(true) ?? throw new NotImplementedException();


                IOptionContext context = new OptionContext(optionAttribute.Names, optionAttribute.Description, optionAttribute.Required, propertyInfo);
                contexts.Add(context);
            }

            return contexts.ToArray();
        }
    }
}
