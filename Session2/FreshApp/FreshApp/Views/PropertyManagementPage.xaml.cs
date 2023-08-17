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
        loadCancellationPolicies();
        EditItemPriceStackLayout.IsVisible = false;

        this.BindingContext = this; 
    }

    ApiService ApiService { get; set; }

    public List<CancellationPolicy> CancellationPolicies { get; set; }

    async void loadItemPrices()
    {
        var res = await ApiService.GetItemPrices(this.Item.ID);

        ItemPriceListView.ItemsSource = res;
    }

    async void loadCancellationPolicies()
    {
        CancellationPolicies = await ApiService.GetCancellationPolicies();

        WeekEndPicker.ItemsSource = CancellationPolicies;
        WeekEndPicker.ItemDisplayBinding = new Binding("Name");

        HolidayPicker.ItemsSource = CancellationPolicies;
        HolidayPicker.ItemDisplayBinding = new Binding("Name");

        OtherPicker.ItemsSource = CancellationPolicies;
        OtherPicker.ItemDisplayBinding = new Binding("Name");

        SelectedItemPricePicker.ItemsSource = CancellationPolicies;
        SelectedItemPricePicker.ItemDisplayBinding = new Binding("Name");
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

    public ItemPrice SelectedItemPrice { get; set; }

    public Command EditItemPriceSwipeCommand
    {
        get
        {
            return new Command<ItemPrice>(async (itemPrice) =>
            {
                if (itemPrice.Status == "booked")
                {
                    await DisplayAlert("", "the item price is booked, you cannot edit it.", "ok");
                    return;
                }

                SelectedItemPrice = itemPrice;

                AddItemPriceStackLayout.IsVisible = false;

                SelectedItemPriceLabel.Text = itemPrice.Date.ToString();
                SelectedItemPriceEntry.Text = itemPrice.Price.ToString();
                SelectedItemPricePicker.SelectedItem = CancellationPolicies.FirstOrDefault(x => x.ID == itemPrice.CancellationPolicyID);

                EditItemPriceStackLayout.IsVisible = true;
            });
        }
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {
        AddItemPriceStackLayout.IsVisible = true;
        EditItemPriceStackLayout.IsVisible = false;
    }

    private async void Button_Clicked_2(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(SelectedItemPriceEntry.Text) || SelectedItemPricePicker.SelectedIndex == -1)
        {
            await DisplayAlert("", "all the fields cannot be null", "ok");
            return;
        }

        var res = await ApiService.EditItemPrice(new
        {
            ID = SelectedItemPrice.ID,
            Price = decimal.Parse(SelectedItemPriceEntry.Text),
            CancellationPolicyID = ((CancellationPolicy)SelectedItemPricePicker.SelectedItem).ID
        });

        if (res)
        {
            await DisplayAlert("", "item price edited", "ok");
            loadItemPrices();
        }
    }
}