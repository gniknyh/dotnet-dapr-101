using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Validation.AspNetCore;

namespace Link.Mydapr.Service.Basket
{
    public static class Extensions
    {
        public static void AddCustomAuthentication(this WebApplicationBuilder builder)
        {
            // default
            // builder.Services.AddAuthentication().AddJwtBearer();

            // // // Prevent mapping "sub" claim to nameidentifier.
            // JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            // builder.Services.AddAuthentication("Bearer")
            //     .AddJwtBearer(options =>
            //     {
            //         options.Audience = "basket-api";
            //         options.Authority = builder.Configuration.GetValue<string>("IdentityUrl");
            //         options.RequireHttpsMetadata = false;
            //     });

            // // Register the OpenIddict validation components.
            builder.Services.AddOpenIddict()
                .AddValidation(options =>
                {
                    // Note: the validation handler uses OpenID Connect discovery
                    // to retrieve the issuer signing keys used to validate tokens.
                    
                    // var identityUrl = "http://localhost:7004";
                    var identityUrl = builder.Configuration.GetValue<string>("IdentityUrl") ?? throw new ArgumentNullException("IdentityUrl invalid");
                    // var identityUrlExternal = builder.Configuration.GetValue<string>("IdentityUrlExternal")  ?? throw new ArgumentNullException("IdentityUrl invalid");

                    // Console.WriteLine($"identityUrlExternal: {identityUrlExternal}");
                    options.SetIssuer(identityUrl);
                    // options.AddAudiences("basket");

                    options.UseIntrospection()
                        .SetClientId("basket")
                        .SetClientSecret("846B62D0-DEF9-4215-A99D-86E6B8DAB342");

                    // Register the encryption credentials. This sample uses a symmetric
                    // encryption key that is shared between the server and the Api2 sample
                    // (that performs local token validation instead of using introspection).
                    //
                    // Note: in a real world application, this encryption key should be
                    // stored in a safe place (e.g in Azure KeyVault, stored as a secret).
                    // options.AddEncryptionKey(new SymmetricSecurityKey(
                    //     Convert.FromBase64String("DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY=")));

                    // Register the System.Net.Http integration.
                    options.UseSystemNetHttp();

                    // Register the ASP.NET Core host.
                    options.UseAspNetCore();
                });

            builder.Services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);

        }

        public static void AddCustomAuthorization(this WebApplicationBuilder builder)
        {
            // builder.Services.AddAuthorization();
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    // policy.RequireClaim("scope", "basket");
                });
            });
        }
    }


}