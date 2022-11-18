using NewAdvertApi.Models;
namespace WebAdvert.Web.ServiceClients
{
    public interface  IAdvertApiClient
    {
        /* we are using models of AdvertAPI inside website. Only place that you can use this type is when you access the API and you get data
        *Anywhere outside the code that only calls the API and gets a response and serialize it to a class, an object in c# any where else you should use your own model
        * So, here also we need to create a class 
        * for CreateAdvertResponse with the same structure
        * for AdvertModel with the same structure
       */
         Task<AdvertResponse> Create(CreateAdvertModel model);
        Task<bool> Confirm(ConfirmAdvertRequest model);
    }
}
