using AutoMapper;
using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERPServer.Application.Features.RecipeDetails.UpdateRecipeDetail;
public sealed record UpdateRecipeDetailCommand(
    Guid Id,
    Guid ProductId,
    decimal Quantity) : IRequest<Result<string>>;

internal sealed class UpdateRecipeDetailCommandHandler(
    IRecipeDetailRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<UpdateRecipeDetailCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateRecipeDetailCommand request, CancellationToken cancellationToken)
    {
        RecipeDetail recipeDetail = await repository.GetByExpressionWithTrackingAsync(r => r.Id == request.Id, cancellationToken);

        if (recipeDetail is null)
            return Result<string>.Failure("Reçeteye ait bu ürün bulunamadı");

        RecipeDetail? oldRecipeDetail = await repository.Where(r => 
            r.Id != request.Id &&
            r.ProductId == request.ProductId &&
            r.RecipeId == recipeDetail.RecipeId)
            .FirstOrDefaultAsync(cancellationToken);

        if (oldRecipeDetail is not null)
        {
            repository.Delete(recipeDetail);
            oldRecipeDetail.Quantity += request.Quantity;
            repository.Update(oldRecipeDetail);
        }
        else
        {
            mapper.Map(request, recipeDetail);

        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Reçetedeki ürün başarıyla güncellendi";
    }
}
