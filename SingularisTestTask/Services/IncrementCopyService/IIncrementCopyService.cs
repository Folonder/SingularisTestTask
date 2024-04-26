namespace SingularisTestTask.Services.IncrementCopyService;

public interface IIncrementCopyService
{
    /// <summary>
    /// Runs IncrementsCopyService
    /// </summary>
    /// <param name="startTime"></param>
    public void Run(DateTime startTime);
}