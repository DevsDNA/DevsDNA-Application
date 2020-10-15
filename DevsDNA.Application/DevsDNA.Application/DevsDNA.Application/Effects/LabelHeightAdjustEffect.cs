namespace DevsDNA.Application.Effects
{
    using DevsDNA.Application.Common;
    using Xamarin.Forms;

    public class LabelHeightAdjustEffect : RoutingEffect
    {
        public LabelHeightAdjustEffect() : base($"{SettingsKeyValues.ResolutionGroupName}.{nameof(LabelHeightAdjustEffect)}")
        {
        }
    }
}
