using MassTransit;
using MessageBroker.Consumer;

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
                    r.UseMessageRetry(r => r.Interval(2, 100));
                    r.ConfigureConsumer<TicketConsumer>(provider);
                });

            }));

            //Aqui está uma outra forma de configurar
            c.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("admin");
                    h.Password("admin");
                });


                cfg.ReceiveEndpoint("ticket-queue", r =>
                {
                    r.UseMessageRetry(r => r.Interval(2, 100));
                    r.ConfigureConsumer<TicketConsumer>(context);
                });
            });

        });
    })
    .Build();

await host.RunAsync();