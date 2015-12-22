using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace DataCollector
{
    public class GreenhouseDataCollector : IDataCollector
    {
        private readonly string _baseUri;
        private readonly string _apiCredentialUserId;
        private readonly string _apiCredentialPassword;

        public enum PrimaryDataType
        {
            Jobs,
            Candidates,
            Applications,
            Users,
            End
        }

        public enum SecondaryDataType
        {
            Stages,
            End
        }

        public GreenhouseDataCollector()
        {
            _baseUri = ConfigurationManager.AppSettings["GreenhouseBaseUri"];
            //ToDo:  The UserId should be saved encrypted in web.config, and decrypted here as we read it
            _apiCredentialUserId = ConfigurationManager.AppSettings["GreenhouseUserId"];
            _apiCredentialPassword = "";
        }

        public string GetItems(string items)
        {
            if (!IsItemTypeValid(items))
            {
                throw new InvalidDataException("Item type is invalid");
            }

            var uri = $"{_baseUri}/{items}";
            return DownloadDataFromProvider(uri);
        }

        public string GetItem(string items, string itemId)
        {
            if (!IsItemTypeValid(items))
            {
                throw new InvalidDataException("Item type is invalid");
            }

            var uri = $"{_baseUri}/{items}/{itemId}";
            return DownloadDataFromProvider(uri);
        }

        public string GetSubItems(string items, string itemId, string subItems)
        {
            if (!IsItemTypeValid(items))
            {
                throw new InvalidDataException("Item type is invalid");
            }

            var uri = $"{_baseUri}/{items}/{itemId}/{subItems}";
            return DownloadDataFromProvider(uri);
        }

        public IEnumerable<string> GetItemTypes()
        {
            var itemTypes = new List<string>();
            for (var itemType = PrimaryDataType.Jobs; itemType < PrimaryDataType.End; itemType++)
            {
                itemTypes.Add(itemType.ToString().ToLower());
            }

            return itemTypes;
        }

        public IEnumerable<string> GetSubItemTypes()
        {
            var subItemTypes = new List<string>();
            for (var subItemType = SecondaryDataType.Stages; subItemType < SecondaryDataType.End; subItemType++)
            {
                subItemTypes.Add(subItemType.ToString().ToLower());
            }

            return subItemTypes;
        }

        private string DownloadDataFromProvider(string uri)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes($"{_apiCredentialUserId}:{_apiCredentialPassword}")));
                var response = client.GetAsync(uri).Result;
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new HttpRequestException($"Failed to get data from Greenhouse API: {uri}");
                }

                using (var content = response.Content)
                {
                    var result = content.ReadAsStringAsync().Result;
                    return result;
                }
            }
        }

        private bool IsItemTypeValid(string itemType)
        {
            return GetItemTypes().ToList().Any(i => i == itemType);
        }
    }
}