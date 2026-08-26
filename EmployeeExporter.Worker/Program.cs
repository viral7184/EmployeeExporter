using EmployeeExporter.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddTransient<IDataRepository, SqlDataRepository>();
builder.Services.AddTransient<ICsvGenerator, CsvGenerator>();
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
