using Microsoft.Extensions.Configuration;
using NewAdvertApi.Models;
using Newtonsoft.Json;

namespace WebAdvert.Web.ServiceClients
{
    public class AdvertApiClient : IAdvertApiClient
    {
        public readonly IConfiguration _configuraton;
        public readonly HttpClient _client;

        public AdvertApiClient(IConfiguration configuration, HttpClient client)
        {
            _configuraton = configuration;
            _client = client;

            var createUrl = _configuraton.GetSection(key: "AdvertApi").GetValue<string>(key: "CreateUrl");
            _client.BaseAddress = new Uri(createUrl);
            _client.DefaultRequestHeaders.Add(name: "Content-type", value: "application/json");
        } 
        
        public async Task<AdvertResponse> Create(CreateAdvertModel model)
        {
            var advertApiModel = new AdvertModel();//automapper
            var jsonModel = JsonConvert.SerializeObject(model);
            var response = await _client.PostAsync(_client.BaseAddress, new StringContent(jsonModel)).ConfigureAwait(false);
            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext:false);
            var createAdvertResponse = JsonConvert.DeserializeObject<CreateAdvertResponse>(responseJson);
            var advertResponse = new AdvertResponse();//automapper
            return advertResponse;

        }
    }
}
