using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adc19SignalR
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // SignalR Hinzufügen
            services.AddSignalR();
                    
            services.AddCors(options =>
            {
                options.AddPolicy("AllowEveryGet",
                    builder => builder.AllowAnyOrigin()
                        .WithMethods("GET")
                        .AllowAnyHeader()
                        .AllowCredentials());

                options.AddPolicy("AllowExampleDomain",
                    builder => builder.WithOrigins("http://adc.ms")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            // SignalR Verwenden
            app.UseAuthentication();

            app.UseCors("AllowEveryGet")
               .UseCors("AllowExampleDomain");

            app.UseSignalR(routes =>
            {
                routes.MapHub<AdcHub>("/adcHub");
            });

            app.UseMvc();
        }
    }
}
