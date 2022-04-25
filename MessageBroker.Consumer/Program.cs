
using MassTransit;
using MessageBroker.Consumer;
using RabbitMQ.Client;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(c =>
        {
            c.AddConsumer<TicketConsumer>();
            c.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("admin");
                    h.Password("admin");
                });
        
                cfg.ReceiveEndpoint("ticket-queue", r =>
                {
                    r.ConfigureConsumeTopology = false;
                    r.ExchangeType = ExchangeType.Direct;
                    
                    r.ConfigureConsumer<TicketConsumer>(provider);
                    
                });
                
            }));
            
        });
    })
    .Build();

await host.RunAsync();