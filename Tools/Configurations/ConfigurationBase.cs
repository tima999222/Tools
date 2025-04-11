using System.Reflection;
using System.Text;
using Tools.Environment;

namespace Tools.Configurations
{
    public abstract class ConfigurationBase
    {
        public virtual string GetEnvironmentVariablesConfig<T>(T config) where T : class
        {
            var stringBuilder = new StringBuilder();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<EnvironmentVariableKeyAttribute>();
                if (attribute != null)
                {
                    if (attribute.IsPublic == false) continue;
                    var key = attribute.Name;

                    var value = property.GetValue(config)?.ToString() ?? "null";
                    stringBuilder.AppendLine($"{key}: {value}");
                }
            }

            return stringBuilder.ToString();
        }
    }
}
