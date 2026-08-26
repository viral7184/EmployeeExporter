using System.Data;
using System.Globalization;
using CsvHelper;
using Dapper;
using Microsoft.Data.SqlClient;

namespace EmployeeExporter.Worker;

public interface IDataRepository
{
    Task<IEnumerable<EmployeeDto>> GetEmployeeDataAsync();
}

public class SqlDataRepository(IConfiguration configuration) : IDataRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection") 
        ?? throw new InvalidOperationException("Connection string missing.");

    public async Task<IEnumerable<EmployeeDto>> GetEmployeeDataAsync()
    {
        using IDbConnection db = new SqlConnection(_connectionString);
        return await db.QueryAsync<EmployeeDto>("sp_GetEmployees", commandType: CommandType.StoredProcedure);
    }
}

public interface ICsvGenerator
{
    Task GenerateCsvAsync(IEnumerable<EmployeeDto> data, string filePath);
}

public class CsvGenerator : ICsvGenerator
{
    public async Task GenerateCsvAsync(IEnumerable<EmployeeDto> data, string filePath)
    {
        await using var writer = new StreamWriter(filePath);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        await csv.WriteRecordsAsync(data);
    }
}
