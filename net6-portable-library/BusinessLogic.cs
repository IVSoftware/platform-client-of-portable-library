using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace net6_portable_library
{
    public abstract class BusinessLogic
    {
        protected abstract Task<bool> DisplayAlert(string title, string message, string accept, string cancel);
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
