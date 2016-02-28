namespace Xamarin.Forms.Essentials.Controls
{
    public class BlurryPanel : ContentView
    {
        public static readonly BindableProperty BlurAmountProperty = BindableProperty.Create<BlurryPanel, BlurryType>(x => x.BlurAmount, BlurryType.Light);

        public BlurryType BlurAmount
        {
            get { return (BlurryType)this.GetValue(BlurAmountProperty); }
            set { this.SetValue(BlurAmountProperty, value); }
        }
    }
}