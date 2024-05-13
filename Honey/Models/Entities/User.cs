namespace Honey.Models.Entities
{
	public class User
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public int age { get; set; }
		public string Email { get; set; }
		public string city { get; set; }
		public string mobile { get; set; }
		public string Password { get; set; }
		public string Job { get; set; }
		public string Description {  get; set; }
		public string Image {  get; set; }



		public virtual List<Experience> Experiences { get; set; }
		public virtual List<Education> Educations { get; set; }
	}
}
