namespace SingularisTestTask.Infrastructure;

public class IncrementCopyRepositoryOptions
{
    public const string Key = "IncrementCopyRepositoryOptions";
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
}