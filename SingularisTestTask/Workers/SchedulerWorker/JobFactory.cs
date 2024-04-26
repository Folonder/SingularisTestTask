using Quartz;
using Quartz.Spi;

namespace SingularisTestTask.Workers.SchedulerWorker;

public class JobFactory : IJobFactory
{
    private readonly IServiceProvider _service;
        
    public JobFactory(IServiceProvider serviceProvider)
    {
        _service = serviceProvider;
    }
        
    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        var jobDetail = bundle.JobDetail;
        return (IJob)_service.GetService(jobDetail.JobType)!;
    }

    public void ReturnJob(IJob job)
    {
            
    }
}