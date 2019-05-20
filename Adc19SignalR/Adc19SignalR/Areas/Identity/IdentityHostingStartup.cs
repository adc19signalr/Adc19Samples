using System;
using Adc19SignalR.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Adc19SignalR.Areas.Identity.IdentityHostingStartup))]
namespace Adc19SignalR.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<Adc19SignalRContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("Adc19SignalRContextConnection")));

                services.AddDefaultIdentity<IdentityUser>()
                    .AddEntityFrameworkStores<Adc19SignalRContext>();
            });
        }
    }
}