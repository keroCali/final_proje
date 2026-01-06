namespace final_V2
{
    public partial class App : Application
    {
        private static DataBase? _database;
        public static DataBase Database => _database ??= new DataBase();

        public static User? CurrentUser { get; set; }

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
