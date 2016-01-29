using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Essentials.Controls;
using Xamarin.Forms.Platform.iOS;

namespace Controls.iOS.Renderers
{
    public class BlurryPanelRenderer : ViewRenderer<BlurryPanel, UIVisualEffectView>
    {
        private UIVisualEffectView _visualEffectView;

        protected override void OnElementChanged(ElementChangedEventArgs<BlurryPanel> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null)
                return;

            if (Control == null)
            {
                this.SetNativeControl(this.SetupLayer());
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            _visualEffectView.Frame = this.Frame;
        }

        private UIVisualEffectView SetupLayer()
        {
            UIVisualEffect blurEffect = UIBlurEffect.FromStyle(ChooseBlurAmount(this.Element.BlurAmount));
            _visualEffectView = new UIVisualEffectView(blurEffect)
            {
                Frame = this.Frame
            };
            return _visualEffectView;
        }

        private UIBlurEffectStyle ChooseBlurAmount(BlurryType blurAmount)
        {
            switch (blurAmount)
            {
                case BlurryType.ExtraLight:
                    return UIBlurEffectStyle.ExtraLight;
                case BlurryType.Light:
                    return UIBlurEffectStyle.Light;
                case BlurryType.Dark:
                    return UIBlurEffectStyle.Dark;
            }

            return UIBlurEffectStyle.Light;
        }
    }
}
