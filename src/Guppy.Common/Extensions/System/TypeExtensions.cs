﻿using Guppy.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Recursively return all <see cref="Type"/> ancestors
        /// between a given child and parent type (inclusive)
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetAncestors(this Type child, Type parent)
        {
            ThrowIf.Type.IsNotAssignableFrom(parent, child);

            Type? type = child;

            if (child == parent)
            {
                yield return child;
            }
            if (parent.IsInterface)
            { // Recersively add types until the interface is no longer implemented...
                while (type is not null && type.GetInterfaces().Contains(parent))
                {
                    yield return type;
                    type = type.BaseType;
                }

                yield return parent;
            }
            else
            { // return types until the base type is hit...
                while (parent.IsAssignableFrom(type))
                {
                    yield return type;
                    type = type.BaseType;
                }
            }
        }

        public static IEnumerable<Type> GetAncestors<TParent>(this Type child)
        {
            return child.GetAncestors(typeof(TParent));
        }

        public static IEnumerable<Type> AssignableFrom(this IEnumerable<Type> types, Type parent)
        {
            return types.Where(t => parent.IsAssignableFrom(t));
        }

        public static IEnumerable<Type> AssignableFrom<TParent>(this IEnumerable<Type> types)
        {
            return types.AssignableFrom(typeof(TParent));
        }

        public static IEnumerable<Type> WithAttribute(this IEnumerable<Type> types, Type attribute, bool inherit)
        {
            ThrowIf.Type.IsNotAssignableFrom<Attribute>(attribute);


            return types.Where(t =>
            {
                var info = t.GetCustomAttributes(attribute, inherit);
                return info != null && info.Length > 0;
            });
        }

        public static IEnumerable<Type> WithAttribute<TAttribute>(this IEnumerable<Type> types, bool inherit)
            where TAttribute : Attribute
        {
            return types.WithAttribute(typeof(TAttribute), inherit);
        }
    }
}