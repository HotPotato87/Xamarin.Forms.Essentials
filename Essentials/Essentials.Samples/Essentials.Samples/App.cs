using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Essentials.Samples.Views;
using Xamarin.Forms;
using Xamarin.Forms.Essentials.Controls;

namespace Essentials.Samples
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application'
            StateContainer.Init();
            MainPage = new NavigationPage(new SamplesGalleryView());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
