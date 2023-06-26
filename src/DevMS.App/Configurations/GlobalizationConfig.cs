using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace DevMS.App.Configurations
{
    public static class GlobalizationConfig
    {
        public static IApplicationBuilder UseGlobalizationConfig (this IApplicationBuilder app)
        {
            var culturePtBr = new CultureInfo("pt-br");
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culturePtBr),
                SupportedUICultures = new List<CultureInfo> { culturePtBr },
                SupportedCultures = new List<CultureInfo> { culturePtBr }
            };
            app.UseRequestLocalization(localizationOptions);

            return app;
        }
    }
}
