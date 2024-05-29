using Balosar.Server;
using Link.Mydapr.Service.Identity;
using Link.Mydapr.Service.Identity.Areas.Identity.Data;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddEndpointsApiExplorer();
builder.AddCustomConfiguration();

builder.Services.AddAuthorization();
builder.AddCustomDatabase();
builder.AddCustomIdentity();
builder.AddCustomIdentityServer();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            var basketUrl = builder.Configuration.GetValue<string>("BasketUrl") ?? throw new ArgumentNullException("IdentityUrl invalid");
            policy.WithOrigins(basketUrl, "http://localhost:5000");
        });
});


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddHostedService<Worker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllers();
    _ = endpoints.MapDefaultControllerRoute();
    _ = endpoints.MapRazorPages();
});

app.Run();
