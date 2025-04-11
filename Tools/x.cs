using System.Reflection;

namespace Tools
{
    public static class x
    {
        public static IEnumerable<PropertyInfo> GetAttributedPropertiesInfo<T>(this object value)
            where T : Attribute
        {
            var result = new Dictionary<string, object?>();

            var type = value.GetType();

            var props = type.GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(T)));

            return props;
        }
    }
}
