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
        private Dictionary<string, string> modelEndpoints;
        private string baseUrlAddress;

        public DataService(string baseAddress)
        {
            
#if DEBUG
            HttpClientHandler insecureHandler = DependencyService.Get<IHttpClientHandlerService>().GetInsecureHandler(); // insecure SSL certificate handler to HttpClient (only in debug)
            this.client = new HttpClient(insecureHandler);
#else
            this.client = new HttpClient();
#endif
            this.baseUrlAddress = baseAddress; // set base address of RESTful web service 
            this.modelEndpoints = new Dictionary<string, string>(); // dictionary of endpoints - one for each model class in service
        }

        public DataService AddEntityModelEndpoint<TEntity>(string endpoint)//reflection to get the name of class type and use that as the dictionary entry's key
        {
            this.modelEndpoints.Add(typeof(TEntity).FullName, endpoint);
            return this;
        }

        private string GetEntityEndpoint<TEntity>(string nonDefaultEndpoint = null) // private helper method to get the endpoint, factored out into separate method for code reuse purposes
        {
            StringBuilder endpoint = new StringBuilder(this.modelEndpoints[typeof(TEntity).FullName]);
            if(!string.IsNullOrEmpty(nonDefaultEndpoint)) { endpoint.Append($"/{nonDefaultEndpoint}"); }
            return endpoint.ToString();
        }

        public async Task<List<TEntity>> GetAllAsync<TEntity>(Expression<Func<TEntity, bool>> searchLambda = null, string nonDefaultEndpoint = null) where TEntity : new() // form URI for the webservice GET request 
        {
            var endpoint = GetEntityEndpoint<TEntity>(nonDefaultEndpoint);
            var url = $"{this.baseUrlAddress}/{endpoint}?searchExpression={searchLambda}";
            var uri = new Uri(string.Format(url));
            HttpResponseMessage response = await client.GetAsync(uri); //GET request to the URI
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(); // read content returned by GET request
                var deserialisedContent = JsonConvert.DeserializeObject<IEnumerable<TEntity>>(content);// deserialise back to C# objects
                return deserialisedContent.ToList<TEntity>(); // return deserialised objects to caller as List<TEntity> collection
            }
            else { throw new Exception(response.ReasonPhrase); }
        }

        public async Task<List<TEntity>> GetAllAsync<TEntity, T>(T id, string nonDefaultEndpoint = null) where TEntity : new() // form URI for the webservice GET request 
        {
            var endpoint = GetEntityEndpoint<TEntity>(nonDefaultEndpoint);
            var url = $"{this.baseUrlAddress}/{endpoint}/{id}";
            var uri = new Uri(string.Format(url));
            HttpResponseMessage response = await client.GetAsync(uri); //GET request to the URI //expression not supported
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(); // read content returned by GET request
                var deserialisedContent = JsonConvert.DeserializeObject<IEnumerable<TEntity>>(content);// deserialise back to C# objects
                return deserialisedContent.ToList<TEntity>(); // return deserialised objects to caller as List<TEntity> collection
            }
            else { throw new Exception(response.ReasonPhrase); }
        }

        public async Task<TEntity> GetAsync<TEntity, T>(T id, string nonDefaultEndpoint = null) where TEntity : new()
        {
            var endpoint = GetEntityEndpoint<TEntity>(nonDefaultEndpoint); // form URI webservice GET request 
            var uri = new Uri($"{this.baseUrlAddress}/{endpoint}/{id}");
            HttpResponseMessage response = await client.GetAsync(uri);// make GET request to URI
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync(); // read content returned by GET request
                var deserialisedContent = JsonConvert.DeserializeObject<TEntity>(content); // deserialise read content to C# object
                return deserialisedContent; // return deserialised object to caller as TEntity type
            }
            else { throw new Exception(response.ReasonPhrase); }
        }

        public async Task<bool> UpdateAsync<TEntity, T>(TEntity entity, T id, string nonDefaultEndpoint = null)
        {
            
            var endpoint = GetEntityEndpoint<TEntity>(nonDefaultEndpoint); // form URI webservice PUT request 
            var url = $"{this.baseUrlAddress}/{endpoint}/{id}";
            var uri = new Uri(url);
            string jsonEntity = JsonConvert.SerializeObject(entity);  // serialise TEntity object to JSON string
            StringContent content = new StringContent(jsonEntity, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(uri, content); // make the PUT request to the URI
            if (!response.IsSuccessStatusCode) {  throw new Exception(response.ReasonPhrase); }
            return response.IsSuccessStatusCode;
        }

        public async Task<TEntity> InsertAsync<TEntity>(TEntity entity, string nonDefaultEndpoint = null)
        {

            var endpoint = GetEntityEndpoint<TEntity>(nonDefaultEndpoint);// form URI for webservice POST request 
            var url = $"{this.baseUrlAddress}/{endpoint}";
            var uri = new Uri(url);
            string jsonEntity = JsonConvert.SerializeObject(entity); // serialise the TEntity object to JSON string
            StringContent content = new StringContent(jsonEntity, Encoding.UTF8, "application/json");
            HttpResponseMessage response = response = await client.PostAsync(uri, content); // make POST request to URI
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();// read the response content returned by the POST request (the created object)
                var deserialisedContent =  JsonConvert.DeserializeObject<TEntity>(responseContent); // deserialise the read content back to C# object
                return deserialisedContent;
            }
            else { throw new Exception(response.ReasonPhrase); }
        }

        public async Task<bool> DeleteAsync<TEntity, T>(TEntity entity, T id, string nonDefaultEndpoint = null)
        {
            var endpoint = GetEntityEndpoint<TEntity>(nonDefaultEndpoint); // form URI for webservice DELETE request 
            var url = $"{this.baseUrlAddress}/{endpoint}/{id}";
            var uri = new Uri(url);
            HttpResponseMessage response = await client.DeleteAsync(uri); // makeDELETE request to URI

            if (!response.IsSuccessStatusCode) { throw new Exception(response.ReasonPhrase); }
            return response.IsSuccessStatusCode;
        }
    }
}
