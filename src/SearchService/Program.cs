using SearchService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var app = builder.Build();

app.UseAuthorization();

app.MapControllers();


await MongoDB.Entities.DB.InitAsync("SearchDb", MongoDB.Driver.MongoClientSettings.FromConnectionString(builder.Configuration.GetConnectionString("MongoDbConnection")));

await MongoDB.Entities.DB.Index<Item>()
    .Key(x => x.Make, MongoDB.Entities.KeyType.Text)
    .Key(x => x.Model, MongoDB.Entities.KeyType.Text)
    .Key(x => x.Color, MongoDB.Entities.KeyType.Text)
    .CreateAsync();

app.Run();
