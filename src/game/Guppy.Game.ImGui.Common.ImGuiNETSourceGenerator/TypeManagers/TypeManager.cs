using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Guppy.Game.ImGui.Common.ImGuiNETSourceGenerator.TypeManagers
{
    internal abstract class TypeManager
    {
        public static TypeManager GetTypeManager(Type type)
        {
            if (_instances.TryGetValue(type, out TypeManager manager))
            {
                return manager;
            }

            if (type.Assembly == typeof(ImGuiNET.ImGui).Assembly && type.IsEnum)
            {
                manager = new EnumTypeManager(type);
            }
            else if (type.Assembly == typeof(ImGuiNET.ImGui).Assembly && type.IsEnum == false && type.IsGenericType == false && typeof(Delegate).IsAssignableFrom(type) == false)
            {
                manager = new DecoratingTypeManager(type);
            }
            else
            {
                manager = new DefaultTypeManager(type);
            }

            _instances.Add(type, manager);
            _sourceGenerators.Enqueue(manager);

            return manager;
        }

        public static void GenerateAllSourceFiles(CodeBuilder source)
        {
            if (_sourceGenerators.Any() == false)
            {
                foreach (TypeManager manager in _instances.Values)
                {
                    _sourceGenerators.Enqueue(manager);
                }
            }

            while (_sourceGenerators.Any())
            {
                _sourceGenerators.Dequeue().GenerateSourceFiles(source);
            }
        }

        private static readonly Queue<TypeManager> _sourceGenerators = new Queue<TypeManager>();
        private static readonly Dictionary<Type, TypeManager> _instances = new Dictionary<Type, TypeManager>()
        {
            { typeof(System.Numerics.Vector2), new UnsafeAsTypeManager(typeof(System.Numerics.Vector2), "Microsoft.Xna.Framework.Vector2") },
            { typeof(System.Numerics.Vector3), new UnsafeAsTypeManager(typeof(System.Numerics.Vector3), "Microsoft.Xna.Framework.Vector3") },
            { typeof(System.Numerics.Vector4), new UnsafeAsTypeManager(typeof(System.Numerics.Vector4), "Microsoft.Xna.Framework.Vector4") },
        };

        private bool _generated = false;
        public readonly Type ImGuiType;
        public readonly string GuppyType;

        public virtual string ReturnTypeName => this.GuppyType;

        public virtual string GuppyParameterType => this.GuppyType.TrimEnd('&');

        protected TypeManager(Type imGuiType, string guppyType)
        {
            this.ImGuiType = imGuiType;
            this.GuppyType = guppyType.Replace("System.Void*", "void*");
        }

        public abstract string GetGuppyToImGuiConverter(string parameter);
        public abstract string GetImGuiToGuppyConverter(string parameter);

        public void GenerateSourceFiles(CodeBuilder source)
        {
            if (this._generated == true)
            {
                return;
            }

            this.InternalGenerateSourceFiles(source);
            this._generated = true;
        }

        protected abstract void InternalGenerateSourceFiles(CodeBuilder source);
    }
}