using AutoMapper;
using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Products.UpdateProduct;

internal sealed class UpdateProductCommandHandler(
    IProductRepository repository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<UpdateProductCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        Product product = await repository.GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

        if (product is null)
            return Result<string>.Failure("Ürün bulunamadı");

        if (product.Name != request.Name)
        {
            bool isNameExists = await repository.AnyAsync(p => p.Name == request.Name, cancellationToken);

            if (isNameExists)
                return Result<string>.Failure("Bu ürün daha önce kaydedilmiş");
        }

        mapper.Map(request, product);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Ürün başarıyla güncellendi";
    }
}
