using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiUsageExamples.ConsoleProject.MongoModels
{
    internal class ActionStatus
    {
        public string Status { get; set; } = default!;
        public int StatusProgressCounter { get; set; }
    }
}
