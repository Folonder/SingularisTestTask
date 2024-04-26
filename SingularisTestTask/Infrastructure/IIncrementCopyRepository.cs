using SingularisTestTask.Models;

namespace SingularisTestTask.Infrastructure;

public interface IIncrementCopyRepository
{
    /// <summary>
    /// Creates model or updates model if it exists
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public Task CreateOrUpdateAsync(IncrementCopyModel model);

    /// <summary>
    /// Gets model by destination folder (unique key)
    /// </summary>
    /// <param name="destinationFolder"></param>
    /// <returns></returns>
    public Task<IncrementCopyModel?> Get(string destinationFolder);
    
    public Task DeleteAsync(string destinationFolder);
}