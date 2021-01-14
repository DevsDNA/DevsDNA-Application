namespace DevsDNA.Application.Features.Main
{
	using DevsDNA.Application.Common;
	using DevsDNA.Application.Features.AboutUs;
	using DevsDNA.Application.Features.News;
	using DevsDNA.Application.Features.SocialNetwork;
	using DevsDNA.Application.Features.Videos;
	using DevsDNA.Application.Services;
	using ReactiveUI;
	using System;
	using System.Linq;
	using System.Reactive;
	using System.Reactive.Disposables;
	using System.Reactive.Linq;
	using System.Threading.Tasks;
	using Xamarin.Forms;

	public partial class MainView
	{
		private const int scrollOffsetToHideHeader = 200;

		private readonly IDependencyService dependencyService;
		private readonly ILogService logService;
		private readonly ITrackService trackService;
		private readonly IAccessibilityCheckerService accessibilityCheckerService;
		private readonly TabMain initialTab;

		private readonly CompositeDisposable disposables;

		private TabMain currentTab;
		private bool flagPosition;
		private bool isAlreadyInitialized;
		private bool transitionExecuting;
		private bool flagInitialAnimation;
		private bool initialAnimationExecuting;
		private double row0Height;
		private bool isVoiceAssistantActive;

		private NewsView newsView;
		private VideosView videosView;
		private SocialNetworkView socialNetworkView;
		private AboutUsView aboutUsView;


		public MainView() : this(TabMain.News) { }

		public MainView(TabMain initialTab)
		{
			dependencyService = CustomDependencyService.Instance;
			logService = dependencyService.Get<ILogService>();
			trackService = dependencyService.Get<ITrackService>();
			accessibilityCheckerService = dependencyService.Get<IAccessibilityCheckerService>();

			InitializeComponent();
			disposables = new CompositeDisposable();
			Padding = GlobalInfo.SafeAreaInset;

			SetCommands();

			currentTab = TabMain.Unassigned;
			this.initialTab = initialTab;
			isVoiceAssistantActive = accessibilityCheckerService.IsVoiceAssistantActive();
		}

		~MainView()
		{
			disposables?.Dispose();
		}

		public bool TransitionExecuting
		{
			get => transitionExecuting;
			set { transitionExecuting = value; OnPropertyChanged(); }
		}

		public bool InitialAnimationExecuting
		{
			get => initialAnimationExecuting;
			set { initialAnimationExecuting = value; OnPropertyChanged(); }
		}

		public ReactiveCommand<TabMain, Unit> SelectedTabCommand { get; private set; }

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			if (isAlreadyInitialized)
				return;
			isAlreadyInitialized = true;

			FixPositionControls();
			DoInitialAnimation();

			if (currentTab != TabMain.Unassigned)
				await SetScreenAsync(currentTab);
			else
				await SetScreenAsync(initialTab, true);
		}


		private async void FixPositionControls()
		{
			if (flagPosition)
				return;
			flagPosition = true;

			row0Height = MainGrid.Height * 0.2;
			MainGrid.RowDefinitions[0].Height = new GridLength(row0Height);
			MainGrid.RowDefinitions[1].Height = new GridLength(Math.Min(row0Height / 4, 40));
			double row3Height = (MainGrid.Height * 0.44) - (MainGrid.RowDefinitions[0].Height.Value + MainGrid.RowDefinitions[1].Height.Value + MainGrid.RowDefinitions[2].Height.Value);
			MainGrid.RowDefinitions[3].Height = new GridLength(row3Height);
			LottieHeader.Margin = new Thickness(30, row0Height / 8, 30, row0Height / 8);

			// HorizontalOptions and VerticalOptions not works in Lottie. 
			double emptySpace1 = BoxBackground.Y;
			LottieBackground1.TranslationY = emptySpace1 - LottieBackground1.Height;
			LottieBackground1.TranslationX = LottieBackground1.Width / 2;
			LottieBackground1.Play();

			double movementX = LottieBackground2.Width / 2;
			double emptySpace2 = (LottieBackground2.Height - LottieBackground2.Width) / 2;
			LottieBackground2.Scale = 0.75;
			LottieBackground2.TranslationY = -emptySpace2;
			LottieBackground2.TranslationX = movementX;
			LottieBackground2.Play();

			if (initialTab == TabMain.AboutUs)
				await LottieBackground2.FadeTo(1);
			else
				await LottieBackground1.FadeTo(1);
		}

		private async void DoInitialAnimation()
		{
			if (flagInitialAnimation)
				return;
			flagInitialAnimation = true;

			InitialAnimationExecuting = true;

			LottieHeader.Play();
			await BtnNews.WriteText(Strings.Strings.MainNewButton, true);
			await BtnVideos.WriteText(Strings.Strings.MainVideosButton);
			await BtnSocialNetworks.WriteText(Strings.Strings.MainSocialNetworkButton);
			await BtnAboutUs.WriteText(Strings.Strings.MainAboutUs);
			await MarkButtonAsSelectedAsync(initialTab);
			await BtnBottom.FadeTo(1);
			MainGrid.RaiseChild(BtnBottom);

			InitialAnimationExecuting = false;
		}

		private void SetCommands()
		{
			SelectedTabCommand = ReactiveCommand.CreateFromTask<TabMain>(tab => SetScreenAsync(tab), SelectedTabCommandCanExecute());
			BtnNews.Command = SelectedTabCommand;
			BtnVideos.Command = SelectedTabCommand;
			BtnSocialNetworks.Command = SelectedTabCommand;
			BtnAboutUs.Command = SelectedTabCommand;

			TapFrameButtonLogo.Command = ReactiveCommand.Create(() => LogoInitScrollCommand());
			LottieLogo.ClickedCommand = ReactiveCommand.Create(() => LogoInitScrollCommand());
		}

		private void LogoInitScrollCommand()
		{
			View currentView = GetCurrentContent()?.Content;
			if (currentView is AboutUsView aboutUsView)
			{
				aboutUsView.ScrollToInit();
			}
			else if (currentView is SocialNetworkView socialNetworkView)
			{
				socialNetworkView.ScrollToInitAsync().ConfigureAwait(false);
			}
		}

		private IObservable<bool> SelectedTabCommandCanExecute()
		{
			return this.WhenAnyValue(v => v.IsBusy, v => v.TransitionExecuting, v => v.InitialAnimationExecuting, (ib, te, ia) => !ib && !te && !ia);
		}

		private async Task SetScreenAsync(TabMain nextTab, bool isInitialAnimation = false)
		{
			try
			{
				if (IsBusy || nextTab == currentTab)
					return;

				IsBusy = true;
				await ChangeTabVisibleAsync(nextTab, isInitialAnimation);
			}
			catch (Exception ex)
			{
				logService.LogError(ex);
			}
			IsBusy = false;
		}

		private async Task ChangeTabVisibleAsync(TabMain nextTab, bool isInitialAnimation)
		{
			try
			{
				ContentView currentContent = GetCurrentContent() ?? ContentAboutUs;
				ContentView nextContent = null;

				switch (nextTab)
				{
					case TabMain.News:
						nextContent = ContentNews;
						nextContent.Content ??= CreateNewsView();
						MainGrid.RaiseChild(nextContent);
						break;
					case TabMain.Videos:
						nextContent = ContentVideos;
						nextContent.Content ??= CreateVideosView();
						MainGrid.RaiseChild(nextContent);
						break;
					case TabMain.SocialNetwork:
						nextContent = ContentSocialNetworks;
						nextContent.Content ??= CreateSocialNetworkView();
						MainGrid.RaiseChild(FrameButtonLogo);
						break;
					case TabMain.AboutUs:
						nextContent = ContentAboutUs;
						nextContent.Content ??= CreateAboutUsView();
						MainGrid.RaiseChild(FrameButtonLogo);
						break;
					case TabMain.Unassigned:
					default:
						throw new Exception("Tab not supported.");
				}

				MainGrid.RaiseChild(BtnBottom);
				BtnBottom.CurrentTab = nextTab;

				await DoTransitionToNextTab(currentContent, nextContent, nextTab, isInitialAnimation);
				(nextContent.Content as VideosView)?.ViewModel?.SetData();
				if (isVoiceAssistantActive)
					ClearPreviousTabContent(nextTab);
			}
			catch (Exception ex)
			{
				trackService.TrackError(ex);
				logService.LogError(ex);
			}
		}

		private void ClearPreviousTabContent(TabMain tab)
		{
			switch (tab)
			{
				case TabMain.News:
					ContentVideos.Content = null;
					ContentSocialNetworks.Content = null;
					ContentAboutUs.Content = null;
					break;
				case TabMain.Videos:
					ContentNews.Content = null;
					ContentSocialNetworks.Content = null;
					ContentAboutUs.Content = null;
					break;
				case TabMain.SocialNetwork:
					ContentNews.Content = null;
					ContentVideos.Content = null;
					ContentAboutUs.Content = null;
					break;
				case TabMain.AboutUs:
					ContentNews.Content = null;
					ContentVideos.Content = null;
					ContentSocialNetworks.Content = null;
					break;
			}
		}

		private NewsView CreateNewsView()
		{
			NewsView newsView = new NewsView { ViewModel = new NewsViewModel(dependencyService) };

			disposables.Add(newsView.ScrolledObservable.Subscribe(async _ => await BtnBottom.CloseAsync()));
			disposables.Add(newsView.OpeningDetailObservable.Subscribe(async _ => await BtnBottom.CloseAsync()));

			return newsView;
		}

		private VideosView CreateVideosView()
		{
			VideosView videosView = new VideosView { ViewModel = new VideosViewModel(dependencyService) };

			disposables.Add(videosView.ScrolledObservable.Subscribe(async _ => await BtnBottom.CloseAsync()));
			disposables.Add(videosView.OpeningVideoObservable.Subscribe(async _ => await BtnBottom.CloseAsync()));

			return videosView;
		}

		private SocialNetworkView CreateSocialNetworkView()
		{
			SocialNetworkView socialNetworkView = new SocialNetworkView(MainGrid.RowDefinitions[0].Height.Value + MainGrid.RowDefinitions[1].Height.Value + 20) { ViewModel = new SocialNetworkViewModel(dependencyService) };

			disposables.Add(socialNetworkView.ScrolledObservable.ObserveOn(RxApp.MainThreadScheduler)
								.Where(c => !InitialAnimationExecuting).Subscribe(ScrolledObservableSubscription, logService.LogError));

			disposables.Add(socialNetworkView.OpeningDetailObservable.Subscribe(async _ => { await BtnBottom.CloseAsync(); await BtnBottom.DisappearAsync(); }));

			disposables.Add(socialNetworkView.ClosingDetailObservable
										.WithLatestFrom(socialNetworkView.ScrolledObservable.Select(s => s.VerticalOffset).StartWith(0), (a, b) => b)
										.Where(vo => vo <= scrollOffsetToHideHeader).Subscribe(async _ => await BtnBottom.AppearAsync()));
			return socialNetworkView;
		}

		private AboutUsView CreateAboutUsView()
		{
			AboutUsView aboutUsView = new AboutUsView(MainGrid.RowDefinitions[0].Height.Value + MainGrid.RowDefinitions[1].Height.Value + 20) { ViewModel = new AboutUsViewModel(dependencyService) };

			disposables.Add(aboutUsView.ScrolledObservable.ObserveOn(RxApp.MainThreadScheduler)
											.Where(c => !InitialAnimationExecuting).Subscribe(ScrolledObservableSubscription, logService.LogError));
			return aboutUsView;
		}


		private async Task DoTransitionToNextTab(ContentView currentContent, ContentView nextContent, TabMain nextTab, bool isInitialAnimation)
		{
			TransitionExecuting = true;
			ITransitionable currentAnimatedView = (ITransitionable)currentContent.Content;
			ITransitionable nextAnimatedView = (ITransitionable)nextContent.Content;
			await nextAnimatedView.Reset();

			await AnimationTransition(currentContent, nextContent, nextTab, currentAnimatedView, nextAnimatedView, isInitialAnimation);
		}

		private async Task AnimationTransition(ContentView currentContent, ContentView nextContent, TabMain nextTab, ITransitionable currentAnimatedView, ITransitionable nextAnimatedView, bool isInitialAnimation)
		{
			if (isInitialAnimation)
				await (currentAnimatedView?.DoDissappearingAnimationAsync() ?? Task.CompletedTask);
			else
				await Task.WhenAll(MarkButtonAsSelectedAsync(nextTab), currentAnimatedView?.DoDissappearingAnimationAsync() ?? Task.CompletedTask, BtnBottom.CloseAsync());

			currentContent.Opacity = 0;
			currentContent.InputTransparent = true;
			LastTabAnimation(nextTab, nextContent);

			nextContent.InputTransparent = false;
			nextContent.Opacity = 1;

			await (nextAnimatedView?.DoAppearingAnimationAsync() ?? Task.CompletedTask);
			currentTab = nextTab;
			TransitionExecuting = false;
		}

		private void LastTabAnimation(TabMain nextTab, ContentView nextContent)
		{
			if (nextTab != TabMain.AboutUs && currentTab != TabMain.AboutUs && currentTab != TabMain.Unassigned)
				return;

			if (currentTab == TabMain.AboutUs || nextTab != TabMain.AboutUs)
				DisappearLastTabAnimation();
			else if (currentTab != TabMain.AboutUs || nextTab == TabMain.AboutUs)
				AppearLastTabAnimation(nextContent);
		}

		private void AppearLastTabAnimation(ContentView nextContent)
		{
			BoxBackground.VerticalOptions = LayoutOptions.Start;
			Animation appearLastTab = new Animation(x => { BoxBackground.HeightRequest = x; BoxBackground.TranslationY = nextContent.Height - x; }, nextContent.Height, 0);
			appearLastTab.Commit(this, nameof(appearLastTab), length: 500);
			Task.WhenAll(LottieBackground1.FadeTo(0), LottieBackground2.FadeTo(1));
		}

		private void DisappearLastTabAnimation()
		{
			BoxBackground.VerticalOptions = LayoutOptions.End;
			BoxBackground.TranslationY = 0;
			Animation disappearLastTab = new Animation(x => { BoxBackground.HeightRequest = x; }, 0, Height);
			disappearLastTab.Commit(this, nameof(disappearLastTab), length: 500);
			Task.WhenAll(LottieBackground1.FadeTo(1), LottieBackground2.FadeTo(0));
		}


		private async Task MarkButtonAsSelectedAsync(TabMain nextTab)
		{
			try
			{
				BtnNews.IsSelected = nextTab == TabMain.News;
				BtnVideos.IsSelected = nextTab == TabMain.Videos;
				BtnSocialNetworks.IsSelected = nextTab == TabMain.SocialNetwork;
				BtnAboutUs.IsSelected = nextTab == TabMain.AboutUs;
				await MoveBottomLineAsync(nextTab);
			}
			catch (Exception ex)
			{
				trackService.TrackError(ex);
				logService.LogError(ex);
			}
		}

		private async Task MoveBottomLineAsync(TabMain nextTab)
		{
			try
			{
				int movement = nextTab - currentTab;
				bool moveToLeft = movement < 0;
				if (moveToLeft)
					await MoveBottomLineToLeftAsync((uint)-movement);
				else
					await MoveBottomLineToRigthAsync((uint)movement);
				await BoxSelected.FadeTo(1, 50);
			}
			catch (Exception ex)
			{
				trackService.TrackError(ex);
				logService.LogError(ex);
			}
		}

		private async Task MoveBottomLineToLeftAsync(uint durationIndex)
		{
			uint miliseconds = 200;
			TabButton selectedButton = GetSelectedButton();
			TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

			if (selectedButton is null)
				return;

			Animation animationBall = new Animation(x => BoxSelected.WidthRequest = x, BoxSelected.WidthRequest, 4, Easing.CubicIn);
			animationBall.Commit(this, nameof(animationBall), length: miliseconds,
				finished: (d, b) =>
				{
					if (b)
						tcs.TrySetResult(b);

					Animation animationMovements = new Animation(x => BoxSelected.TranslationX = x, BoxSelected.TranslationX, selectedButton.X + selectedButton.Width - 4, Easing.CubicOut);
					animationMovements.Commit(this, nameof(animationMovements), length: miliseconds * durationIndex,
					finished: (d, b) =>
					{
						if (b)
							tcs.TrySetResult(b);

						double savedTranslationX = BoxSelected.TranslationX;
						Animation animationResize = new Animation(x =>
						{
							BoxSelected.WidthRequest = x;
							if (x >= 4)
								BoxSelected.TranslationX = savedTranslationX - x + 4;
						}, 4, selectedButton.Width, Easing.CubicInOut);
						animationResize.Commit(this, nameof(animationResize), length: miliseconds, finished: (d, b) =>
						{
							tcs.TrySetResult(b);
						});
					});
				});

			await tcs.Task;
		}

		private async Task MoveBottomLineToRigthAsync(uint durationIndex)
		{
			uint miliseconds = 200;
			double savedTranslationX = BoxSelected.TranslationX;
			double saveWitdhRequest = BoxSelected.WidthRequest;
			TabButton selectedButton = GetSelectedButton();
			TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

			if (selectedButton is null)
				return;

			Animation animationBall = new Animation(x =>
			{
				BoxSelected.WidthRequest = x;

				if (x >= 4)
					BoxSelected.TranslationX = savedTranslationX + (saveWitdhRequest - x);

			}, BoxSelected.WidthRequest, 4, Easing.CubicIn);
			animationBall.Commit(this, nameof(animationBall), length: miliseconds,
				finished: (d, b) =>
				{
					BoxSelected.WidthRequest = 4;
					if (b)
						tcs.TrySetResult(b);

					Animation animationMovements = new Animation(x => BoxSelected.TranslationX = x, BoxSelected.TranslationX, selectedButton.X, Easing.CubicOut);
					animationMovements.Commit(this, nameof(animationMovements), length: miliseconds * durationIndex,
						finished: (d, b) =>
						{
							if (b)
								tcs.TrySetResult(b);

							Animation animationResize = new Animation(x => BoxSelected.WidthRequest = x, 4, selectedButton.Width, Easing.CubicInOut); ;
							animationResize.Commit(this, nameof(animationResize), length: miliseconds,
								finished: (d, b) =>
								{
									BoxSelected.WidthRequest = selectedButton.Width;
									tcs.TrySetResult(b);
								});
						});
				});

			await tcs.Task;
		}

		private TabButton GetSelectedButton()
		{
			return (TabButton)GridButtons.Children.FirstOrDefault(c => c is TabButton button && button.IsSelected);
		}

		private ContentView GetCurrentContent()
		{
			return (ContentView)MainGrid.Children.FirstOrDefault(c => c is ContentView view && view.Opacity == 1);
		}

		private void ScrolledObservableSubscription(ItemsViewScrolledEventArgs e)
		{
			HandleButtonFanAspectAsync(e).ConfigureAwait(false);

			double marginLottie = LottieHeader.Margin.Top;

			if (e.VerticalOffset < scrollOffsetToHideHeader && e.VerticalOffset > 5)
			{
				marginLottie = (row0Height / 4) - (e.VerticalOffset / 10);
				Task.WhenAll(GridButtons.FadeTo(0), GridButtons.ScaleTo(0), BoxSelected.FadeTo(0, 50), LottieHeader.FadeTo(1 - (marginLottie / 100), 50), FrameButtonLogo.FadeTo(0), LottieLogo.FadeTo(0));
			}
			else if (e.VerticalOffset < 5)
			{
				Task.WhenAll(GridButtons.FadeTo(1), GridButtons.ScaleTo(1, 50), BoxSelected.FadeTo(1, 50), LottieHeader.FadeTo(1, 50), FrameButtonLogo.FadeTo(0), LottieLogo.FadeTo(0));
				marginLottie = row0Height / 4;
			}
			else if (e.VerticalOffset > scrollOffsetToHideHeader)
			{
				if (FrameButtonLogo.Opacity == 0)
				{
					Task.WhenAll(FrameButtonLogo.FadeTo(1), LottieLogo.FadeTo(1));
					LottieLogo.Play();
				}

				marginLottie = 0;
				Task.WhenAll(GridButtons.FadeTo(0, 50), GridButtons.ScaleTo(0, 50), BoxSelected.FadeTo(0, 50), LottieHeader.FadeTo(1, 50));
			}

			Animation animations = new Animation
			{
				{ 0, 1, new Animation(x => MainGrid.RowDefinitions[0].Height = new GridLength(x), MainGrid.RowDefinitions[0].Height.Value, marginLottie * 4) },
				{ 0, 1, new Animation(x => MainGrid.RowDefinitions[1].Height = new GridLength(x), MainGrid.RowDefinitions[1].Height.Value, Math.Min(marginLottie, 40)) }
			};
			animations.Commit(this, nameof(animations));
		}

		private Task HandleButtonFanAspectAsync(ItemsViewScrolledEventArgs e)
		{
			if (e.VerticalOffset < scrollOffsetToHideHeader)
				return Task.WhenAll(BtnBottom.CloseAsync(), BtnBottom.AppearAsync());
			else
				return Task.WhenAll(BtnBottom.CloseAsync(), BtnBottom.DisappearAsync());
		}
	}
}
