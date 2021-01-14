namespace DevsDNA.Application.Features.Splash
{
	using DevsDNA.Application.Common;
	using ReactiveUI;
	using System;
	using System.Reactive;
	using System.Reactive.Disposables;
	using System.Reactive.Linq;
	using System.Threading.Tasks;
	using Xamarin.Forms;
	using Xamarin.Forms.PlatformConfiguration;
	using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

    public partial class SplashView
	{
		private bool flagPosition;

		public SplashView()
		{
			InitializeComponent();
		}

		protected override CompositeDisposable CreateBindings(CompositeDisposable disposables)
		{
			base.CreateBindings(disposables);
			
			IObservable<EventPattern<object>> observableAnimationEvent = Observable.FromEventPattern(h => LottieLogo.OnFinish += h, h => LottieLogo.OnFinish -= h);
			disposables.Add(observableAnimationEvent.Subscribe(ep => ViewModel.NavigateCommand.Execute()));
			disposables.Add(ViewModel.FinishedSplash.Subscribe(async _ => await FinishedSplashCompletedAsync(), LogService.LogError));
			disposables.Add(this.OneWayBind(ViewModel, vm => vm.StartCommand, v => v.BtnStart.Command)); 
			disposables.Add(this.OneWayBind(ViewModel, vm => vm.StartCommand, v => v.BtnStart2.Command));

			return disposables;
		}

		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);
			FixPositionControls();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			Thickness safeArea = On<iOS>().SafeAreaInsets();
			safeArea.Bottom = 0;
			GlobalInfo.SafeAreaInset = safeArea;
		}

		private async void FixPositionControls()
		{
			if (flagPosition)
				return;
			flagPosition = true;

			AdjustLogoPosition();

			// HorizontalOptions and VerticalOptions not works in Lottie.
			double emptySpace = (LottieBackground.Height - LottieBackground.Width) / 2;
			LottieBackground.TranslationY = -emptySpace + 5;
			await Task.WhenAll(LottieBackground.FadeTo(1), LottieLogo.FadeTo(1));
			LottieBackground.Play();
			LottieLogo.Play();
		}

		private void AdjustLogoPosition()
        {
			if (On<iOS>().SafeAreaInsets().Top > ImgLogo.Margin.Top)
			{
				ImgLogo.Margin = new Thickness(ImgLogo.Margin.Left, On<iOS>().SafeAreaInsets().Top, ImgLogo.Margin.Right, ImgLogo.Margin.Bottom);
			}
		}

		private async Task FinishedSplashCompletedAsync()
		{
			BtnStart.IsVisible = true;
			FrameStart.IsVisible = true;
			AutomationProperties.SetIsInAccessibleTree(BtnStart, true);
			AutomationProperties.SetIsInAccessibleTree(BtnStart2, true);

			await Task.WhenAll(GridSplash.FadeTo(0), GridInitialScreen.FadeTo(1));
			await ShowBackgroundInitialScreenAsync();
			await LblSlogan.ShowAsync();
			await ShowLogoAsync();
			await BtnStart.FadeTo(1);
			await FrameStart.FadeTo(1);
		}

		private async Task ShowBackgroundInitialScreenAsync()
        {
			double emptySpace = (LottieBackgroundInitialScreen.Height - LottieBackgroundInitialScreen.Width) / 2;
			LottieBackgroundInitialScreen.TranslationY = emptySpace - 5;
			await Task.WhenAll(LottieBackgroundInitialScreen.FadeTo(1));
			LottieBackgroundInitialScreen.Play();
		}

		private async Task ShowLogoAsync()
		{
			await Task.WhenAll(ImgLogo.FadeTo(1), ImgLogo.TranslateTo(0, 0));
		}
	}
}