using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FreshApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            apiService = new ApiService();
        }

        public ApiService apiService { get; set; }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var res = await apiService.Login(usernameEntry.Text, passwordEntry.Text);

            if (res)
            {
                App.Current.MainPage = new NavigationPage(new LisitngsPage());
            }
        }
    }
}