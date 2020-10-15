namespace DevsDNA.Application.Features.SocialNetwork
{
    using System;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public partial class PostLeftControl : PostControl
    {
        private readonly double startingTranslationX;

        public PostLeftControl()
        {
            InitializeComponent();
            startingTranslationX = -(DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density) - Math.Abs(Padding.Left);
            TranslateToInitialPosition();
        }

        protected override double StartingTranslation => startingTranslationX;
        protected override ContentView ContentViewMain => ViewMain;
        protected override Label LabelMessage => LblMessage;
        protected override Label LabelAccountAndDate => LblAccountAndDate;
    }
}