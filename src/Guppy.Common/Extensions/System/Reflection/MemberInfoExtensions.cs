namespace System.Reflection
{
    public static class MemberInfoExtensions
    {
        public static bool HasCustomAttribute<T>(this MemberInfo member)
            where T : Attribute
        {
            return member.GetCustomAttributes<T>().Any();
        }

        public static bool HasCustomAttribute<T>(this MemberInfo member, bool inherit)
             where T : Attribute
        {
            return member.GetCustomAttributes(inherit).Any(x => x is T);
        }
    }
}
