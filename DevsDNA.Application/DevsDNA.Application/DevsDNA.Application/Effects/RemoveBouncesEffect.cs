namespace DevsDNA.Application.Effects
{
    using DevsDNA.Application.Common;
    using Xamarin.Forms;

    public class RemoveBouncesEffect : RoutingEffect
    {
        public RemoveBouncesEffect() : base($"{SettingsKeyValues.ResolutionGroupName}.{nameof(RemoveBouncesEffect)}")
        {
        }
    }
}