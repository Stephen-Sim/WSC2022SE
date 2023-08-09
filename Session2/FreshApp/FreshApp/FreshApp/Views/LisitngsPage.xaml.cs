using FreshApp.Models;
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
    public partial class LisitngsPage : ContentPage
    {
        public long UserId { get; set; }
        public LisitngsPage()
        {
            InitializeComponent();
            apiService = new ApiService();

            UserId = (long)App.Current.Properties["user_id"];

            loadData();
        }

        public ApiService apiService { get; set; }

        async void loadData()
        {
            var res = await apiService.GetListings(UserId);

            ListingListView.ItemsSource = res;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Application.Current.Properties.Clear();
            App.Current.MainPage = new NavigationPage(new MainPage());
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            var item = (Item)(sender as Button).CommandParameter;
            await Application.Current.MainPage.Navigation.PushAsync(new PropertyPriceManagementForm(item));
        }
    }
}