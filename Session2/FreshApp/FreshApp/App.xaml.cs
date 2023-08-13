using FreshApp.Views;

namespace FreshApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (!Preferences.ContainsKey("isLoggedIn"))
            {
                MainPage = new NavigationPage(new MainPage());
            }
            else
            {
                MainPage = new NavigationPage(new PropertyListingForm());
            }
        }

        void navigateLogin()
        {
            
        }
    }
}