using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WMA
{
  public static class TaskExtension
  {
    public static IHostBuilder UseStartup(this IHostBuilder hostBuilder, string taskName)
    {
      hostBuilder.ConfigureServices((ctx, serviceCollection) =>
      {
        var asm = Assembly.GetEntryAssembly();
        if (asm == null)
        {
          throw new InvalidOperationException("Cannot use the assembly called from 'Assembly.GetEntryAssembly()'");
        }
        var type = FindStartupType(asm!, taskName);

        var cfgServicesMethod = type.GetMethod("ConfigureServices",
          new Type[] { typeof(IServiceCollection) });

        var hasConfigCtor = type.GetConstructor(
          new Type[] { typeof(IConfiguration) }) != null;

        var startUpObj = hasConfigCtor ?
          Activator.CreateInstance(type, ctx.Configuration) :
          Activator.CreateInstance(type, null);

        cfgServicesMethod?.Invoke(startUpObj, new object[] { serviceCollection });
      });
      return hostBuilder;
    }

    public static Type FindStartupType(Assembly asm, string taskName)
    {
      var startupName = $"{asm.GetName().Name}.Tasks.{taskName}.{taskName}Startup";
      var type = asm.GetType(startupName, throwOnError: false, ignoreCase: true);
      if(type == null)
      {
        throw new InvalidOperationException($"Cannot find {startupName} in {asm.FullName}");
      }
      return type!;
    }

    public static IEnumerable<TaskDescriptionAttribute> GetTasksDescription(this Assembly assembly)
    {
      foreach (var type in assembly.GetTypesWithHelpAttribute())
      {
        var dnAttribute = type.GetCustomAttributes(
            typeof(TaskDescriptionAttribute), true
        ).FirstOrDefault() as TaskDescriptionAttribute;
        if (dnAttribute != null)
        {
          yield return dnAttribute;
        }
      }
    }

    private static IEnumerable<Type> GetTypesWithHelpAttribute(this Assembly assembly)
    {
      foreach (Type type in assembly.GetTypes())
      {
        if (type.GetCustomAttributes(typeof(TaskDescriptionAttribute), true).Length > 0)
        {
          yield return type;
        }
      }
    }

    public static string? ValidateRequestedTask(string? taskName)
    {
      var tasks = Assembly.GetEntryAssembly()?.GetTasksDescription();
      if (tasks?.Any() == false)
      {
        throw new ApplicationException("There are no tasks to execute. Please attach 'TaskDescription' to at least one task");
      }
      if (string.IsNullOrWhiteSpace(taskName))
      {
        tasks!.PrintTasks();
        return null;
      }
      var task = tasks!.FirstOrDefault(td => td.Name.Equals(taskName, StringComparison.OrdinalIgnoreCase));
      if (task == null)
      {
        tasks!.PrintTasks();
        throw new ArgumentException($"Task '{taskName}' not found.");
      }
      return task.Name;
    }

    public static void PrintTasks(this IEnumerable<TaskDescriptionAttribute> tasks)
    {
      foreach(var t in tasks)
      {
        System.Console.WriteLine($"{t.Name}: {t.Description}");
      }
    }
  }
}
