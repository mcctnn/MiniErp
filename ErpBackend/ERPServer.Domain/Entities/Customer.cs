using ERPServer.Domain.Abstractions;

namespace ERPServer.Domain.Entities;

public sealed class Customer:Entity
{
    public string Name { get; set; } = default!;
    public string TaxDepartment { get; set; } = default!;
    public string TaxNumber { get; set; } = default!;
    public string City { get; set; } = default!;
    public string Town { get; set; } = default!;
    public string FullAddress { get; set; } = default!;
}
