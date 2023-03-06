I do this kind of thing a lot with my apps that are cross-platform between WinForms and things like Xamarin for example. One of the easier ways I've found to do this is to make the portable library class `abstract`, compelling the client to inherit the class supplying the platform dependent code. It can also be handy to make your client identify itself with a `RuntimePlatform` enum. Alternatively, `virtual` methods would work but can produce "silent fails" unless they throw `NotImplementedException` at which point why not `abstract` it.

    using System.Diagnostics;
    using System.Threading.Tasks;

    namespace net6_portable_library
    {
        public enum RuntimePlatform
        {
            WinOS,
            Android,
            iOS,
            Web,
            MAUI,
        }
        public abstract class BusinessLogic
        {

            public BusinessLogic(RuntimePlatform platform) => RuntimePlatform = platform;
            protected abstract Task<bool> DisplayAlert(string title, string message, string accept, string cancel);
            public async Task<bool> TestPopup()
            {
                bool result = await DisplayAlert(
                    title: "Demo", 
                    message: $"Is {RuntimePlatform} popup visible?", accept: "Yes", cancel: "No");

                Debug.WriteLine($"Dialog returned {result}");
                return result;
            }
            public RuntimePlatform RuntimePlatform { get; }
        }
    }

***
Another option I've had success with is having `interface` or `delegate` members  in the PCL lib that the platform-specific client can populate.

***
**WinForms client**

[![winforms client][1]][1]

    using System;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using net6_portable_library;

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            buttonTestLibrary.Click += onClickTestLibrary;
        }
        private void onClickTestLibrary(object sender, EventArgs e)
        {
            _ = _lib.TestPopup();
        }
        // Consume library
        BusinessLogic _lib = new PlatformBusinessLogic(RuntimePlatform.WinOS);
    }
    class PlatformBusinessLogic : BusinessLogic
    {
        public PlatformBusinessLogic(RuntimePlatform platform) : base(platform) { }

        protected override async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            var buttons = $"{accept}.{cancel}".ToUpper();
            switch (buttons) 
            {
                case "YES.NO":
                    bool result = false;
                    await Task.Run(() =>
                    {
                        result = DialogResult.Yes.Equals
                        (
                            MessageBox.Show(text: message, caption: title, MessageBoxButtons.YesNo)
                        );
                    });
                    return result;
                default:
                    throw new NotImplementedException($"TODO: Handler for {buttons}.");
            }
        }
    }

***
**Android client**

[![maui client][2]][2]

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


  [1]: https://i.stack.imgur.com/z7dif.png
  [2]: https://i.stack.imgur.com/DiFL6.png