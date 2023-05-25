using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(DonationNation.Areas.Identity.IdentityHostingStartup))]
namespace DonationNation.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}