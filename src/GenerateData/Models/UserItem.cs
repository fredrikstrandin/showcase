using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateData.Models
{
    public class UserItem
    {
        public UserItem(string firstname, string lastname, string gender, string email, string street, string streetNo, string city, string postcode)
        {
            Firstname = firstname;
            Lastname = lastname;
            Gender = gender;
            Email = email;
            StreetNo = streetNo;
            Street = street;
            City = city;
            Postcode = postcode;
        }

        public string Firstname { get; }
        public string Lastname { get; }
        public string Gender { get; }
        public string Email { get; }
        public string StreetNo { get; }
        public string Street { get; }
        public string City { get; }
        public string Postcode { get; }
    }
}
