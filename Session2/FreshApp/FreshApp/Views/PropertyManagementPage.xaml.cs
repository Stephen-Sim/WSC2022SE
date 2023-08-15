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
}