using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Collections;

namespace Guppy.Utilities.ObjectDumper
{
    /// <summary>
    /// Simple helper class used to dump an object's
    /// members.
    /// </summary>
    public static class ObjectDumper
    {
        #region Enums
        public enum DumpSegment
        {
            Scaffold,
            Name,
            MemberType,
            Type,
            Value,
            Children,
            Exception
        }
        #endregion

        #region Private Classes
        public class TypeInfo
        {
            public readonly Type Type;
            public readonly String PrettyName;
            public readonly Dictionary<MemberTypes, MemberInfo[]> Members;

            internal TypeInfo(Type type)
            {
                this.Type = type;
                this.PrettyName = type.GetPrettyName();
                this.Members = DictionaryHelper.BuildEnumDictionary<MemberTypes, MemberInfo[]>(
                    with: this.Type.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                        .Where(mi => !mi.IsDefined(typeof(CompilerGeneratedAttribute)))
                        .GroupBy(m => m.MemberType)
                        .ToDictionary(
                            keySelector: g => g.Key,
                            elementSelector: g => g.ToArray()),
                    fallback: new MemberInfo[0]);
            }
        }
        #endregion

        #region Private Fields
        private static Dictionary<Type, TypeInfo> _typeInfoCache;
        #endregion

        #region Constructor
        static ObjectDumper()
        {
            _typeInfoCache = new Dictionary<Type, TypeInfo>();
        }
        #endregion

        #region Helper Methods
        public static void Dump(
            Action<String, DumpSegment> writer, 
            Object instance, 
            Int32 depth, 
            MemberTypes memberTypes = MemberTypes.Field | MemberTypes.Property, 
            params Type[] blacklist)
        {
            lock (instance)
            {
                var memberTypesArray = memberTypes.GetFlags().ToArray();
                var info = GetTypeInfo(instance.GetType());
                var cachedInstances = new HashSet<Object>();
                cachedInstances.Add(instance);

                foreach (MemberTypes memberType in memberTypesArray)
                {
                    WriteMemberDataRecersiveEntryPoint(
                        memberType,
                        info.Members[memberType],
                        writer,
                        instance,
                        0,
                        depth,
                        memberTypesArray,
                        "   ",
                        cachedInstances,
                        blacklist);
                }
            }
        }

        private static void WriteMemberDataRecersive(
            Action<String, DumpSegment> writer, 
            MemberInfo member,
            TypeInfo info,
            Func<Object> instanceGetter,
            Int32 position,
            Int32 depth,
            MemberTypes[] memberTypes,
            String indentation,
            HashSet<Object> cachedInstances,
            Type[] blacklist)
        {
            WriteMemberName(writer, member, info, position, indentation, blacklist);
            WriteDataRecursive(writer, info, instanceGetter, position, depth, memberTypes, indentation, cachedInstances, blacklist);
        }

        private static void WriteDataRecursive(
            Action<String, DumpSegment> writer,
            TypeInfo info,
            Func<Object> instanceGetter,
            Int32 position,
            Int32 depth,
            MemberTypes[] memberTypes,
            String indentation,
            HashSet<Object> cachedInstances,
            Type[] blacklist)
        {
            try
            {
                // Attempt to load the type info...
                var instance = instanceGetter();

                if (instance == null)
                {
                    writer("null", DumpSegment.Value);
                    return;
                }
                else if (
                    position >= depth || 
                    info.Type.IsPrimitive || 
                    typeof(IFormattable).IsAssignableFrom(info.Type) || typeof(IConvertible).IsAssignableFrom(info.Type) ||
                    blacklist.Contains(info.Type)
                )
                {
                    writer($"{instance}", DumpSegment.Value);
                    return;
                }
                else if(cachedInstances.Contains(instance))
                {
                    writer($"Recursion detected.", DumpSegment.Value);
                }
                else
                {
                    cachedInstances.Add(instance);
                    writer($"\n{indentation.Duplicate(position)}{{", DumpSegment.Scaffold);
                    foreach (MemberTypes memberType in memberTypes)
                        WriteMemberDataRecersiveEntryPoint(
                            memberType,
                            info.Members[memberType],
                            writer,
                            instance,
                            position + 1,
                            depth,
                            memberTypes,
                            indentation,
                            cachedInstances,
                            blacklist);

                    if (instance is IEnumerable enumerable)
                    {
                        writer($"\n{indentation.Duplicate(position + 1)}Items => [", DumpSegment.Children);
                        var count = 0;
                        var nullStreak = 0;
                        foreach (Object child in enumerable)
                        {
                            if(child == null)
                                nullStreak++;
                            else
                            {
                                if(nullStreak > 0)
                                { // Render the nullstreak and reset
                                    writer($"[{count-nullStreak} - {count-1}]", DumpSegment.Children);
                                    writer($" => ", DumpSegment.Scaffold);
                                    writer($"null", DumpSegment.Value);
                                    nullStreak = 0;
                                }

                                writer($"\n{indentation.Duplicate(position + 2)}[{count}]", DumpSegment.Children);
                                writer($" ({GetTypeInfo(child.GetType()).PrettyName})", DumpSegment.Type);
                                writer($" => ", DumpSegment.Scaffold);

                                WriteDataRecursive(
                                    writer,
                                    GetTypeInfo(child.GetType()),
                                    () => child,
                                    position + 2,
                                    depth,
                                    memberTypes,
                                    indentation,
                                    cachedInstances,
                                    blacklist);
                                count++;
                            }
                        }
                        writer($"\n{indentation.Duplicate(position + 1)}]", DumpSegment.Children);
                    }

                    writer($"\n{indentation.Duplicate(position)}}}", DumpSegment.Scaffold);
                    cachedInstances.Remove(instance);
                }
            }
            catch (Exception e)
            {
                writer(e.Message, DumpSegment.Exception);
            }
        }

        private static void WriteMemberDataRecersiveEntryPoint(
            MemberTypes membersType, 
            MemberInfo[] members,
            Action<String, DumpSegment> writer,
            Object instance,
            Int32 position,
            Int32 depth,
            MemberTypes[] memberTypes,
            String indentation,
            HashSet<Object> cachedInstances,
            Type[] blacklist)
        {
            switch (membersType)
            {
                case MemberTypes.Field:
                    foreach (FieldInfo field in members)
                        WriteMemberDataRecersive(
                            writer,
                            field,
                            GetTypeInfo(field.FieldType),
                            () => field.GetValue(instance),
                            position,
                            depth,
                            memberTypes,
                            indentation,
                            cachedInstances,
                            blacklist);
                    break;
                case MemberTypes.Property:
                    foreach (PropertyInfo property in members)
                        WriteMemberDataRecersive(
                            writer,
                            property,
                            GetTypeInfo(property.PropertyType),
                            () => property.GetValue(instance),
                            position,
                            depth,
                            memberTypes,
                            indentation,
                            cachedInstances,
                            blacklist);
                    break;
            }
        }

        private static void WriteMemberName(
            Action<String, DumpSegment> writer, 
            MemberInfo member,
            TypeInfo info,
            Int32 position,
            String indentation,
            Type[] blacklist)
        {
            writer($"\n{indentation.Duplicate(position)}[{member.MemberType}]", DumpSegment.MemberType);
            writer($" {member.Name} ", DumpSegment.Name);
            writer($"({info.PrettyName}) ", DumpSegment.Type);
            writer($"=> ", DumpSegment.Scaffold);
        }

        public static TypeInfo GetTypeInfo(Type type)
        {
            if (!_typeInfoCache.ContainsKey(type))
                _typeInfoCache[type] = new TypeInfo(type);

            return _typeInfoCache[type];
        }
        #endregion
    }
}
