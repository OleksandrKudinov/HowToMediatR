using HowToMediatR.Models;
using MediatR;

namespace HowToMediatR.Mediator.Notifications;

public sealed class CoffeeBreakNotification : INotification
{
    public DateTime StartTime { get; init; }
    public required Drink Drink { get; init; }
}
