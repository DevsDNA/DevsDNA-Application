namespace DevsDNA.Application.Features.AboutUs
{
	using ReactiveUI;

	public class ThunderMateModel : ReactiveObject
	{
		public string Name { get; set; }
		public string Title { get; set; }
		public string Photo { get; set; }
		public string Description { get; set; }
		public string FunnyDescription { get;  set; }
	}
}