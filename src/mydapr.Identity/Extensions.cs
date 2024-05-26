using Dapr.Client;
using Dapr.Extensions.Configuration;
using Link.Mydapr.Service.Identity.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static OpenIddict.Abstractions.OpenIddictConstants;

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
                options.UseOpenIddict();
            } );

        public static void AddCustomIdentity(this WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<IdentityDataContext>()
                    .AddDefaultUI()
                    .AddDefaultTokenProviders();
        }

        public static void AddCustomIdentityServer(this WebApplicationBuilder builder)
        {
            builder.Services.AddOpenIddict()

            // Register the OpenIddict core components.
            .AddCore(options =>
            {
                // Configure OpenIddict to use the Entity Framework Core stores and models.
                // Note: call ReplaceDefaultEntities() to replace the default entities.
                options.UseEntityFrameworkCore()
                    .UseDbContext<IdentityDataContext>();
            })
            // Register the OpenIddict server components.
            .AddServer(options =>
            {
                // Enable the authorization, logout, token and userinfo endpoints.
                options.SetAuthorizationEndpointUris("connect/authorize")
                       .SetLogoutEndpointUris("connect/logout")
                       .SetTokenEndpointUris("connect/token")
                        .SetIntrospectionEndpointUris("connect/introspect");


                // Mark the "email", "profile" and "roles" scopes as supported scopes.
                options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles);


                // Enable the client credentials flow.
                options.AllowAuthorizationCodeFlow()
                    .AllowRefreshTokenFlow()
                    .AllowClientCredentialsFlow();

                options.AddEncryptionKey(new SymmetricSecurityKey(
                Convert.FromBase64String("DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY=")));


                // Register the signing and encryption credentials.
                options.AddDevelopmentEncryptionCertificate();
                options.AddDevelopmentSigningCertificate();

                // Register the ASP.NET Core host and configure the ASP.NET Core options.
                options.UseAspNetCore()
                    .DisableTransportSecurityRequirement()
                    .EnableTokenEndpointPassthrough()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableUserinfoEndpointPassthrough()
                    .EnableLogoutEndpointPassthrough();
            })

            // Register the OpenIddict client components.
            .AddClient(options =>
            {
                // Note: this sample uses the code flow, but you can enable the other flows if necessary.
                options.AllowAuthorizationCodeFlow();

                // Register the signing and encryption credentials used to protect
                // sensitive data like the state tokens produced by OpenIddict.
                options.AddDevelopmentEncryptionCertificate()
                       .AddDevelopmentSigningCertificate();

                // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
                options.UseAspNetCore()
                       .EnableStatusCodePagesIntegration()
                       .EnableRedirectionEndpointPassthrough();

                // Register the System.Net.Http integration and use the identity of the current
                // assembly as a more specific user agent, which can be useful when dealing with
                // providers that use the user agent as a way to throttle requests (e.g Reddit).
                options.UseSystemNetHttp();

                // Register the Web providers integrations.
                //
                // Note: to mitigate mix-up attacks, it's recommended to use a unique redirection endpoint
                // URI per provider, unless all the registered providers support returning a special "iss"
                // parameter containing their URL as part of authorization responses. For more information,
                // see https://datatracker.ietf.org/doc/html/draft-ietf-oauth-security-topics#section-4.4.
                options.UseWebProviders()
                       .AddGitHub(options =>
                       {
                           options.SetClientId("c4ade52327b01ddacff3")
                                  .SetClientSecret("da6bed851b75e317bf6b2cb67013679d9467c122")
                                  .SetRedirectUri("callback/login/github");
                       });
            })

            // Register the OpenIddict validation components.
            .AddValidation(options =>
            {
                // Import the configuration from the local OpenIddict server instance.
                options.UseLocalServer();

                // Register the ASP.NET Core host.
                options.UseAspNetCore();
            });
            

        }
    }
}
