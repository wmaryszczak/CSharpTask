using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMA
{
  [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false)]
  public class TaskDescriptionAttribute : Attribute
  {
    public string Name;
    public string Description;

    public TaskDescriptionAttribute(string name, string description)
    {
      Name = name;
      Description = description;
    }
  }
}
