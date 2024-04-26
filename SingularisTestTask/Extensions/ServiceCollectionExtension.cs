using SingularisTestTask.Infrastructure;
using SingularisTestTask.Services.IncrementCopyService;

namespace SingularisTestTask.Extensions;

public static class ServiceCollectionExtension
{
    public static void ReconfigureOptionsIfInDocker(this IServiceCollection services)
    {
        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")))
        {
            return;
        }
        
        services.Configure<IncrementCopyRepositoryOptions>(options =>
        {
            options.ConnectionString = options.ConnectionString.ChangeHost("host.docker.internal");
        });
            
        var sourceFolder = Environment.GetEnvironmentVariable("SOURCE_VOLUME");
        var destinationFolder = Environment.GetEnvironmentVariable("DESTINATION_VOLUME");
            
        if (!string.IsNullOrEmpty(sourceFolder))
        {
            services.Configure<IncrementCopyServiceOptions>(options =>
            {
                options.SourceFolder = sourceFolder;
            });
        }

        if (!string.IsNullOrEmpty(destinationFolder))
        {
            services.Configure<IncrementCopyServiceOptions>(options =>
            {
                options.DestinationFolder = destinationFolder;
            });
        }
    }
}