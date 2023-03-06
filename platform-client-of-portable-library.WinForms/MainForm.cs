using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using net_standard_portable_library;

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
}
