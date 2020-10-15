namespace DevsDNA.Application.Controls.Carousel
{
    using System.Collections;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public class CarouselAnimated : CarouselViewScrollFixed
    {
        private readonly CarouselAnimatedScroller carouselAnimatedScroller;

        public CarouselAnimated()
        {
            carouselAnimatedScroller = new CarouselAnimatedScroller();
            ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Horizontal)
            {
                SnapPointsType = SnapPointsType.MandatorySingle,
                SnapPointsAlignment = SnapPointsAlignment.Center,
                ItemSpacing = 0
            };
            PeekAreaInsets = 54;
            HorizontalScrollBarVisibility = ScrollBarVisibility.Never;
            IsBounceEnabled = true;
            Opacity = 0;
        }


        public ContentViewAnimated CenterCell => GetCarouselCell(Position);

        public ContentViewAnimated LeftCell => GetCarouselCell(Position - 1);

        public ContentViewAnimated RightCell => GetCarouselCell(Position + 1);


        public Task OnAppearingAsync()
        {            
            return Task.WhenAll(CenterCell?.EnterDownAsync() ?? Task.CompletedTask, LeftCell?.EnterLeftAsync() ?? Task.CompletedTask, RightCell?.EnterRightAsync() ?? Task.CompletedTask);
        }

        public Task OnDisappearingAsync()
        {
            return Task.WhenAll(CenterCell?.LeaveDownAsync() ?? Task.CompletedTask, LeftCell?.LeaveLeftAsync() ?? Task.CompletedTask, RightCell?.LeaveRightAsync() ?? Task.CompletedTask);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void OnItemsSourcePainted()
        {           
            carouselAnimatedScroller.OnItemsSourceChanged(this);
            OnAppearingAsync();
            this.FadeTo(1, 25);
        }

        public ContentViewAnimated GetCarouselCell(int position)
        {
            var bindingContext = ItemsSource?.Cast<object>()?.ElementAtOrDefault(position);
            if (bindingContext == null)
                return null; 

            return VisibleViews.FirstOrDefault(v => v.BindingContext == bindingContext) as ContentViewAnimated;
        }

        public void UpdateItemsSource(IEnumerable newItemsSource)
        {
            if(newItemsSource == null)
            {             
                Opacity = 0;
                return;
            }
            ItemsSource = newItemsSource;
        }

        public void AdjustCarouselItemsRatio(double targetRatio, Size currentSizeCell)
        {
            Size carouselSizeCell = new Size(currentSizeCell.Width - PeekAreaInsets.HorizontalThickness, currentSizeCell.Height);
            double currentCarouselItemRatio = carouselSizeCell.Height / carouselSizeCell.Width;

            if (currentCarouselItemRatio > targetRatio)
            {
                var heightTarget = carouselSizeCell.Width * targetRatio;
                var newVerticalMargin = (carouselSizeCell.Height - heightTarget) / 2;
                Margin = new Thickness(0, newVerticalMargin);
            }
            else
            {
                var widthTarget = carouselSizeCell.Height / targetRatio;
                var newPeekAreaInsets = (currentSizeCell.Width - widthTarget) / 2;
                PeekAreaInsets = newPeekAreaInsets;
            }
        }

        protected override void OnScrolled(ItemsViewScrolledEventArgs e)
        {
            base.OnScrolled(e);

            carouselAnimatedScroller.OnScrolled(this, e);
        }
    }
}
