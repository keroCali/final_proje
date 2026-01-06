using System.Collections.ObjectModel;

namespace final_V2;

public partial class UserLibraryPage : ContentPage
{
    private string _targetUsername;
    public ObservableCollection<Item> Reels { get; set; } = new();

	public UserLibraryPage(string username)
	{
		InitializeComponent();
        _targetUsername = username;
        TitleLabel.Text = $"{_targetUsername}'ın Arşivi";
        MovieCollection.ItemsSource = Reels;
        
        LoadReels();
        CheckFollowStatus();
	}

    private async void LoadReels()
    {
        var items = await App.Database.GetUserReelsAsync(_targetUsername);
        Reels.Clear();
        foreach (var item in items)
            Reels.Add(item);
    }

    private async void CheckFollowStatus()
    {
        if (App.CurrentUser != null && _targetUsername != App.CurrentUser.Username)
        {
            FollowButton.IsVisible = true;
            bool isFollowing = await App.Database.IsFollowingAsync(App.CurrentUser.Username, _targetUsername);
            UpdateFollowButton(isFollowing);
        }
    }

    private void UpdateFollowButton(bool isFollowing)
    {
        if (isFollowing)
        {
            FollowButton.Text = "Takiptesin";
            FollowButton.BackgroundColor = Colors.Gray;
        }
        else
        {
            FollowButton.Text = "Takip Et";
            FollowButton.BackgroundColor = Color.FromArgb("#00c02c");
        }
    }

    private async void OnFollowClicked(object sender, EventArgs e)
    {
        if (App.CurrentUser == null) return;

        bool isFollowing = await App.Database.IsFollowingAsync(App.CurrentUser.Username, _targetUsername);
        
        if (isFollowing)
        {
            await App.Database.UnfollowUserAsync(App.CurrentUser.Username, _targetUsername);
        }
        else
        {
            await App.Database.FollowUserAsync(App.CurrentUser.Username, _targetUsername);
        }

        UpdateFollowButton(!isFollowing);
    }

    private async void OnMovieSelected(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = e.CurrentSelection.FirstOrDefault() as Item;
        if (selectedItem != null)
        {
            MovieCollection.SelectedItem = null;
            await Navigation.PushAsync(new ReelDetailPage(selectedItem));
        }
    }
}
