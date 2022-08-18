using GenerateData.Models;
using MongoDB.Bson;
using System.Security.Cryptography;
using System.Text.Json;

namespace SampleManager.Repository
{
    public class LoadDataFileRepository
    {
        private List<StreetItem> _streets;
        private List<FirstnameItem> _firstnames = new List<FirstnameItem>();
        private List<string> _lastnames = new List<string>();
        private List<string> _domans = new List<string>();


        private int _maxFirstname = 0;
        private int _maxLastname = 0;
        private int _maxStreet = 0;
        private int _maxEmail = 0;

        public LoadDataFileRepository()
        {
            using (StreamReader r = new StreamReader("Data/street.json"))
            {
                string json = r.ReadToEnd();

                if (json == null)
                {
                    throw new Exception("File for street was empty.");
                }

                _streets = JsonSerializer.Deserialize<List<StreetItem>>(json!);

                if (_streets == null)
                {
                    throw new FormatException("File for street has woring format.");
                }

                _maxStreet = _streets.Count;
            }

            using (StreamReader r = new StreamReader("Data/firstnames.csv"))
            {
                while (!r.EndOfStream)
                {
                    string line = r.ReadLine();

                    string[] nameInfo = line.Split('\u002C');

                    _firstnames.Add(new FirstnameItem(nameInfo[2], nameInfo[1]));
                }

                _maxFirstname = _firstnames.Count;
            }

            using (StreamReader r = new StreamReader("Data/lastnames.csv"))
            {
                while (!r.EndOfStream)
                {
                    string line = r.ReadLine();

                    string[] nameInfo = line.Split('\u002C');

                    _lastnames.Add(nameInfo[0]);
                }

                _maxLastname = _lastnames.Count;
            }

            using (StreamReader r = new StreamReader("Data/domans.txt"))
            {
                while (!r.EndOfStream)
                {
                    string line = r.ReadLine();

                    _domans.Add(line);
                }

                _maxEmail = _domans.Count;
            }
        }

        public UserItem GetUser()
        {
            int randomInt = RandomNumberGenerator.GetInt32(0, _maxFirstname);

            string firstname = _firstnames[randomInt].Firstname;
            string gender = _firstnames[randomInt].Gender;

            randomInt = RandomNumberGenerator.GetInt32(0, _maxLastname);
            string lastname = _lastnames[randomInt];

            randomInt = RandomNumberGenerator.GetInt32(0, _maxEmail);
            var email = $"{firstname}.{lastname}@{_domans[randomInt]}".ToLower();

            randomInt = RandomNumberGenerator.GetInt32(0, _maxStreet);
            var streetItem = _streets[randomInt];

            string street = streetItem.Street;

            string streetNo = string.Empty;
            string[] no = streetItem.StreetNo.Split('-');

            if (int.TryParse(no[0], out int first) && int.TryParse(no[1], out int second))
            {
                if (first < second)
                {
                    randomInt = RandomNumberGenerator.GetInt32(first, second);
                    streetNo = randomInt.ToString();
                }
                else
                {
                    streetNo = first.ToString();
                }
            }

            string city = streetItem.City;
            string postcode = streetItem.Postcode;

            lastname = lastname[0] + lastname.Substring(1).ToLower();
            string id = ObjectId.GenerateNewId().ToString();

            return new UserItem(id, firstname, lastname, gender, email, city, street, streetNo, postcode);
        }
    }
}