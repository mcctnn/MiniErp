using ERPServer.Domain.Abstractions;
using ERPServer.Domain.Enums;

namespace ERPServer.Domain.Entities;

public sealed class Product : Entity
{
    public string Name { get; set; } = default!;
    public ProductTypeEnum Type { get; set; } = ProductTypeEnum.Product;
}