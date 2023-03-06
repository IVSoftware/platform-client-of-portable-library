using net6_portable_library;

namespace maui_client_of_portable_library
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }
        // Consume library
        BusinessLogic _lib = new PlatformBusinessLogic(RuntimePlatform.MAUI);

        private void onClickTestLibrary(object sender, EventArgs e)
        {
            _ = _lib.TestPopup();
        }
    }
    class PlatformBusinessLogic : BusinessLogic
    {
        public PlatformBusinessLogic(RuntimePlatform platform) : base(platform) { }

        protected override async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            return await Shell.Current.CurrentPage.DisplayAlert(title, message, accept, cancel);
        }
    }
}