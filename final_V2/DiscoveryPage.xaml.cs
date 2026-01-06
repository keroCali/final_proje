namespace final_V2;

public partial class DiscoveryPage : ContentPage
{
	public DiscoveryPage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadActivity();
    }

    private async Task LoadActivity()
    {
        if (App.CurrentUser == null) return;

        var activities = await App.Database.GetFriendsActivityAsync(App.CurrentUser.Username);
        ActivityCollection.ItemsSource = activities;
    }

    private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            SearchResultsSection.IsVisible = false;
            ActivitySection.IsVisible = true;
            return;
        }

        SearchResultsSection.IsVisible = true;
        ActivitySection.IsVisible = false;

        var users = await App.Database.SearchUsersAsync(e.NewTextValue, App.CurrentUser?.Username ?? "");
        UsersCollection.ItemsSource = users;
    }

    private async void OnUserSelected(object sender, SelectionChangedEventArgs e)
    {
        var selectedUser = e.CurrentSelection.FirstOrDefault() as User;
        if (selectedUser != null)
        {
            UsersCollection.SelectedItem = null;
            await Navigation.PushAsync(new UserLibraryPage(selectedUser.Username));
        }
    }

    private async void OnActivitySelected(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = e.CurrentSelection.FirstOrDefault() as Item;
        if (selectedItem != null)
        {
            ActivityCollection.SelectedItem = null;
            await Navigation.PushAsync(new ReelDetailPage(selectedItem));
        }
    }
}
