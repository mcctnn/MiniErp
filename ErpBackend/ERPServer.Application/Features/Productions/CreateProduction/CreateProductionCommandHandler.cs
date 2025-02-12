using AutoMapper;
using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERPServer.Application.Features.Productions.CreateProduction;

internal sealed class CreateProductionCommandHandler(
    IProductionRepository repository,
    IRecipeRepository recipeRepository,
    IStockMovementRepository stockMovementRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper) : IRequestHandler<CreateProductionCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateProductionCommand request, CancellationToken cancellationToken)
    {
        Recipe? recipe = await recipeRepository.
            Where(p => p.ProductId == request.ProductId).Include(p => p.Details!).
            ThenInclude(p => p.Product).FirstOrDefaultAsync(cancellationToken);

        List<StockMovement> newMovements = new();

        Production production = mapper.Map<Production>(request);

        if (recipe is not null && recipe.Details is not null)
        {
            var details = recipe.Details;
            foreach (var item in details)
            {
                List<StockMovement> stockMovements = await stockMovementRepository.
                    Where(p => p.ProductId == item.ProductId).ToListAsync(cancellationToken);

                List<Guid> depotIds = stockMovements.GroupBy(p => p.DepotId)
                    .Select(g => g.Key)
                    .ToList();

                decimal stock = stockMovements.Sum(p => p.NumberOfEntries) - stockMovements.Sum(p => p.NumberOfOutputs);

                if (item.Quantity > stock)
                {
                    return Result<string>.Failure(item.Product!.Name + "ürününden üretim için yeterli miktarda yok. Eksik tutar : "
                        + (item.Quantity - stock));
                }

                foreach (var depotId in depotIds)
                {
                    if (item.Quantity <= 0) break;

                    decimal quantity = stockMovements.Where(p => p.DepotId == depotId).
                        Sum(s => s.NumberOfEntries - s.NumberOfOutputs);

                    decimal totalAmount = stockMovements.Where(p => p.DepotId == depotId && p.NumberOfEntries > 0).
                        Sum(s => s.Price * s.NumberOfEntries);

                    decimal totalEntriesQuantity = stockMovements.Where(p => p.DepotId == depotId && p.NumberOfEntries > 0).
                        Sum(s => s.NumberOfEntries);

                    decimal price = totalAmount / totalEntriesQuantity;

                    StockMovement movement = new()
                    {
                        ProductionId = production.Id,
                        ProductId=item.ProductId,
                        DepotId = depotId,
                        Price = price,
                    };

                    if (item.Quantity <= quantity)
                    {
                        movement.NumberOfOutputs = item.Quantity;
                    }
                    else
                    {
                        movement.NumberOfOutputs = quantity;
                    }

                    item.Quantity -= quantity;
                    newMovements.Add(movement);
                }
            }
        }

        await stockMovementRepository.AddRangeAsync(newMovements, cancellationToken);
        await repository.AddAsync(production, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Ürün başarıyla oluşturuldu";
    }
}
