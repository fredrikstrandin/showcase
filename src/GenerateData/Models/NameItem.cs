using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateData.Models
{
    public class NameItem
    {        
        public NameItem(string name, string gender)
        {
            Name = name;
            Gender = gender;
        }

        public string Name { get; }
        public string Gender { get; }

    }
}
