namespace final_V2
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            // Eğer kullanıcı bir sekmeden (TabBar) başka bir sekmeye geçiyorsa
            if (args.Source == ShellNavigationSource.ShellSectionChanged)
            {
                // Hedef "DiscoveryPage" (Keşfet) sekmesi ise
                if (args.Target.Location.OriginalString.Contains("DiscoveryPage"))
                {
                    // Discovery tab'inin navigasyon stack'ini temizle (varsa üstteki sayfaları kapat)
                    // Not: Bu işlem asenkron olduğu için Dispatcher ile ana iş parçacığında çalıştırılması önerilir
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        var navigation = Shell.Current.Navigation;
                        while (navigation.NavigationStack.Count > 1)
                        {
                            await navigation.PopAsync(false);
                        }
                    });
                }
            }
        }
    }
}
