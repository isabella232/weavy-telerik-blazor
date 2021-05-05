using WeavyTelerikBlazor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WeavyTelerikBlazor.Data {
    
    /// <summary>
    /// Chat service for integration with the Weavy API endpoints related to chat.
    /// </summary>
    public class ChatService {

        private HttpClient _client;
        private IHttpContextAccessor _httpContextAccessor;

        public ChatService(IHttpContextAccessor httpContextAccessor, IConfiguration config, HttpClient client) {
            _httpContextAccessor = httpContextAccessor;
            _client = client;
            _client.BaseAddress = new Uri(config.GetSection("Weavy")["Url"]);
        }

        /// <summary>
        /// Calls Weavy and returns the notification badge for the current user.
        /// </summary>
        /// <returns></returns>
        public async Task<Badge> GetBadgeAsync() {
            return await CallApiAsync<Badge>(HttpMethod.Get, "a/notifications/badges");
        }

        /// <summary>
        /// Calls Weavy and returns the conversations for the current user.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Conversation>> GetConversationsAsync() {
            return await CallApiAsync<IEnumerable<Conversation>>(HttpMethod.Get, "api/conversations");
        }


        /// <summary>
        /// Calls Weavy and returns the messages that belongs to the conversation with the specified id, also marks the conversation as read.
        /// </summary>
        /// <param name="conversationId">The id of the conversation to get messages for.</param>
        /// <returns></returns>
        public async Task<PagedList<Message>> GetMessagesAsync(int conversationId) {
            // first call api and mark conversation as read
            await CallApiAsync<Conversation>(HttpMethod.Post, $"api/conversations/{conversationId}/read");
            return await CallApiAsync<PagedList<Message>>(HttpMethod.Get, $"api/conversations/{conversationId}/messages");
        }

        /// <summary>
        /// Calls Weavy and creates a new conversation with the members supplied. 
        /// </summary>
        /// <param name="memberIds">The members in the conversation.</param>
        /// <returns></returns>
        public async Task<Conversation> CreateConversationAsync(List<int> memberIds) {
            return await CallApiAsync<Conversation>(HttpMethod.Post, $"api/conversations", JsonSerializer.Serialize(new { members = memberIds }));
        }

        /// <summary>
        /// Calls Weavy and posts a new message in the specified conversation.
        /// </summary>
        /// <param name="conversationId">The id of the conversion to post message in.</param>
        /// <param name="message">The message content.</param>
        /// <returns></returns>
        public async Task<Message> PostMessageAsync(int conversationId, string message) {
            return await CallApiAsync<Message>(HttpMethod.Post, $"api/conversations/{conversationId}/messages", JsonSerializer.Serialize(new { Text = message }));
        }

        /// <summary>
        /// Calls Weavy and returns all users that can perticipate in the chat.
        /// </summary>
        /// <returns></returns>
        public async Task<PagedList<User>> GetChatUsersAsync() {
            return await CallApiAsync<PagedList<User>>(HttpMethod.Get, $"api/users");
        }

        private async Task<T> CallApiAsync<T>(HttpMethod method, string requestUri, string content = null) {
            // get token for user
            var jwt = _httpContextAccessor.HttpContext.User.FindFirst("jwt").Value;

            if (jwt == null) {
                throw new Exception("Could not get JWT token.");
            }

            // set JWT token and call api 
            var request = new HttpRequestMessage(method, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            
            if (content != null) {
                // add json body
                request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            }
            
            try {
                var response = await _client.SendAsync(request);

                if (response.IsSuccessStatusCode) {
                    // read, deserialize and return
                    return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync());
                }
            } catch (Exception ex) {
                // should do something smart here, but that's out of scope...
                Console.WriteLine(ex.Message);
                return default;
            }
            return default;
        }
    }
}