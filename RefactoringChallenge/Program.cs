using RefactoringChallenge;
using RefactoringChallenge.Dal.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddDatabase(builder.Configuration.GetConnectionString(Constants.ConfigurationKeys.ConnectionString) ??
                 throw new Exception("Missing connection string."));

var host = builder.Build();
host.Run();
