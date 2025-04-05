using HowToMediatR.Mediator.Queries;
using HowToMediatR.Models;
using HowToMediatR.Services;
using MediatR;

namespace HowToMediatR.Mediator.QueryHandlers;

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
}
