using System;
using System.IO;
using System.Threading.Tasks;
//using NewAdvertApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewAdvertApi.Models;
using WebAdvert.Web.Models.AdvertManagement;
using WebAdvert.Web.ServiceClients;
using WebAdvert.Web.Services;


namespace WebAdvert.Web.Controllers
{
    public class AdvertManagementController : Controller
    {
        private readonly IFileUploader _fileUploader;
        private readonly IAdvertApiClient _advertApiClient;
        private readonly IMapper _mapper;

        public AdvertManagementController(IFileUploader fileUploader, IAdvertApiClient advertApiClient, IMapper mapper)
        {
            _fileUploader = fileUploader;
            _advertApiClient = advertApiClient;
            _mapper = mapper;
        }

        public IActionResult Create(CreateAdvertViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        // create method is reciveing the form content so its get the model, apart from the model we recive instanc of IformFile
        public async Task<IActionResult> Create(CreateAdvertViewModel model, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                //we need a ID, random id, we need to make to call to AdvertAPI to store the dtails of advertizement in database and that method will give us actual ID value
                var id = "11111";

                if (imageFile != null)
                {
                    var fileName = !string.IsNullOrEmpty(imageFile.FileName) ? Path.GetFileName(imageFile.FileName) : id;
                    var filePath = $"{id}/{fileName}";

                    try
                    {
                        using (var readStream = imageFile.OpenReadStream())
                        {
                            var result = await _fileUploader.UploadFileAsync(filePath, readStream)
                                .ConfigureAwait(false);
                            if (!result)
                                throw new Exception(
                                    "Could not upload the image to file repository. Please see the logs for details.");
                        }
                        //call advert api and confirm the advertisement
                        return RedirectToAction("Index", controllerName: "Home");
                    }
                    //in case of exceptions, make a call to api to delete or mark it as inactive in DB
                    catch (Exception e)
                    {
                        //write code to AsyncCallback Advert API and cancel arvertisement
                        Console.WriteLine(e);
                    }

                }
            }
            return View(model);
        }
    }
}

