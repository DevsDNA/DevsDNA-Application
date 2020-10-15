namespace DevsDNA.Application.Features.SocialNetwork
{
    using DevsDNA.Application.Features.Main;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public class CollectionViewSocialNetwork : CollectionView, ITransitionable
    {
        public CollectionViewSocialNetwork()
        {
            ItemTemplate = new PostsDataTemplateSelector();
            ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical) { ItemSpacing = 0 };
            ItemSizingStrategy = ItemSizingStrategy.MeasureFirstItem;
            ItemsUpdatingScrollMode = ItemsUpdatingScrollMode.KeepItemsInView;
            Opacity = 0;
        }

        protected IEnumerable<ITransitionable> VisibleTransitionableViews
        {
            get
            {
                if (LogicalChildren == null || ItemsSource == null)
                    return Enumerable.Empty<ITransitionable>();

                return LogicalChildren.Distinct().OrderBy(d => ItemsSource.Cast<object>().ToList().IndexOf(d.BindingContext)).OfType<ITransitionable>();
            }
        }


        [EditorBrowsable(EditorBrowsableState.Never)]
        public void OnItemsSourcePainted()
        {                 
            DoAppearingAnimationAsync().ConfigureAwait(false);
            this.FadeTo(1, 25);
        }

        public async Task DoAppearingAnimationAsync()
        {
            await Reset();
            var appearingTasks = new List<Task>();
            foreach (var transitionableView in VisibleTransitionableViews)
            {
                appearingTasks.Add(transitionableView.DoAppearingAnimationAsync());
                await Task.Delay(100);
            }
            await Task.WhenAll(appearingTasks);
        }

        public async Task DoDissappearingAnimationAsync()
        {
            var disappearingTasks = new List<Task>();
            foreach (var transitionableView in VisibleTransitionableViews.Reverse())
            {
                disappearingTasks.Add(transitionableView.DoDissappearingAnimationAsync());
                await Task.Delay(100);
            }
            await Task.WhenAll(disappearingTasks);
        }

        public Task Reset()
        {
            return Task.WhenAll(VisibleTransitionableViews.Select(t => t.Reset()));
        }
    }
}
