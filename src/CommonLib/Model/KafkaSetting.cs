using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Model
{
    public class KafkaSetting
    {
        public string bootstrapServers { get; set; }

        public string schemaRegistryUrl { get; set; }
        public string topicName { get; set; }
        public string GroupId { get; set; }
    }
}
