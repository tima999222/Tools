namespace Tools.Environment
{
    public class EnvironmentVariableKeyAttribute : Attribute
    {
        public EnvironmentVariableKeyAttribute(string name, bool isPublic = true, string? defaultValue = null)
        {
            Name = name;
            IsPublic = isPublic;
            DefaultValue = defaultValue;
        }

        public string Name { get; private set; }
        public bool IsPublic { get; private set; }
        public string? DefaultValue { get; private set; }
    }
}