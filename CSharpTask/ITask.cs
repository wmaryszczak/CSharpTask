using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMA
{
  public interface ITask
  {
    public Task ExecuteAsync(CancellationToken token);
  }
}
