///////////////////////////////////////////////////////////////////////////////
// BUILD TASKS
///////////////////////////////////////////////////////////////////////////////

#define DEBUG

#load ".cake/build/build.cake"
#load ".cake/tasks/tasks.cake"

SetupNLogLogging();

Task("Default")
   .Description("Run default task")
   .IsDependentOn("tasks")
   .Does(() =>
   {
      #break
      Information("Task completed successfully");
   });

RunTarget(target);