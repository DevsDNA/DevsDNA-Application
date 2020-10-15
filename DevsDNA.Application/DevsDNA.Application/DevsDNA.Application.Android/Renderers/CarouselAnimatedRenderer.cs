[assembly: Xamarin.Forms.ExportRenderer(typeof(DevsDNA.Application.Controls.Carousel.CarouselAnimated), typeof(DevsDNA.Application.Droid.Renderers.CarouselAnimatedRenderer))]
namespace DevsDNA.Application.Droid.Renderers
{
    using Android.Content;
    using Android.Views;
    using DevsDNA.Application.Controls.Carousel;
    using ReactiveUI;
    using System;
    using System.Reactive.Linq;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.Android;

    public partial class CarouselAnimatedRenderer : CarouselViewRenderer
    {
        private bool onItemsSourcePaintedIsPending;
        private ItemsViewOnGlobalLayoutListener carouselViewLayoutListener;

        private CarouselAnimated CarouselAnimated => Carousel as CarouselAnimated;

        public CarouselAnimatedRenderer(Context context) : base(context)
        {
        }

        protected override void SetUpNewElement(ItemsView newElement)
        {
            base.SetUpNewElement(newElement);

            AddLayoutListener();
        }

        protected override void TearDownOldElement(ItemsView oldElement)
        {
            ClearLayoutListener();

            base.TearDownOldElement(oldElement);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ClearLayoutListener();
            }

            base.Dispose(disposing);
        }

        protected override void UpdateItemsSource()
        {
            base.UpdateItemsSource();

            if (CarouselAnimated?.ItemsSource != null)
                onItemsSourcePaintedIsPending = true;
        }

        private void AddLayoutListener()
        {
            if (carouselViewLayoutListener != null)
                return;

            carouselViewLayoutListener = new ItemsViewOnGlobalLayoutListener();
            carouselViewLayoutListener.LayoutReady += LayoutReady;

            ViewTreeObserver.AddOnGlobalLayoutListener(carouselViewLayoutListener);
        }

        private void ClearLayoutListener()
        {
            if (carouselViewLayoutListener == null)
                return;

            ViewTreeObserver?.RemoveOnGlobalLayoutListener(carouselViewLayoutListener);
            carouselViewLayoutListener.LayoutReady -= LayoutReady;
            carouselViewLayoutListener = null;
        }

        private void LayoutReady(object sender, EventArgs e)
        {
            if (!onItemsSourcePaintedIsPending)
                return;

            onItemsSourcePaintedIsPending = false;
            Device.BeginInvokeOnMainThread(()=>
            {
                if (CarouselAnimated.Position != 0)
                {
                    CarouselAnimated.WhenAny(c => c.Position, c => c.Value).ObserveOn(RxApp.MainThreadScheduler).Where(c => c == 0).Take(1)
                                .Throttle(TimeSpan.FromMilliseconds(200)).Subscribe(_ => CarouselAnimated.OnItemsSourcePainted());
                    CarouselAnimated.ScrollTo(0, position: Xamarin.Forms.ScrollToPosition.Center, animate: true);
                }
                else
                { 
                    CarouselAnimated.OnItemsSourcePainted();
                }
            });
        }
    }
}