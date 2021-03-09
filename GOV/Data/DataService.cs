using System;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Forms;
namespace GOV.Data
{
    public class DataService
    {
        private HttpClient client;
        private Dictionary<string, string> modalEndpoints;
        private string baseUrlAddress;
        public DataService(string baseAddress)//insecure SSL certificate handler to HttpClient only if a debug build
        {
            #if DEBUG
            HttpClientHandler insecureHandler = DependencyService.Get<IHttpClientHandlerService>().GetInsecureHandler();
            client = new HttpClient(insecureHandler);
            #else
            this.client = new HttpClient();
            #endif
            baseUrlAddress = baseAddress;// set base address RESTful web service
            modalEndpoints = new Dictionary<string, string>();// dictionary of endpoints, for each class in service
        }
        public DataService AddEntityModelEndpoint<TEntity>(string endpoint)
        {
            //reflection get name of class type
            modalEndpoints.Add(typeof(TEntity).FullName, endpoint);//endpoint value for key/value pair
            return this;
        }
        private string GetEntityEndpoint<TEntity>()
        {
            return modalEndpoints[typeof(TEntity).FullName];// private helper method to get the endpoint
        }
        public async Task<List<TEntity>> GetAllAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : new() // setup for grab list request
        {
            var endpoint = GetEntityEndpoint<TEntity>();//set endpoint URI webservice GET request
            var url = $"{baseUrlAddress}/{endpoint}?searchExpression={filter}";
            var uri = new Uri(string.Format(url));
            HttpResponseMessage response = await client.GetAsync(uri);// make GET to URI

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(); // read GET request
                var deserialisedContent = JsonConvert.DeserializeObject<IEnumerable<TEntity>>(content);// deserialise content to C# objects
                return deserialisedContent.ToList<TEntity>(); // return real objects back to calling function as List<TEntity> collection
            }
            return default; //this is needed to pass a list of objects by index accessable by sorting, search & manipulatuion lists
        }
        public async Task<TEntity> GetAsync<TEntity, T>(T id)where TEntity : new() // form URI for list request
        {
           
            var endpoint = GetEntityEndpoint<TEntity>();//set endpoint URI webservice GET request
            var uri = new Uri($"{baseUrlAddress}/{endpoint}/{id}");
            HttpResponseMessage response = await client.GetAsync(uri);// make GET to URI

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();// read GET request
                var deserialisedContent = JsonConvert.DeserializeObject<TEntity>(content); // deserialise content to C# objects
                return deserialisedContent; // return real object back to calling function
            }
            return default; //this is needed to pass object possible for sorting, search & manipulatuion lists
        }
        public async Task<bool> UpdateAsync<TEntity, T>(TEntity entity, T id)//form URI for PUT request
        {
            var endpoint = GetEntityEndpoint<TEntity>();//set endpoint URI webservice GET request
            var url = $"{baseUrlAddress}/{endpoint}/{id}";
            var uri = new Uri(url);// make PUT to URI
            string jsonEntity = JsonConvert.SerializeObject(entity); // deserialise content to C# objects
            StringContent content = new StringContent(jsonEntity, Encoding.UTF8, "application/json"); // make the PUT request to the URI
            HttpResponseMessage response = await client.PutAsync(uri, content);// make GET to URI
            return response.IsSuccessStatusCode; // return boolean
        }
        public async Task<TEntity> InsertAsync<TEntity>(TEntity entity)// form URI for POST request
        {
            var endpoint = GetEntityEndpoint<TEntity>();//set endpoint URI webservice GET request
            var url = $"{baseUrlAddress}/{endpoint}";
            var uri = new Uri(url);// make POST to URI
            string jsonEntity = JsonConvert.SerializeObject(entity); // deserialise content to C# objects
            StringContent content = new StringContent(jsonEntity, Encoding.UTF8, "application/json"); // make the PUT request to the URI
            HttpResponseMessage response = response = await client.PostAsync(uri, content);// make GET to URI
            if (response.IsSuccessStatusCode)//bool check
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var deserialisedContent =JsonConvert.DeserializeObject<TEntity>(responseContent);
                return deserialisedContent;// return boolean
            }
            return default;
        }
        public async Task<bool> DeleteAsync<TEntity, T>(TEntity entity, T id)// form URI for DELETE request
        {
            var endpoint = GetEntityEndpoint<TEntity>();//set endpoint URI webservice GET request
            var url = $"{baseUrlAddress}/{endpoint}/{id}";
            var uri = new Uri(url);
            HttpResponseMessage response = await client.DeleteAsync(uri);// make DELETE to URI
            return response.IsSuccessStatusCode;// return boolean
        }

    }
}