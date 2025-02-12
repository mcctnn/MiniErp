using AutoMapper;
using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Products.CreateProduct;

internal sealed class CreateProductCommandHandler(
    IProductRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<CreateProductCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        bool isNameExists = await repository.AnyAsync(p => p.Name == request.Name, cancellationToken);

        if (isNameExists)
            return Result<string>.Failure("Bu ürün daha önce kaydedilmiş");

        Product product=mapper.Map<Product>(request);

        await repository.AddAsync(product,cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Ürün başarıyla kaydedildi";
    }
}
