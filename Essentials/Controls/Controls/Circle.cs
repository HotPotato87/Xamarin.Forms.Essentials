using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.Essentials.Controls.Controls
{
    public class Circle : ContentView
    {
        public static readonly BindableProperty StrokeProperty = BindableProperty.Create(nameof(StrokeThickness), typeof(double), typeof(Circle), (double)2);
        
        public double StrokeThickness
        {
            get { return (double)this.GetValue(StrokeProperty); }
            set { this.SetValue(StrokeProperty, value); }
        }

        public static readonly BindableProperty StrokeColorProperty = BindableProperty.Create(nameof(StrokeColor), typeof(Color), typeof(Circle), Color.Black);


        public Color StrokeColor
        {
            get { return (Color)this.GetValue(StrokeColorProperty); }
            set { this.SetValue(StrokeColorProperty, value); }
        }
        public static readonly BindableProperty FillColorProperty = BindableProperty.Create(nameof(FillColor), typeof(Color), typeof(Circle), Color.Transparent);


        public Color FillColor
        {
            get { return (Color)this.GetValue(FillColorProperty); }
            set { this.SetValue(FillColorProperty, value); }
        }
    }
}
