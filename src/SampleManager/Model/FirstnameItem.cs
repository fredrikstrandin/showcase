﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateData.Models
{
    public class FirstnameItem
    {        
        public NameItem(string firstname, string lastname, string gender)
        {
            Firstname = firstname;
            Gender = gender;
        }

        public string Firstname { get; }
        public string Gender { get; }

    }
}