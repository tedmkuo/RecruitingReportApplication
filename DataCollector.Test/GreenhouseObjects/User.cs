using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector.Test.GreenhouseObjects
{
    class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Site_admin { get; set; }
        public List<string> Emails { get; set; }
    }
}
