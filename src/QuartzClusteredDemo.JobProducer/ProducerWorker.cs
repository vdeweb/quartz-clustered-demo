using Quartz;
using QuartzClusteredDemo.Jobs;

namespace QuartzClusteredDemo.JobProducer;

public class ProducerWorker(ILogger<ProducerWorker> logger, ISchedulerFactory schedulerFactory)
    : BackgroundService
{
    private readonly ILogger<ProducerWorker> _logger = logger;

    private readonly ISchedulerFactory _schedulerFactory = schedulerFactory;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await Task.Yield();

        try
        {
            var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);

            while (!cancellationToken.IsCancellationRequested)
            {
                var jobName = $"{nameof(ExampleJob)}_{DateTime.Now.Ticks}";

                var job = JobBuilder.Create<ExampleJob>()
                    .WithIdentity(jobName, "DefaultGroup")
                    .UsingJobData("GUID", Guid.NewGuid())
                    .Build();

                var trigger = TriggerBuilder.Create()
                    .WithIdentity($"Trigger_{jobName}", "DefaultGroup")
                    .StartNow()
                    .Build();

                await scheduler.ScheduleJob(job, trigger, cancellationToken);

                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "JobProducer has stopped with exception");
        }
    }
}
