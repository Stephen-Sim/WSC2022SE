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
    public partial class PropertyPriceManagementForm : ContentPage
    {
        public Item Item { get; set; }
        ApiService ApiService { get; set; }

        public PropertyPriceManagementForm(Item Item)
        {
            InitializeComponent();

            this.Item = Item;

            ApiService = new ApiService();

            this.Title = $"Property {Item.Title} Prices";

            loadItemPrices();
        }

        async void loadItemPrices()
        {
            var res = await ApiService.GetItemPrices(this.Item.Id);

            if (res != null)
            {
                itemPriceList.ItemsSource = res;
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PopAsync();
        }
    }
}