namespace DevsDNA.Application.Features.AboutUs
{
	using System.Collections.Generic;

	public interface IWeService
	{
		List<ThunderMateModel> GetThunderMates();
	}
}