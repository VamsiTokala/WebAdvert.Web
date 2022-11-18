using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using WebAdvert.Web.ServiceClients;
using System.Net;
using WebAdvert.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddCognitoIdentity();
builder.Services.AddHttpClient<IAdvertApiClient, AdvertApiClient>().AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPatternPolicy());

IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPatternPolicy()
{
    return HttpPolicyExtensions.HandleTransientHttpError()
        .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30));//after 3 tries for 30 secs any calls should be broken
}

IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions.HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryAsync(5, retryAttempy => TimeSpan.FromSeconds(Math.Pow(2, retryAttempy)));//try for the first time wait 2 ^1 secs, second time 2^2  and so on
}


/*builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "Accounts/Login"; //It means, if any unauthenticated user tries to access secured URLs of the App then he will be automatically redirected to the /Accounts/Login URL (the login page).

            });*/

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Accounts/Login";
    //options.AccessDeniedPath = "/User/Login";
    //options.LogoutPath = "/User/Logout";
    //options.SlidingExpiration = true;
});

builder.Services.AddTransient<IFileUploader, S3FileUploader>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
    //pattern: "{controller=Accounts}/{action=signup}/{id?}");

app.Run();
