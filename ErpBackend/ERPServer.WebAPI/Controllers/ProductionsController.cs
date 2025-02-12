using ERPServer.Application.Features.Productions.CreateProduction;
using ERPServer.Application.Features.Productions.DeleteProductionById;
using ERPServer.Application.Features.Productions.GetAllProduction;
using ERPServer.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERPServer.WebAPI.Controllers;

public sealed class ProductionsController : ApiController
{
    public ProductionsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> GetAll(GetAllProductionQuery request, CancellationToken token)
    {
        var response = await _mediator.Send(request, token);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductionCommand request, CancellationToken token)
    {
        var response = await _mediator.Send(request, token);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteById(DeleteProductionByIdCommand request, CancellationToken token)
    {
        var response = await _mediator.Send(request, token);

        return StatusCode(response.StatusCode, response);
    }
}
