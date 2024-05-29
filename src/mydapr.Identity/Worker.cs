
using Link.Mydapr.Service.Identity.Areas.Identity.Data;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using OpenIddict.Validation.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Balosar.Server;

public class Worker : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public Worker(IServiceProvider serviceProvider,
    IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<IdentityDataContext>();
        await context.Database.EnsureCreatedAsync();

        async Task CreateApplicationsAsync()
        {

            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
            var identityUrl = _configuration.GetValue<string>("IdentityUrl") ?? throw new ArgumentNullException("IdentityUrl invalid");
            var webServerUrl = _configuration.GetValue<string>("WebServerUrl") ?? throw new ArgumentNullException("WebServerUrl invalid");
            var blazor_client = await manager.FindByClientIdAsync("blazor-client");
            if (blazor_client != null)
            {
                await manager.DeleteAsync(blazor_client);
            }
            // var basket_client = await manager.FindByClientIdAsync("basket");
            // if (basket_client != null)
            // {
            //     await manager.DeleteAsync(basket_client);
            // }

            if (await manager.FindByClientIdAsync("blazor-client") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "blazor-client",
                    ClientType = ClientTypes.Public,
                    ConsentType = ConsentTypes.Explicit,
                    DisplayName = "Blazor client application",
                    RedirectUris =
                    {
                        new Uri(webServerUrl)
                    },
                    Permissions =
                    {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Logout,
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.ClientCredentials,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.ResponseTypes.Code,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles,
                        Permissions.Prefixes.Scope + "api1",
                    }
                });
            }
            
            if (await manager.FindByClientIdAsync("basket") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "basket",
                    ClientSecret = "846B62D0-DEF9-4215-A99D-86E6B8DAB342",
                    ConsentType = ConsentTypes.Explicit,
                    Permissions =
                    {
                        Permissions.Endpoints.Introspection
                    }
                });
            }
        }
        await CreateApplicationsAsync();

        async Task CreateScopesAsync()
        {

            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();
            // var api1 = await manager.FindByNameAsync("api1");
            // if (api1 is not null)
            // {
            //     await manager.DeleteAsync(api1);
            // }
            if (await manager.FindByNameAsync("api1") is null)
            {
                await manager.CreateAsync(new OpenIddictScopeDescriptor
                {
                    Name = "api1",
                    Resources =
                    {
                        "basket"
                    }
                });
            }
        }
        await CreateScopesAsync();

    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
