// See https://aka.ms/new-console-template for more information

using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WMA;

IHost host = null!;

try
{
  var taskName = TaskExtension.ValidateRequestedTask(args.FirstOrDefault());
  if (string.IsNullOrEmpty(taskName))
  {
    Environment.Exit(0);
  }

  var hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
      logging.ClearProviders();
      logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    })
    .UseStartup(taskName)
    .UseConsoleLifetime()
  ;
  using (host = hostBuilder.Build())
  {
    var appLifeTime = host.Services.GetRequiredService<IHostApplicationLifetime>();
    var taskRunner = host.Services.GetRequiredService<ITask>();
    await taskRunner.ExecuteAsync(appLifeTime.ApplicationStopping);
  }
}
catch (Exception ex)
{
  Console.WriteLine(ex);
  if (host != null)
  {
    await host.StopAsync();
  }
  throw;
}
