namespace Honey.Models.Entities
{
	public class Education
	{
        public long Id { get; set; }
        public string Evidence { get; set; }
		public string University { get; set; }
		public string DescriptionEduction { get; set; }
		public long UserId { get; set; }
		public virtual User User { get; set; }
	}
}
