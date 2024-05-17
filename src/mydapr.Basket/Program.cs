
using Link.Mydapr.Service.Basket.Infrastucture;
using Link.Mydapr.Service.Basket.Events;
using Link.Mydapr.Util.Pubsub;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddDapr();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<OrderStatusChangedToSubmittedIntegrationEventHandler>();
builder.Services.AddScoped<IEventBus, DaprEventBus>();

builder.Services.AddAuthentication("Bearer").AddJwtBearer();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCloudEvents();
app.MapControllers();
app.MapSubscribeHandler();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
