using Tools.Configurations;
using Tools.Environment;

namespace Tools.Logging
{
    public class ELKConfiguration : ConfigurationBase
    {
        [EnvironmentVariableKey("ELK_USER")] public string Username { get; set; }
        [EnvironmentVariableKey("ELK_PASSWORD", false)] public string Password { get; set; }
        [EnvironmentVariableKey("ELK_HOST")] public string Host { get; set; }
        [EnvironmentVariableKey("INDEX_FORMAT")] public string IndexFormat { get; set; }
    }
}
