It's absolutely OK for a portable library to rely on the platform to do things. For example, it's not a stretch that the portable library might want to display a message box prompt. All that's needed is a way for the platform client to inject the dependency somehow.

One of the easier ways I've found to do this is to simply make the library class abstract, compelling the client to inherit the class supplying the platform dependent code:

    namespace net6_portable_library
    {
        public abstract class BusinessLogic
        {
            protected abstract Task<bool> DisplayAlert(
                string title, 
                string message, 
                string accept, 
                string cancel);

            public async Task<bool> TestPopup()
            {
                bool result = await DisplayAlert(
                    title: "Demo", 
                    message: "Do you see this popup?", accept: "Yes", cancel: "No");

                Debug.WriteLine($"Dialog returned {result}");
                return result;
            }
        }
    }

***
**WinForms client**

![screenshot]

    using System;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using net6_portable_library;

    namespace platform_client_of_portable_library.WinForms
    {
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
            BusinessLogic _lib = new PlatformBusinessLogic();
        }
        class PlatformBusinessLogic : BusinessLogic
        {
            public override async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
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
    }

***
**Android client**

![screenshot]

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
            BusinessLogic _lib = new PlatformBusinessLogic();

            private void onClickTestLibrary(object sender, EventArgs e)
            {
                _ = _lib.TestPopup();
            }
        }
        class PlatformBusinessLogic : BusinessLogic
        {
            public override async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
            {
                return await Shell.Current.CurrentPage.DisplayAlert(title, message, accept, cancel);
            }
        }
    }