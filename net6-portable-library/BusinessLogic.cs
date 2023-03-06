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
