namespace FreshApp.Views;

public partial class PropertyListingForm : ContentPage
{
    public int user_id { get; set; }
    public ApiService apiService { get; set; }

    public PropertyListingForm()
	{
		InitializeComponent();

        this.user_id = Preferences.Get("user_id", 0);
        apiService = new ApiService(); 

        loadList();
    }

    async void loadList()
    {
        var res = await apiService.GetItems(this.user_id);

        ItemListView.ItemsSource = res;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Preferences.Clear();

        App.Current.MainPage = new NavigationPage(new MainPage());
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {

    }
}