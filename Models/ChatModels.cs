using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeavyTelerikBlazor.Models {

    public class Conversation {

        private string _thumb;

        public int id { get; set; }
        public string type { get; set; }
        public bool is_room { get; set; }
        public string name { get; set; }

        public string title {
            get {
                if (is_room) {
                    return name;
                } else if (members.Any()) {
                    return members[0].name ?? members[0].username;
                }
                return "conversation:" + id;
            }
        }

        public DateTime? delivered_at { get; set; }
        public DateTime? read_at { get; set; }
        public bool is_read { get; set; }
        public bool is_pinned { get; set; }
        public Message last_message { get; set; }
        public Member[] members { get; set; }
        public DateTime created_at { get; set; }
        public User created_by { get; set; }
        public int[] followed_by { get; set; }
        public Icon icon { get; set; }
        public string kind { get; set; }
        public string thumb {
            get {
                return _thumb?.Replace("{options}", "96");
            }
            set {
                _thumb = value;
            }
        }
        public string url { get; set; }
        public bool selected { get; set; }
    }

    public class PagedList<T> {
        public IEnumerable<T> data { get; set; }
        public long top { get; set; }

        public long skip { get; set; }
        public string next { get; set; }
    }

    public class Message {

        private string _thumb;

        public int id { get; set; }
        public string type { get; set; }
        public int conversation { get; set; }
        public string text { get; set; }
        public string html { get; set; }
        public DateTime created_at { get; set; }
        public User created_by { get; set; }
        public Icon icon { get; set; }
        public string kind { get; set; }
        public string thumb {
            get {
                return _thumb?.Replace("{options}", "96");
            }
            set {
                _thumb = value;
            }
        }

        public string url { get; set; }
    }

    public class User {

        private string _thumb;

        public string title {
            get {
                return name ?? username;
            }
        }

        public int id { get; set; }
        public string type { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string thumb {
            get {
                return _thumb?.Replace("{options}", "96");
            }
            set {
                _thumb = value;
            }
        }

        public string url { get; set; }
    }

    public class Icon {
        public string name { get; set; }
        public string color { get; set; }
    }

    public class Member {

        private string _thumb;
        public int id { get; set; }
        public string type { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string thumb {
            get {
                return _thumb?.Replace("{options}", "96");
            }
            set {
                _thumb = value;
            }
        }

        public string url { get; set; }
        public DateTime? delivered_at { get; set; }
        public DateTime? read_at { get; set; }
        public string presence { get; set; }
    }

    public class Badge {
        public int conversations { get; set; }
        public int notifications { get; set; }
        public int total { get; set; }
    }

    public static class Helpers {
        public static string AsAbsoluteUrl(this string url) {            
            return Startup.StaticConfig.GetSection("Weavy")["Url"] + url;
        }
    }
}
