using HowToMediatR.Mediator.Notifications;
using MediatR;

namespace HowToMediatR.Mediator.NotificationHandlers;

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
