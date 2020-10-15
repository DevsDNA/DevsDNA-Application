namespace DevsDNA.Application.Effects
{
    using DevsDNA.Application.Common;
    using Xamarin.Forms;

    public class PageStatusBarEffect : RoutingEffect
    {
        public Xamarin.Forms.Color Color { get; set; }

        public PageStatusBarEffect() : base($"{SettingsKeyValues.ResolutionGroupName}.{nameof(PageStatusBarEffect)}")
        {
        }
    }
}


