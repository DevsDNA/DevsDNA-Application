namespace DevsDNA.Application.Features.Videos
{
    using ReactiveUI;
    using System;
    using System.Reactive;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using Xamarin.Forms;

    public partial class VideoDetailView
    {
        public VideoDetailView()
        {
            InitializeComponent();
        }

        protected override CompositeDisposable CreateBindings(CompositeDisposable disposables)
        {
            base.CreateBindings(disposables);

            IObservable<EventPattern<object>> observableVideoReadyEvent = Observable.FromEventPattern(v => WebViewYoutube.OnVideoReady += v, v => WebViewYoutube.OnVideoReady -= v);
            disposables.Add(observableVideoReadyEvent.ObserveOn(RxApp.MainThreadScheduler).Subscribe(ep => OnVideoReady()));
            disposables.Add(this.OneWayBind(ViewModel, vm => vm.VideoModel, v => v.WebViewYoutube.VideoModel));
            disposables.Add(this.OneWayBind(ViewModel, vm => vm.CloseCommand, v => v.ButtonClose.Command));
            disposables.Add(this.OneWayBind(ViewModel, vm => vm.VideoModel, v => v.WebViewYoutube.VideoModel));
            disposables.Add(this.OneWayBind(ViewModel, vm => vm.VideoModel, v => v.WebViewYoutube.IsVisible, videoModel => videoModel != null));
            disposables.Add(this.OneWayBind(ViewModel, vm => vm.VideoModel, v => v.IndicatorVideoLoading.IsVisible, videoModel => videoModel != null));            

            return base.CreateBindings(disposables);
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            WebViewYoutube.HeightRequest = (ViewModel.VideoModel.Height / ViewModel.VideoModel.Width) * width;
            base.OnSizeAllocated(width, height);
        }

        private void OnVideoReady()
        {
            IndicatorVideoLoading.IsVisible = false;
            IndicatorVideoLoading.IsRunning = false;
            ImageThumbnail.FadeTo(0);
        }
    }
}