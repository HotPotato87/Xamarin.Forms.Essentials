using Essentials.Samples.UWP.Renderers;

namespace Essentials.Samples.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new Essentials.Samples.App());
            CircleRenderer.Init();
        }
    }
}
