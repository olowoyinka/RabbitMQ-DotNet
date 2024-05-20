using eShopMassTransit.EventBus;
using eShopMassTransit.MessageBroker;
using eShopMassTransit.Models;
using eShopMassTransit.Service;
using MassTransit;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));

builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<ProductCreatedEventConsumer>();

    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        MessageBrokerSettings settings = context.GetRequiredService<MessageBrokerSettings>();

        configurator.Host(new Uri(settings.Host), h =>
        {
            h.Username(settings.Username);
            h.Password(settings.Password);
        });

        //configurator.ReceiveEndpoint("ProductCreatedQueue", e =>
        //{
        //    e.ConfigureConsumeTopology = false;

        //    e.Consumer<ProductCreatedEventConsumer>(context);

        //    //e.PrefetchCount = 1;
        //    e.ExchangeType = ExchangeType.Direct;
        //    //e.AutoDelete = false;
        //    //e.Durable = true;
        //    e.Bind("ProductCreatedExchange", x =>
        //    {
        //        //x.AutoDelete = false;
        //        x.ExchangeType = ExchangeType.Direct;
        //        //x.Durable = true;
        //        x.RoutingKey = "ProductCreatedRouteKey";
        //    });

        //    //e.ConfigureConsumeTopology = false;
        //    //x.QueueExpiration = TimeSpan.FromDays(1);

        //    e.ConfigureConsumer<ProductCreatedEventConsumer>(context);
        //});

        configurator.Publish<ProductCreatedExchange>(x =>
        {
            //x.AutoDelete = false;
            //x.Durable = true;
            x.ExchangeType = ExchangeType.Direct;
            x.BindQueue("ProductCreatedExchange", "ProductCreatedQueue", x =>
            {
                //x.AutoDelete = false;
                x.ExchangeType = ExchangeType.Direct;
                //x.Exclusive = false;
                //x.Durable = true;
                x.RoutingKey = "ProductCreatedRouteKey";
                
                //x.QueueExpiration = TimeSpan.FromDays(1);
            });
        });
    });
});

builder.Services.AddTransient<IEventBus, EventBus>();
builder.Services.AddTransient<IProductService, ProductService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();