using MediatR;

// 'Query' postfix just to highlight our intention to use this IRequest implementation as a Query. 
public sealed class WhatIsYourFavoriteDrinkQuery : IRequest<Drink>
{
    // you can pass some filters here, if you want to
}