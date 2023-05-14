using chat.Data;
using chat.Migrations;
using chat.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace chat.Controllers
{
    public class UserController : Controller
    {
        private readonly ChatContext context;

        public UserController(ChatContext context)
        {
            this.context = context;
        }
        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }



        // GET: UserController/Register
        [Route("/Register")]
        public IActionResult Register()
        {
            return View();
        }

        // POST: UserController/Register
        [Route("/Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return View();

            TblUser user = new TblUser
            {
                UserName = model.UserName,
                Email = model.Email,
                Password = model.Password
            };


            context.TblUser.Add(user);
            int res = await context.SaveChangesAsync();
            if (res > 0)
            {
                TempData["SMessage"] = "Successfully Registerd!";
                return RedirectToAction(nameof(Login));
            }
            else
            {
                TempData["Err"] = "Ooops! Something went wrong.";
                return View();
            }


        }





        // GET: UserController/Login
        [Route("/Login")]
        public ActionResult Login(string ReturnUrl = "/")
        {
            LoginModel model = new LoginModel();
            model.ReturnUrl = ReturnUrl;
            return View(model);
        }

        // POST: UserController/Login
        [Route("/Login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = context.TblUser.Where(x => x.UserName == model.UserName && x.Password == model.Password).FirstOrDefault();
                if (user == null)
                {
                    //Add logic here to display some message to user    
                    TempData["LErr"] = "Invalid Credential";
                    return View(model);
                }
                else
                {
                    //A claim is a statement about a subject by an issuer and    
                    //represent attributes of the subject that are useful in the context of authentication and authorization operations.    
                    var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.UserId)),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, "User"),

                };
                    //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme    
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity    
                    var principal = new ClaimsPrincipal(identity);
                    //SignInAsync is a Extension method for Sign in a principal for the specified scheme.    
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                    {
                        IsPersistent = model.RememberLogin
                    });
                    return LocalRedirect("/Home/Index");
                }
            }
            return View(model);
        }



        [Route("/LogOut")]
        public async Task<IActionResult> LogOut()
        {
            //SignOutAsync is Extension method for SignOut    
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //Redirect to home page    
            return LocalRedirect("/Home/Index");
        }




        //var data = context.TblUser.Where(x => x.UserId == id).FirstOrDefault().ChatSenderUser.ToList();
        //var data = context.TblChat.Where(x => x.ChatId == id).FirstOrDefault().SenderUser;


    }
}
