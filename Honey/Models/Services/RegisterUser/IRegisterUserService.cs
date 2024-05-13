using Honey.Models.Dto;
using Honey.Models.Entities;
using System.Text.RegularExpressions;

namespace Honey.Models.Services.RegisterUser
{
    public interface IRegisterUserService
    {
        ResultDto<ResultRegister> Execute(RequestRegister request);
    }
    public class RegisterUserService : IRegisterUserService
    {
        private readonly IDataBaseContext _context;
        private readonly IWebHostEnvironment _environment;
        public RegisterUserService(IDataBaseContext context,IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public ResultDto<ResultRegister> Execute(RequestRegister request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name) ||
                    string.IsNullOrWhiteSpace(request.Email) ||
                    string.IsNullOrWhiteSpace(request.Password) ||
                    string.IsNullOrWhiteSpace(request.Job)||
                    string.IsNullOrWhiteSpace(request.city) ||
                    string.IsNullOrWhiteSpace(request.mobile))
                {
                    return new ResultDto<ResultRegister>()
                    {
                        Data = new ResultRegister()
                        {
                            UserId = 0,
                        },
                        IsSuccess=false,
                        Message="تمامی مقادیر را وارد نمایید",
                    };
                }

                string emailRegex = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[A-Z0-9.-]+\.[A-Z]{2,}$";
                var match=Regex.Match(request.Email,emailRegex,RegexOptions.IgnoreCase);
                if(!match.Success)
                {
                    return new ResultDto<ResultRegister>()
                    {
                        Data= new ResultRegister()
                        {
                            UserId= 0,
                        },
                        IsSuccess = false,
                        Message="ایمیل را به درستی وارد کنید",
                    };
                }

                var passwordHasher=new PasswordHasher();
                var hashedPassword=passwordHasher.HashPassword(request.Password);


                var uploadedResult = UploadFile(request.Image);


                User user = new User()
                {
                    Name = request.Name,
                    age=request.age,
                    Email = request.Email,
                    Password=hashedPassword,
                    city=request.city,
                    mobile=request.mobile,
                    Job=request.Job, 
                    Description=request.Description,
                    Image=uploadedResult,
                    
                   
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                return new ResultDto<ResultRegister>()
                {
                    Data=new ResultRegister()
                    {
                        UserId=user.Id,
                    },
                    IsSuccess=true,
                    Message="ثبت نام انجام شد",
                };
            }
            catch (Exception)
            {

                return new ResultDto<ResultRegister>()
                {
                    Data=new ResultRegister()
                    {
                        UserId=0,
                    },
                    IsSuccess=false,
                    Message="ثبت نام انجام نشد",
                };
            }

        }
        private string UploadFile(IFormFile file)
        {
            if (file != null)
            {
                var fileName=Path.GetFileName(file.FileName);
                string folder = $@"images\ProfileImage\";
                var uploadRootFolder = Path.Combine(_environment.WebRootPath, folder);
                var filePath = Path.Combine(uploadRootFolder, fileName);
				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					file.CopyTo(fileStream);
				}
                string FileNameAddress = folder + fileName;
                return FileNameAddress;
                
            }
            return null;
        }
    }


    public class RequestRegister
    {
        public string Name { get; set; }
        public int age { get; set; }
        public string Email { get; set; }
        public string city { get; set; }
        public string mobile { get; set; }
        public string Password { get; set; }
        public string Job {  get; set; }
        public string Description {  get; set; }
        public IFormFile Image { get; set; }
    }

    public class UploadDto
    {
        public long Id {  get; set; }
        
        public string FileNameAddress {  get; set; }
    }


    public class ResultRegister
    {
        public long UserId {  get; set; }
    }
}
