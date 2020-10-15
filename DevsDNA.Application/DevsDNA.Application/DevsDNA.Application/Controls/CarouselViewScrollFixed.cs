namespace DevsDNA.Application.Controls
{
    using DevsDNA.Application.Common;
    using DevsDNA.Application.Messages;
    using Xamarin.Forms;
   
    public class CarouselViewScrollFixed : CarouselView
    {
        private Thickness originalPeekAreaInsets;
        private int positionBeforeLandscape;

        public CarouselViewScrollFixed()
        {
            MessagingCenter.Instance.Subscribe<Application, OrientationMessage>(Application.Current, SettingsKeyValues.MessagingCenterOrientationChangedMessage, (a, o) => FixScrollBug(o));
        }

        ~CarouselViewScrollFixed()
        {
            MessagingCenter.Instance.Unsubscribe<Application>(Application.Current, SettingsKeyValues.MessagingCenterOrientationChangedMessage);
        }

        private void FixScrollBug(OrientationMessage orientationMessage)
        {
            if (Device.RuntimePlatform != Device.Android)
                return;

            if (orientationMessage.Orientation == Orientation.Landscape)
            {
                positionBeforeLandscape = Position;
                originalPeekAreaInsets = PeekAreaInsets;
                PeekAreaInsets = 0;
            }
            else if(orientationMessage.Orientation == Orientation.Portrait)
            {
                PeekAreaInsets = originalPeekAreaInsets;
                ScrollTo(positionBeforeLandscape, position: ScrollToPosition.Center, animate: true);
            }
        }
    }
}
