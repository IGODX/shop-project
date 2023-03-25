using MyShopPet.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureServices();
var app = builder.Build();
await app.Configure();


app.Run();
