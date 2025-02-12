using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERPServer.Application.Features.RecipeDetails.GetRecipeByIdWithDetails;

internal sealed class GetRecipeByIdWithDetailsQueryHandler(
    IRecipeRepository repository) : IRequestHandler<GetRecipeByIdWithDetailsQuery, Result<Recipe>>
{
    public async Task<Result<Recipe>> Handle(GetRecipeByIdWithDetailsQuery request, CancellationToken cancellationToken)
    {
        Recipe? recipe = await repository.Where(r => r.Id == request.RecipeId)
            .Include(r => r.Product)
            .Include(r => r.Details!.OrderBy(r=>r.Product!.Name))
            .ThenInclude(r => r.Product)
            .FirstOrDefaultAsync(cancellationToken);

        if (recipe is null)
            return Result<Recipe>.Failure("Ürüne ait reçete bulunamadı");

        return recipe;
    }
}
