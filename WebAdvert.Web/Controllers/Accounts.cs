using Amazon.Extensions.CognitoAuthentication;
using Amazon.AspNetCore.Identity.Cognito;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAdvert.Web.Models.Accounts;

namespace WebAdvert.Web.Controllers
{
    public class Accounts : Controller
    {
        public readonly SignInManager<CognitoUser> _signInManager;
        public readonly UserManager<CognitoUser> _userManager;
        public readonly CognitoUserPool _pool;

        public Accounts(SignInManager<CognitoUser> signInManager, UserManager<CognitoUser> userManager, CognitoUserPool pool)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _pool = pool;
        }

        public string CognitoAttributesConstants { get; private set; }

        public async Task<IActionResult> signup()
        {
            var model = new SignupModel(); //Empty model
            return View(model);//return the view with model
        }
        [HttpPost]
        /* 
         In the signup, create dependecncy injection, creae  a service that abstracts everything. We are using cognito, and i am putting logic in controller,
        Its not best to put entire logic here
        To create a user, if 
         */
        public async Task<IActionResult> signup(SignupModel model)
        {
           if (ModelState.IsValid)
            {
                var user = _pool.GetUser(model.Email);
                if (user.Status != null)
                {
                    ModelState.AddModelError("UserExists", "User with this mail already esits");
                    return View(model);
                }

                 //user.Attributes.Add(CognitoAttributesConstants.Name, model.Email);//key
                user.Attributes.Add(CognitoAttribute.Name.AttributeName,model.Email);
                 /*
            * If you do not pass password(model.Password), you can create user.However, in this case, a password will be auto generated and it will be sent as a temporary password
                 to your user ID and user on the first login will have to change the password.That means you will need to create a change password form as well and or use the built in sign up page
                 as sign in page of cognitive. easier is that you just put the password in here. Then user will be created with that given password.They will just have to confirm their email address
                 so it you know that your you've managed to create
                 the user.
                  */
                 var createdUser=await _userManager.CreateAsync(user, model.Password);//
                if (createdUser.Succeeded)
                {
                    //if created user succeed succeeded, then we can say a direct to page ConfrimPassword or Redirect to action with actionMName "confirm"
                    //RedirectToPage("./ConfirmPassword");
                    RedirectToAction("Confirm");
                }
            }
            return View();
        }
    }
}
