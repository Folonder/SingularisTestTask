using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using SingularisTestTask.Extensions;
using SingularisTestTask.Infrastructure;
using SingularisTestTask.Services.IncrementCopyService;
using SingularisTestTask.Workers.SchedulerWorker;
using SingularisTestTask.Workers.SchedulerWorker.Jobs;

namespace SingularisTestTask;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);
        builder.ConfigureAppConfiguration((config) =>
        {
            config.AddJsonFile("appsettings.json", false, true);
        });
        
        builder.ConfigureServices((context, services) =>
        {
            services.Configure<SchedulerWorkerOptions>(context.Configuration.GetSection(SchedulerWorkerOptions.Key));
            services.Configure<IncrementCopyServiceOptions>(context.Configuration.GetSection(IncrementCopyServiceOptions.Key));
            services.Configure<IncrementCopyRepositoryOptions>(context.Configuration.GetSection(IncrementCopyRepositoryOptions.Key));
            
            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<IncrementCopyJob>();

            services.AddSingleton<IIncrementCopyService, IncrementCopyService>();
            services.AddSingleton<IIncrementCopyRepository, IncrementCopyRepository>();
            
            services.AddHostedService<SchedulerWorker>();
        });

        await builder.Build().RunAsync();
    }
}