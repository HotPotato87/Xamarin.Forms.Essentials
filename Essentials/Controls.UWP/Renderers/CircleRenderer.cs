using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Controls.UWP;
using Essentials.Samples.UWP.Renderers;
using Xamarin.Forms.Essentials.Controls.Controls;
using Xamarin.Forms.Platform.UWP;

[assembly:ExportRenderer(typeof(Circle), typeof(CircleRenderer))]
namespace Essentials.Samples.UWP.Renderers
{
    public class CircleRenderer : ViewRenderer<Circle, Ellipse>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Circle> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                this.SetNativeControl(CreateCircle());
            }
        }

        public static void Init()
        {
            
        }

        private Ellipse CreateCircle()
        {
            return new Ellipse()
            {
                StrokeThickness = this.Element.StrokeThickness,
                Stroke = new SolidColorBrush(this.Element.StrokeColor.ToMediaColor()),
                Fill = new SolidColorBrush(this.Element.FillColor.ToMediaColor())
            };
        }
    }
}
