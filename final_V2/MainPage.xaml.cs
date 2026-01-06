using System.Collections.ObjectModel;

namespace final_V2
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Item> Reels { get; set; } = new();

        public MainPage()
        {
            InitializeComponent();
            ReelsCollection.ItemsSource = Reels;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadReels();
        }

        private async Task LoadReels()
        {
            if (App.CurrentUser == null) return;

            var items = await App.Database.GetUserReelsAsync(App.CurrentUser.Username);
            Reels.Clear();
            foreach (var item in items)
                Reels.Add(item);
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddReelPage());
        }

        private async void OnReelSelected(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = e.CurrentSelection.FirstOrDefault() as Item;
            if (selectedItem != null)
            {
                ReelsCollection.SelectedItem = null;
                await Navigation.PushAsync(new ReelDetailPage(selectedItem));
            }
        }
    }
}
