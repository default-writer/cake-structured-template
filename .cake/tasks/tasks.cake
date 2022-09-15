///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

#load "../../.cake/common/common.cake"

Task("reset").Does(() => RunUnloggedExternalTask("git", "reset", "HEAD", "--hard"));

Task("cleanup").Does(() => RunUnloggedExternalTask("git", "clean", "-f", "-d", "-x", "-e", ".logs", "-e", ".tools", "-e", ".dotnet"));

#if (DEBUG)
#else

Task("Default")
   .Description("Default")
   .Does(() =>
   {
      #break
      LogInformation("Task completed successfully");
   });

RunTarget(target);

#endif