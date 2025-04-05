### MediatR: Your Code’s Friendly Messenger

In today’s fast-paced development world, keeping your code neat and loosely connected is essential.
Imagine a world where every component of your application communicates through a single, powerful hub - no more complicated dependencies or hidden coupling.
That’s where MediatR comes in.

![image](https://github.com/user-attachments/assets/0a2dbb73-5a75-4e50-9029-01a1de6a263d)


MediatR - a .NET library built on the mediator pattern, acting like an easy-to-use hub that manages communication between different parts of your app. With MediatR, your code stays clean, making it easier to tweak, test, and grow over time.

## What Is the Mediator Pattern?

You always have several parts of your application that need to interact. Instead of letting each component call another component directly, they send messages to a single point - the mediator. The mediator knows who is responsible for handling any specific kind of message.

You have a few ways to set up communication via the mediator::
- **Notifications:** When an event occurs, you broadcast a message. For example, "It's coffee time!" Multiple handlers might react to this notification.
- **Queries:** When you need data, you ask a question. For example, "What is your favorite drink?" The mediator routes this query to the right handler and returns an answer.
- **Commands:** When you want to change something, you issue a command. For example, "Make me a coffee!" The command instructs a specific handler to perform the action.

This approach keeps your components isolated, so they don’t need to know about each other’s inner workings. 

So let's see how these communication types - query, command, and notification - are implemented within the MediatR library.

From MediatR’s perspective, both Queries and Commands can use the same interface, `IRequest`.
In general, the difference is that a Query is responsible only for retrieving data, while a Command represents a strong intention to perform an action, like creating or modifying data.

With MediatR, you decide whether an `IRequestHandler` will handle a query or a command.
I’ll show you only a Query example, just to keep this article short.

---

## Querying example: "What's your favorite drink?"

Let’s see how this works with a simple example. 
First, let's define some typical components: a data model for the drink and a custom service:

```c#
public sealed class Drink
{
    public required string Name { get; init; }
}

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
```

Now it's time to use this stuff with MediatR.
First, we create a query that asks: "What is your favorite drink?". This query returns a `Drink` object:

```c#
// 'Query' postfix just to highlight our intention to use this IRequest implementation as a Query. 
public sealed class WhatIsYourFavoriteDrinkQuery : IRequest<Drink>
{
    // you can pass some filters here, if you want to
}
```

Next, we need to implement a handler to process the query:

```c#
public sealed class WhatIsYourFavoriteDrinkQueryHandler : IRequestHandler<WhatIsYourFavoriteDrinkQuery, Drink>
{
    public async Task<Drink> Handle(WhatIsYourFavoriteDrinkQuery request, CancellationToken cancellationToken)
    {
        Drink favoriteDrink = await _service.GetMyFavoriteDrink();
        return favoriteDrink;
    }

    // if you want to inject dependencies into your handler - here is the place for that:  
    public WhatIsYourFavoriteDrinkQueryHandler(CustomService service)
    {
        _service = service;
    }

    private readonly CustomService _service;
}```


To "ask the question", you simply create and send the query via the mediator:

```c#
// do not worry about provider, it will be explained later.
IMediator mediator = provider.GetRequiredService<IMediator>();

var query = new WhatIsYourFavoriteDrinkQuery();
Drink yourFavoriteDrink = await mediator.Send(query);
```

That's it!
## Notification: "It's coffee time!"

Now, as you might expect, we start by implementing `INotification`:
```c#
public sealed class CoffeeBreakNotification : INotification
{
    public DateTime StartTime { get; init; }
    public required Drink Drink { get; init; }
}
```

The next step is pretty straightforward: just add a handler for this notification:

```c#
public sealed class CoffeeBreakNotificationHandler : INotificationHandler<CoffeeBreakNotification>
{
    public async Task Handle(CoffeeBreakNotification notification, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        var time = notification.StartTime.TimeOfDay.ToString(@"hh\:mm");
        var drink = notification.Drink;

        Console.WriteLine($"It's {time}! Let's make a cup of {drink.Name} and take a break!");
    }
}
```

Let’s send the notification!
```c#
Drink yourFavoriteDrink = ... ; // let's reuse what we got from the query result

var notification = new CoffeeBreakNotification { StartTime = DateTime.Now, Drink = yourFavoriteDrink };
await mediator.Publish(notification);
```

That's it!    
## Setting Up MediatR with Dependency Injection

Now it's time to explain the `provider` you’ve seen in the example above.

Before you start using MediatR, you have to set it up properly.
First of all, you have to install the MediatR package:
```ps
dotnet add package MediatR --version 12.5.0
```

Then, you need to register your queries, commands, notifications, and their handlers with the Dependency Injection container.
In .NET, you can easily set this up using `ServiceCollection` which is part of widely used Microsoft.Extensions.DependencyInjection package:
```PS
dotnet add package Microsoft.Extensions.DependencyInjection --version 9.0.3
```

Let’s make the configuration as simple as possible:
```c#
var services = new ServiceCollection();

// don’t forget about our custom service used by WhatIsYourFavoriteDrinkQueryHandler
services.AddSingleton<CustomService>();

// Register all MediatR-related classes from the current assembly
services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(Program).Assembly));

// that's how we get this mysterious provider!
var provider = services.BuildServiceProvider();

// finally, here we get the configured, ready-to-use mediator object. 
var mediator = provider.GetRequiredService<IMediator>();
```

By configuring your DI container this way, MediatR becomes aware of all your handler components and can use them as needed. 

---
As you see, whether you're dealing with commands, queries, or notifications, MediatR keeps things consistent and clean. And with just a bit of setup, it fits right into your .NET application thanks to built-in dependency injection support.

MediatR is a versatile tool that can greatly simplify your application's architecture, making your code more modular, testable, and easier to maintain.

So next time you're building something and want your components to stay focused and independent - let MediatR do the talking.

Happy coding! ☕
