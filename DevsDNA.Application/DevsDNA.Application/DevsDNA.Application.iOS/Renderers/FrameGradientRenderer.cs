[assembly: Xamarin.Forms.ExportRenderer(typeof(DevsDNA.Application.Controls.FrameGradient), typeof(DevsDNA.Application.iOS.Renderers.FrameGradientRenderer))]
namespace DevsDNA.Application.iOS.Renderers
{
    using CoreAnimation;
    using CoreGraphics;
    using DevsDNA.Application.Controls;
    using UIKit;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.iOS;

    public class FrameGradientRenderer : FrameRenderer
    {
        private CAGradientLayer gradientLayer = new CAGradientLayer();
        private UIView frameView;

        private FrameGradient FrameGradient => Element as FrameGradient;

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                frameView = Subviews[0];
                AddGradient();
            }
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            gradientLayer.Frame = frameView.Frame;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (gradientLayer != null)
                {
                    gradientLayer.RemoveFromSuperLayer();
                    gradientLayer.Dispose();
                    gradientLayer = null;
                }
            }
        }

        private void AddGradient()
        {
            frameView.BackgroundColor = UIColor.Clear;
            gradientLayer.Colors = new[] { FrameGradient.StartColor.ToCGColor(), FrameGradient.EndColor.ToCGColor() };
            gradientLayer.StartPoint = new CGPoint(0, 0.5);
            gradientLayer.EndPoint = new CGPoint(1.0, 0.5);
            gradientLayer.Frame = frameView.Layer.Bounds;
            gradientLayer.CornerRadius = frameView.Layer.CornerRadius;
            frameView.Layer.InsertSublayer(gradientLayer, 0);
        }

    }
}