[assembly: Xamarin.Forms.ExportEffect(typeof(DevsDNA.Application.iOS.Effects.LabelShadowEffect),
                                      nameof(DevsDNA.Application.iOS.Effects.LabelShadowEffect))]
namespace DevsDNA.Application.iOS.Effects
{
    using CoreGraphics;
    using Foundation;
    using System.Linq;
    using UIKit;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.iOS;

    public class LabelShadowEffect : PlatformEffect
    {
        private Color shadowColor;

        protected override void OnAttached()
        {
            shadowColor = ((DevsDNA.Application.Effects.LabelShadowEffect)(Element.Effects.FirstOrDefault(e => e is DevsDNA.Application.Effects.LabelShadowEffect))).Color;

            SetLabelStroke();
            SetLabelShadow();
            if (Element != null)
                Element.PropertyChanged += OnElementPropertyChanged;
        }

        protected override void OnDetached()
        {
            if (Element != null)
                Element.PropertyChanged -= OnElementPropertyChanged;
        }


        private void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == Label.TextProperty.PropertyName)
                SetLabelStroke();
        }

        private void SetLabelStroke()
        {
            if (Container is LabelRenderer labelRenderer && labelRenderer?.Control != null)
            {
                var attributedTextWithStroke = new NSMutableAttributedString(labelRenderer.Control.AttributedText);
                var range = new NSRange(0, attributedTextWithStroke.Length);

                attributedTextWithStroke.AddAttribute(UIStringAttributeKey.StrokeColor, shadowColor.ToUIColor(), range);
                attributedTextWithStroke.AddAttribute(UIStringAttributeKey.StrokeWidth, NSNumber.FromFloat(-2), range);

                labelRenderer.Control.AttributedText = attributedTextWithStroke;
            }
        }

        private void SetLabelShadow()
        {
            Control.Layer.ShadowRadius = 0;
            Control.Layer.ShadowColor = shadowColor.ToCGColor();
            Control.Layer.ShadowOffset = new CGSize(0.5, 0.5);
            Control.Layer.ShadowOpacity = 0.5f;
        }
    }
}

