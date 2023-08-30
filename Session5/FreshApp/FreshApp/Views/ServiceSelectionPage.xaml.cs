using FreshApp.Models;

namespace FreshApp.Views;

public partial class ServiceSelectionPage : ContentPage
{
    bool isDateSelected = false;
    public User User { get; set; }

    private Service selectedService;

    public Service SelectedService
    {
        get { return selectedService; }
        set { selectedService = value; OnPropertyChanged(); }
    }

    private int bookings;
    private decimal price;

    private ApiService api;
    public ServiceType ServiceType { get; set; }
    public ServiceSelectionPage(ServiceType serviceType)
    {
        InitializeComponent();
        CartCountLabel.Text = $"Cart ({App.User.CartCount})";

        AddToCartScrollView.IsVisible = false;

        api = new ApiService();
        selectedService = new Service();
        this.ServiceType = serviceType;
        User = App.User;

        this.Title = $"Seoul Stay - {this.ServiceType.Name}";
        stDescLabel.Text = ServiceType.Description;

        this.loadListView();

        this.BindingContext = this;
    }

    private async void loadListView()
    {
        var res = await api.getServices(this.ServiceType.ID);

        if (res != null)
        {
            SerivceListView.ItemsSource = res;
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        //App.Current.MainPage = new NavigationPage(new ServicesMenuPage());
    }

    private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new ShoppingCartPage());
    }

    private void TapGestureRecognizer_Tapped_2(object sender, TappedEventArgs e)
    {
        App.Current.MainPage = new NavigationPage(new AboutPage());
    }

    private void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
    {
        sDatePicker.Date = new DateTime(2023, 1, 1);
        AddToCartScrollView.IsVisible = true;
        SelectedService = (Service)(sender as Grid).BindingContext;
        isDateSelected = false;

        FaimlyCountEntry.Text = App.User.FamilyCount.ToString();

        bookings = App.User.FamilyCount / (int)SelectedService.BookingCap;
        if (App.User.FamilyCount % (int)SelectedService.BookingCap != 0)
        {
            bookings++;
        }

        bookingCapLabel.Text = $"in {bookings} bookings";
        price = SelectedService.Price * bookings;

        PriceLabel.Text = $"Amount payable : $ {price.ToString("0.00")}";
    }

    private async void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(SelectedService.DayOfWeek))
        {
            string[] daylist = SelectedService.DayOfWeek.Split(',');

            var list = new List<int>();

            foreach (string day in daylist)
            {
                if (day == "*")
                {
                    for (int i = 0; i < 7; i++)
                    {
                        list.Add(i + 1);
                    }

                    break;
                }

                if (day.Contains('-'))
                {
                    string[] days = day.Split('-');

                    for (int i = int.Parse(days[0]); i <= int.Parse(days[1]); i++)
                    {
                        list.Add(i);
                    }

                    continue;
                }

                list.Add(int.Parse(day));
            }

            int dayOfWeek = (int)sDatePicker.Date.DayOfWeek;

            if (dayOfWeek == 0)
            {
                dayOfWeek = 7;
            }

            if (!list.Any(x => x == dayOfWeek))
            {
                await DisplayAlert("Alert", "The service is not available on the date.", "Ok");
                return;
            }
        }
        else
        {
            string[] daylist = SelectedService.DayOfMonth.Split(',');

            var list = new List<int>();

            foreach (string day in daylist)
            {
                if (day == "*")
                {
                    for (int i = 0; i < 31; i++)
                    {
                        list.Add(i + 1);
                    }

                    break;
                }

                if (day.Contains('-'))
                {
                    string[] days = day.Split('-');

                    for (int i = int.Parse(days[0]); i <= int.Parse(days[1]); i++)
                    {
                        list.Add(i);
                    }

                    continue;
                }

                list.Add(int.Parse(day));
            }

            if (!list.Any(x => x == sDatePicker.Date.Day))
            {
                await DisplayAlert("Alert", "The service is not available on the date.", "Ok");
                return;
            }
        }

        var res = await api.getPostByDate(SelectedService.ID, sDatePicker.Date);

        if (res <= 0)
        {
            await DisplayAlert("Alert", "The service is no post on the date.", "Ok");
            return;
        }

        postLabel.Text = $"Remaining: {res} post(s)";

        isDateSelected = true;
    }

    private async void FaimlyCountEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(FaimlyCountEntry.Text))
        {
            return;
        }

        if (int.Parse(FaimlyCountEntry.Text) <= 0)
        {
            await DisplayAlert("Alert", "Cannot less than or equal to 0", "Ok");

            bookingCapLabel.Text = $"in {bookings} bookings";

            PriceLabel.Text = $"Amount payable : $ {(SelectedService.Price * bookings).ToString("0.00")}";
            return;
        }

        bookings = int.Parse(FaimlyCountEntry.Text) / (int)SelectedService.BookingCap;

        if (App.User.FamilyCount % (int)SelectedService.BookingCap != 0)
        {
            bookings++;
        }

        price = SelectedService.Price * bookings;

        bookingCapLabel.Text = $"in {bookings} bookings";
        PriceLabel.Text = $"Amount payable : $ {price.ToString("0.00")}";
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (!isDateSelected || string.IsNullOrEmpty(noteEntry.Text))
        {
            await DisplayAlert("Alert", "Date is not selected or additional note is empty", "Ok");
            return;
        }

        AddonServiceDetail addonServiceDetail = new AddonServiceDetail()
        {
            GUID = Guid.NewGuid(),
            AddonServiceID = App.User.CartID,
            ServiceID = SelectedService.ID,
            Price = price,
            FromDate = sDatePicker.Date,
            Notes = noteEntry.Text,
            NumberOfPeople = int.Parse(FaimlyCountEntry.Text),
            isRefund = false
        };

        var res = await api.storeAddonService(addonServiceDetail);

        if (!res)
        {
            await DisplayAlert("Alert", "Store service failed", "Ok");
            return;
        }

        await DisplayAlert("Alert", "Succefully store service.", "Ok");

        Application.Current.MainPage = new NavigationPage(new ServicesMenuPage());
    }
}