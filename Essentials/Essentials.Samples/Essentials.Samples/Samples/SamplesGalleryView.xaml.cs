using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Essentials.Samples.Viewmodels;
using Xamarin.Forms;

namespace Essentials.Samples.Views
{
    public partial class SamplesGalleryView : ContentPage
    {
        public SamplesGalleryView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            (this.BindingContext as SamplesGalleryViewmodel).Navigation = this.Navigation;
        }
    }
}
