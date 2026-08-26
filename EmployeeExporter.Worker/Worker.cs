namespace EmployeeExporter.Worker;

public class Worker(
    IDataRepository dataRepository,
    ICsvGenerator csvGenerator,
    IConfiguration configuration,
    ILogger<Worker> logger,
    IHostApplicationLifetime hostLifetime) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogInformation("Starting Employee CSV export process...");
            var filePath = configuration["ExportSettings:OutputFilePath"] ?? "employees.csv";
            
            var data = await dataRepository.GetEmployeeDataAsync();
            var employeeList = data.ToList();
            logger.LogInformation("Fetched {Count} employees. Generating CSV...", employeeList.Count);

            await csvGenerator.GenerateCsvAsync(employeeList, filePath);
            logger.LogInformation("CSV successfully generated at {FilePath}", Path.GetFullPath(filePath));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Export process failed.");
            Environment.ExitCode = 1;
        }
        finally
        {
            hostLifetime.StopApplication();
        }
    }
}
