namespace EmployeeExporter.Worker;
public record EmployeeDto(int Id, string FirstName, string LastName, string Department, decimal Salary, DateTime HireDate);
