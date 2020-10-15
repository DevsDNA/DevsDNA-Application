namespace DevsDNA.Application.Features.Main
{
    using DevsDNA.Application.Common;
    using DevsDNA.Application.Services;
	using DevsDNA.Application.Strings;
    using ReactiveUI;
    using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Xamarin.Essentials;
	using Xamarin.Forms;

	public partial class ButtonFan
	{
		private const uint animationLenght = 500;
		private readonly double shadowOpacity;
		private readonly ILogService logService;
		private readonly ITrackService trackService;
		private bool isOpen;
		private bool isDisappeared;

		public ButtonFan()
		{
			logService = CustomDependencyService.Instance.Get<ILogService>();
			trackService = CustomDependencyService.Instance.Get<ITrackService>();
			InitializeComponent();
			shadowOpacity = Device.RuntimePlatform == Device.iOS ? 0.7 : 0.5;
			TapFrameButtonLogo.Command = ReactiveCommand.CreateFromTask(TapFrameButtonLogoCommandExecuteAsync);
			Btn1.Command = ReactiveCommand.CreateFromTask(Btn1CommandExecuteAsync);
			Btn2.Command = ReactiveCommand.CreateFromTask(Btn2CommandExecuteAsync);
			Btn3.Command = ReactiveCommand.CreateFromTask(Btn3CommandExecuteAsync);
			Btn4.Command = ReactiveCommand.CreateFromTask(Btn4CommandExecuteAsync);
		}

		public TabMain CurrentTab { get; set; }

		public Task DisappearAsync()
        {
			if (isDisappeared)
				return Task.CompletedTask;

			isDisappeared = true;
			return this.TranslateTo(0, Height, 400, easing: Easing.Linear);
		}

		public async Task AppearAsync()
        {
			if (!isDisappeared)
				return;

			isDisappeared = false;
			await this.TranslateTo(0, 0, 400, easing: Easing.Linear);
		}

		public async Task CloseAsync()
		{
			if (!isOpen)
				return;

			isOpen = false;
			await CloseAnimationAsync();
		}

		public async Task OpenAsync()
		{
			if (isOpen)
				return;

			isOpen = true;
			if (CurrentTab != TabMain.SocialNetwork)
				await OpenDefaultAnimationAsync();
			else
				await OpenSocialNetworkAnimationAsync();
		}	

		private async Task OpenDefaultAnimationAsync()
        {			
			LottieLogo.Progress = 0;
			LottieLogo.Play();

			Btn1.Source = "phone.png"; Btn1.Rotation = -90;
			Btn2.Source = "talk.png"; Btn2.Rotation = 180;
			Btn3.Source = "shareColor.png"; Btn3.Rotation = 90;

			AutomationProperties.SetName(Btn1, Strings.AccessibleBtnCallDevsDNA);
			AutomationProperties.SetName(Btn2, Strings.AccessibleBtnMailDevsDNA);
			AutomationProperties.SetName(Btn3, Strings.AccessibleBtnShareDevsDNAApp);

			AutomationProperties.SetIsInAccessibleTree(Btn1, true);
			AutomationProperties.SetIsInAccessibleTree(Btn2, true);
			AutomationProperties.SetIsInAccessibleTree(Btn3, true);
			AutomationProperties.SetIsInAccessibleTree(Btn4, false);

			Easing easing = Easing.CubicOut;

			Shadows1.IsVisible = true;
			Shadows2.IsVisible = true;
			Shadows3.IsVisible = true;

			await Task.WhenAll(Shadows1.TranslateTo(65, -20, animationLenght, easing),
							   Shadows1.ScaleTo(1, animationLenght / 5, easing),
							   Btn1.RotateTo(0, animationLenght, easing),

							   Shadows2.TranslateTo(0, -70, animationLenght, easing),
							   Shadows2.ScaleTo(1, animationLenght / 5, easing),
							   Btn2.RotateTo(0, animationLenght, easing),

							   Shadows3.TranslateTo(-65, -20, animationLenght, easing),
							   Shadows3.ScaleTo(1, animationLenght / 5, easing),
							   Btn3.RotateTo(0, animationLenght, easing));
		}
	 
		private async Task OpenSocialNetworkAnimationAsync()
        {
			LottieLogo.Progress = 0;
			LottieLogo.Play();
			
			Btn1.Source = "facebook_white.png"; Btn1.Rotation = -90;
			Btn2.Source = "twitter_white.png"; Btn2.Rotation = -90;
			Btn3.Source = "instagram_white.png"; Btn3.Rotation = 90;
			Btn4.Source = "linkedin_white.png"; Btn4.Rotation = 90;

			AutomationProperties.SetName(Btn1, Strings.AccessibleBtnOpenFacebookDevsDNA);
			AutomationProperties.SetName(Btn2, Strings.AccessibleBtnOpenTwitterDevsDNA);
			AutomationProperties.SetName(Btn3, Strings.AccessibleBtnOpenInstagramDevsDNA);
			AutomationProperties.SetName(Btn4, Strings.AccessibleBtnOpenLinkedinDevsDNA);

			AutomationProperties.SetIsInAccessibleTree(Btn1, true);
			AutomationProperties.SetIsInAccessibleTree(Btn2, true);
			AutomationProperties.SetIsInAccessibleTree(Btn3, true);
			AutomationProperties.SetIsInAccessibleTree(Btn4, true);

			Easing easing = Easing.CubicOut;

			Shadows1.IsVisible = true;
			Shadows2.IsVisible = true;
			Shadows3.IsVisible = true;
			Shadows4.IsVisible = true;

			await Task.WhenAll(Shadows1.TranslateTo(35, -65, animationLenght, easing),
							   Shadows1.ScaleTo(1, animationLenght / 5, easing),
							   Btn1.RotateTo(0, animationLenght, easing),

							   Shadows2.TranslateTo(66, 0, animationLenght, easing),
							   Shadows2.ScaleTo(1, animationLenght / 5, easing),
							   Btn2.RotateTo(0, animationLenght, easing),

							   Shadows3.TranslateTo(-35, -65, animationLenght, easing),
							   Shadows3.ScaleTo(1, animationLenght / 5, easing),
							   Btn3.RotateTo(0, animationLenght, easing),

							   Shadows4.TranslateTo(-66, 0, animationLenght, easing),
							   Shadows4.ScaleTo(1, animationLenght / 5, easing),
							   Btn4.RotateTo(0, animationLenght, easing));
		}
	 
		private async Task CloseAnimationAsync()
        {
			float from = Device.RuntimePlatform == Device.iOS ? 0.45f : 1f;
			LottieLogo.Progress = from;
			LottieLogo.PlayProgressSegment(from, 0f);

			Easing easing = Easing.CubicIn;
			await Task.WhenAll(Shadows1.TranslateTo(0, 0, animationLenght, easing),
							   Shadows1.ScaleTo(0, animationLenght * 5, easing),
							   Btn1.RotateTo(-90, animationLenght, easing),

							   Shadows2.TranslateTo(0, 0, animationLenght, easing),
							   Shadows2.ScaleTo(0, animationLenght * 5, easing),
							   Btn2.RotateTo(-180, animationLenght, easing),

							   Shadows3.TranslateTo(0, 0, animationLenght, easing),
							   Shadows3.ScaleTo(0, animationLenght * 5, easing),
							   Btn3.RotateTo(90, animationLenght, easing),

							   Shadows4.TranslateTo(0, 0, animationLenght, easing),
							   Shadows4.ScaleTo(0, animationLenght * 5, easing),
							   Btn4.RotateTo(180, animationLenght, easing));

			Shadows1.IsVisible = false;
			Shadows2.IsVisible = false;
			Shadows3.IsVisible = false;
			Shadows4.IsVisible = false;
		}


		private async Task TapFrameButtonLogoCommandExecuteAsync()
		{
			if (isOpen)
				await CloseAsync();
			else
				await OpenAsync();
        }

		private Task Btn1CommandExecuteAsync()
		{
			if (CurrentTab == TabMain.SocialNetwork)
				return Task.WhenAll(OpenBrowserAsync(SettingsKeyValues.UrlDevsDNAFacebook), CloseAsync());

			return Task.WhenAll(OpenPhoneAsync(), CloseAsync());
        }
		   
        private Task Btn2CommandExecuteAsync()
        {
			if (CurrentTab == TabMain.SocialNetwork)
				return Task.WhenAll(OpenBrowserAsync(SettingsKeyValues.UrlDevsDNATwitter), CloseAsync());

			return Task.WhenAll(SendEmailAsync(), CloseAsync());
        }
       
        private Task Btn3CommandExecuteAsync()
        {
			if (CurrentTab == TabMain.SocialNetwork)
				return Task.WhenAll(OpenBrowserAsync(SettingsKeyValues.UrlDevsDNAInstagram), CloseAsync());

			return Task.WhenAll(ShareAsync(), CloseAsync());
        }

		private Task Btn4CommandExecuteAsync()
		{
			if(CurrentTab == TabMain.SocialNetwork)
				return Task.WhenAll(OpenBrowserAsync(SettingsKeyValues.UrlDevsDNALinkedIn), CloseAsync());

			return CloseAsync();
		}


		private async Task OpenPhoneAsync()
		{
			try
			{
				PhoneDialer.Open(SettingsKeyValues.PhoneNumberDevsDNA);
			}
			catch (FeatureNotSupportedException ex)
			{
				logService.LogError(ex);
				trackService.TrackError(ex);
				await Application.Current.MainPage.DisplayAlert(Strings.AlertError, Strings.AlertPhoneDialerNotSupported, Strings.AlertOk);
			}
			catch (Exception ex)
			{
				logService.LogError(ex);
				trackService.TrackError(ex);
				await Application.Current.MainPage.DisplayAlert(Strings.AlertError, Strings.AlertUnexpectedError, Strings.AlertOk);
			}
		}

		private async Task SendEmailAsync()
		{
			try
			{
				EmailMessage message = new EmailMessage
				{
					Subject = Strings.ButtonFanMailSubject,
					Body = Strings.ButtonFanMailText,
					To = new List<string> { SettingsKeyValues.EmailDevsDNA }
				};

				await Email.ComposeAsync(message);
			}
			catch (FeatureNotSupportedException ex)
			{
				logService.LogError(ex);
				trackService.TrackError(ex);
				await Application.Current.MainPage.DisplayAlert(Strings.AlertError, Strings.AlertMailNotSupported, Strings.AlertOk);
			}
			catch (Exception ex)
			{
				logService.LogError(ex);
				trackService.TrackError(ex);
				await Application.Current.MainPage.DisplayAlert(Strings.AlertError, Strings.AlertUnexpectedError, Strings.AlertOk);
			}
		}

		private Task ShareAsync()
		{
			return Share.RequestAsync(new ShareTextRequest
			{
				Text = Strings.ButtonFanShareTitle,
				Title = Strings.ButtonFanShareTitle,
				Uri = Device.RuntimePlatform == Device.Android ? SettingsKeyValues.UrlDevsDNAPlayStore : SettingsKeyValues.UrlDevsDNAAppStore
			});
		}

		private Task OpenBrowserAsync(string uri)
        {
			return Browser.OpenAsync(uri, BrowserLaunchMode.External);
		}
	}
}