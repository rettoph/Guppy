using Guppy.Core.Commands.Common.Attributes;
using Guppy.Core.Commands.Common.Extensions;
using System.Reflection;

namespace Guppy.Core.Commands.Common.Contexts
{
    public sealed class ArgumentContext : IArgumentContext
    {
        public string Name { get; }
        public string? Description { get; }
        public PropertyInfo PropertyInfo { get; }

        private ArgumentContext(string? name, string? description, PropertyInfo propertyInfo)
        {
            this.Name = name ?? propertyInfo.Name.ToArgumentName();
            this.Description = description;
            this.PropertyInfo = propertyInfo;
        }

        public static IArgumentContext[] CreateAll<TCommand>()
            where TCommand : ICommand
        {
            List<IArgumentContext> contexts = new List<IArgumentContext>();

            IEnumerable<PropertyInfo> propertyInfos = typeof(TCommand).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => x.HasCustomAttribute<ArgumentAttribute>(true));
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                ArgumentAttribute optionAttribute = propertyInfo.GetCustomAttribute<ArgumentAttribute>(true) ?? throw new NotImplementedException();


                IArgumentContext context = new ArgumentContext(optionAttribute.Name, optionAttribute.Description, propertyInfo);
                contexts.Add(context);
            }

            return contexts.ToArray();
        }
    }
}
