﻿using Microsoft.JSInterop;
using System.Threading;
using System.Threading.Tasks;

namespace WeavyTelerikBlazor.Data {
    public class ExtendableJSObjectReference : IJSObjectReference {
        public IJSObjectReference objectReference;

        public ExtendableJSObjectReference(IJSObjectReference reference = null) {
            objectReference = reference;
        }

        public ValueTask DisposeAsync() {
            return objectReference.DisposeAsync();
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object[] args) {
            return objectReference.InvokeAsync<TValue>(identifier, args);
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object[] args) {
            return objectReference.InvokeAsync<TValue>(identifier, cancellationToken, args);
        }
    }
}
