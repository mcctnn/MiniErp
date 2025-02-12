using AutoMapper;
using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Depots.CreateDepot;

internal sealed class CreateDepotCommandHandler(
    IDepotRepository repository,
    IMapper mapper,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateDepotCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateDepotCommand request, CancellationToken cancellationToken)
    {
        bool isDepotNameExists = await repository.AnyAsync(p => p.Name == request.Name,cancellationToken);

        if (isDepotNameExists)
            return Result<string>.Failure("Bu isimle daha önceden kaydedilmiş");

        Depot depot = mapper.Map<Depot>(request); 

        await repository.AddAsync(depot,cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Depo kaydı başarıyla oluşturuldu";
    }
}
