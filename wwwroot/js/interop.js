﻿var wvy = wvy || {};
wvy.interop = (function ($) {

    var weavyConnection = $.hubConnection("https://showcase.weavycloud.com/signalr", { useDefaultPath: false });

    // enable additional logging
    //weavyConnection.logging = true;

    // log errors
    weavyConnection.error(function (error) {
        console.warn('SignalR error: ' + error)
    });

    var rtmProxy = weavyConnection.createHubProxy('rtm');

    rtmProxy.on('eventReceived', function (name, data) {
        // log incoming event
        console.debug(name, data);

        // when we receive an event on the websocket we invoke a method on our chat component
        DotNet.invokeMethodAsync('WeavyTelerikBlazor', 'EventReceived', name, data);
    });

    // connect to weavy realtime hub (only works when we have an auth cookie for the weavy server)
    function connect() {

        weavyConnection.start().done(function () {
            console.debug("weavy connection:", weavyConnection.id);
        }).fail(function () {
            console.error("could not connect to weavy");
        });
    }

    return {
        connect: connect
    }

})(jQuery);
