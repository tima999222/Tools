using Serilog;
using Tools.Environment;

namespace Tools.Logging
{
    public class LoggerConfiguratorHelper
    {
        public static ILogger CreateLoggerByEnableElkFlag(int enable, string logLevel) 
        {
            ILogger logger;
            if (enable == 1)
            {
                var elkConfiguration = EnvironmentBinder.Bind<ELKConfiguration>();
                logger = LoggingConfigurator.ElasticLogger(elkConfiguration.IndexFormat,
                        elkConfiguration.Username,
                        elkConfiguration.Password,
                        elkConfiguration.Host,
                        logLevel);
            }
            else
            {
                logger = LoggingConfigurator.ConsoleLogger(logLevel);
            }

            Log.Logger = logger;
            return logger;
        }
    }
}
