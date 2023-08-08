using FreshApp.Models;
using FreshApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FreshApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            NavigatePage();
        }

        void NavigatePage()
        {
            if (App.Current.Properties.ContainsKey("user_id"))
            {
                MainPage = new NavigationPage(new LisitngsPage());
            }
            else
            {
                MainPage = new NavigationPage(new MainPage());
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
