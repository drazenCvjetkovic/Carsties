using System.Text.Json;
using MongoDB.Entities;

namespace SearchService.Data;

public class DbInitializer
{
    public static async Task InitDb(WebApplication app)
    {
        await MongoDB.Entities.DB.InitAsync("SearchDb", MongoDB.Driver.MongoClientSettings
            .FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));

        await MongoDB.Entities.DB.Index<Item>()
            .Key(x => x.Make, MongoDB.Entities.KeyType.Text)
            .Key(x => x.Model, MongoDB.Entities.KeyType.Text)
            .Key(x => x.Color, MongoDB.Entities.KeyType.Text)
            .CreateAsync();

        var count = await DB.CountAsync<Item>();
        if (count == 0)
        {
            Console.WriteLine("No data - need to seed");
            var itemData = await File.ReadAllTextAsync("Data/auctions.json");
            // Console.WriteLine(itemData);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true};
            var items = JsonSerializer.Deserialize<List<Item>>(itemData, options);
            await DB.SaveAsync(items);
        }
    }

}