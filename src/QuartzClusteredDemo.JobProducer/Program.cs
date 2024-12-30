using NLog.Extensions.Logging;
using Quartz;
using QuartzClusteredDemo.JobProducer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, configBuilder) =>
    {
        configBuilder.AddJsonFile("quartz.json", optional: false, reloadOnChange: false);
    })
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;

        services.AddLogging(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Debug);
            builder.AddNLog(new NLogProviderOptions
            {
                ReplaceLoggerFactory = true,
            });
        });
        
        services.Configure<QuartzOptions>(configuration.GetSection("Quartz"));

        services.AddQuartz();

        services.AddHostedService<ProducerWorker>();
    })
    .Build();

host.Run();
