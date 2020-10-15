namespace DevsDNA.Application.Features.AboutUs
{
	using DevsDNA.Application.Base;
	using DevsDNA.Application.Services;
	using ReactiveUI;
	using System.Collections.ObjectModel;

	public class AboutUsViewModel : BaseViewModel
	{
		private readonly IWeService weService;
		private ObservableCollection<ThunderMateModel> thunderMates;


		public AboutUsViewModel(IDependencyService dependencyService) : base(dependencyService)
		{
			weService = DependencyService.Get<IWeService>();

			ThunderMates = new ObservableCollection<ThunderMateModel>(weService.GetThunderMates());
		}


		public ObservableCollection<ThunderMateModel> ThunderMates
		{
			get => thunderMates;
			set => this.RaiseAndSetIfChanged(ref thunderMates, value);
		}
	}
}
