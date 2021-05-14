using PlushkinForms.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PlushkinForms.Services
{
    class BookmarkService
    {
        const string Url = "http://188.226.96.115:8000/core/bookmarks/";
        JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        private bool IsAuth = false;

        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            IsAuth = Application.Current.Properties.ContainsKey("authToken");
            if (IsAuth)
            {
                string authToksen = Application.Current.Properties["authToken"].ToString();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", authToksen);
            }
            
            return client;
        }

        public class TypeFilter
        {
            private TypeFilter(string value) { Value = value; }
            public string Value { get; set; }

            public static TypeFilter Empty { get { return new TypeFilter(""); } }
            public static TypeFilter Unsorted { get { return new TypeFilter("?type=U"); } }
            public static TypeFilter Liked { get { return new TypeFilter("?type=L"); } }
            public static TypeFilter Trash { get { return new TypeFilter("?type=T"); } }

        }

        public async Task<IEnumerable<Bookmark>> Get(TypeFilter filter)
        {
            HttpClient client = GetClient();
            if (!IsAuth) return new List<Bookmark>();
            string result = await client.GetStringAsync(Url + filter.Value);
            return JsonSerializer.Deserialize<IEnumerable<Bookmark>>(result, options);
        }

        public async Task<IEnumerable<Bookmark>> Search(string filter)
        {
            HttpClient client = GetClient();
            if (!IsAuth) return new List<Bookmark>();
            string result = await client.GetStringAsync(Url + "?search=" + filter);
            return JsonSerializer.Deserialize<IEnumerable<Bookmark>>(result, options);
        }

        public async Task<Bookmark> Add(Bookmark bookmark)
        {
            HttpClient client = GetClient(); 
            if (!IsAuth) return null;
            var response = await client.PostAsync(Url,
                new StringContent(
                    JsonSerializer.Serialize(bookmark),
                    Encoding.UTF8, "application/json"));

            //var ss = response.Content.ReadAsStringAsync().Result;

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonSerializer.Deserialize<Bookmark>(
                await response.Content.ReadAsStringAsync(), options);
        }

        public async Task<Bookmark> Update(Bookmark bookmark)
        {
            HttpClient client = GetClient();
            if (!IsAuth) return null;
            var response = await client.PostAsync("http://188.226.96.115:8000/core/bookmark_update/",
                new StringContent(
                    JsonSerializer.Serialize(bookmark),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonSerializer.Deserialize<Bookmark>(
                await response.Content.ReadAsStringAsync(), options);
        }

        public async Task<Bookmark> Delete(int id)
        {
            HttpClient client = GetClient();
            if (!IsAuth) return null;
            await client.DeleteAsync(Url + id);
            return null;
        }
    }
}
