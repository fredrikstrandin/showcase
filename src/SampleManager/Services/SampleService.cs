using SampleManager.Interface;
using System.Text.Json;
using UserManager.Model;

namespace SampleManager.Services
{
    public class SampleService : ISampleService
    {
        private readonly ISampleRepository _sampleRepository;

        public SampleService(ISampleRepository sampleRepository)
        {
            _sampleRepository = sampleRepository;
        }

        public Task<UsersRespons> CreateUsersAsync(int count)
        {
            List<StreetItem> streets;
            List<NameItem> firstnames = new List<NameItem>();
            List<string> lastnames = new List<string>();
            List<string> emails = new List<string>();
            List<string> ids = new List<string>();
            List<UserCreateRequestMessage> users = new List<UserCreateRequestMessage>();

            int maxFirstname = 0;
            int maxLastname = 0;
            int maxStreet = 0;
            int maxEmail = 0;
            using (StreamReader r = new StreamReader("Data/street.json"))
            {
                string json = r.ReadToEnd();
                streets = JsonSerializer.Deserialize<List<StreetItem>>(json);

                maxStreet = streets.Count;
            }

            using (StreamReader r = new StreamReader("Data/firstnames.csv"))
            {
                while (!r.EndOfStream)
                {
                    string line = r.ReadLine();

                    string[] nameInfo = line.Split('\u002C');

                    firstnames.Add(new NameItem(nameInfo[2], nameInfo[1]));
                }

                maxFirstname = firstnames.Count;
            }

            using (StreamReader r = new StreamReader("Data/lastnames.csv"))
            {
                while (!r.EndOfStream)
                {
                    string line = r.ReadLine();

                    string[] nameInfo = line.Split('\u002C');

                    lastnames.Add(nameInfo[0]);
                }

                maxLastname = lastnames.Count;
            }

            using (StreamReader r = new StreamReader("Data/emails.txt"))
            {
                while (!r.EndOfStream)
                {
                    string line = r.ReadLine();

                    emails.Add(line);
                }

                maxEmail = emails.Count;
            }

            for (int i = 0; i < 1000; i++)
            {
                int randomInt = RandomNumberGenerator.GetInt32(0, maxFirstname);

                string firstname = firstnames[randomInt].Name;
                string gender = firstnames[randomInt].Gender;

                randomInt = RandomNumberGenerator.GetInt32(0, maxLastname);
                string lastname = lastnames[randomInt];

                randomInt = RandomNumberGenerator.GetInt32(0, maxEmail);
                var email = $"{firstname}.{lastname}@{emails[randomInt]}".ToLower();

                randomInt = RandomNumberGenerator.GetInt32(0, maxStreet);
                var streetItem = streets[randomInt];

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
                ids.Add(id);

                var user = new UserCreateRequestMessage()
                {
                    Id = id,
                    FirstName = firstname,
                    LastName = lastname,
                    Email = email,
                    City = city,
                    Street = $"{street} {streetNo}",
                    Zip = postcode
                };

                if (ids.Count > 5)
                {
                    int length = RandomNumberGenerator.GetInt32(0, 200);
                    HashSet<int> destinct = new HashSet<int>(length);

                    for (int f = 0; f < length; f++)
                    {
                        int pos = RandomNumberGenerator.GetInt32(0, ids.Count);
                        if (!destinct.Contains(pos))
                        {
                            destinct.Add(pos);
                            _followings.Add(new FollowingCreateRequestMessage() { UserId = id, FollowingId = ids[pos] });
                        }
                    }
                }

                users.Add(user);
            }

            try
            {
                using var call = _userClient.CreateStream();

                foreach (var item in users)
                {
                    await call.RequestStream.WriteAsync(item);
                }

                await call.RequestStream.CompleteAsync();

                var response = await call;

                Console.WriteLine($"Count: {response.Count}");
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public Task<UsersRespons> CreateFollowersAsync(int count)
        {
            foreach (var item in _followings)
            {
                await _followingClient.CreateAsync(item);
            }
        }
    }
}
