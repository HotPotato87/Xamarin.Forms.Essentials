using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PropertyChanged;
using Xamarin.Forms;

namespace Essentials.Samples.Viewmodels.StateContainerSample
{
    [ImplementPropertyChanged]
    public class LoginViewmodel
    {
        public State LoggedInState { get; set; } = State.Idle;
        public string UserName { get; set; }
        public string Password { get; set; }

        public ICommand LoginCommand => new Command(OnLogin);

        private async void OnLogin()
        {
            LoggedInState = State.LoggingIn;

            //do login routine here
            await Task.Run(() => Task.Delay(2500));

            LoggedInState = State.LoggedIn;
        }

        public enum State
        {
            Idle,
            LoggingIn,
            LoggedIn,
            Problem
        }
    }
}
