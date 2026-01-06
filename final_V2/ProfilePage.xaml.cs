namespace final_V2;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadProfileData();
    }

    private async Task LoadProfileData()
    {
        if (App.CurrentUser != null)
        {
            var user = App.CurrentUser; 
            
            UsernameLabel.Text = user.Username;
            BioLabel.Text = string.IsNullOrEmpty(user.Bio) ? "Henüz bir bio eklenmemiş." : user.Bio;
            JoinDateLabel.Text = $"{user.JoinDate:MMMM yyyy}'den beri Reelsbox'ta";

            var reels = await App.Database.GetUserReelsAsync(user.Username);
            ReelCountLabel.Text = reels.Count.ToString();
            
            FollowersLabel.Text = (await App.Database.GetFollowerCountAsync(user.Username)).ToString();
            FollowingLabel.Text = (await App.Database.GetFollowingCountAsync(user.Username)).ToString();

            if (reels.Count > 0)
            {
                var avg = reels.Average(r => r.Rating ?? 0);
                AvgRatingLabel.Text = avg.ToString("F1");

                var topReel = await App.Database.GetTopRatedReelAsync(user.Username);
                if (topReel != null)
                {
                    TopReelSection.IsVisible = true;
                    TopReelNameLabel.Text = topReel.Name;
                    TopReelRatingLabel.Text = $"Puan: {topReel.Rating}/10";
                }
            }
            else
            {
                AvgRatingLabel.Text = "0.0";
                TopReelSection.IsVisible = false;
            }
        }
    }

    private async void OnEditBioClicked(object sender, EventArgs e)
    {
        string result = await DisplayPromptAsync("Bio Düzenle", "Kendin hakkında bir şeyler yaz:", "Kaydet", "İptal", initialValue: App.CurrentUser?.Bio);
        
        if (result != null)
        {
            await App.Database.UpdateUserBioAsync(App.CurrentUser.Username, result);
            App.CurrentUser.Bio = result; 
            BioLabel.Text = result;
        }
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        App.CurrentUser = null;
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
