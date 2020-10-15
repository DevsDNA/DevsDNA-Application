namespace DevsDNA.Application.Effects
{
    using DevsDNA.Application.Common;
    using Xamarin.Forms;

    public class FixedFrameClippedToBoundsEffect : RoutingEffect
    {
        public FixedFrameClippedToBoundsEffect() : base($"{SettingsKeyValues.ResolutionGroupName}.{nameof(FixedFrameClippedToBoundsEffect)}")
        {
        }
    }
}
