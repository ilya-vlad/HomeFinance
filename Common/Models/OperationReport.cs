using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class OperationReport
    {
        public decimal Income { get; set; }

        public decimal Expenses { get; set; }

        public IEnumerable<Operation> Operations { get; set; }
    }
}
