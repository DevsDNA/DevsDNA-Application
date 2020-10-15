namespace DevsDNA.Application.Features.SocialNetwork
{
    using DevsDNA.Application.Features.Main;
    using System.Threading.Tasks;
    using Xamarin.Forms;
    using DevsDNA.Application.Strings;

    public abstract class PostControl : ContentView, ITransitionable
    {       
        protected abstract double StartingTranslation { get; }
        protected abstract Label LabelMessage { get; }
        protected abstract Label LabelAccountAndDate { get; }
        protected abstract ContentView ContentViewMain { get; }


        public Task DoAppearingAnimationAsync()
        {
            ViewExtensions.CancelAnimations(ContentViewMain);
            TranslateToInitialPosition();
            return ContentViewMain.TranslateTo(0, 0, 500, Easing.CubicOut);
        }

        public Task DoDissappearingAnimationAsync()
        {
            ViewExtensions.CancelAnimations(ContentViewMain);
            return ContentViewMain.TranslateTo(StartingTranslation, 0, 500, Easing.CubicOut);
        }

        public Task Reset()
        {
            ViewExtensions.CancelAnimations(ContentViewMain);
            TranslateToInitialPosition();
            return Task.CompletedTask;
        }


        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            DoAppearingAnimationAsync();

            if (BindingContext is PostModel postModel)
            {
                SetUIProperties(postModel);
            }
        }
     
        protected void TranslateToInitialPosition()
        {
            ContentViewMain.TranslationX = StartingTranslation;
        }


        private void SetUIProperties(PostModel postModel)
        {
            LabelMessage.Text = postModel.Message;
            LabelAccountAndDate.Text = string.Concat(Strings.SocialNetworkPostAccount, " · ", string.Format("{0:dd MMMM}", postModel.CreationDate));

            AutomationProperties.SetName(ContentViewMain, postModel.Message);
        }
    }
}
