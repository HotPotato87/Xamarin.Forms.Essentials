using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Essentials.Samples.Annotations;
using Essentials.Samples.Helpers;
using PropertyChanged;
using Xamarin.Forms;

namespace Essentials.Samples.Samples.StateCondition.Example2
{
    [ImplementPropertyChanged]
    public class FindUsernameViewmodel : ViewmodelBase
    {
        public UsernameState State { get; private set; } = UsernameState.Idle;
        public ICommand ContinueCommand => new Command(DoContinue, CanContinue);
        
        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged();
                ValidateUsername();
            }
        }

        private async void ValidateUsername()
        {
            if (UserName.Length < 2)
            {
                State = UsernameState.Idle;
                return;
            };

            State = UsernameState.Searching;

            //pretend to search user in background thread
            await Task.Run(() => Task.Delay(TimeSpan.FromSeconds(1)));
            
            if (UserName.Length >= 2)
            {
                State = UserName.Equals("User1")
                ? UsernameState.Valid
                : UsernameState.Taken;
            }
        }

        private void DoContinue()
        {
            //continue to next screen
        }

        private bool CanContinue()
        {
            return this.State == UsernameState.Valid;
        }

        public enum UsernameState
        {
            Idle,
            Searching,
            Valid,
            Taken
        }
    }
}
