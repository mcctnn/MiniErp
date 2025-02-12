using AutoMapper;
using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Customers.CreateCustomer;

internal sealed class CreateCustomerCommandHandler(
    ICustomerRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<CreateCustomerCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        bool isTaxNumberExists = await repository.AnyAsync(p=>p.TaxNumber==request.TaxNumber,cancellationToken);

        if (isTaxNumberExists)
            return Result<string>.Failure("Bu vergi numarası daha önceden kaydedilmiş");

        Customer customer = mapper.Map<Customer>(request);

        await repository.AddAsync(customer,cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Müşteri kaydı başarıyla tamamlandı";
    }
}
