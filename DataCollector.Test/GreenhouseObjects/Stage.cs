using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Test.GreenhouseObjects
{
    class Stage
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Interview> Interviews { get; set; }
    }
}
