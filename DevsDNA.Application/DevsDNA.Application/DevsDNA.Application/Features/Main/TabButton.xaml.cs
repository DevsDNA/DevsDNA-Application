namespace DevsDNA.Application.Features.Main
{
	using ReactiveUI;
	using System;
	using System.Reactive;
	using System.Reactive.Disposables;
	using System.Reactive.Linq;
	using System.Threading.Tasks;
	using Xamarin.Forms;

	public partial class TabButton
	{
		private CompositeDisposable disposables;

		public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ReactiveCommand<TabMain, Unit>), typeof(TabButton), null);
		public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create(nameof(IsSelected), typeof(bool), typeof(TabButton), false, propertyChanged: IsSelectedPropertyChanged);
		public static readonly BindableProperty CommandParametersProperty = BindableProperty.Create(nameof(CommandParameter), typeof(TabMain), typeof(TabButton), TabMain.Unassigned);

		public TabButton()
		{
			InitializeComponent();
			BindingContext = this;

			disposables = new CompositeDisposable();
			IObservable<EventPattern<object>> tappedObservable = Observable.FromEventPattern(h => tapGestureRecognizer.Tapped += h, h => tapGestureRecognizer.Tapped -= h);
			disposables.Add(tappedObservable.Subscribe(ep => TapGestureRecognizerTappedSubscription()));
		}

		~TabButton()
		{
			disposables?.Dispose();
			disposables = null;
		}


		public bool IsSelected
		{
			get => (bool)GetValue(IsSelectedProperty);
			set => SetValue(IsSelectedProperty, value);
		}

		public ReactiveCommand<TabMain, Unit> Command
		{
			get => (ReactiveCommand<TabMain, Unit>)GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}

		public TabMain CommandParameter
		{
			get => (TabMain)GetValue(CommandParametersProperty);
			set => SetValue(CommandParametersProperty, value);
		}



		public async Task WriteText(string text, bool includedInitialBlink = false)
		{
			LblText.Text = text;
			await LblText.ShowAsync(50, includedInitialBlink);
		}


		private static void IsSelectedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			bool isSelected = (bool)newValue;
			((TabButton)bindable).LblText.TextColor = isSelected ? Color.White : (Color)Application.Current.Resources["HighlightColor"];
		}


		private async void TapGestureRecognizerTappedSubscription()
		{
			await LblText.FadeTo(0.5, 50);
			await LblText.FadeTo(1, 50);
		}
	}
}