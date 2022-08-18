using System.Security.Cryptography;
using System.Text.Json;

// See https://aka.ms/new-console-template for more information
using GenerateData.Models;
using Grpc.Net.Client;
using MongoDB.Bson;
using Northstar.Message;

Console.WriteLine("Hello, World!");

List<StreetItem> streets;
List<NameItem> firstnames = new List<NameItem>();
List<string> lastnames = new List<string>();
List<string> emails = new List<string>();
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

for (int i = 0; i < 1000000; i++)
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

    users.Add(new UserCreateRequestMessage()
    {
        Id = ObjectId.GenerateNewId().ToString(),
        FirstName = firstname,
        LastName = lastname,
        Email = email,
        City = city,
        Street = $"{street} {streetNo}",
        Zip = postcode
    });
}

//var usersJson = JsonSerializer.Serialize<List<UserCreateRequestMessage>>(users);

//using (StreamWriter w = new StreamWriter("Data/Users.json"))
//{
//    w.Write(usersJson);
//}

using var channel = GrpcChannel.ForAddress("https://localhost:5102");
var client = new UserGrpcService.UserGrpcServiceClient(channel);

foreach (var item in users)
{
    var reply = await client.CreateAsync(item);

    Console.WriteLine(reply.Success.Id);
}
