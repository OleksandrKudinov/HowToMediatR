using HowToMediatR.Mediator.Notifications;
using HowToMediatR.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace HowToMediatR;

internal class Program
{
    static async Task Main(string[] args)
    {
        var services = new ServiceCollection();

        services.AddSingleton<CustomService>();

        // Register all MediatR-related classes from the current assembly
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(Program).Assembly));

        // that's how we get this mysterious provider!
        var provider = services.BuildServiceProvider();

        // finally, here we get the configured mediator  
        var mediator = provider.GetRequiredService<IMediator>();

        // ask for your favorite drink
        var query = new WhatIsYourFavoriteDrinkQuery();
        Drink yourFavoriteDrink = await mediator.Send(query);

        Console.WriteLine($"Now I know! your favorite drink is {yourFavoriteDrink.Name}");

        var notification = new CoffeeBreakNotification { StartTime = DateTime.Now, Drink = yourFavoriteDrink };
        await mediator.Publish(notification);
    }
}
