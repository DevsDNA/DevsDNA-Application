namespace DevsDNA.Application.Services
{
	using Xamarin.Forms;

	public class CustomDependencyService : IDependencyService
	{
		private static CustomDependencyService instance;

		private CustomDependencyService()
		{

		}

		public static IDependencyService Instance
		{
			get
			{
				instance ??= new CustomDependencyService();
				return instance;
			}
		}

		public TService Get<TService>() where TService : class
		{
			return DependencyService.Get<TService>();
		}
	}
}
