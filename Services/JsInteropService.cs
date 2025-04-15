// JsInteropService.cs
using Microsoft.JSInterop;

namespace ESIID42025.Services
{
    public class JsInteropService
    {
        private readonly IJSRuntime _jsRuntime;

        public JsInteropService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<bool> Confirm(string message)
        {
            return await _jsRuntime.InvokeAsync<bool>("confirm", message);
        }
    }
}