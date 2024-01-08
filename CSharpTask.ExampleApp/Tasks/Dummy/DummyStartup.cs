using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMA;

namespace CSharpTask.ExampleApp.Tasks.Dummy
{
  public class DummyStartup
  {
    private readonly IConfiguration config;

    public DummyStartup(IConfiguration config)
    {
      this.config = config;
    }

    /// <summary>
    /// Declare tasks and it's lifecycle with any dependencies
    /// </summary>
    public void ConfigureServices(IServiceCollection services)
    {
      services.Configure<DummyOptions>(this.config.GetSection(DummyOptions.SectionName));
      services.AddTransient<ITask, DummyTask>();
    }
  }
}
