[assembly: Xamarin.Forms.Dependency(typeof(DevsDNA.Application.Features.AboutUs.WeService))]
namespace DevsDNA.Application.Features.AboutUs
{
	using DevsDNA.Application.Strings;
	using System.Collections.Generic;

	public class WeService : IWeService
	{
		public List<ThunderMateModel> GetThunderMates()
		{
			List<ThunderMateModel> thunderMates = new List<ThunderMateModel>
			{
				GetBlackWidow(),
				GetNickFury(),
				GetBeast(),
				GetDoctorStrange(),
				GetSpiderMan(),
				GetStarLord()
			};
			return thunderMates;
		}

		private ThunderMateModel GetBlackWidow()
		{
			return new ThunderMateModel
			{
				Name = "Beatriz Márquez Heredia",
				Title = Strings.AboutUsCEO,
				Description = Strings.AboutUsDescriptionBeatriz,
				FunnyDescription = Strings.AboutUsFunnyDescriptionBeatriz,
				Photo = "beatriz.png"
			};
		}

		private ThunderMateModel GetNickFury()
		{
			return new ThunderMateModel
			{
				Name = "Josué Yeray Julián Ferreiro",
				Title = Strings.AboutUsCTO,
				Description = Strings.AboutUsDescriptionYeray,
				FunnyDescription = Strings.AboutUsFunnyDescriptionYeray,
				Photo = "yeray.png"
			};
		}

		private ThunderMateModel GetBeast()
		{
			return new ThunderMateModel
			{
				Name = "Ciani Afonso Díaz",
				Title = Strings.AboutUsMobileDeveloperSenior,
				Description = Strings.AboutUsDescriptionCiani,
				FunnyDescription = Strings.AboutUsFunnyDescriptionCiani,
				Photo = "ciani.png"
			};
		}

		private ThunderMateModel GetDoctorStrange()
		{
			return new ThunderMateModel
			{
				Name = "Marcos Antonio Blanco Arellano",
				Title = Strings.AboutUsMobileDeveloperSenior,
				Description = Strings.AboutUsDescriptionMarco,
				FunnyDescription = Strings.AboutUsFunnyDescriptionMarco,
				Photo = "marco.png"
			};
		}

		private ThunderMateModel GetSpiderMan()
		{
			return new ThunderMateModel
			{
				Name = "Jorge Diego Crespo",
				Title = Strings.AboutUsMobileDeveloper,
				Description = Strings.AboutUsDescriptionJorge,
				FunnyDescription = Strings.AboutUsFunnyDescriptionJorge,
				Photo = "jorge.png"
			};
		}

		private ThunderMateModel GetStarLord()
		{
			return new ThunderMateModel
			{
				Name = "Luis Marcos Rivera",
				Title = Strings.AboutUsMobileDeveloper,
				Description = Strings.AboutUsDescriptionLuis,
				FunnyDescription = Strings.AboutUsFunnyDescriptionLuis,
				Photo = "luis.png"
			};
		}
	}
}
