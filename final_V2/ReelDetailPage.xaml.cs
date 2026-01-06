namespace final_V2;

public partial class ReelDetailPage : ContentPage
{
    private Item _item;

	public ReelDetailPage(Item item)
	{
		InitializeComponent();
        _item = item;
        BindingContext = _item;

        NameLabel.Text = _item.Name;
        RatingLabel.Text = $"{_item.Rating} / 10";
        CommentLabel.Text = string.IsNullOrEmpty(_item.Comment) ? "Yorum yok." : _item.Comment;
        AddedByLabel.Text = $"Eleyen: {_item.Username}";
        
        LoadThumbnail();
	}

    //thumbnail
    private void LoadThumbnail()
    {
        if (string.IsNullOrEmpty(_item.Link))
        {
            LinkButton.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;
            return;
        }

        try
        {
            
            string thumbUrl = _item.Link;
            if (!thumbUrl.EndsWith("/")) thumbUrl += "/";
            
            
            if (thumbUrl.Contains("?"))
            {
                int queryIdx = thumbUrl.IndexOf("?");
                thumbUrl = thumbUrl.Substring(0, queryIdx);
                if (!thumbUrl.EndsWith("/")) thumbUrl += "/";
            }

            
            thumbUrl += "media/?size=l";

            ReelThumbnail.Source = ImageSource.FromUri(new Uri(thumbUrl));
            ReelThumbnail.IsVisible = true;
            PlaceholderLabel.IsVisible = false;
        }
        catch
        {
            ReelThumbnail.IsVisible = false;
            PlaceholderLabel.IsVisible = true;
        }
        finally
        {
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;
        }
    }

    private async void OnLinkClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(_item.Link))
        {
            try
            {
                await Launcher.Default.OpenAsync(_item.Link);
            }
            catch
            {
                await DisplayAlert("Hata", "Link açılamadı.", "Tamam");
            }
        }
    }
}
