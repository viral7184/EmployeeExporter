using EmployeeExporter.Worker;
using FluentAssertions;

namespace EmployeeExporter.Tests;

public class CsvGeneratorTests
{
    [Fact]
    public async Task GenerateCsvAsync_ShouldWriteValidCsvFormat()
    {
        var generator = new CsvGenerator();
        var testFilePath = $"test_emp_{Guid.NewGuid()}.csv";
        var sampleData = new List<EmployeeDto>
        {
            new(1, "John", "Doe", "IT", 60000.00m, new DateTime(2026, 1, 1))
        };

        try
        {
            await generator.GenerateCsvAsync(sampleData, testFilePath);
            var fileContent = await File.ReadAllTextAsync(testFilePath);
            var lines = fileContent.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            
            lines.Should().HaveCount(2); 
            lines[0].Should().Be("Id,FirstName,LastName,Department,Salary,HireDate");
            lines[1].Should().Contain("John").And.Contain("IT").And.Contain("60000.00");
        }
        finally
        {
            if (File.Exists(testFilePath)) File.Delete(testFilePath);
        }
    }
}
