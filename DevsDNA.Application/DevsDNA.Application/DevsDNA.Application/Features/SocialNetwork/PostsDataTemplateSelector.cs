namespace DevsDNA.Application.Features.SocialNetwork
{
    using System.Linq;
    using Xamarin.Forms;

    public class PostsDataTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate leftTemplate = new DataTemplate(typeof(PostLeftControl));
        private readonly DataTemplate rightTemplate = new DataTemplate(typeof(PostRightControl));

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            int itemIndex = ((CollectionView)container).ItemsSource.Cast<object>().ToList().IndexOf(item);
            return (itemIndex % 2 == 0) ? leftTemplate : rightTemplate;
        }
    }
}