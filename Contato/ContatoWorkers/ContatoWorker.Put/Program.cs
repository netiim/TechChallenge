using ContatoAPI.Extension;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Prometheus;
using Microsoft.AspNetCore.Hosting;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddInjecoesDependencias();
        services.AddAutoMapper();
        services.AddFluentValidation();
        services.AddDatabaseConfiguration(hostContext.Configuration);
        services.AddMassTransitWithRabbitMq(hostContext.Configuration);
    })
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.Configure(app =>
        {
            app.UseRouting();
            app.UseHttpMetrics();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapMetrics();
            });
        });
        webBuilder.UseKestrel(options =>
        {
            options.ListenAnyIP(8080);
        });
    });

var host = builder.Build();
host.Run();
