

using Link.Mydapr.Service.Ordering.Infrastructure;
using Link.Mydapr.Util.Pubsub;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddDapr();
builder.Services.AddDbContext<OrderingDbContext>(option =>
    option.UseInMemoryDatabase("test")
);
builder.Services.AddScoped<IEventBus, DaprEventBus>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCloudEvents();
app.MapControllers();
app.MapSubscribeHandler();
app.UseAuthorization();

app.Run();
