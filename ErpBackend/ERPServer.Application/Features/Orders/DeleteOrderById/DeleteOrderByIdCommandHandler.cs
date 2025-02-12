using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace ERPServer.Application.Features.Orders.DeleteOrderById;

internal sealed class DeleteOrderByIdCommandHandler(
    IOrderRepository repository,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteOrderByIdCommand, Result<string>>
{
    public async Task<Result<string>> Handle(DeleteOrderByIdCommand request, CancellationToken cancellationToken)
    {
        Order order = await repository.GetByExpressionAsync(p => p.Id == request.Id, cancellationToken);

        if (order is null)
            return Result<string>.Failure("Sipariş bulunamadı");

        repository.Delete(order);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Sipariş başarıyla silindi";
    }
}
