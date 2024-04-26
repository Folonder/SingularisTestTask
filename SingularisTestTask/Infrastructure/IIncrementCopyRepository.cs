using SingularisTestTask.Models;

namespace SingularisTestTask.Infrastructure;

public interface IIncrementCopyRepository
{
    public Task CreateOrUpdateAsync(IncrementCopyModel model);

    public Task<IncrementCopyModel?> Get(string destinationFolder);
    
    public Task DeleteAsync(string destinationFolder);
}