using Microsoft.Extensions.Logging;
using Quartz;

namespace QuartzClusteredDemo.Jobs;

public class ExampleJob(ILogger<ExampleJob> logger) : IJob
{
    private readonly ILogger<ExampleJob> _logger = logger;

    public async Task Execute(IJobExecutionContext context)
    {
        var dataMap = context.JobDetail.JobDataMap;
        var guid = dataMap.GetGuidValue("GUID");

        var jobName = context.JobDetail.Key.Name;

        _logger.LogInformation("Job '{jobName}': started with GUID={guid}", jobName, guid);

        const int totalSteps = 10;
        for (int step = 1; step <= totalSteps; step++)
        {
            _logger.LogInformation("Job '{jobName}': step {step}/{totalSteps}", jobName, step, totalSteps);

            await Task.Delay(5000);
        }

        _logger.LogInformation("Job '{jobName}': completed", jobName);
    }
}
