
using Link.Mydapr.Service.Basket.Infrastucture;
using Link.Mydapr.Service.Basket.IntegrationEvent;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddDapr();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<OrderStatusChangedToSubmittedIntegrationEventHandler>();

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
app.UseAuthorization();

app.Run();
