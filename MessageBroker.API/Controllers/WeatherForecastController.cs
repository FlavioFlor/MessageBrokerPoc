using MassTransit;
using MessageBroker.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace MessageBroker.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IBus _bus;
    private readonly IPublishEndpoint _endpoint;
    
    public WeatherForecastController( IBus bus, IPublishEndpoint endpoint)
    {
        _bus = bus;
        _endpoint = endpoint;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Ticket ticket)
    {
        try
        {
            // Uri uri = new Uri("rabbitmq://localhost/ticket-queue");
            // var endpoint = await _bus.GetSendEndpoint(uri);
            // await endpoint.Send(ticket, a =>
            // {
            //     a.SetRoutingKey("priority");
            //     a.CorrelationId = a.MessageId;
            //     a.RoutingKey();
            // });

            // await _bus.Publish(ticket);
            await _endpoint.Publish(ticket, a =>
            {
                a.SetRoutingKey("priority");
                a.CorrelationId = a.MessageId;
                a.RoutingKey();
            });
            // await _endpoint.Publish(ticket);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        };

        return Ok();
    }
}