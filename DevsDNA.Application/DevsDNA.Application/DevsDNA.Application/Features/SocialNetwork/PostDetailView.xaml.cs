namespace DevsDNA.Application.Features.SocialNetwork
{
    using DevsDNA.Application.Features.SocialNetwork.PostParser;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    public partial class PostDetailView : Grid
    {
        private const string facebookHashtagUrl = "https://www.facebook.com/hashtag/";
        private double bottomTranslation;        
        private bool isSwipingUp;

        public static readonly BindableProperty LinkCommandProperty = BindableProperty.Create(nameof(LinkCommand), typeof(ICommand), typeof(PostDetailView), null);

        public PostDetailView()
        {
            InitializeComponent();
        }

        public ICommand LinkCommand
        {
            get { return (ICommand)GetValue(LinkCommandProperty); }
            set { SetValue(LinkCommandProperty, value); }
        }

        public Task ShowAsync()
        {
            bottomTranslation = Height;
            TranslationY = bottomTranslation;
            InputTransparent = false;
            LottieLogo.Play();
            return Task.WhenAll(this.TranslateTo(0, 0, 300), this.FadeTo(1), ScrollViewDetail.ScrollToAsync(0, 0, false));
        }

        public Task HideAsync()
        {
            InputTransparent = true;
            BindingContext = null;
            return Task.WhenAll(this.TranslateTo(0, bottomTranslation, 300));
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if(BindingContext is PostModel postModel)
            {
                SetUIProperties(postModel);
            }
        }

        private void SetUIProperties(PostModel postModel)
        {
            LabelDate.Text = string.Format("{0:dd MMMM yyyy}", postModel.CreationDate);
            SetPostMessageUI(postModel.Message);
            var attachment = postModel.Attachments?.FirstOrDefault();
            SetAttachmentUI(attachment);

        }

        private void SetPostMessageUI(string message)
        {
            LabelMessage.FormattedText = new FormattedString();
            var spans = PostTextParser.Parse(message).Select(CreateSpan).ToList();
            foreach (var span in spans)
            {
                LabelMessage.FormattedText.Spans.Add(span);
            }
        }

        private Span CreateSpan(PostSpan postSpan)
        {
            var span = new Span()
            {
                Text = postSpan.Text
            };

            if (postSpan.Type != SpanType.Plain)
            {
                span.TextColor = (Color)Application.Current.Resources["DarkBlueColor"];
                if(postSpan.Type == SpanType.Url)
                {
                    span.GestureRecognizers.Add(new TapGestureRecognizer() { Command = LinkCommand, CommandParameter = postSpan.Text });
                }
                else if(postSpan.Type == SpanType.Hashtag)
                {
                    span.GestureRecognizers.Add(new TapGestureRecognizer() { Command = LinkCommand, CommandParameter = string.Concat(facebookHashtagUrl, postSpan.Text.Substring(1)) });
                }
            }

            return span;
        }

        private void SetAttachmentUI(Attachment attachment)
        {
            ImagePost.IsVisible = false;
            GridShare.IsVisible = false; 
            if (attachment != null)
            {
                if (attachment.Type == AttachmentType.Share)
                {
                    LabelShareTitle.Text = attachment.Title;
                    LabelShareDescription.Text = attachment.Description;
                    ImageShare.Source = attachment.Image;
                    GridShare.IsVisible = true;
                }
                else
                {
                    ImagePost.Source = attachment.Image;
                    ImagePost.IsVisible = true;
                }
            }
        }

        private async void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            await ManagePanUpdatedAsync(e);
        } 

        private async Task ManagePanUpdatedAsync(PanUpdatedEventArgs eventArgs)
        {
            switch (eventArgs.StatusType)
            {
                case GestureStatus.Started:
                    isSwipingUp = false;
                    break;
                case GestureStatus.Running:
                    double translationY = Math.Max(Device.RuntimePlatform == Device.iOS? ( eventArgs.TotalY) : (TranslationY + eventArgs.TotalY),0);   
                    isSwipingUp = TranslationY > translationY;
                    TranslationY = translationY;
                    break;
                default:
                    if (isSwipingUp)
                        await this.TranslateTo(0, 0);            
                    else
                        await HideAsync();
                    break;
            }
        }

        private async void OnCloseClicked(object sender, EventArgs e)
        {
            await HideAsync();
        }
    }
}