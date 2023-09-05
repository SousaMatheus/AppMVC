using DevMS.App.Configurations;
using DevMS.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DevMS.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            if(builder.Environment.IsProduction())
                builder.Configuration.AddUserSecrets<Program>();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.ResolveKissLog();
            builder.Services.ResolveIdentityConfig(connectionString);            
            builder.Services.ResolveConfig();

            builder.Services.AddDbContext<MeuDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.ResolveMvcConfig();


            var app = builder.Build();

            if(app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/erro/500");
                app.UseStatusCodePagesWithRedirects("/erro/{0}");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseGlobalizationConfig();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.RegisterKissLogMiddleware(builder.Configuration);

            app.Run();
        }
    }
}