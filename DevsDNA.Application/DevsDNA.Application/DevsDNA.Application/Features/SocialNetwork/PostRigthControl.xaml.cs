namespace DevsDNA.Application.Features.SocialNetwork
{
    using System;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    public partial class PostRightControl : PostControl
    {
        private readonly double startingTranslationX;

        public PostRightControl()
        {
            InitializeComponent();
            startingTranslationX = (DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density) + Math.Abs(Padding.Right);
            TranslateToInitialPosition();
        }

        protected override double StartingTranslation => startingTranslationX;
        protected override ContentView ContentViewMain => ViewMain;
        protected override Label LabelMessage => LblMessage;
        protected override Label LabelAccountAndDate => LblAccountAndDate;
    }
}