using AutoMapper;
using ERPServer.Application.Features.Customers.CreateCustomer;
using ERPServer.Application.Features.Customers.UpdateCustomer;
using ERPServer.Application.Features.Depots.CreateDepot;
using ERPServer.Application.Features.Depots.UpdateDepot;
using ERPServer.Application.Features.Invoices.CreateInvoice;
using ERPServer.Application.Features.Invoices.UpdateInvoice;
using ERPServer.Application.Features.Orders.CreateOrder;
using ERPServer.Application.Features.Orders.UpdateOrder;
using ERPServer.Application.Features.Productions.CreateProduction;
using ERPServer.Application.Features.Products.CreateProduct;
using ERPServer.Application.Features.Products.UpdateProduct;
using ERPServer.Domain.Entities;
using ERPServer.Domain.Enums;

namespace ERPServer.Application.Mapping
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //customer
            CreateMap<CreateCustomerCommand,Customer>();
            CreateMap<UpdateCustomerCommand,Customer>();
            //depot
            CreateMap<CreateDepotCommand,Depot>();
            CreateMap<UpdateDepotCommand,Depot>();
            //product
            CreateMap<CreateProductCommand,Product>().
                ForMember(member=>member.Type,opt=>
                            opt.MapFrom(p=>ProductTypeEnum.FromValue(p.TypeValue)));
            CreateMap<UpdateProductCommand, Product>().
                ForMember(member => member.Type, opt =>
                            opt.MapFrom(p => ProductTypeEnum.FromValue(p.TypeValue)));
            //order
            CreateMap<CreateOrderCommand, Order>().
                ForMember(member => member.Details,
                opt =>
                opt.MapFrom(p => p.Details.Select(s => new OrderDetail
                {
                    Price = s.Price,
                    Quantity = s.Quantity,
                    ProductId = s.ProductId
                }).ToList()));
            CreateMap<UpdateOrderCommand, Order>().
                ForMember(member=>member.Details,
                opt=>opt.Ignore());
            //invoice
            CreateMap<CreateInvoiceCommand, Invoice>().
                ForMember(member=>member.Type,opt=>
                opt.MapFrom(p=>InvoiceTypeEnum.FromValue(p.TypeValue))).
                ForMember(member => member.Details,
                opt =>
                opt.MapFrom(p => p.Details.Select(s => new InvoiceDetail
                {
                    Price = s.Price,
                    Quantity = s.Quantity,
                    ProductId = s.ProductId,
                    DepotId=s.DepotId
                }).ToList()));
            CreateMap<UpdateInvoiceCommand, Invoice>().
                ForMember(member => member.Details,
                opt => opt.Ignore());
            //production
            CreateMap<CreateProductionCommand, Production>();
        }
    }
}
