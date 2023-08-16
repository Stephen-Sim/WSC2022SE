using FreshApp.Models;

namespace FreshApp.Views;

public partial class PropertyManagementPage : ContentPage
{
    public Item Item { get; set; }

    public PropertyManagementPage(Item Item)
	{
		InitializeComponent();

		this.Item = Item;

        ApiService = new ApiService();

        this.Title = $"Propety {this.Item.Title} Prices";

        loadItemPrices();

        this.BindingContext = this; 
    }

    ApiService ApiService { get; set; }

    async void loadItemPrices()
    {
        var res = await ApiService.GetItemPrices(this.Item.ID);

        ItemPriceListView.ItemsSource = res;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await App.Current.MainPage.Navigation.PopAsync();
    }

    public Command DeleteItemPriceSwipeCommand
    {
        get
        {
            return new Command<ItemPrice>(async (itemPrice) =>
            {
                if (itemPrice.Status == "booked")
                {
                    await DisplayAlert("", "the item price is booked, you cannot delete it.", "ok");
                    return;
                }

                var result = await DisplayAlert("", "Are you sure to delete this item price?", "yes", "no");

                if (result)
                {
                    var res = await ApiService.DeleteItemPrice(itemPrice.ID);

                    if (res)
                    {
                        await DisplayAlert("", "Item Price is deleted.", "Ok");
                        loadItemPrices();
                    }
                }
            });
        }
    }
}