namespace DevsDNA.Application.Effects
{
    using DevsDNA.Application.Common;
    using Xamarin.Forms;

    public class LabelShadowEffect : RoutingEffect
    {
        public Color Color { get; set; }

        public LabelShadowEffect() : base($"{SettingsKeyValues.ResolutionGroupName}.{nameof(LabelShadowEffect)}")
        {
        }
    }
}

