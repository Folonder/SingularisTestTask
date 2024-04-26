namespace SingularisTestTask.Workers.SchedulerWorker;

public class JobMetadata
{
    public Guid Id { get; }
    public Type Type { get; set; }
    public string Name { get; set; }
    public string CronExpression { get; set; }

    public JobMetadata(Guid id, Type type, string name)
    {
        Id = id;
        Type = type;
        Name = name;
    }

    public static List<JobMetadata> MergeCronData(List<JobMetadata> jobsMetadata, List<JobCronModel> models)
    {
        foreach (var jobMetadata in jobsMetadata)
        {
            jobMetadata.CronExpression = models.First(x => x.JobName == jobMetadata.Name).CronExpression;
        }

        return jobsMetadata;
    }
}