[assembly: Xamarin.Forms.ExportRenderer(typeof(DevsDNA.Application.Features.SocialNetwork.CollectionViewSocialNetwork), typeof(DevsDNA.Application.Droid.Renderers.CollectionViewSocialNetworkRenderer))]
namespace DevsDNA.Application.Droid.Renderers
{
    using Android.Content;
    using DevsDNA.Application.Features.SocialNetwork;
    using System;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.Android;

    public class CollectionViewSocialNetworkRenderer : CollectionViewRenderer
    {
        private bool onItemsSourcePaintedIsPending;
        private ItemsViewOnGlobalLayoutListener collectionViewLayoutListener;

        private CollectionViewSocialNetwork CollectionViewSocialNetwork => Element as CollectionViewSocialNetwork;

        public CollectionViewSocialNetworkRenderer(Context context) : base(context)
        {
        }

        protected override void SetUpNewElement(GroupableItemsView newElement)
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

            if (CollectionViewSocialNetwork?.ItemsSource != null)
                onItemsSourcePaintedIsPending = true;
        }

        private void AddLayoutListener()
        {
            if (collectionViewLayoutListener != null)
                return;

            collectionViewLayoutListener = new ItemsViewOnGlobalLayoutListener();
            collectionViewLayoutListener.LayoutReady += LayoutReady;

            ViewTreeObserver.AddOnGlobalLayoutListener(collectionViewLayoutListener);
        }

        private void ClearLayoutListener()
        {
            if (collectionViewLayoutListener == null)
                return;

            ViewTreeObserver?.RemoveOnGlobalLayoutListener(collectionViewLayoutListener);
            collectionViewLayoutListener.LayoutReady -= LayoutReady;
            collectionViewLayoutListener = null;
        }

        private void LayoutReady(object sender, EventArgs e)
        {
            if (!onItemsSourcePaintedIsPending)
                return;

            onItemsSourcePaintedIsPending = false;
            Device.BeginInvokeOnMainThread(CollectionViewSocialNetwork.OnItemsSourcePainted);
        }
    }
}