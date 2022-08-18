using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateData.Models
{
    public class UserItem
    {
        public UserItem(string id, string firstname, string lastname, string gender, string email, string street, string streetNo, string city, string postcode)
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
            Gender = gender;
            Email = email;
            StreetNo = streetNo;
            Street = street;
            City = city;
            Postcode = postcode;
        }

        public string Id { get; }
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
