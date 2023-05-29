# CSharpTask
Microframework with provides tasks functionality into C# console application similar to rake in Ruby

## How to implement new task

Check `CSharpTask.ExampleApp/Tasks/Dummy` task example

1. Implement 3 classes inside `{RootNamespace}.{TaskName}` namespace:
     * `{TaskName}Task` - with task implementation
     * `{TaskName}Options` - for task configuration
     * `{TaskName}Startup` - to register the task with its dependencies
2. Provide `TaskDescriptionAttribute` with values for `{TaskName}Task` class

## How to run task

Check `CSharpTask.ExampleApp/Program.cs` for the example setup.

1. Register the task in your application using the `TaskExtensions.UseStartup` with task name provided from application arguments
2. Validate task name from arguments with `TaskExtension.ValidateRequestedTask` method.
3. Run application with task name as an argument to execute task
4. Run application without a ny arguments to display all implemented tasks

Example output from the `CSharpTask.ExampleApp`

```
dotnet run --project CSharpTask.ExampleApp/CSharpTask.ExampleApp.csproj

Dummy: this is an example of task description
```

```
dotnet run --project CSharpTask.ExampleApp/CSharpTask.ExampleApp.csproj -- Dummy

DummyTask.ExecuteAsync Test Setting
```
