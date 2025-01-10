using System;
using System.Linq;
using System.Reflection;
using Guppy.Game.ImGui.Common.ImGuiNETSourceGenerator.TypeManagers;

namespace Guppy.Game.ImGui.Common.ImGuiNETSourceGenerator
{
    public static class TypeDecoratorHelper
    {
        private static void AddMethodDecorations(CodeBuilder source, string invoker, MethodInfo[] methods)
        {
            foreach (MethodInfo method in methods.Where(x => x.IsGenericMethod == false))
            {
                TypeManager returnType = TypeManager.GetTypeManager(method.ReturnType);

                (ParameterInfo parameter, TypeManager manager)[] parameterManagers = method.GetParameters()
                    .Select(x => (x, TypeManager.GetTypeManager(x.ParameterType)))
                    .ToArray();

                string guppyMethodParameters = string.Join(", ", parameterManagers.Select(x => $"{x.parameter.GetPrefix()}{x.manager.GuppyParameterType} {x.parameter.Name.Sanitize()}"));
                string imguiMethodParameters = string.Join(", ", parameterManagers.Select(x => $"{x.parameter.GetPrefix()}{x.manager.GetGuppyToImGuiConverter(x.parameter.Name.Sanitize())}"));

                string methodModifiers = GetMethodModifiers(method, typeof(object));

                using (source.Section($"{methodModifiers} {returnType.ReturnTypeName} {method.Name}({guppyMethodParameters})"))
                {
                    if (method.ReturnType.IsVoid())
                    {
                        source.AppendLine($"{invoker}.{method.Name}({imguiMethodParameters});");
                    }
                    else
                    {
                        string returnPrefix = (method.ReturnType.IsByRef ? "ref " : "");
                        source.AppendLine($"{returnPrefix}{returnType.ImGuiType.FullName.Sanitize()} result = {invoker}.{method.Name}({imguiMethodParameters});");
                        source.AppendLine($"return {returnPrefix}{returnType.GetImGuiToGuppyConverter("result")};");
                    }
                }
            }
        }

        public static void AddStaticMethodDecorations(Type type, CodeBuilder source)
        {
            MethodInfo[] methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public);

            AddMethodDecorations(source, type.FullName, methods);
        }

        public static void AddPublicMethodDecorations(Type type, string instance, CodeBuilder source)
        {
            MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.IsSpecialName == false && x.IsPrivate == false).ToArray();

            AddMethodDecorations(source, instance, methods);
        }

        public static void AddFieldDecorations(Type type, string instance, CodeBuilder source)
        {
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.FieldType.IsGenericType == false).ToArray();

            foreach (FieldInfo field in fields)
            {
                AddDecoratingProperty(TypeManager.GetTypeManager(field.FieldType), field.Name, true, field.IsInitOnly, instance, source);
            }
        }

        public static void AddPropertyDecorations(Type type, string instance, CodeBuilder source)
        {
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.PropertyType.IsGenericType == false).ToArray();

            foreach (PropertyInfo property in properties)
            {
                AddDecoratingProperty(TypeManager.GetTypeManager(property.PropertyType), property.Name, property.GetGetMethod() != null, property.GetSetMethod() != null, instance, source);
            }
        }

        private static void AddDecoratingProperty(TypeManager type, string name, bool getter, bool setter, string instance, CodeBuilder source)
        {
            string typePrefix = (type.ImGuiType.IsByRef ? "ref " : "");

            using (source.Section($"public {typePrefix}{type.GuppyParameterType} {name}"))
            {
                if (getter == true)
                {
                    using (source.Section("get"))
                    {

                        source.AppendLine($"return {typePrefix}{type.GetImGuiToGuppyConverter($"{instance}.{name}")};");
                    }
                }

                if (setter == true)
                {
                    throw new NotImplementedException();
                }
            }
        }

        private static string GetMethodModifiers(MethodInfo method, Type baseType)
        {
            string modifiers = string.Empty;

            if (method.IsPublic == true)
            {
                modifiers += "public unsafe";
            }
            else
            {
                modifiers += "protected unsafe";
            }

            MethodInfo baseMethod = baseType.GetMethod(method.Name, method.GetParameters().Select(x => x.ParameterType).ToArray());
            if ((baseMethod is null) == false)
            {
                modifiers += baseMethod.IsVirtual ? " override " : " new";
            }

            return modifiers;
        }
    }
}