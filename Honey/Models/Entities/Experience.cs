namespace Honey.Models.Entities
{
	public class Experience
    {
        public long Id { get; set; }

		public string Position {  get; set; }
        public string Company {  get; set; }
        public string DescriptionExperience { get; set; }

        public long UserId {  get; set; }
        public virtual User User { get; set; }

	}
}
