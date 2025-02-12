using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Depots.DeleteDepotById;

internal sealed class DeleteDepotByIdCommandHandler(
    IDepotRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteDepotByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteDepotByIdCommand request, CancellationToken cancellationToken)
    {
        Depot depot = await repository.GetByExpressionAsync(p => p.Id == request.Id, cancellationToken);

        if (depot is null)
            return Result<string>.Failure("Depo bulunamadı");

        repository.Delete(depot);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Depo silme işlemi başarıyla tamamlandı";
    }
}
