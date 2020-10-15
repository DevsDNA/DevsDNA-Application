namespace DevsDNA.Application.Features.Main
{
	using System.Threading.Tasks;

	public interface ITransitionable
	{
		Task Reset();
		Task DoAppearingAnimationAsync();
		Task DoDissappearingAnimationAsync();
	}
}
