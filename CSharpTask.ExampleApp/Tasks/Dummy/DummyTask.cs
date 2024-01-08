using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using WMA;

namespace CSharpTask.ExampleApp.Tasks.Dummy
{
  [TaskDescription(name: "Dummy", description: "this is an example of task description")]
  public class DummyTask : ITask
  {
    private readonly IOptions<DummyOptions> options;

    public DummyTask(IOptions<DummyOptions> options)
    {
      this.options = options;
    }

    public Task ExecuteAsync(CancellationToken token)
    {
      Console.WriteLine($"DummyTask.ExecuteAsync {this.options.Value.AnyOption}");
      return Task.CompletedTask;
    }
  }
}
