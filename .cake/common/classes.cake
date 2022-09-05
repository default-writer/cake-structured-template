///////////////////////////////////////////////////////////////////////////////
// CLASSES
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

using System.IO;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;

public class Task: ITask
{
   public string[] Commands { get; set; }
   private Task(params string[] commands)
   {
      Commands = commands;
   }
   public static implicit operator Task(string command) => new Task(command.Split(" "));
   public static implicit operator string[](Task task) => task.Commands;
}