using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestdataGenerator.Model;

namespace TestdataGenerator
{
    public class TestPerson 
    {
        static readonly HttpClient client = new HttpClient();
        private ILogger _logger;
        private Dictionary<string, Personnummer> _pnrs = new Dictionary<string, Personnummer>();

        private const string _filename = "pnr.txt";

        public int Count => _pnrs.Count;

        public ICollection<Personnummer> Values => _pnrs.Values;

        public Personnummer this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public TestPerson(ILogger logger)
        {
            _logger = logger;
            List<string> temp = new List<string>();

            temp = RetriveFromFile();
            if (temp == null)
            {
                temp = RetriveFromTaxAgency();
            }

            foreach (var item in temp)
            {
                _pnrs.Add(item, new Personnummer(item));
            }
        }

        private List<string>? RetriveFromFile()
        {
            try
            {
                return File.ReadAllLines(_filename).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Exeption from reading pnr.txt: {ex.Message}");

                return null;
            }
        }

        private List<string> RetriveFromTaxAgency()
        {
            string url = "http://skatteverket.entryscape.net/rowstore/dataset/b4de7df7-63c0-4e7e-bb59-1f156a591763?_limit=500";
            List<string> pnr = new List<string>();
            while (true)
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
                HttpResponseMessage response = client.Send(request);

                if (response.IsSuccessStatusCode is not true)
                {
                    switch ((int)response.StatusCode)
                    {
                        case 400:
                            _logger.LogWarning("Invalid parameters");
                            break;
                        case 404:
                            _logger.LogWarning("Dataset not found");
                            break;
                        case 429:
                            _logger.LogWarning("Too many requests");
                            break;
                        case 500:
                            _logger.LogWarning("Internal error");
                            break;
                        case 503:
                            _logger.LogWarning("Query timeout");
                            break;
                    }

                    break;
                }


                TestPersonnummer? personnummer =
                    JsonSerializer.Deserialize<TestPersonnummer>(response.Content.ReadAsStream());

                if (personnummer != null)
                {
                    pnr.AddRange(personnummer.results.Select(x => x.testpersonnummer).ToList());

                    if (string.IsNullOrEmpty(personnummer.next))
                    {
                        break;
                    }

                    url = personnummer.next;
                }
                else
                {
                    break;
                }
            }

            _logger.LogInformation($"{pnr.Count}");

            File.WriteAllLines(_filename, pnr);

            return pnr;
        }

        public void Add(string key, Personnummer value)
        {
            _pnrs.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return _pnrs.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return _pnrs.Remove(key);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out Personnummer value)
        {
            return _pnrs.TryGetValue(key, out value);
        }

        public void Clear()
        {
            _pnrs.Clear();
        }
    }
}
