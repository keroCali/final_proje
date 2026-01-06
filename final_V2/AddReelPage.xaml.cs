namespace final_V2;

public partial class AddReelPage : ContentPage
{
	public AddReelPage()
	{
		InitializeComponent();
	}

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            await DisplayAlert("Hata", "Lütfen bir başlık girin.", "Tamam");
            return;
        }

        var newItem = new Item
        {
            Name = NameEntry.Text,
            Link = LinkEntry.Text,
            Rating = (int)RatingSlider.Value,
            Comment = CommentEditor.Text,
            Username = App.CurrentUser?.Username
        };

        await App.Database.AddReelsAsync(newItem);
        await Navigation.PopAsync();
    }
}
