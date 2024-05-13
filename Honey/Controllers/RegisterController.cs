using Honey.Models.Dto;
using Honey.Models.Services.LoginUser;
using Honey.Models.Services.RegisterUser;
using Honey.Models.ViewModels.Register;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Honey.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IRegisterUserService _registerUserService;
        private readonly IUserLoginService _userLoginService;

        public RegisterController(IRegisterUserService registerUserService,IUserLoginService userLoginService)
        {
            _registerUserService = registerUserService;
            _userLoginService = userLoginService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl="/")
        {
            ViewBag.url = ReturnUrl;   
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel request,string url="/")
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return Json(new ResultDto()
                {
                    IsSuccess = false,
                    Message = "مقادیر را وارد کنید",
                });
            }
            var signUpResult= _userLoginService.Execute(new RequestUserLoginDto()
            {
                Email = request.Email,
                Password=request.Password,
            });
            if (signUpResult.IsSuccess == true)
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,signUpResult.Data.Id.ToString()),
                    new Claim(ClaimTypes.Email,request.Email),
                    new Claim(ClaimTypes.Name,signUpResult.Data.Name),
                };
                var identity= new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                var principle=new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties()
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.Now.AddDays(5),
                };
                HttpContext.SignInAsync(principle, properties);
            }

            return Json(signUpResult);
        }

        [HttpGet]
		public IActionResult Register()
		{
			return View();
		}

        [HttpPost]
		public IActionResult Register(RegisterViewModel request,IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                request.Image = file;
            }


                if (string.IsNullOrWhiteSpace(request.Name) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Job)||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.city) ||
                string.IsNullOrWhiteSpace(request.mobile))
            {
                return Json(new ResultDto { IsSuccess = false, Message = "تمامی مقادیر را وارد کنید" });
            }

            if (User.Identity.IsAuthenticated == true)
            {
                return Json(new ResultDto { IsSuccess = false, Message = "شما قبلا ثبت نام کرده اید" });
            }

            string emailRegex = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[A-Z0-9.-]+\.[A-Z]{2,}$";
            var match=Regex.Match(request.Email,emailRegex, RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                return Json(new ResultDto { IsSuccess = false, Message = "ایمیل به درستی وارد نشده است" });
            }


            var RegisterResult = _registerUserService.Execute(new RequestRegister()
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password,
                age = request.age,
                city = request.city,
                mobile = request.mobile,
                Job = request.Job,
                Description=request.Description,
                Image=request.Image,
            });

            if (RegisterResult.IsSuccess == true)
            {
                var clims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,RegisterResult.Data.UserId.ToString()),
                    new Claim(ClaimTypes.Name,request.Name),
                    new Claim (ClaimTypes.Email,request.Email),
                    
                };
                var identity = new ClaimsIdentity(clims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principle = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties()
                {
                    IsPersistent = true,
                };
                HttpContext.SignInAsync(principle, properties);
            }

            return Json(RegisterResult);
        }
        public IActionResult SignOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index","Home");
        }

    }
}
