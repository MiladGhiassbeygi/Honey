using Honey.Models.Dto;

namespace Honey.Models.Services.LoginUser
{
    public interface IUserLoginService
    {
        ResultDto<ResultUserLoginDto> Execute(RequestUserLoginDto request);
    }

    public class UserLoginService : IUserLoginService
    {
        private readonly IDataBaseContext _context;

        public UserLoginService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultUserLoginDto> Execute(RequestUserLoginDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return new ResultDto<ResultUserLoginDto>()
                {
                    Data= new ResultUserLoginDto() 
                    {
                        
                    },
                    IsSuccess= false,
                    Message="مقادیر را وارد نمایید",
                };
            }

            var user = _context.Users
                .Where(p => p.Email.Equals(request.Email)).FirstOrDefault();

            if (user == null)
            {
                return new ResultDto<ResultUserLoginDto>()
                {
                    Data=new ResultUserLoginDto()
                    {

                    },
                    IsSuccess=false,
                    Message="کاربری با این ایمیل یافت نشد",
                };
            }
            var passwordHasher=new PasswordHasher();
            bool resultVerifyPassword = passwordHasher.VerifyPassword(user.Password, request.Password);

            if (resultVerifyPassword == false)
            {
                return new ResultDto<ResultUserLoginDto>()
                {
                    Data=new ResultUserLoginDto()
                    {

                    },
                    IsSuccess=false,
                    Message="رمز  وارد شده صحیح نیست",
                };
            }

            return new ResultDto<ResultUserLoginDto>()
            {
                Data=new ResultUserLoginDto()
                {
                    Id=user.Id,
                    Name=user.Name,
                },
                IsSuccess=true,
                Message="ورود به سایت با موفقیت انجام شد",
            };
        }
    }

    public class RequestUserLoginDto
    {
        public string Password { get; set; }    
        public string Email { get; set; }
    }

    public class ResultUserLoginDto
    {
        public long Id { get; set; }
        public string Name {  get; set; }
    }
}
