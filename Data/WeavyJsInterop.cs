using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace WeavyTelerikBlazor.Data {
    public class WeavyJsInterop : IDisposable {
        private bool _initialized = false;
        private readonly IJSRuntime _js;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IJSObjectReference _bridge;
        private ValueTask<IJSObjectReference> _whenImport;


        public WeavyJsInterop(IJSRuntime js, IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
            _js = js;
        }

        public async Task Init() {
            if (!_initialized) {
                _whenImport = _js.InvokeAsync<IJSObjectReference>("import", "./js/weavyJsInterop.js");
                _bridge = await _whenImport;
            } else {
                await _whenImport;
            }
        }

        public async ValueTask<IJSObjectReference> Weavy(object options = null) {
            await Init();
            // get jwt from current user claims
            var jwt = new { jwt = _httpContextAccessor?.HttpContext?.User?.FindFirst("jwt")?.Value };
            return await _bridge.InvokeAsync<IJSObjectReference>("weavy", new object[] { jwt, options });
        }

        public void Dispose() {
            _bridge?.DisposeAsync();
        }
    }
}

