///////////////////////////////////////////////////////////////////////////////
// BUILD TASKS
///////////////////////////////////////////////////////////////////////////////

#load "../../.cake/common/common.cake"

Task("task1")
   .Description("Does task 1")
   .Does(() =>
   {
      #break
      Information("Task completed successfully");
   });

Task("task2")
   .Description("Does task 2")
   .Does(() =>
   {
      #break
      Information("Task completed successfully");
   });

Task("tasks")
   .Description("Run several tasks")
   .IsDependentOn("task1")
   .IsDependentOn("task2")
   .Does(() =>
   {
      #break
      Information("Task completed successfully");
   });

#if (DEBUG)
#else

Task("Default")
   .Description("Default")
   .IsDependentOn("tasks")
   .Does(() =>
   {
      #break
      Information("Task completed successfully");
   });

RunTarget(target);

#endif