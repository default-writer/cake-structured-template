///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

var target      = Argument("target", "Default");

var taskCount   = 0;
var taskCounter = 0;

void LogInformation(string format, params object[] args)
{
   NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
   log.Info(string.Format(format, args));
   Information(format, args);
}

TaskSetup((taskSetupContext) => {
   ICakeTaskInfo task = taskSetupContext.Task;
   var dependencies = string.Join(",",task.Dependencies.Select(dependecy => dependecy.Name).ToList());
   dependencies = dependencies.Replace(",","").Trim() == "" ? "none" : dependencies;
   LogInformation("Executing Task {0} of {1}", ++taskCounter, taskCount);
   LogInformation("Name: {0}", task.Name);
   LogInformation("Description: {0}", task.Description ?? "none");
   LogInformation("Dependencies: {0}", dependencies);
});

Setup(context => {

   // declare recursive task count function
   Func<string, List<string>, int> countTask = null;
   countTask = (taskName, countedTasks) => {
      if (string.IsNullOrEmpty(taskName) || countedTasks.Contains(taskName)) return 0;

      countedTasks.Add(taskName);

      var task = Tasks.Where(t => string.Equals(t.Name, taskName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
      if (task == null) return 0;

      int result = 1;
      countedTasks.Add(task.Name);

      foreach(var dependecy in task.Dependencies.Select(dependecy => dependecy.Name))
      {
         result += countTask(dependecy, countedTasks);
      }
      return result;
   };

   // count the task and store in globally available variable
   taskCount = countTask(target, new List<string>());

   // Executed BEFORE the first task.
   Information("Running tasks...");
});

Teardown(ctx =>
{
   // Executed AFTER the last task.
   Information("Finished running tasks.");
});
