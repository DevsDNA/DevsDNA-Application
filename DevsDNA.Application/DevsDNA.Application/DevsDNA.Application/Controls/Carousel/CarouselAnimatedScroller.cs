namespace DevsDNA.Application.Controls.Carousel
{
    using System.Collections.Generic;
    using System.Linq;
    using Xamarin.Forms;

    public class CarouselAnimatedScroller
    {
        private readonly List<double> cellCenters = new List<double>();

        public void OnItemsSourceChanged(CarouselAnimated carousel)
        {
            if (carousel.ItemsSource == null || !carousel.VisibleViews.Any())
                return;

            cellCenters.Clear();
            double cellWidth = carousel.VisibleViews.FirstOrDefault().Width;
            int carouselItemsCount = carousel.ItemsSource.Cast<object>().Count();
            for (int i = 0; i < carouselItemsCount; i++)
            {
                cellCenters.Add(cellWidth * i);
            }
        }

        public void OnScrolled(CarouselAnimated carousel, ItemsViewScrolledEventArgs e)
        {
            if (!carousel.VisibleViews.Any() || !cellCenters.Any())
                return;


            double offset = AdjustOffsetInBounds(e.HorizontalOffset);

            if (IsCellCenter(offset))
            {
                carousel.CenterCell?.GoToFront(100);
                carousel.RightCell?.GoToBack(100);
                carousel.LeftCell?.GoToBack(100);
                return;
            }

            bool goRight = e.HorizontalDelta > 0;

            int nextCellIndex = goRight ? cellCenters.FindIndex(c => c > offset) : cellCenters.FindLastIndex(c => c < offset);
            int backCellIndex = goRight ? nextCellIndex - 1 : nextCellIndex + 1;
            double progressPercentage = backCellIndex != -1 ? (offset - cellCenters[backCellIndex]) * 100 / (cellCenters[nextCellIndex] - cellCenters[backCellIndex]) : 0;

            var nextCell = carousel.GetCarouselCell(nextCellIndex);
            var backCell = carousel.GetCarouselCell(backCellIndex);

            nextCell?.GoToFront(progressPercentage);
            backCell?.GoToBack(progressPercentage);
        }


        private double AdjustOffsetInBounds(double offset)
        {
            if (offset > cellCenters.LastOrDefault())
                return cellCenters.LastOrDefault();
            else if (offset < cellCenters.FirstOrDefault())
                return cellCenters.FirstOrDefault();

            return offset;
        }

        private bool IsCellCenter(double offset)
        {
            return cellCenters.Any(c => c == offset);
        }

    }
}
