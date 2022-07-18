using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Model
{
    public class KafkaSetting
    {
        public string BootstrapServers { get; set; }

        public string SchemaRegistryUrl { get; set; }
        public Dictionary<string, string> Topics{ get; set; }
        public string GroupId { get; set; }
    }
}
