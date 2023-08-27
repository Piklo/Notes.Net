using DbUp;
using Microsoft.Extensions.Configuration;
using System.Reflection;

var builder = new ConfigurationBuilder();
builder.AddUserSecrets<Program>();
var config = builder.Build();

var connectionString = config.GetSection("Notes:ConnectionString").Value;


var upgrader = DeployChanges.To.SQLiteDatabase(connectionString)
    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
    .LogToConsole()
    .Build();

var result = upgrader.PerformUpgrade();

if (!result.Successful)
{
    Console.WriteLine(result.Error);
}
else
{
    Console.WriteLine("success!");
}

Console.WriteLine();
