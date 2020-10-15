namespace DevsDNA.Application.Services
{
	public interface IDependencyService
	{
		TService Get<TService>() where TService : class;
	}
}