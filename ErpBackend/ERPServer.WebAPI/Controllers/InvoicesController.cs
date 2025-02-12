using ERPServer.Application.Features.Invoices.CreateInvoice;
using ERPServer.Application.Features.Invoices.DeleteInvoiceById;
using ERPServer.Application.Features.Invoices.GetAllInvoice;
using ERPServer.Application.Features.Invoices.UpdateInvoice;
using ERPServer.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERPServer.WebAPI.Controllers;

public class InvoicesController : ApiController
{
    public InvoicesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> GetAll(GetAllInvoiceQuery request, CancellationToken token)
    {
        var response = await _mediator.Send(request, token);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateInvoiceCommand request, CancellationToken token)
    {
        var response = await _mediator.Send(request, token);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteById(DeleteInvoiceByIdCommand request, CancellationToken token)
    {
        var response = await _mediator.Send(request, token);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateInvoiceCommand request, CancellationToken token)
    {
        var response = await _mediator.Send(request, token);

        return StatusCode(response.StatusCode, response);
    }
}
