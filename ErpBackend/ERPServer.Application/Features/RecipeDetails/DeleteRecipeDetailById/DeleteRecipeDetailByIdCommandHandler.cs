using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.RecipeDetails.DeleteRecipeDetailById;

internal sealed class DeleteRecipeDetailByIdCommandHandler(
    IRecipeDetailRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteRecipeDetailByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteRecipeDetailByIdCommand request, CancellationToken cancellationToken)
    {
        RecipeDetail recipeDetail = await repository.GetByExpressionAsync(r => r.Id == request.Id, cancellationToken);

        if (recipeDetail is null)
            return Result<string>.Failure("Reçeteye ait bu ürün bulunamadı");

        repository.Delete(recipeDetail);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<string>.Succeed("Ürün reçeteden başarıyla silindi ");
    }
}
