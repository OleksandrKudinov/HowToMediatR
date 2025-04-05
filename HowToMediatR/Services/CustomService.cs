namespace HowToMediatR.Services;

public sealed class CustomService
{
    public async Task<Drink> GetMyFavoriteDrink()
    {
        // here we can query database, call external API etc...
        await Task.CompletedTask;

        // ...but this example is not about external calls, so just simply create one instance of Drink:
        var favoriteDrink = new Drink { Name = "Macchiato" };
        return favoriteDrink;
    }
}