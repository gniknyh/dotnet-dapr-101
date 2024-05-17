using Dapr.Client;
using Dapr.Extensions.Configuration;
using Link.Mydapr.Service.Identity.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Link.Mydapr.Service.Identity
{
    public static class Extensions
    {
        public static void AddCustomConfiguration(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddDaprSecretStore(
            "eshopondapr-secretstore",
            new DaprClientBuilder().Build());
        }
        public static void AddCustomDatabase(this WebApplicationBuilder builder) =>
        builder.Services.AddDbContext<IdentityDataContext>(
            
            options => {
                // var connectStr = "Host=localhost; Port=5432; Database=default_database; Username=username; Password=password";
                var connectStr = builder.Configuration.GetConnectionString("IdentityDataContextConnection");
                options.UseNpgsql(connectStr);  
            } );

        public static void AddCustomIdentity(this WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<IdentityDataContext>()
                    .AddDefaultTokenProviders();
        }
    }
}
