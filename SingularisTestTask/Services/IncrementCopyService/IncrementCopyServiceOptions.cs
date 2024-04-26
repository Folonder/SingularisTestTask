namespace SingularisTestTask.Services.IncrementCopyService;

public class IncrementCopyServiceOptions
{
    public const string Key = "IncrementCopyServiceOptions";
    
    public string SourceFolder { get; set; }
    
    public string DestinationFolder { get; set; }
}