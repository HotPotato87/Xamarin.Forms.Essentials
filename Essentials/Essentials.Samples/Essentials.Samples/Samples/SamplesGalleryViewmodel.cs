using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Essentials.Samples.Samples.Circle;
using Essentials.Samples.Samples.StateCondition.Example2;
using Essentials.Samples.Views;
using Xamarin.Forms;

namespace Essentials.Samples.Viewmodels
{
    public class SamplesGalleryViewmodel
    {
        public ICommand StateContainerCommand => new Command(RunStateContainer);
        public ICommand StateContainer2Command => new Command(RunStateContainer2);
        public ICommand CircleSamplesCommand => new Command(() => Navigation.PushAsync(new CircleSamplesView()));

        private void RunStateContainer()
        {
            Navigation.PushAsync(new LoginView());
        }

        private void RunStateContainer2()
        {
            Navigation.PushAsync(new FindUsernameView());
        }

        public INavigation Navigation { get; set; }
    }
}
