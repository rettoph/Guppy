using Guppy.Extensions.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Guppy.DependencyInjection
{
    public struct ServiceConfigurationKey
    {
        #region Public Fields
        /// <summary>
        /// A unique hash based on a combination of the
        /// <see cref="Type"/> & <see cref="Name"/> values.
        /// </summary>
        public readonly UInt32 Id;

        /// <summary>
        /// <para>
        /// A <see cref="System.Type"/> instance, making up
        /// one part of the composite unique key.
        /// </para>
        /// 
        /// <para>
        /// Defaults to <see cref="Object"/>
        /// </para>
        /// </summary>
        public readonly Type Type;

        /// <summary>
        /// <para>
        /// A <see cref="System.String"/> instance, making up
        /// one part of the composite unique key.
        /// </para>
        /// 
        /// <para>
        /// Defaults to <see cref="String.Empty"/>
        /// </para>
        /// </summary>
        public readonly String Name;
        #endregion

        #region Constructors
        /// <summary>
        /// Unique & inheritable identifier for identifying service desciptors. 
        /// </summary>
        /// <param name="type">
        /// <para>
        /// A <see cref="System.Type"/> instance, making up
        /// one part of the composite unique key.
        /// </para>
        /// 
        /// <para>
        /// Defaults to <see cref="Object"/>
        /// </para>
        /// </param>
        /// <param name="name">
        /// <para>
        /// A <see cref="System.String"/> instance, making up
        /// one part of the composite unique key.
        /// </para>
        /// 
        /// <para>
        /// Defaults to <see cref="String.Empty"/>
        /// </para>
        /// </param>
        public ServiceConfigurationKey(Type type = null, String name = null)
        {
            this.Type = type ?? typeof(Object);
            this.Name = name ?? String.Empty;
            this.Id = (this.Type.AssemblyQualifiedName + this.Name).xxHash();
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Determine if the current <see cref="ServiceConfigurationKey"/> inherits
        /// a recieved <see cref="ServiceConfigurationKey"/> value.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public Boolean Inherits(ServiceConfigurationKey parent)
        {
            return (
                    parent.Type.IsAssignableFrom(this.Type) 
                    || (
                        parent.Type.IsGenericTypeDefinition
                        && this.Type.IsSubclassOfRawGeneric(parent.Type)
                    )
                )
                && this.Name.StartsWith(parent.Name);
        }

        /// <summary>
        /// Return all <see cref="ServiceConfigurationKey"/> instances with possible
        /// <see cref="ServiceConfigurationKey.Type"/> ancestry between the recieved
        /// type.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public IEnumerable<ServiceConfigurationKey> GetAncestors(ServiceConfigurationKey parent)
        {
            if(this.Inherits(parent))
                foreach (Type ancestor in this.Type.GetAncestors(parent.Type))
                {
                    yield return new ServiceConfigurationKey(ancestor, this.Name);
                };

            yield return this;
        }

        public ServiceConfigurationKey TryGetGenericKey(out Type[] generics)
        {
            if (this.Type.IsConstructedGenericType)
            {
                generics = this.Type.GetGenericArguments();
                return new ServiceConfigurationKey(this.Type.GetGenericTypeDefinition(), this.Name);
            }
            else
            {
                generics = default;
                return this;
            }
        }

        public ServiceConfigurationKey TryConstructGenericKey(Type[] generics)
        {
            if(this.Type.IsGenericType && !this.Type.IsConstructedGenericType && generics != default)
                return new ServiceConfigurationKey(this.Type.MakeGenericType(generics), this.Name);

            return this;
        }

        public override string ToString()
            => $"{this.Type.GetPrettyName()}({this.Name})";
        #endregion

        #region Explicit Overloading
        public static bool operator ==(ServiceConfigurationKey k1, ServiceConfigurationKey k2)
            => k1.Id == k2.Id;

        public static bool operator !=(ServiceConfigurationKey k1, ServiceConfigurationKey k2) => !(k1 == k2);

        // public static implicit operator ServiceConfigurationKey(String name) => new ServiceConfigurationKey(name: name);
        // public static implicit operator ServiceConfigurationKey(Type type) => new ServiceConfigurationKey(type: type);
        // public static implicit operator ServiceConfigurationKey((Type, String) key) => new ServiceConfigurationKey(type: key.Item1, name: key.Item2);
        #endregion

        #region Static Helper Methods
        public static ServiceConfigurationKey From<T>(String name = "")
            => new ServiceConfigurationKey(typeof(T), name);

        public static ServiceConfigurationKey From(Type type, String name = "")
            => new ServiceConfigurationKey(type, name);

        public override bool Equals(object obj)
        {
            return obj is ServiceConfigurationKey key &&
                   Id == key.Id &&
                   EqualityComparer<Type>.Default.Equals(Type, key.Type) &&
                   Name == key.Name;
        }

        public override int GetHashCode()
        {
            int hashCode = 1880411771;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(Type);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
        #endregion
    }
}
