using MassTransit;
using MessageBroker.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace MessageBroker.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TicketController : ControllerBase
{
    private readonly IBus _bus;

    public TicketController(IBus bus)
    {
        _bus = bus;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Ticket ticket)
    {
        try
        {
            Uri uri = new("rabbitmq://localhost/ticket-queue");
            var endpoint = await _bus.GetSendEndpoint(uri);
            await endpoint.Send(ticket);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        };

        return Ok();
    }
}