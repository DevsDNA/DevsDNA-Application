namespace DevsDNA.Application.Effects
{
    using DevsDNA.Application.Common;
    using Xamarin.Forms;

    public class DropShadowEffect : RoutingEffect
    {
        public DropShadowEffect() : base($"{SettingsKeyValues.ResolutionGroupName}.{nameof(DropShadowEffect)}")
        {
        }

        public Point Offset { get; set; }
        public float Radius { get; set; }
        public Color ShadowColor { get; set; }
        public float Opacity { get; set; }
    }
}