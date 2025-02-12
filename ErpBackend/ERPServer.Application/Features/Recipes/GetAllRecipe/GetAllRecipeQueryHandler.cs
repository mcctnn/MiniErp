using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERPServer.Application.Features.Recipes.GetAllRecipe;

internal sealed class GetAllRecipeQueryHandler(
    IRecipeRepository repository) : IRequestHandler<GetAllRecipeQuery, Result<List<Recipe>>>
{
    public async Task<Result<List<Recipe>>> Handle(GetAllRecipeQuery request, CancellationToken cancellationToken)
    {
        List<Recipe> recipes = await repository.GetAll()
            .Include(r => r.Product).OrderBy(x => x.Product!.Name).ToListAsync(cancellationToken);

        return recipes;
    }
}
