[assembly: Xamarin.Forms.ExportRenderer(typeof(DevsDNA.Application.Features.SocialNetwork.CollectionViewSocialNetwork), typeof(DevsDNA.Application.iOS.Renderers.CollectionViewSocialNetworkRenderer))]
namespace DevsDNA.Application.iOS.Renderers
{
    using DevsDNA.Application.Features.SocialNetwork;
    using Xamarin.Forms.Platform.iOS;

    public class CollectionViewSocialNetworkRenderer : CollectionViewRenderer
    {
        private CollectionViewSocialNetwork CollectionViewSocialNetwork => Element as CollectionViewSocialNetwork;

        protected override void UpdateItemsSource()
        {
            base.UpdateItemsSource();

            Controller?.CollectionView?.PerformBatchUpdatesAsync(() => { CollectionViewSocialNetwork?.OnItemsSourcePainted(); });
        }
    }
}