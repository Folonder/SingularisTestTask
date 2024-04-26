using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Spi;
using SingularisTestTask.Workers.SchedulerWorker.Helpers;
using SingularisTestTask.Workers.SchedulerWorker.Jobs;

namespace SingularisTestTask.Workers.SchedulerWorker;

public class SchedulerWorker : IHostedService
{
    private IScheduler _scheduler;
    private readonly IJobFactory _jobFactory;
    private List<JobMetadata> _jobsMetadata = new();
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly IOptionsMonitor<SchedulerWorkerOptions> _schedulerJobsOptions;
    private readonly ILogger<SchedulerWorker> _logger;
    
    public SchedulerWorker(ILogger<SchedulerWorker> logger,
        IOptionsMonitor<SchedulerWorkerOptions> schedulerOptions,
        IJobFactory jobFactory,
        ISchedulerFactory schedulerFactory)
    {
        _logger = logger;
        _schedulerJobsOptions = schedulerOptions;
        _jobFactory = jobFactory;
        _schedulerFactory = schedulerFactory;
        _jobsMetadata.Add(new JobMetadata(Guid.NewGuid(), typeof(IncrementCopyJob), "Increment Copy Job"));
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _jobsMetadata = JobMetadata.MergeCronData(_jobsMetadata, _schedulerJobsOptions.CurrentValue.JobCronModels);
        _schedulerJobsOptions.OnChange(async _ => await HandleOptionsChangeAsync(cancellationToken));
        await StartSchedulerAsync(cancellationToken);
    }
    
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _scheduler.Shutdown(true, cancellationToken);
    }
    
    private async Task StartSchedulerAsync(CancellationToken cancellationToken)
    {
        _scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
        _scheduler.JobFactory = _jobFactory;

        foreach (var jobMetadata in _jobsMetadata)
        {
            var jobDetail = CreateJob(jobMetadata);
            var trigger = CreateTrigger(jobMetadata);

            _scheduler.ScheduleJob(jobDetail, trigger, cancellationToken).GetAwaiter();
        }
        
        await _scheduler.Start(cancellationToken);
    }
    
    private ITrigger CreateTrigger(JobMetadata jobMetadata)
    {
        try
        {
            return TriggerBuilder.Create()
                .WithIdentity(jobMetadata.Id.ToString())
                .WithCronSchedule(CronToQuartzHelper.GetQuartz(jobMetadata.CronExpression))
                .WithDescription(jobMetadata.Name)
                .Build();
        }
        catch (FormatException)
        {
            _logger.LogError("Invalid Cron expression");
            Environment.Exit(1);
            return null;
        }
    }

    private IJobDetail CreateJob(JobMetadata jobMetadata)
    {
        return JobBuilder.Create(jobMetadata.Type)
            .WithIdentity(jobMetadata.Id.ToString())
            .WithDescription(jobMetadata.Name)
            .Build();
    }
    
    private async Task HandleOptionsChangeAsync(CancellationToken cancellationToken)
    {
        await _scheduler.Shutdown(true, cancellationToken);
        
        _jobsMetadata = JobMetadata.MergeCronData(_jobsMetadata, _schedulerJobsOptions.CurrentValue.JobCronModels);

        await StartSchedulerAsync(cancellationToken);
    }
}
