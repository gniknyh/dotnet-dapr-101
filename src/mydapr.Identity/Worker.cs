
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
            var blazor_client = await manager.FindByClientIdAsync("blazor-client");
            if (blazor_client != null)
            {
                await manager.DeleteAsync(blazor_client);
            }

            if (await manager.FindByClientIdAsync("blazor-client") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "blazor-client",
                    ClientSecret = "846B62D0-DEF9-4215-A99D-86E6B8DAB341",
                    DisplayName = "Blazor client application",
                    RedirectUris =
                    {
                        new Uri("http://identity/callback/login/local")
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
                        Permissions.Prefixes.Scope + "api2",
                    }
                });
            }
            
            if (await manager.FindByClientIdAsync("basket") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "basket",
                    ClientSecret = "846B62D0-DEF9-4215-A99D-86E6B8DAB342",
                    ClientType = ClientTypes.Confidential,
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

            if (await manager.FindByNameAsync("api2") is null)
            {
                await manager.CreateAsync(new OpenIddictScopeDescriptor
                {
                    Name = "api2",
                    Resources =
                    {
                        "resource_server_2"
                    }
                });
            }
        }
        await CreateScopesAsync();

    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
