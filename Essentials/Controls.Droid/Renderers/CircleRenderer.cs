using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Controls.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Essentials.Controls.Controls;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Circle), typeof(CircleRenderer))]
namespace Controls.Droid.Renderers
{
    public class CircleRenderer : ViewRenderer<Circle, CircleView>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Circle> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                SetNativeControl(new CircleView(Resources.DisplayMetrics.Density, Context)
                {
                    ShapeView = Element
                });
            }
        }
    }
}