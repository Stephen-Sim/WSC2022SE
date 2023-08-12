using FreshApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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

            this.BindingContext = this;
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

        public ICommand SwipeLeftDeleteItemCommand
        { 
            get{
                return new Command(async (obj) =>
                {
                    var itemPrice = (ItemPrice)obj;
                    
                    if (itemPrice.Status == "booked")
                    {
                        await DisplayAlert("", "the item prices is booked, you are not able to delete it.", "ok");
                        return;
                    }

                    bool isTrue = await DisplayAlert("", "are you sure to delete the item prices?", "yes", "no");

                    if (isTrue)
                    {
                        var res = await ApiService.DeleteItemPrice(itemPrice.Id);

                        if (res)
                        {
                            await DisplayAlert("", "item price is removed.", "ok");
                            loadItemPrices();
                        }
                        else
                        {
                            await DisplayAlert("", "item price failed to remove.", "ok");
                        }
                    }
                });
            }
        }

        public ICommand SwipeRightEditItemCommand
        {
            get
            {
                return new Command(async (obj) =>
                {
                    var itemPrice = (ItemPrice)obj;

                    if (itemPrice.Status == "booked")
                    {
                        await DisplayAlert("", "the item prices is booked, you are not able to edit it.", "ok");
                        return;
                    }

                    bool isTrue = await DisplayAlert("", "are you sure to delete the item prices?", "yes", "no");

                    if (isTrue)
                    {

                    }
                });
            }
        }
    }
}