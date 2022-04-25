using MassTransit;
using MessageBroker.Shared.Models;

namespace MessageBroker.Consumer;

public class TicketConsumer : IConsumer<Ticket>
{
    private readonly ILogger<TicketConsumer> _logger;

    public TicketConsumer(ILogger<TicketConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<Ticket>? context)
    {
        if (context != null) _logger.LogInformation(context.Message.UserName);

        return Task.CompletedTask;
    }
}