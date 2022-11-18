using Microsoft.Extensions.Configuration;
using NewAdvertApi.Models;
using Newtonsoft.Json;
using AutoMapper;
using System.Net;
using System.Text;
using Amazon.DynamoDBv2.Model;

namespace WebAdvert.Web.ServiceClients
{
    public class AdvertApiClient : IAdvertApiClient
    {
        public readonly IConfiguration _configuraton;
        public readonly HttpClient _client;
        private readonly IMapper _mapper;

        public AdvertApiClient(IConfiguration configuration, HttpClient client, IMapper mapper)
        {
            _configuraton = configuration;
            _client = client;
            _mapper = mapper;


            var createUrl = _configuraton.GetSection(key: "AdvertApi").GetValue<string>(key: "CreateUrl");
            _client.BaseAddress = new Uri(createUrl);
            _client.DefaultRequestHeaders.Add(name: "Content-type", value: "application/json");
        } 
        
        public async Task<AdvertResponse> Create(CreateAdvertModel model)
        {
            // var advertApiModel = new AdvertModel();//automapper
            var advertApiModel = _mapper.Map<AdvertModel>(model);
            var jsonModel = JsonConvert.SerializeObject(model);
            var response = await _client.PostAsync(_client.BaseAddress, new StringContent(jsonModel)).ConfigureAwait(false);
            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext:false);
            var createAdvertResponse = JsonConvert.DeserializeObject<CreateAdvertResponse>(responseJson);
            // var advertResponse = new AdvertResponse();//automapper
            var advertResponse = _mapper.Map<AdvertResponse>(createAdvertResponse);
            return advertResponse;

        }

        public async Task<bool> Confirm(ConfirmAdvertRequest model)
        {
            var advertModel = _mapper.Map<ConfirmAdvertModel>(model);//coverts website model to api model
            var jsonModel = JsonConvert.SerializeObject(advertModel);
            //Put call
            /*var response = await _client
                .PutAsync(new Uri($"{_baseAddress}/confirm"),
                    new StringContent(jsonModel, Encoding.UTF8, "application/json"))
                .ConfigureAwait(false);*/

            var response = await _client
                .PutAsync(new Uri($"{_client.BaseAddress}/confirm"),
                  new StringContent(jsonModel, Encoding.UTF8, "application/json"))
              .ConfigureAwait(false);
            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}
