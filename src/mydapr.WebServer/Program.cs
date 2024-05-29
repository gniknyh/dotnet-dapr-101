using Dapr.Client;
using Dapr.Extensions.Configuration;
using Link.Mydapr.WebServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using mydapr.WebServer.Components;
using OpenIddict.Client;
using static OpenIddict.Abstractions.OpenIddictConstants;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// builder.Configuration.AddDaprSecretStore(
//             "eshopondapr-secretstore",
//             new DaprClientBuilder().Build());

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseOpenIddict();
    // var connectStr = builder.Configuration.GetConnectionString("IdentityDataContextConnection");
    // options.UseNpgsql(connectStr);  
    options.UseInMemoryDatabase("webclient_db");

}
);

// builder.Services.AddAntiforgery(options =>
//         {
//             options.HeaderName = "X-XSRF-TOKEN";
//             options.Cookie.Name = "__Host-X-XSRF-TOKEN";
//             options.Cookie.SameSite = SameSiteMode.Strict;
//             options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//         });

builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })

        .AddCookie(options =>
        {
            options.LoginPath = "/login";
            options.LogoutPath = "/logout";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(50);
            options.SlidingExpiration = false;
        });

builder.Services.AddOpenIddict()

            // Register the OpenIddict core components.
            .AddCore(options =>
            {
                // Configure OpenIddict to use the Entity Framework Core stores and models.
                // Note: call ReplaceDefaultEntities() to replace the default OpenIddict entities.
                options.UseEntityFrameworkCore()
                       .UseDbContext<ApplicationDbContext>();

                // Developers who prefer using MongoDB can remove the previous lines
                // and configure OpenIddict to use the specified MongoDB database:
                // options.UseMongoDb()
                //        .UseDatabase(new MongoClient().GetDatabase("openiddict"));

                // Enable Quartz.NET integration.
                // options.UseQuartz();
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
                        .DisableTransportSecurityRequirement()
                        // .Configure(options => options.CookieBuilder.SameSite = SameSiteMode.Lax)
                       .EnableStatusCodePagesIntegration()
                       .EnableRedirectionEndpointPassthrough()
                       .EnablePostLogoutRedirectionEndpointPassthrough();

                // Register the System.Net.Http integration and use the identity of the current
                // assembly as a more specific user agent, which can be useful when dealing with
                // providers that use the user agent as a way to throttle requests (e.g Reddit).
                options.UseSystemNetHttp();

                // Add a client registration matching the client application definition in the server project.
                var identityUrl = builder.Configuration.GetValue<string>("IdentityUrl") ?? throw new ArgumentNullException("IdentityUrl invalid");
                options.AddRegistration(new OpenIddictClientRegistration
                {
                    // var identityUrl = _configuration.GetValue<string>("IdentityUrl") ?? throw new ArgumentNullException("IdentityUrl invalid");
                    Issuer = new Uri(identityUrl, UriKind.Absolute),

                    ClientId = "blazor-client",
                    // ClientSecret = "846B62D0-DEF9-4215-A99D-86E6B8DAB341",
                    Scopes = { Scopes.Email, Scopes.Profile },

                    // Note: to mitigate mix-up attacks, it's recommended to use a unique redirection endpoint
                    // URI per provider, unless all the registered providers support returning a special "iss"
                    // parameter containing their URL as part of authorization responses. For more information,
                    // see https://datatracker.ietf.org/doc/html/draft-ietf-oauth-security-topics#section-4.4.
                    // RedirectUri = new Uri("http://identity/callback/login/local", UriKind.Absolute),
                    // RedirectUri = new Uri("https://openidconnect.net/callback", UriKind.Absolute),
                    RedirectUri = new Uri("callback/login/local", UriKind.Relative),
                    // PostLogoutRedirectUri = new Uri("callback/logout/local", UriKind.Relative)
                });
            });

builder.Services.AddControllersWithViews();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseDeveloperExceptionPage();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
    _ = endpoints.MapDefaultControllerRoute();
});


app.Run();
