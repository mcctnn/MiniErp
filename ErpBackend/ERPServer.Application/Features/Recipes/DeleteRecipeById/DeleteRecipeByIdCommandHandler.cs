using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Recipes.DeleteRecipeById;

internal sealed class DeleteRecipeByIdCommandHandler(
    IRecipeRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteRecipeByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteRecipeByIdCommand request, CancellationToken cancellationToken)
    {
        Recipe recipe = await repository.GetByExpressionAsync(r => r.Id == request.Id, cancellationToken);

        if (recipe is null)
            return Result<string>.Failure("Reçete bulunamadı");

        repository.Delete(recipe);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Reçete başarıyla silindi";
    }
}
