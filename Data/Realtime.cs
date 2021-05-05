using Microsoft.JSInterop;
using System;
using System.Text.Json;
using WeavyTelerikBlazor.Models;

namespace WeavyTelerikBlazor.Data {

    /// <summary>
    /// Class that publishes realtime event to subsribers
    /// </summary>
    public class Realtime {

        /// <summary>
        /// Genric event handler for all realtime events
        /// </summary>
        public static event EventHandler<RealTimeEventArgs> OnEvent;

        /// <summary>
        /// Event handler for badge events.
        /// </summary>
        public static event EventHandler<BadgeEventArgs> OnBadge;

        /// <summary>
        /// Event handler for message events.
        /// </summary>
        public static event EventHandler<MessageEventArgs> OnMessage;

        /// <summary>
        /// Event handler for typing events.
        /// </summary>
        public static event EventHandler<TypingEventArgs> OnTyping;

        // called from js when we receive an event via the weavy rtm hub
        [JSInvokable]
        public static void ReceivedEvent(string name, string data) {

            // Raise generic event to subscribers
            if (OnEvent != null) {
                var e = new RealTimeEventArgs(name, data);
                OnEvent(null, e);
            }

            // Raise specific events to subscribers
            switch(name) {
                // TODO: handle more events...
                case "badge.weavy":
                    if (OnBadge != null) {
                        var badge = JsonSerializer.Deserialize<Badge>(data);
                        var e = new BadgeEventArgs(badge);
                        OnBadge(null, e);
                    }
                    break;
                case "message-inserted.weavy":
                    if (OnMessage != null) {
                        var message = JsonSerializer.Deserialize<Message>(data);
                        var e = new MessageEventArgs(message);
                        OnMessage(null, e);
                    }
                    break;
                case "typing.weavy":
                    if (OnTyping != null) {
                        var e = JsonSerializer.Deserialize<TypingEvent>(data);
                        var args = new TypingEventArgs(e);
                        OnTyping(null, args);
                    }
                    break;
                default:
                    break;

            }

        }
    }

    // define a class to hold realtime event info
    public class RealTimeEventArgs : EventArgs {
        public RealTimeEventArgs(string name, string data) {
            Name = name;
            Data = data;
        }
        public string Name { get; set; }
        public string Data { get; set; }
    }

    // define a class to hold badge event info
    public class BadgeEventArgs : EventArgs {
        public BadgeEventArgs(Badge data) {
            Data = data;
        }
        public Badge Data { get; set; }
    }

    // define a class to hold message event info
    public class MessageEventArgs : EventArgs {
        public MessageEventArgs(Message data) {
            Data = data;
        }
        public Message Data { get; set; }
    }
    // define a class to hold message event info
    public class TypingEventArgs : EventArgs {
        public TypingEventArgs(TypingEvent data) {
            Data = data;
        }
        public TypingEvent Data { get; set; }
    }

}