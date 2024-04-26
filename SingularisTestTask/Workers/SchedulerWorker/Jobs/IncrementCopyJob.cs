using Quartz;

namespace SingularisTestTask.Workers.SchedulerWorker.Jobs;

public class IncrementCopyJob : IJob
{
    private readonly ILogger<IncrementCopyJob> _logger;
    private bool _isRunning;
        
    public IncrementCopyJob(ILogger<IncrementCopyJob> logger)
    {
        _logger = logger;
    }
        
    public Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Job triggered");
        return Task.CompletedTask;
    }
}