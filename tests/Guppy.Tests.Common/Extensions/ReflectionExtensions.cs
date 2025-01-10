using System.Reflection;
using System.Reflection.Emit;

namespace Guppy.Tests.Common.Extensions
{
    public static class ReflectionExtensions
    {
        public static AssemblyBuilder DefineDefaultModule(this AssemblyBuilder assemblyBuilder, Action<ModuleBuilder> moduleBuilder)
        {
            ModuleBuilder moduleBuilderInstance = assemblyBuilder.DefineDynamicModule(assemblyBuilder.FullName ?? throw new NotImplementedException());
            moduleBuilder(moduleBuilderInstance);

            return assemblyBuilder;
        }

        public static ModuleBuilder DefineType(this ModuleBuilder moduleBuilder, string typeName, TypeAttributes typeAttributes, Action<TypeBuilder> typeBuilder)
        {
            TypeBuilder typeBuilderInstance = moduleBuilder.DefineType(typeName, typeAttributes);
            typeBuilder(typeBuilderInstance);

            return moduleBuilder;
        }
    }
}