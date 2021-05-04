using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeavyTelerikBlazor.Models {

    public class PresenceEvent {
        public int user { get; set; }
        public string status { get; set; }
    }

    public class TypingEvent {
        public int conversation { get; set; }
        public Member user { get; set; }
    }
}
