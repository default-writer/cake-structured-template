///////////////////////////////////////////////////////////////////////////////
// HELPER METHODS
///////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;

void RunExternalTasks(params Task[] list)
{
   foreach(var task in list)
   {
      if (task.Commands.Length == 0)
      {
         continue;
      }
      if (task.Commands.Length == 1)
      {
         RunExternalTask(
            task.Commands[0]
         );
         continue;
      }
      RunExternalTask(
         task.Commands
      );
   }
}

void RunUnloggedExternalTask(params string[] list)
{
   RunUnloggedExternalTaskAt(new List<string>(list), GetRootDirectory());
}

void RunUnloggedExternalTaskAt(List<string> list, DirectoryPath workingDirectory=null)
{
   var command = list.First();
   var builder = new ProcessArgumentBuilder();
   foreach(var arg in list.Skip(1))
   {
      builder.Append(arg);
   }

   var settings = new ProcessSettings() { Arguments = builder };
   if (workingDirectory != null)
   {
      settings.WorkingDirectory = workingDirectory;
   }
   var commandArgs = string.Join(" ", builder);
   Information("");
   Information("========================================");
   Information($"{command} {commandArgs}");
   Information("========================================");
   Information("");

   settings.RedirectStandardOutput = true;
   settings.RedirectStandardError = true;
   settings.RedirectedStandardOutputHandler = (s) =>
   {
      if (s != null)
      {
         Information(s);
      }
      return s;
   };
   settings.RedirectedStandardErrorHandler = (s) =>
   {
      if (s != null)
      {
         Information(s);
      }
      return s;
   };

   using(var process = StartAndReturnProcess(command, settings))
   {
      process.WaitForExit();
      Information("");
      if (process.GetExitCode() != 0)
      {
         throw new Exception("Task execution failed");
      }
   }
}

void RunExternalTask(params string[] list)
{
   RunExternalTaskAt(new List<string>(list));
}

void LogInformation(string format, params object[] args)
{
   NLog.Logger log = NLog.LogManager.GetLogger(taskName);
   log.Info(format, args);
   Information(format, args);
}

void LogSetupInformation(string format, params object[] args)
{
   NLog.Logger log = NLog.LogManager.GetLogger("SYSTEM");
   log.Info(format, args);
   Information(format, args);
}

void RunExternalTaskAt(List<string> list, DirectoryPath workingDirectory=null)
{
   NLog.Logger log = NLog.LogManager.GetLogger(taskName);

   var command = list.First();
   var builder = new ProcessArgumentBuilder();
   foreach(var arg in list.Skip(1))
   {
      builder.Append(arg);
   }

   var settings = new ProcessSettings() { Arguments = builder };
   if (workingDirectory != null)
   {
      settings.WorkingDirectory = workingDirectory;
   }
   var commandArgs = string.Join(" ", builder);
   LogInformation("");
   LogInformation("========================================");
   LogInformation($"{command} {commandArgs}");
   LogInformation("========================================");
   LogInformation("");

   settings.RedirectStandardOutput = true;
   settings.RedirectStandardError = true;
   settings.RedirectedStandardOutputHandler = (s) =>
   {
      if (s != null)
      {
         log.Info(s);
         LogInformation(s);
      }
      return s;
   };
   settings.RedirectedStandardErrorHandler = (s) =>
   {
      if (s != null)
      {
         log.Info(s);
         LogInformation(s);
      }
      return s;
   };

   using(var process = StartAndReturnProcess(command, settings))
   {
      process.WaitForExit();
      LogInformation("");
      if (process.GetExitCode() != 0)
      {
         throw new Exception("Task execution failed");
      }
   }
}

string GetRootDirectory()
{
   var result = new StringBuilder();
   var list = new List<string> { "git", "rev-parse", "--show-toplevel" };
   var command = list.First();
   var builder = new ProcessArgumentBuilder();
   foreach(var arg in list.Skip(1))
   {
      builder.Append(arg);
   }

   var settings = new ProcessSettings() { Arguments = builder };
   var commandArgs = string.Join(" ", builder);

   settings.RedirectStandardOutput = true;
   settings.RedirectStandardError = true;
   settings.RedirectedStandardOutputHandler = (s) =>
   {
      if (!string.IsNullOrWhiteSpace(s))
      {
         result.Append(s);
      }
      return s;
   };
   using(var process = StartAndReturnProcess(command, settings))
   {
      process.WaitForExit();
      if (process.GetExitCode() != 0)
      {
         throw new Exception("Task execution failed");
      }
   }
   return result.ToString();
}

void SetupNLogLogging()
{
   var config = new NLog.Config.LoggingConfiguration();
   var logfiletarget = new NLog.Targets.FileTarget { FileName = _nlog_.FullPath };
   // var logconsoletarget = new NLog.Targets.ConsoleTarget();
   // var dbTarget = new DatabaseTarget();
   // dbTarget.ConnectionString = @"<server>;Initial Catalog=<adatabase>;Persist Security Info=True;User ID=<user>;Password=<password>";
   // dbTarget.CommandText = @"INSERT INTO [Log] (Date, Thread, Level, Logger, Message, Exception) VALUES(GETDATE(), @thread, @level, @logger, @message, @exception)";
   // dbTarget.Parameters.Add(new DatabaseParameterInfo("@thread", new NLog.Layouts.SimpleLayout("${threadid}")));
   // dbTarget.Parameters.Add(new DatabaseParameterInfo("@level", new NLog.Layouts.SimpleLayout("${level}")));
   // dbTarget.Parameters.Add(new DatabaseParameterInfo("@logger", new NLog.Layouts.SimpleLayout("${logger}")));
   // dbTarget.Parameters.Add(new DatabaseParameterInfo("@message", new NLog.Layouts.SimpleLayout("${message}")));
   // dbTarget.Parameters.Add(new DatabaseParameterInfo("@exception", new NLog.Layouts.SimpleLayout("${exception}")));

   config.AddTarget("file", logfiletarget);
   // config.AddTarget("console", logconsoletarget);
   // config.AddTarget("database", dbTarget);

   var logfilerule = new NLog.Config.LoggingRule("*", NLog.LogLevel.Debug, logfiletarget);
   // var logconsolerule = new NLog.Config.LoggingRule("*", NLog.LogLevel.Debug, logconsoletarget);
   // var dbRule = new LoggingRule("*", LogLevel.Debug, dbTarget);

   config.LoggingRules.Add(logfilerule);
   // config.LoggingRules.Add(logconsolerule);
   // config.LoggingRules.Add(dbRule);

   NLog.LogManager.Configuration = config;
}