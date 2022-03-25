using ShoppingCart;
using ShoppingCart.Core.Gateways;
using ShoppingCart.Infrastructure.Gateways;
using ShoppingCart.UseCases.Checkout;
using ShoppingCart.UseCases.CreateShoppingCartWithProduct;
using ShoppingCart.UseCases.GetCartByUserId;
using ShoppingCart.UseCases.UpdateAmountOfProductInShoppingCart;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddSerilog("ShoppingCart");

builder.Services
    .AddCustomCors()
    .AddEndpointsApiExplorer()
    .AddHttpContextAccessor()
    .AddCustomMediatR(new[] {typeof(Anchor)})
    .AddCustomValidators(new[] {typeof(Anchor)})
    .AddSwaggerGen()
    .AddCustomDaprClient()
    .AddHealthChecks();

// builder.Services.AddCustomAuth<Anchor>(builder.Configuration);

builder.Services.AddScoped<ISecurityContextAccessor, SecurityContextAccessor>();
builder.Services.AddScoped<IProductCatalogGateway, ProductCatalogGateway>();
builder.Services.AddScoped<IPromoGateway, PromoGateway>();
builder.Services.AddScoped<IShippingGateway, ShippingGateway>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseSerilogRequestLogging();

app.MapGet("/error", () => Results.Problem("An error occurred.", statusCode: 500))
    .ExcludeFromDescription();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCustomCors();
app.UseRouting();

app.UseCloudEvents();

// app.UseAuthentication();
// app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapHealthChecks("/api/healthz");
app.MapFallback(() => Results.Redirect("/swagger"));
app.MapSubscribeHandler();

app.MapGet("/info", (IConfiguration config) => Results.Content(config.BuildAppStatus()));

app.MapGet("api/carts", async (ISender sender) => await sender.Send(new GetCartByUserIdQuery()));
app.MapPost("api/carts", async (CreateShoppingCartWithProductCommand command, ISender sender) => await sender.Send(command));
app.MapPut("api/carts", async (UpdateAmountOfProductInShoppingCartCommand command, ISender sender) => await sender.Send(command));
app.MapPut("api/carts/checkout", async (ISender sender) => await sender.Send(new CheckOutCommand()));

await WithSeriLog(async () => await app.RunAsync());
