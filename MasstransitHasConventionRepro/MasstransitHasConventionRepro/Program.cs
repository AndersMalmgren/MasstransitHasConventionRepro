using MassTransit;
using MassTransit.Configuration;
using MasstransitHasConventionRepro;
using MasstransitHasConventionRepro.Entities;
using MasstransitHasConventionRepro.Events;
using MasstransitHasConventionRepro.Saga;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var sp = new ServiceCollection()
    .AddDbContext<MyDbContext>(b => b.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyReproDb;Integrated Security=True;"))
    .AddLogging()

    .AddMassTransit(cfg =>
    {
            cfg.AddSagaStateMachine<MySagaStateMachine, MySagaState>();
            cfg.SetEntityFrameworkSagaRepositoryProvider(r => r.ExistingDbContext<MyDbContext>());

            cfg.UsingInMemory((ctx, mem) => mem.ConfigureEndpoints(ctx));
        })


    .BuildServiceProvider();

Console.WriteLine($"Using version {typeof(EntityFrameworkSagaRepositoryRegistrationProvider).Assembly.GetName().Version} of EntityFrameworkSagaRepositoryProvider");
Console.WriteLine("Creating DB");
await sp.GetRequiredService<MyDbContext>().Database.EnsureCreatedAsync();
Console.WriteLine("DB Created");

Console.WriteLine("Starting services");
var hosts = sp.GetServices<IHostedService>();
var source = new CancellationTokenSource();
foreach (var host in hosts) await host.StartAsync(source.Token);

Console.WriteLine("Services started");
var guid = Guid.NewGuid();

var bus = sp.GetRequiredService<IBus>();
await bus.Publish(new MyEvent { Id = guid });
await bus.Publish(new MyEvent { Id = guid });
await bus.Publish(new MyEvent { Id = guid });


Console.ReadLine();
