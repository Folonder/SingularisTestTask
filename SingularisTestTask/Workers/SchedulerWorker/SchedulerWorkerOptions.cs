namespace SingularisTestTask.Workers.SchedulerWorker;

public class SchedulerWorkerOptions
{
    public const string Key = "SchedulerWorkerOptions";
    public List<JobCronModel> JobCronModels { get; set; }
}