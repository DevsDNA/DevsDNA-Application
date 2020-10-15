namespace DevsDNA.Application.Features.AboutUs
{
	using DevsDNA.Application.Services;
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using Xamarin.Forms;

	public partial class ThunderMateControl
	{
		private readonly ILogService logService;
		private readonly int writterTime = 75;
		private CancellationTokenSource cancellationTokenSource;
		private CancellationToken cancellationToken;

		public ThunderMateControl()
		{
			logService = CustomDependencyService.Instance.Get<ILogService>();
			InitializeComponent();
		}


		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			if (BindingContext is ThunderMateModel compi)
			{
				cancellationTokenSource?.Cancel();
				cancellationTokenSource?.Dispose();
				SetData(compi).ConfigureAwait(true);
			}
		}


		private async Task SetData(ThunderMateModel thunderMate)
		{
			try
			{
				AutomationProperties.SetName(LblTitle, thunderMate.Title);
				AutomationProperties.SetName(LblName, thunderMate.Name);
				AutomationProperties.SetName(LblDescription, thunderMate.Description);
				AutomationProperties.SetHelpText(LblDescription, thunderMate.FunnyDescription);

				LblDescription.Opacity = 1;
				LblDescription.Scale = 1;
				LblDescription.GestureRecognizers.Clear();
				LblFunnyDescription.Opacity = 0;
				LblFunnyDescription.Scale = 0;
				LblFunnyDescription.GestureRecognizers.Clear();

				ImgThunderMate.Source = ImageSource.FromFile(thunderMate.Photo);
				LblName.Text = thunderMate.Name;
				LblDescription.Text = thunderMate.Description;
				LblFunnyDescription.Text = thunderMate.FunnyDescription;

				LblDescription.GestureRecognizers.Add(new TapGestureRecognizer { Command = TapDescriptionCommandExecute(), NumberOfTapsRequired = 2 });
				LblFunnyDescription.GestureRecognizers.Add(new TapGestureRecognizer { Command = TapFunnyDescriptionCommandExecute(), NumberOfTapsRequired = 2 });
				await SetTitle(thunderMate);
			}
			catch (OperationCanceledException ex)
			{
				logService.LogError(ex);
			}
		}

		private ICommand TapDescriptionCommandExecute()
		{
			return new Command<string>(
				newDescription =>
				{
					LblDescription.FadeTo(0);
					LblDescription.ScaleTo(0);
					LblFunnyDescription.FadeTo(1);
					LblFunnyDescription.ScaleTo(1, easing: Easing.BounceIn);
					RaiseChild(LblFunnyDescription);
				});
		}

		private ICommand TapFunnyDescriptionCommandExecute()
		{
			return new Command<string>(
				newDescription =>
				{
					LblFunnyDescription.FadeTo(0);
					LblFunnyDescription.ScaleTo(0);
					LblDescription.FadeTo(1);
					LblDescription.ScaleTo(1, easing: Easing.BounceIn);
					RaiseChild(LblDescription);
				});
		}

		private async Task SetTitle(ThunderMateModel compi)
		{
			cancellationTokenSource = new CancellationTokenSource();
			cancellationToken = cancellationTokenSource.Token;

			LblTitle.Text = compi.Title;
			cancellationToken.ThrowIfCancellationRequested();
			await LblTitle.ShowAsync(cancellationToken, writterTime, false);
		}
	}
}