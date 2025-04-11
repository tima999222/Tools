using System.Reflection;

namespace Tools.Environment
{
    public static class EnvironmentBinder
    {
        public static T Bind<T>()
        {
            var value = Activator.CreateInstance<T>();
            return Bind(value);
        }

        public static T Bind<T>(T valueToBind)
        {
            var keyValuePairs = valueToBind.GetAttributedPropertiesInfo<EnvironmentVariableKeyAttribute>();

            foreach (var propertyToBind in keyValuePairs)
            {
                var propertyInfo = propertyToBind;
                var metadata = propertyInfo.GetCustomAttribute<EnvironmentVariableKeyAttribute>();
                var variableName = metadata.Name;

                var variable = System.Environment.GetEnvironmentVariable(variableName);
                if (variable == null && metadata.DefaultValue == null) throw new InvalidOperationException(variableName + " is declared but not found");
                else variable = metadata.DefaultValue;

                try
                {
                    var convertedValue = Convert.ChangeType(variable, propertyInfo.PropertyType);
                    propertyInfo.SetValue(valueToBind, convertedValue);
                }
                catch (InvalidCastException ex)
                {
                    throw new InvalidOperationException($"Cannot convert the value of {variableName} to type {propertyInfo.PropertyType.Name}.", ex);
                }
                catch (FormatException ex)
                {
                    throw new InvalidOperationException($"The value of {variableName} is not in a valid format for type {propertyInfo.PropertyType.Name}.", ex);
                }
            }

            return valueToBind;
        }


    }
}
