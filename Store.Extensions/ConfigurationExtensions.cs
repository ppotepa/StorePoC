using Microsoft.Extensions.Configuration;
using Store.Extensions.Configuration;
using Store.Extensions.Exceptions;
using System;

namespace Store.Extensions
{
    /// <summary>
    /// Allows IConfiguration interface that is being injected to obtain Environment Variables.
    /// </summary>
    public static class ConfigurationExtensions
    {
        private static ConfigSettings ApplicationSettings = null;       

        public static EnvironmentVariables GetEnvironmentVariables(this IConfiguration @this)
        {
            return new EnvironmentVariables
            {
                DB_CONNECTIONSTRING_DEV = Environment.GetEnvironmentVariable(Constants.EnvironmentVariables.OX_SS_DB_CONNECTIONSTRING_DEV) ?? throw new EnvironmentVariableMissingException("STORE_CONNECTIONSTRING_DEV"),                
                ASPNETCORE_ENVIRONMENT = Environment.GetEnvironmentVariable(Constants.EnvironmentVariables.ASPNETCORE_ENVIRONMENT) ?? throw new EnvironmentVariableMissingException("ASPNETCORE_ENVIRONMENT")
            };
        }

        public static ConfigSettings GetApplicationSettings(this IConfiguration @this)
        {
            if (ApplicationSettings is null)
            {
                ConfigSettings instance = new ConfigSettings();
                @this.Bind("ApplicationSettings", instance);
                ApplicationSettings = instance;
            }

            lock (ApplicationSettings)
            {
                return ApplicationSettings;
            }
        }  
    }

}
