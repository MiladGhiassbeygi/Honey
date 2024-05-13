namespace Honey.Models.ViewModels.Register
{
	public class RegisterViewModel
	{
		public string Name { get; set; }
		public int age { get; set; }
		public string Email { get; set; }
		public string city { get; set; }
		public string mobile { get; set; }
		public string Password { get; set; }
		public string Job { get; set; }
		public string Description { get; set; }
		public IFormFile Image { get; set; }
	}
}
