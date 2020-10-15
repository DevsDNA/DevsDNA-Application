[assembly: Xamarin.Forms.ExportRenderer(typeof(DevsDNA.Application.Controls.Carousel.CarouselAnimated), typeof(DevsDNA.Application.iOS.Renderers.CarouselAnimatedRenderer))]
namespace DevsDNA.Application.iOS.Renderers
{
    using DevsDNA.Application.Controls.Carousel;
    using ReactiveUI;
    using System;
    using System.Reactive.Linq;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.iOS;

    public class CarouselAnimatedRenderer : CarouselViewRenderer
    {
        private CarouselAnimated CarouselAnimated => Element as CarouselAnimated;

        protected override void UpdateItemsSource()
        {
            base.UpdateItemsSource();

            Controller?.CollectionView?.PerformBatchUpdatesAsync(() =>
            {
                if (CarouselAnimated.Position != 0)
                {
                    CarouselAnimated.WhenAny(c => c.Position, c => c.Value).ObserveOn(RxApp.MainThreadScheduler).Where(c => c == 0).Take(1)
                                .Throttle(TimeSpan.FromMilliseconds(200)).Subscribe(_ => CarouselAnimated.OnItemsSourcePainted());
                    CarouselAnimated.ScrollTo(0, position: ScrollToPosition.Center, animate: false);
                }
                else
                {
                    CarouselAnimated.OnItemsSourcePainted();
                }               
            });
        }
    }
}