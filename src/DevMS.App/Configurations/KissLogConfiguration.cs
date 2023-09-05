using KissLog.AspNetCore;
using KissLog.CloudListeners.Auth;
using KissLog.CloudListeners.RequestLogsListener;
using KissLog.Formatters;

namespace DevMS.App.Configurations
{
    public static class KissLogConfiguration
    {
        public static IServiceCollection ResolveKissLog(this IServiceCollection services)
        {
            services.AddLogging(provider =>
                provider.AddKissLog(options =>
                {
                    options.Formatter = (FormatterArgs args) =>
                    {
                        if(args.Exception == null)
                            return args.DefaultValue;

                        string exceptionStr = new ExceptionFormatter().Format(args.Exception, args.Logger);
                        return string.Join(Environment.NewLine, new[] { args.DefaultValue, exceptionStr });
                    };
                }));

            services.AddHttpContextAccessor();
            return services;
        }

        public static IApplicationBuilder RegisterKissLogMiddleware(this IApplicationBuilder builder, IConfiguration configuration)
        {
            builder.UseKissLogMiddleware(options => {
                options.Listeners.Add(new RequestLogsApiListener(new Application(
                    configuration["KissLog.OrganizationId"],
                    configuration["KissLog.ApplicationId"])
                )
                {
                    ApiUrl = configuration["KissLog.ApiUrl"]
                });
            });
            return builder;
        }
    }
}
