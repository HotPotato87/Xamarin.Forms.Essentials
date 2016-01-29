using CoreAnimation;
using UIKit;
using Xamarin.Forms.Essentials.Controls;
using Xamarin.Forms.Platform.iOS;

namespace Controls.iOS.Renderers
{
    public class GradientLayerRenderer : ViewRenderer<GradientLayer, UIView>
    {
        private CAGradientLayer _gradientLayer;

        protected override void OnElementChanged(ElementChangedEventArgs<GradientLayer> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                this.SetNativeControl(this.CreateControl());
            }
        }

        private UIView CreateControl()
        {
            var control = new UIView(this.Frame);
            _gradientLayer = new CAGradientLayer
            {
                Frame = control.Frame,
                Colors = new[] { this.Element.StartColor.ToCGColor(), this.Element.EndColor.ToCGColor() }
            };
            control.Layer.InsertSublayer(_gradientLayer, 0);
            return control;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            //todo : may have to sync frames.
            
        }
    }
}