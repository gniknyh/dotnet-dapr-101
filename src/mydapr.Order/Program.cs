

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddDapr();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCloudEvents();
app.MapControllers();
app.MapSubscribeHandler();
app.UseAuthorization();

app.Run();
