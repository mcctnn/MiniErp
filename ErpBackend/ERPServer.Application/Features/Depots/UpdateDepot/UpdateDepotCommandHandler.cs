using AutoMapper;
using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Depots.UpdateDepot;

internal sealed class UpdateDepotCommandHandler(
    IDepotRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<UpdateDepotCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateDepotCommand request, CancellationToken cancellationToken)
    {
        Depot depot = await repository.GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

        if (depot is null)
            return Result<string>.Failure("Depo bulunamadı");

        if (depot.Name != request.Name)
        {
            bool isNameExists = await repository.AnyAsync(p => p.Name == request.Name, cancellationToken);

            if (isNameExists)
                return Result<string>.Failure("Bu depo adı önceden kaydedilmiş");
        }

        mapper.Map(request, depot);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Depo başarıyla güncellendi";
    }
}
