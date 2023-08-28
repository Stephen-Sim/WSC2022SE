using FreshApp.Models;
using FreshApp.Views;

namespace FreshApp
{
    public partial class App : Application
    {
        public static User User { get; set; }
        public App()
        {
            InitializeComponent();

            User = new User();

            MainPage = new NavigationPage(new MainPage());
        }
    }
}